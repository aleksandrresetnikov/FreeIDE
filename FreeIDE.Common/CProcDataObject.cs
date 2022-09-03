using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using static FreeIDE.Common.ShellDll;

namespace FreeIDE.Common
{
    /// <Summary>This class takes the IDataObject or .Net DataObjectpassed into DragEnter
    /// and builds a IDataObject that is suitable for use when interacting with the
    /// IDropTarget of a Folder.
    /// Requirements:
    /// The input IDataObject must contain at least one of the following Formats and Data
    ///   1. An ArrayList of CShItems
    ///   2. A Shell IDList Array
    ///   3. A FileDrop structure
    /// A Valid instance of this class will have and expose
    /// 1. Dobj -- A COM IDataObject suitable for use in interaction with a Folder's IDropTarget
    ///             Dobj will have, at least, a valid Shell IDList Array representing the Dragged Items
    /// 2. DragData -- An ArrayList of 1 or more CShItems representing the Dragged Items
    /// Processing Steps:
    /// 1. Check for presence of one or more of the required Formats with valid Data
    /// 2. Build or use the provided ArrayList of CShItems
    /// 3. Ensure that all items are of same FileSystem/Non-FileSystem classification
    /// 4. Build or use the provided Shell IDList Array 
    /// (Note that we don't necessarily build the FileDrop structure)
    /// 5. if classification is FileSystem 
    /// 5a.   Store Shell IDList Array into DataObject, if not already there
    /// 5b.   Obtain COM IDataObject
    /// 6, Else for non-FileSystem classification
    /// 6a.   Obtain the IDataObject of the Virtual Folder  (A COM object)
    /// 6b.   Store Shell IDList Array into that Object
    /// 7. If no errors to this point, set m_IsValid to True
    /// 8. Done
    /// The class also contains a number of useful shared methods for dealing with
    /// IDataObject 
    /// </Summary>


    public partial class CProcDataObject
    {
        private IntPtr m_DataObject;          // The built ptr to COM IDataObject
        private ArrayList m_Draglist = new ArrayList();   // The built list of items in the original
        private bool m_IsValid = false;    // True once m_DataObject & m_Droplist are OK
        private MemoryStream m_StreamCIDA;    // A memorystream containing a CIDA
        private bool IsNet = false;        // True if dealing with a .Net DataObject
        private System.Windows.Forms.IDataObject NetIDO;
        private ShellDll.IDataObject IDO;

        public IntPtr DataObject => m_DataObject;
        public ArrayList DragList => m_Draglist;
        public bool IsValid => m_IsValid;

        /// <Summary>Constructor starting with a .Net Data object
        /// .Net DataObjects are easy to work with, but are useless
        /// if the Dragged items are non-FileSystem
        /// The DragWrapper class will never call this.
        /// It is here for playing around with the CDragWrapper class
        /// </Summary>
        public CProcDataObject(ref System.Windows.Forms.DataObject NetObject)
        {
            NetIDO = NetObject;
            ProcessNetDataObject(NetObject);
            if (m_IsValid)
            {
                try
                {
                    m_DataObject = Marshal.GetComInterfaceForObject(NetObject, Type.GetTypeFromCLSID(new Guid("0000010e-0000-0000-C000-000000000046"), true));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to get COM IDataObject:" + ex.ToString());
                    m_DataObject = IntPtr.Zero;
                    m_Draglist = new ArrayList();  // let GC clean em up
                    m_IsValid = false;
                }
            }
        }
        /// <Summary>This constructor takes a pointer to an IDataObject obtained from a
        /// IDropTarget Interface's DragEnter method.
        /// If the pointer points to a .Net DataObject (which only happens within the same app),
        /// convert it and call ProcessNetDataObject
        /// Otherwise, it from another app, possibly Win Explorer, so
        /// check it for the required formats and build m_DragList.
        /// Any error just quits, leaving m_IsValid as False ... Caller must check
        /// </Summary>
        public CProcDataObject(ref IntPtr pDataObj)   // Assumed to be a pointer to an IDataObject
        {
            // save it off -- DragWrapper class won't use it
            // CDragWrapper class may, but should, in this version, use the original
            m_DataObject = pDataObj;
            bool HadError = false;     // used for various error conditions
            try
            {
                IDO = (IDataObject)Marshal.GetTypedObjectForIUnknown(pDataObj, typeof(ShellDll.IDataObject));
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("Exception Thrown in CMyDataObject -Getting COM interface: " & vbCrLf & ex.ToString)
                HadError = true;
            }
            // If it is really a .Net IDataObject, then treat it as such
            if (HadError)
            {
                try
                {
                    NetIDO = (System.Windows.Forms.IDataObject)Marshal.GetTypedObjectForIUnknown(pDataObj, typeof(System.Windows.Forms.IDataObject));
                    IsNet = true;
                }
                catch
                {
                    IsNet = false;
                }
            }
            if (IsNet)
            {
                // Any error in ProcessNetDataObject will leave m_IsValid as False -- our only Error Indicator
                ProcessNetDataObject(NetIDO);
            }
            else    // IDataObject not from Net, Do it the hard way
            {
                if (HadError)
                    return; // can do no more
                ProcessCOMIDataObject(IDO);
                // It either worked or not.  m_IsValid is set accordingly, so we are done
            }
        }


        /// <Summary>Given an IDataObject from some non-net source, Build the 
        /// m_DragList ArrayList. 
        /// Also ensure that the IDataObject has "Shell IDList Array" formatted data
        /// If not, build it in m_StreamCIDA, if so, copy to m_StreamCIDA
        /// If dealing with all FileSystem Items,
        /// </Summary>
        private void ProcessCOMIDataObject(ShellDll.IDataObject IDO)
        {
            // Don't even look for an ArrayList. If there, we don't know how to access
            // Therefore, we need either a "FileDrop" or a "Shell IDList Array" to 
            // extract the info for m_DragList and to ensure that the IDataObject
            // actually has a "Shell IDList Array"

            int HR;       // general use response variable
            var fmtEtc = default(FORMATETC);
            var stg = default(STGMEDIUM);
            // ensure m_StreamCIDA is nothing  - will test for this later
            m_StreamCIDA = null;
            // First check for "Shell IDList Array" -- preferred in this case (and most others)
            int cf = RegisterClipboardFormat("Shell IDList Array");
            if (cf != 0)
            {
                fmtEtc.cfFormat = (CF)cf;
                fmtEtc.lindex = -1;
                fmtEtc.dwAspect = ShellDll.DVASPECT.CONTENT;
                fmtEtc.ptd = IntPtr.Zero;
                fmtEtc.Tymd = TYMED.HGLOBAL;
                stg.hGlobal = IntPtr.Zero;
                stg.pUnkForRelease = IntPtr.Zero;
                stg.tymed = (int)TYMED.HGLOBAL;
                HR = IDO.GetData(fmtEtc, stg);
                if (HR == 0)
                {
                    var cidaptr = Marshal.ReadIntPtr(stg.hGlobal);
                    m_StreamCIDA = MakeStreamFromCIDA(cidaptr);
                    MakeDragListFromCIDA();
                    ReleaseStgMedium(ref stg);       // done with this
                }
                else
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(HR);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            // Check for "FileDrop" (CF.HDROP) if we have to
            if (m_Draglist.Count < 1)     // skip this if already have list
            {
                fmtEtc.cfFormat = ShellDll.CF.HDROP;
                fmtEtc.lindex = -1;
                fmtEtc.dwAspect = ShellDll.DVASPECT.CONTENT;
                fmtEtc.ptd = IntPtr.Zero;
                fmtEtc.Tymd = TYMED.HGLOBAL;
                stg.hGlobal = IntPtr.Zero;
                stg.pUnkForRelease = IntPtr.Zero;
                stg.tymed = 0;
                HR = IDO.GetData(fmtEtc, stg);
                if (HR == 0)      // we have an HDROP and stg.hGlobal points to the info
                {
                    var pHdrop = Marshal.ReadIntPtr(stg.hGlobal);
                    int nrItems = DragQueryFile(pHdrop, -1, default, 0);
                    int i;
                    var loopTo = nrItems - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        int plen = DragQueryFile(pHdrop, i, default, 0);
                        var SB = new StringBuilder(plen + 1);
                        int flen = DragQueryFile(pHdrop, i, SB, plen + 1);
                        Debug.WriteLine("Fetched from HDROP: " + SB.ToString());
                        try  // if GetCShitem returns Nothing(it's failure marker) then catch it
                        {
                            m_Draglist.Add(CShItem.GetCShItem(SB.ToString()));
                        }
                        catch (Exception ex)  // in this case, just skip it
                        {
                            Debug.WriteLine("Error: CMyDataObject.ProcessComIDataObject - Adding via HDROP" + Constants.vbCrLf + Constants.vbTab + ex.Message);
                        }
                    }
                    ReleaseStgMedium(ref stg);  // done with this stg
                                            // Else
                                            // Marshal.ThrowExceptionForHR(HR)
                }
            }
            // Have done what we can to get m_DragList -- exit on failure
            if (m_Draglist.Count < 1)  // Can't do any more -- Quit
            {
                return;                  // leaving m_IsValid as False
            }

            // May not have Shell IDList Array in IDataObject.  If not, give it one
            if (m_StreamCIDA == null)  // IDO does not yet have "Shell IDList Array" Data
            {
                m_StreamCIDA = MakeShellIDArray(m_Draglist);
                if (m_StreamCIDA == null)
                    return; // failed to make it
                            // Now put the CIDA into the original IDataObject
                fmtEtc.cfFormat = (CF)cf;      // registered at routine entry
                fmtEtc.lindex = -1;
                fmtEtc.dwAspect = ShellDll.DVASPECT.CONTENT;
                fmtEtc.ptd = IntPtr.Zero;
                fmtEtc.Tymd = TYMED.HGLOBAL;
                // note the hGlobal item in stg is a pointer to a pointer->Data
                var m_hg = Marshal.AllocHGlobal(Convert.ToInt32(m_StreamCIDA.Length));
                Marshal.Copy(m_StreamCIDA.ToArray(), 0, m_hg, (int)m_StreamCIDA.Length);
                var hg = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(hg, 0, m_hg);
                stg.tymed = (int)TYMED.HGLOBAL;
                stg.hGlobal = hg;
                stg.pUnkForRelease = IntPtr.Zero;

                HR = IDO.SetData(fmtEtc, stg, true);   // callee responsible for releasing stg
                if (HR == 0)
                {
                    m_IsValid = true;
                }
                else   // failed -- we have to release stg  -- and leave m_IsValid as False
                {
                    ReleaseStgMedium(ref stg);
                    return;
                }        // m_isvalid stays False
            }
            m_IsValid = true;  // already had a Shell IDList Array, so all is OK
        }

        // Note: GetdataPresent is not reliable for IDataObject that did not originate 
        // from .Net. This rtn is called if it did, but we still don't use GetdataPresent.
        private void ProcessNetDataObject(System.Windows.Forms.IDataObject NetObject)
        {
            if (!(NetObject.GetData(typeof(ArrayList)) == null))
            {
                var AllowedType = typeof(CShItem);
                foreach (var oCSI in (ArrayList)NetObject.GetData(typeof(ArrayList)))
                {
                    if (!AllowedType.Equals(oCSI.GetType()))
                    {
                        m_Draglist = new ArrayList();
                        break;
                    }
                    else
                    {
                        m_Draglist.Add(oCSI);
                    }
                }
            }

            // Shell IDList Array is preferred to HDROP, see if we have one
            if (!(NetObject.GetData("Shell IDList Array") == null))
            {
                // Get it and also mark that we had one
                m_StreamCIDA = (MemoryStream)NetObject.GetData("Shell IDList Array");
                // has one, ASSUME that it matchs what we may have gotten from 
                // ArrayList, if we had one of those
                if (m_Draglist.Count < 1)    // if we didn't have an ArrayList, have to build m_DragList
                {
                    if (!MakeDragListFromCIDA())    // Could not make it
                    {
                        return;        // leaving m_IsValid as false
                    }
                }
            }
            // FileDrop is only used to build m_DragList if not already done
            if (m_Draglist.Count < 1)
            {
                if (!(NetObject.GetData("FileDrop") == null))
                {
                    foreach (string S in (ICollection)NetObject.GetData("FileDrop", true))
                    {
                        try   // if GetCShitem returns Nothing(it's failure marker) then catch it
                        {
                            m_Draglist.Add(CShItem.GetCShItem(S));
                        }
                        catch (Exception ex)   // Some problem, throw the whole thing away
                        {
                            Debug.WriteLine("CMyDataObject -- Error in creating CShItem for " + S + Constants.vbCrLf + "Error is: " + ex.ToString());
                            m_Draglist = new ArrayList();
                        }
                    }
                }
            }
            // At this point we must have a valid m_DragList
            if (m_Draglist.Count < 1)
                return; // no list, not valid

            // ensure that DataObject has a Shell IDList Array
            if (m_StreamCIDA == null)        // wouldn't be Nothing if it had one
            {
                m_StreamCIDA = MakeShellIDArray(m_Draglist);
                NetObject.SetData("Shell IDList Array", true, m_StreamCIDA);
            }
            // At this point, we have a valid DragList and have ensured that the DataObject
            // has a CIDA.  
            m_IsValid = true;
            return;

        }

        /// <Summary>
        /// Shell Folders prefer their IDragData to contain this format which is
        /// NOT directly supported by .Net.  The underlying structure is the CIDA structure
        /// which is basically VB and VB.Net Hostile.
        /// If "Make ShortCut(s) here" is the desired or
        /// POSSIBLE effect of the drag, then this format is REQUIRED -- otherwise the
        /// Folder will interpret the DragDropEffects.Link to be "Create Document Shortcut"
        /// which is NEVER the desired effect in this case
        /// The normal CIDA contains the Absolute PIDL of the source Folder and 
        /// Relative PIDLs for each Item in the Drag. 
        /// I cheat a bit an provide the Absolute PIDL of the Desktop (00, a short)
        /// and the Absolute PIDLs for the Items (all such Absolute PIDLS ar 
        /// relative to the Desktop.
        /// </Summary>
        /// <Credit>http://www.dotnetmonster.com/Uwe/Forum.aspx/dotnet-interop/3482/Drag-and-Drop
        /// The overall concept and much code taken from the above reference
        /// Dave Anderson's response, translated from C# to VB.Net, was the basis
        /// of this routine
        /// An AHA momemnt and a ref to the above url came from
        /// http://www.Planet-Source-Code.com/vb/scripts/ShowCode.asp?txtCodeId=61324%26lngWId=1
        /// 
        /// </Credit>
        public static MemoryStream MakeShellIDArray(ArrayList CSIList)
        {
            MemoryStream MakeShellIDArrayRet = default;
            // ensure that we have an arraylist of only CShItems
            var AllowedType = typeof(CShItem);
            foreach (var oCSI in CSIList)
            {
                if (!AllowedType.Equals(oCSI.GetType()))
                {
                    return null;
                }
            }
            // ensure at least one item
            if (CSIList.Count < 1)
                return null;

            // bArrays is an Array of Byte() each containing the bytes of a PIDL
            var bArrays = new object[CSIList.Count];
            int i = 0;
            foreach (CShItem CSI in CSIList)
            {
                bArrays[i] = new CShItem.cPidl(CSI.PIDL).PidlBytes;
                i += 1;
            }

            MakeShellIDArrayRet = new MemoryStream();
            var BW = new BinaryWriter(MakeShellIDArrayRet);

            BW.Write(Convert.ToUInt32(CSIList.Count));   // we don't count the parent (Desktop)
            var Desktop = default(int);  // we only use the lowval 2 bytes (VB lacks meaninful uint)
            int Offset;   // offset into Structure of a PIDL

            // Calculate and write the offset to each pidl (defined as an array of uint32)
            // The first pidl is 2 bytes long (0 0) and represents the desktop
            // The 2 in the statement below is for the offset to the 
            // folder pidl and the count field in the CIDA structure
            Offset = Marshal.SizeOf(typeof(uint)) * (bArrays.Length + 2);
            BW.Write(Convert.ToUInt32(Offset));       // offset to desktop pidl
            Offset += 2; // Marshal.SizeOf(GetType(UInt16)) 'point to the next one
            var loopTo = bArrays.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                BW.Write(Convert.ToUInt32(Offset));
                Offset += ((byte[])bArrays[i]).Length;
            }
            // done with the array of offsets, write the parent pidl (0 0) = Desktop

            // Write the pidl bytes
            BW.Write(Convert.ToUInt16(Desktop));
            foreach (byte[] b in bArrays)
                BW.Write(b);

            // done, returning the memorystream
            Debug.WriteLine("Done MakeShellIDArray");
            return MakeShellIDArrayRet;
        }

        /// <Summary>Given an IntPtr pointing to a CIDA,
        /// copy the CIDA to a new MemoryStream</Summary>
        private MemoryStream MakeStreamFromCIDA(IntPtr ptr)
        {
            MemoryStream MakeStreamFromCIDARet = default;
            MakeStreamFromCIDARet = null;    // assume failure
            if (ptr.Equals(IntPtr.Zero))
                return MakeStreamFromCIDARet;
            int nrItems = Marshal.ReadInt32(ptr, 0);
            if (!(nrItems > 0))
                return MakeStreamFromCIDARet;
            var offsets = new int[nrItems + 1];
            int curB = 4; // already read first 4
            int i;
            var loopTo = nrItems;
            for (i = 0; i <= loopTo; i++)
            {
                offsets[i] = Marshal.ReadInt32(ptr, curB);
                curB += 4;
            }
            var pidlLen = default(int);
            var pidlobjs = new object[nrItems + 1];
            var loopTo1 = nrItems;
            for (i = 0; i <= loopTo1; i++)
            {
                var ipt = new IntPtr(ptr.ToInt32() + offsets[i]);
                var cp = new CShItem.cPidl(ipt);
                pidlobjs[i] = cp.PidlBytes;
                pidlLen += ((byte[])pidlobjs[i]).Length;
            }
            MakeStreamFromCIDARet = new MemoryStream(pidlLen + 4 * offsets.Length + 4);
            var BW = new BinaryWriter(MakeStreamFromCIDARet);
            BW.Write(nrItems);
            var loopTo2 = nrItems;
            for (i = 0; i <= loopTo2; i++)
                BW.Write(offsets[i]);
            var loopTo3 = nrItems;
            for (i = 0; i <= loopTo3; i++)
                BW.Write((byte[])pidlobjs[i]);
            // DumpHex(MakeStreamFromCIDA.ToArray)
            MakeStreamFromCIDARet.Seek(0L, SeekOrigin.Begin);
            return MakeStreamFromCIDARet;
        }

        /// <Summary>Builds m_DragList from m_StreamCIDA</Summary>
        /// <returns> True if Successful, otherwise False</returns>
        private bool MakeDragListFromCIDA()
        {
            bool MakeDragListFromCIDARet = default;
            MakeDragListFromCIDARet = false;    // assume failure
            if (m_StreamCIDA == null)
                return MakeDragListFromCIDARet;
            var BR = new BinaryReader(m_StreamCIDA);
            var offsets = new int[BR.ReadInt32() + 1 + 1];   // 0=parent, last = total length
            offsets[offsets.Length - 1] = (int)BR.BaseStream.Length;
            int i;
            var loopTo = offsets.Length - 2;
            for (i = 0; i <= loopTo; i++)
                offsets[i] = BR.ReadInt32();
            var bArrays = new object[offsets.Length - 2 + 1];   // my objects are byte()
            var loopTo1 = bArrays.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                int thisLen = offsets[i + 1] - offsets[i];
                bArrays[i] = BR.ReadBytes(thisLen);
            }
            m_Draglist = new ArrayList();
            var loopTo2 = bArrays.Length - 1;
            for (i = 1; i <= loopTo2; i++)
            {
                bool isOK = true;
                try   // if GetCShitem returns Nothing(it's failure marker) then catch it
                {
                    m_Draglist.Add(CShItem.GetCShItem((byte[])bArrays[0], (byte[])bArrays[i]));
                }
                catch (Exception ex)
                {
                    Debug.Write("Error in making CShiTem from CIDA: " + ex.ToString());
                    isOK = false;
                }
                if (!isOK)
                    goto ERRXIT;
            }
            // on fall thru, all is done OK
            MakeDragListFromCIDARet = true;
            return MakeDragListFromCIDARet;

            // Error cleanup and Exit
            ERRXIT:;
            m_Draglist = new ArrayList();
            Debug.WriteLine("MakeDragListFromCIDA failed");
            return MakeDragListFromCIDARet;
        }
    }
}
