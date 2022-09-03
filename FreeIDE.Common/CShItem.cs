using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using static FreeIDE.Common.ShellDll;

namespace FreeIDE.Common
{
    public partial class CShItem : IDisposable, IComparable
    {
        // This class has occasion to refer to the TypeName as reported by
        // SHGetFileInfo. It needs to compare this to the string
        // (in English) "System Folder"
        // on non-English systems, we do not know, in the general case,
        // what the equivalent string is to compare against
        // The following variable is set by Sub New() to the string that
        // corresponds to "System Folder" on the current machine
        // Sub New() depends on the existance of My Computer(CSIDL.DRIVES),
        // to determine what the equivalent string is
        private static string m_strSystemFolder;

        // My Computer is also commonly used (though not internally),
        // so save & expose its name on the current machine
        private static string m_strMyComputer;

        // To get My Documents sorted first, we need to know the Locale 
        // specific name of that folder.
        private static string m_strMyDocuments;

        // The DesktopBase is set up via Sub New() (one time only) and
        // disposed of only when DesktopBase is finally disposed of
        private static CShItem DesktopBase;

        // We can avoid an extra SHGetFileInfo call once this is set up
        private static int OpenFolderIconIndex = -1;

        // It is also useful to know if the OS is XP or above.  
        // Set up in Sub New() to avoid multiple calls to find this info
        private static bool XPorAbove;
        // Likewise if OS is Win2K or Above
        private static bool Win2KOrAbove;

        // DragDrop, possibly among others, needs to know the Path of
        // the DeskTopDirectory in addition to the Desktop itself
        // Also need the actual CShItem for the DeskTopDirectory, so get it
        private static CShItem m_DeskTopDirectory;

        // Keep the local System Name for IsRemote testing
        private static string SystemName;                              // 4/14/2012
                                                                       // Keep list of Drives and their DriveType for IsRemote testing
        private static Dictionary<string, bool> DriveDict = new Dictionary<string, bool>();   // 4/16/2012


        // m_Folder and m_Pidl must be released/freed at Dispose time
        private IShellFolder m_Folder;    // if item is a folder, contains the Folder interface for this instance
        private IntPtr m_Pidl;            // The Absolute PIDL for this item (not retained for files)
        private string m_DisplayName = "";
        private string m_Path;
        private string m_TypeName;
        private CShItem m_Parent; // = Nothing
        private int m_IconIndexNormal;   // index into the System Image list for Normal icon
        private int m_IconIndexOpen; // index into the SystemImage list for Open icon
        private bool m_IsBrowsable;
        private bool m_IsFileSystem;
        private bool m_IsFolder;
        private bool m_HasSubFolders;
        private bool m_IsLink;
        private bool m_IsDisk;
        private bool m_IsShared;
        private bool m_IsHidden;
        private bool m_IsNetWorkDrive; // = False
        private bool m_IsRemovable; // = False
        private bool m_IsReadOnly; // = False
                                   // Properties of interest to Drag Operations
        private bool m_CanMove; // = False
        private bool m_CanCopy; // = False
        private bool m_CanDelete; // = False
        private bool m_CanLink; // = False
        private bool m_IsDropTarget; // = False
        private FileAttributes m_Attributes;    // Added 10/09/2011 'True FileAttributes from FileInfo
        private SFGAO m_SFGAO_Attributes;       // the original, returned from GetAttributesOf Added 10/09/2011 
        private bool m_IsRemote;           // 4/14/2012

        private int m_SortFlag; // = 0 'Used in comparisons

        private ArrayList m_Directories;

        // The following elements are only filled in on demand
        private bool m_XtrInfo; // = False
        private DateTime m_LastWriteTime;
        private DateTime m_CreationTime;
        private DateTime m_LastAccessTime;
        private long m_Length;

        // Indicates whether DisplayName, TypeName, SortFlag have been set up
        private bool m_HasDispType; // = False

        // Indicates whether IsReadOnly has been set up
        private bool m_IsReadOnlySetup; // = False

        // Holds a byte() representation of m_PIDL -- filled when needed
        private cPidl m_cPidl;

        // Flags for Dispose state
        private bool m_IsDisposing;
        private bool m_Disposed;


        /// <summary>
        /// Summary of Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off of the finalization queue
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Deallocates CoTaskMem contianing m_Pidl and removes reference to m_Folder
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // Allow your Dispose method to be called multiple times,
            // but throw an exception if the object has been disposed.
            // Whenever you do something with this class, 
            // check to see if it has been disposed.
            if (!m_Disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                m_Disposed = true;
                if (disposing)
                {
                }
                // Release unmanaged resources. If disposing is false,
                // only the following code is executed. 
                if (!(m_Folder == null))
                {
                    Marshal.ReleaseComObject(m_Folder);
                }
                if (!m_Pidl.Equals(IntPtr.Zero))
                {
                    Marshal.FreeCoTaskMem(m_Pidl);
                }
            }
            else
            {
                throw new Exception("CShItem Disposed more than once");
            }
        }

        // This Finalize method will run only if the 
        // Dispose method does not get called.
        // By default, methods are NotOverridable. 
        // This prevents a derived class from overriding this method.
        /// <summary>
        /// Summary of Finalize.
        /// </summary>
        ~CShItem()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        /// <summary>
        /// Private Constructor, creates new CShItem from the item's parent folder and
        /// the item's PIDL relative to that folder.</summary>
        /// <param name="folder">the folder interface of the parent</param>
        /// <param name="pidl">the Relative PIDL of this item</param>
        /// <param name="parent">the CShitem of the parent</param>
        private CShItem(IShellFolder folder, IntPtr pidl, CShItem parent)
        {
            if (DesktopBase == null)
            {
                DesktopBase = new CShItem(); // This initializes the Desktop folder
            }
            m_Parent = parent;
            m_Pidl = concatPidls(parent.PIDL, pidl);

            // Get some attributes
            SetUpAttributes(folder, pidl);

            // Set unfetched value for IconIndex....
            m_IconIndexNormal = -1;
            m_IconIndexOpen = -1;
            // finally, set up my Folder
            if (m_IsFolder)
            {
                int HR;
                HR = folder.BindToObject(pidl, IntPtr.Zero, IID_IShellFolder, m_Folder);
                if (HR != NOERROR)
                {
                    Marshal.ThrowExceptionForHR(HR);
                }
            }
        }

        /// <summary>
        /// Private Constructor. Creates CShItem of the Desktop
        /// </summary>
        private CShItem()           // only used when desktopfolder has not been intialized
        {
            if (!(DesktopBase == null))
            {
                throw new Exception("Attempt to initialize CShItem for second time");
            }

            int HR;
            // firstly determine what the local machine calls a "System Folder" and "My Computer"
            IntPtr tmpPidl = default;
            HR = SHGetSpecialFolderLocation(0, (int)CSIDL.DRIVES, ref tmpPidl);
            var shfi = new SHFILEINFO();
            int dwflag = (int)(SHGFI.DISPLAYNAME | SHGFI.TYPENAME | SHGFI.PIDL);

            int dwAttr = 0;
            SHGetFileInfo(tmpPidl, dwAttr, ref shfi, cbFileInfo, dwflag);
            m_strSystemFolder = shfi.szTypeName;
            m_strMyComputer = shfi.szDisplayName;
            Marshal.FreeCoTaskMem(tmpPidl);
            // set OS version info
            XPorAbove = ShellDll.IsXpOrAbove();
            Win2KOrAbove = ShellDll.Is2KOrAbove();

            // With That done, now set up Desktop CShItem
            m_Path = ("::{" + DesktopGUID.ToString() + "}");
            m_IsFolder = true;
            m_HasSubFolders = true;
            m_IsBrowsable = false;
            HR = SHGetDesktopFolder(ref m_Folder);
            m_Pidl = GetSpecialFolderLocation(IntPtr.Zero, (int)CSIDL.DESKTOP);
            dwflag = (int)(SHGFI.DISPLAYNAME | SHGFI.TYPENAME | SHGFI.SYSICONINDEX | SHGFI.PIDL);


            dwAttr = 0;
            IntPtr H = SHGetFileInfo(m_Pidl, dwAttr, ref shfi, cbFileInfo, dwflag);
            m_DisplayName = shfi.szDisplayName;
            m_TypeName = strSystemFolder;   // not returned correctly by SHGetFileInfo
            m_IconIndexNormal = shfi.iIcon;
            m_IconIndexOpen = shfi.iIcon;
            m_HasDispType = true;
            m_IsDropTarget = true;
            m_IsReadOnly = false;
            m_IsReadOnlySetup = true;

            // also get local name for "My Documents"
            var pchEaten = default(int);
            tmpPidl = IntPtr.Zero;
            HR = m_Folder.ParseDisplayName(default, default, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", pchEaten, tmpPidl, default);
            shfi = new SHFILEINFO();
            dwflag = (int)(SHGFI.DISPLAYNAME | SHGFI.TYPENAME | SHGFI.PIDL);

            dwAttr = 0;
            SHGetFileInfo(tmpPidl, dwAttr, ref shfi, cbFileInfo, dwflag);
            m_strMyDocuments = shfi.szDisplayName;
            Marshal.FreeCoTaskMem(tmpPidl);
            // this must be done after getting "My Documents" string
            m_SortFlag = ComputeSortFlag();
            // Set DesktopBase
            DesktopBase = this;

            SystemName = Environment.MachineName;    // 4/14/2012
                                                     // Lastly, get the Path and CShItem of the DesktopDirectory -- useful for DragDrop
            m_DeskTopDirectory = new CShItem(CSIDL.DESKTOPDIRECTORY);
        }

        /// <summary>Create instance based on a non-desktop CSIDL.
        /// Will create based on any CSIDL Except the DeskTop CSIDL</summary>
        /// <param name="ID">Value from CSIDL enumeration denoting the folder to create this CShItem of.</param>
        public CShItem(CSIDL ID)
        {
            if (DesktopBase == null)
            {
                DesktopBase = new CShItem(); // This initializes the Desktop folder
            }
            int HR;
            if (ID == CSIDL.MYDOCUMENTS)
            {
                var pchEaten = default(int);
                HR = DesktopBase.m_Folder.ParseDisplayName(default, default, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", pchEaten, m_Pidl, default);
            }
            else
            {
                HR = SHGetSpecialFolderLocation(0, (int)ID, ref m_Pidl);
            }
            if (HR == NOERROR)
            {
                IShellFolder pParent;
                var relPidl = IntPtr.Zero;

                pParent = GetParentOf(m_Pidl, ref relPidl);
                // Get the Attributes
                SetUpAttributes(pParent, relPidl);
                // Set unfetched value for IconIndex....
                m_IconIndexNormal = -1;
                m_IconIndexOpen = -1;
                // finally, set up my Folder
                if (m_IsFolder)
                {
                    HR = pParent.BindToObject(relPidl, IntPtr.Zero, IID_IShellFolder, m_Folder);
                    if (HR != NOERROR)
                    {
                        Marshal.ThrowExceptionForHR(HR);
                    }
                }
                Marshal.ReleaseComObject(pParent);
                // if PidlCount(m_Pidl) = 1 then relPidl is same as m_Pidl, don't release
                if (PidlCount(m_Pidl) > 1)
                    Marshal.FreeCoTaskMem(relPidl);
            }
            else
            {
                Marshal.ThrowExceptionForHR(HR);
            }
        }

        /// <summary>Create a new CShItem based on a Path Must be a valid FileSystem Path</summary>
        /// <param name="path"></param>
        public CShItem(string path)
        {
            if (DesktopBase == null)
            {
                DesktopBase = new CShItem(); // This initializes the Desktop folder
            }
            // Removal of following code allows Path(GUID) of Special FOlders to serve
            // as a valid Path for CShItem creation (part of Calum's refresh code needs this
            // If Not Directory.Exists(path) AndAlso Not File.Exists(path) Then
            // Throw New Exception("CShItem -- Invalid Path specified")
            // End If
            int HR;
            HR = DesktopBase.m_Folder.ParseDisplayName(0, IntPtr.Zero, path, 0, m_Pidl, 0);
            if (!(HR == NOERROR))
                Marshal.ThrowExceptionForHR(HR);
            IShellFolder pParent;
            var relPidl = IntPtr.Zero;

            pParent = GetParentOf(m_Pidl, ref relPidl);

            // Get the Attributes
            SetUpAttributes(pParent, relPidl);
            // Set unfetched value for IconIndex....
            m_IconIndexNormal = -1;
            m_IconIndexOpen = -1;
            // finally, set up my Folder
            if (m_IsFolder)
            {
                HR = pParent.BindToObject(relPidl, IntPtr.Zero, IID_IShellFolder, m_Folder);
                if (HR != NOERROR)
                {
                    Marshal.ThrowExceptionForHR(HR);
                }
            }
            Marshal.ReleaseComObject(pParent);
            // if PidlCount(m_Pidl) = 1 then relPidl is same as m_Pidl, don't release
            if (PidlCount(m_Pidl) > 1)
            {
                Marshal.FreeCoTaskMem(relPidl);
            }
        }

        /// <Summary>Given a Byte() containing the Pidl of the parent
        /// folder and another Byte() containing the Pidl of the Item,
        /// relative to the Folder, Create a CShItem for the Item.
        /// This is of primary use in dealing with "Shell IDList Array" 
        /// formatted info passed in a Drag Operation
        /// </Summary>
        public CShItem(byte[] FoldBytes, byte[] ItemBytes)
        {
            Debug.WriteLine("CShItem.New(FoldBytes,ItemBytes) Fold len= " + FoldBytes.Length + " Item Len = " + ItemBytes.Length);
            if (DesktopBase == null) DesktopBase = new CShItem(); // This initializes the Desktop folder

            IntPtr ipParent = cPidl.BytesToPidl(FoldBytes);
            IntPtr ipItem = cPidl.BytesToPidl(ItemBytes);
            if (ipParent.Equals(IntPtr.Zero) | ipItem.Equals(IntPtr.Zero)) goto XIT;

            IShellFolder pParent = MakeFolderFromBytes(FoldBytes);
            if (pParent == null) goto XIT;    // m_PIDL will = IntPtr.Zero for really bad CShitem

            // Now process just like sub new(folder,pidl,parent) version
            m_Pidl = concatPidls(ipParent, ipItem);

            // Get some attributes
            SetUpAttributes(pParent, ipItem);

            // Set unfetched value for IconIndex....
            m_IconIndexNormal = -1;
            m_IconIndexOpen = -1;
            // finally, set up my Folder
            if (m_IsFolder)
            {
                int HR = pParent.BindToObject(ipItem, IntPtr.Zero, IID_IShellFolder, m_Folder);
                if (HR != NOERROR)
                {
                    Marshal.ThrowExceptionForHR(HR);
                }
            }
            XIT:;
            if (ipParent != null) Marshal.FreeCoTaskMem(ipParent);
            if (ipItem != null) Marshal.FreeCoTaskMem(ipItem);
        }

        /// <Summary>It is impossible to validate a PIDL completely since its contents
        /// are arbitrarily defined by the creating Shell Namespace.  However, it
        /// is possible to validate the structure of a PIDL.</Summary>
        public static bool IsValidPidl(byte[] b)
        {
            bool IsValidPidlRet = false;
            int bMax = b.Length - 1;   // max value that index can have
            if (bMax < 1) return IsValidPidlRet; // min size is 2 bytes
            int cb = b[0] + b[1] * 256;
            int indx = 0;
            while (cb > 0)
            {
                if (indx + cb + 1 > bMax) return IsValidPidlRet; // an error
                indx += cb;
                cb = b[indx] + b[indx + 1] * 256;
            }
            // on fall thru, it is ok as far as we can check
            IsValidPidlRet = true;
            return IsValidPidlRet;
        }

        public static ShellDll.IShellFolder MakeFolderFromBytes(byte[] b)
        {
            ShellDll.IShellFolder MakeFolderFromBytesRet = default;
            if (!IsValidPidl(b)) return default;
            if (b.Length == 2 && b[0] == 0 & b[1] == 0) // this is the desktop
            {
                return DesktopBase.Folder;
            }
            else if (b.Length == 0)   // Also indicates the desktop
            {
                return DesktopBase.Folder;
            }
            else
            {
                var ptr = Marshal.AllocCoTaskMem(b.Length);
                if (ptr.Equals(IntPtr.Zero)) return default;
                Marshal.Copy(b, 0, ptr, b.Length);
                // the next statement assigns a IshellFolder object to the function return, or has an error
                int hr = DesktopBase.Folder.BindToObject(ptr, IntPtr.Zero, IID_IShellFolder, MakeFolderFromBytesRet);
                if (hr != 0) MakeFolderFromBytesRet = default;
                Marshal.FreeCoTaskMem(ptr);
            }

            return MakeFolderFromBytesRet;
        }

        /// <Summary>Returns both the IShellFolder interface of the parent folder
        /// and the relative pidl of the input PIDL</Summary>
        /// <remarks>Several internal functions need this information and do not have
        /// it readily available. GetParentOf serves those functions</remarks>
        private static IShellFolder GetParentOf(IntPtr pidl, ref IntPtr relPidl)
        {
            IShellFolder GetParentOfRet = default;
            int HR;
            int itemCnt = PidlCount(pidl);
            if (itemCnt == 1)         // parent is desktop
            {
                HR = SHGetDesktopFolder(ref GetParentOfRet);
                relPidl = pidl;
            }
            else
            {
                IntPtr tmpPidl;
                tmpPidl = TrimPidl(pidl, ref relPidl);
                HR = DesktopBase.m_Folder.BindToObject(tmpPidl, IntPtr.Zero, IID_IShellFolder, GetParentOfRet);
                Marshal.FreeCoTaskMem(tmpPidl);
            }
            if (!(HR == NOERROR)) Marshal.ThrowExceptionForHR(HR);
            return GetParentOfRet;
        }

        /// <summary>Get the base attributes of the folder/file that this CShItem represents</summary>
        /// <param name="folder">Parent Folder of this Item</param>
        /// <param name="pidl">Relative Pidl of this Item.</param>
        private void SetUpAttributes(IShellFolder folder, IntPtr pidl)
        {
            SFGAO attrFlag = SFGAO.BROWSABLE;
            attrFlag = attrFlag | SFGAO.FILESYSTEM;
            // attrFlag = attrFlag Or SFGAO.HASSUBFOLDER   'made into an on-demand attribute
            attrFlag = attrFlag | SFGAO.FOLDER;
            attrFlag = attrFlag | SFGAO.LINK;
            attrFlag = attrFlag | SFGAO.SHARE;
            attrFlag = attrFlag | SFGAO.HIDDEN;
            attrFlag = attrFlag | SFGAO.REMOVABLE;
            // attrFlag = attrFlag Or SFGAO.RDONLY   'made into an on-demand attribute
            attrFlag = attrFlag | SFGAO.CANCOPY;
            attrFlag = attrFlag | SFGAO.CANDELETE;
            attrFlag = attrFlag | SFGAO.CANLINK;
            attrFlag = attrFlag | SFGAO.CANMOVE;
            attrFlag = attrFlag | SFGAO.DROPTARGET;
            // Note: for GetAttributesOf, we must provide an array, in  all cases with 1 element
            var aPidl = new IntPtr[1];
            aPidl[0] = pidl;
            if (folder != null) folder.GetAttributesOf(1, aPidl, ref attrFlag);
            m_SFGAO_Attributes = attrFlag;
            m_IsBrowsable = Convert.ToBoolean(attrFlag & SFGAO.BROWSABLE);
            m_IsFileSystem = Convert.ToBoolean(attrFlag & SFGAO.FILESYSTEM);
            // m_HasSubFolders = CBool(attrFlag And SFGAO.HASSUBFOLDER)  'made into an on-demand attribute
            m_IsFolder = Convert.ToBoolean(attrFlag & SFGAO.FOLDER);
            m_IsLink = Convert.ToBoolean(attrFlag & SFGAO.LINK);
            m_IsShared = Convert.ToBoolean(attrFlag & SFGAO.SHARE);
            m_IsHidden = Convert.ToBoolean(attrFlag & SFGAO.HIDDEN);
            m_IsRemovable = Convert.ToBoolean(attrFlag & SFGAO.REMOVABLE);
            // m_IsReadOnly = CBool(attrFlag And SFGAO.RDONLY)      'made into an on-demand attribute
            m_CanCopy = Convert.ToBoolean(attrFlag & SFGAO.CANCOPY);
            m_CanDelete = Convert.ToBoolean(attrFlag & SFGAO.CANDELETE);
            m_CanLink = Convert.ToBoolean(attrFlag & SFGAO.CANLINK);
            m_CanMove = Convert.ToBoolean(attrFlag & SFGAO.CANMOVE);
            m_IsDropTarget = Convert.ToBoolean(attrFlag & SFGAO.DROPTARGET);

            // Get the Path
            var strr = Marshal.AllocCoTaskMem(MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            var buf = new StringBuilder(MAX_PATH);
            SHGDN itemflags = SHGDN.FORPARSING;
            if (folder != null) folder.GetDisplayNameOf(pidl, itemflags, strr);
            int HR = StrRetToBuf(strr, pidl, buf, MAX_PATH);
            Marshal.FreeCoTaskMem(strr); // now done with it
            if (HR == NOERROR)
            {
                m_Path = buf.ToString();
                if (m_IsFolder && m_IsFileSystem && XPorAbove)
                {
                    aPidl[0] = pidl;
                    attrFlag = SFGAO.STREAM;
                    if (folder != null) folder.GetAttributesOf(1, aPidl, attrFlag);
                    if (Convert.ToBoolean(attrFlag & SFGAO.STREAM)) m_IsFolder = false;
                }
                if (m_Path.Length == 3 && m_Path.Substring(1).Equals(@":\"))
                {
                    m_IsDisk = true;
                    try                 // 04/16/2012 Entire Try Block
                    {
                        var disk = new System.Management.ManagementObject("win32_logicaldisk.deviceid=\"" + Path.Substring(0, 2) + "\"");
                        m_Length = (long)disk.GetPropertyValue("Size");
                        if ((((uint)disk.GetPropertyValue("DriveType")).ToString()) == (4.ToString()))
                        {
                            m_IsNetWorkDrive = true;
                            m_IsRemote = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Disconnected Network Drives etc. will generate 
                        // an error here, just assume that it is a network
                        // drive
                        m_IsNetWorkDrive = true;
                        m_IsRemote = true;
                    }
                    finally
                    {
                        m_XtrInfo = true;
                        if (!DriveDict.ContainsKey(m_Path))
                        {
                            DriveDict.Add(m_Path, m_IsRemote);
                            Debug.WriteLine(m_Path + " " + DriveDict[m_Path].ToString());
                        }
                    }
                }

                // Setup IsRemote             '4/14/2012
                if (!(m_IsDisk || m_Path.StartsWith("::")))
                {
                    string ItemRoot = System.IO.Path.GetPathRoot(m_Path);
                    if (!(ItemRoot.Length == 3 && ItemRoot.Substring(1).Equals(@":\")))
                    {
                        var tmp = ItemRoot.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length > 0 && tmp[0].Equals(SystemName, StringComparison.InvariantCultureIgnoreCase)) m_IsRemote = false;
                        else m_IsRemote = true;
                    }
                    else if (DriveDict.ContainsKey(ItemRoot) && DriveDict[ItemRoot]) m_IsRemote = true; // 4/16/2012
                }
            }
            else
            {
                Marshal.ThrowExceptionForHR(HR);
            }
        }


        public static CShItem GetCShItem(string path)
        {
            CShItem GetCShItemRet = default;
            GetCShItemRet = null;    // assume failure
            int HR;
            IntPtr tmpPidl = default;
            HR = GetDeskTop().Folder.ParseDisplayName(0, IntPtr.Zero, path, 0, tmpPidl, 0);
            if (HR == 0)
            {
                GetCShItemRet = FindCShItem(tmpPidl);
                if (GetCShItemRet == null)
                {
                    try
                    {
                        GetCShItemRet = new CShItem(path);
                    }
                    catch
                    {
                        GetCShItemRet = null;
                    }
                }
            }
            if (!tmpPidl.Equals(IntPtr.Zero))  Marshal.FreeCoTaskMem(tmpPidl);

            return GetCShItemRet;
        }

        public static CShItem GetCShItem(CSIDL ID)
        {
            CShItem GetCShItemRet = default;
            GetCShItemRet = null;      // avoid VB2005 Warning
            if (ID == CSIDL.DESKTOP) return GetDeskTop();

            int HR;
            IntPtr tmpPidl = default;
            if (ID == CSIDL.MYDOCUMENTS)
            {
                var pchEaten = default(int);
                HR = GetDeskTop().Folder.ParseDisplayName(default, default, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", pchEaten, ref tmpPidl, default);
            }
            else
            {
                HR = SHGetSpecialFolderLocation(0, (int)ID, ref tmpPidl);
            }

            if (HR == NOERROR)
            {
                GetCShItemRet = FindCShItem(tmpPidl);
                if (GetCShItemRet == null)
                {
                    try
                    {
                        GetCShItemRet = new CShItem(ID);
                    }
                    catch
                    {
                        GetCShItemRet = null;
                    }
                }
            }
            if (!tmpPidl.Equals(IntPtr.Zero)) Marshal.FreeCoTaskMem(tmpPidl);

            return GetCShItemRet;
        }

        public static CShItem GetCShItem(byte[] FoldBytes, byte[] ItemBytes)
        {
            CShItem GetCShItemRet = default;
            GetCShItemRet = null; // assume failure
            var b = cPidl.JoinPidlBytes(FoldBytes, ItemBytes);
            if (b == null) return GetCShItemRet; // can do no more with invalid pidls
                                      // otherwise do like below, skipping unnecessary validation check
            var thisPidl = Marshal.AllocCoTaskMem(b.Length);
            if (thisPidl.Equals(IntPtr.Zero)) return null;
            Marshal.Copy(b, 0, thisPidl, b.Length);
            GetCShItemRet = FindCShItem(thisPidl);
            Marshal.FreeCoTaskMem(thisPidl);
            if (GetCShItemRet == null)   // didn't find it, make new
            {
                try
                {
                    GetCShItemRet = new CShItem(FoldBytes, ItemBytes);
                }
                catch
                { 

                }
            }
            if (GetCShItemRet.PIDL.Equals(IntPtr.Zero))
                GetCShItemRet = null;
            return GetCShItemRet;
        }

        public static CShItem FindCShItem(byte[] b)
        {
            CShItem FindCShItemRet = default;
            if (!IsValidPidl(b)) return null;
            var thisPidl = Marshal.AllocCoTaskMem(b.Length);
            if (thisPidl.Equals(IntPtr.Zero)) return null;
            Marshal.Copy(b, 0, thisPidl, b.Length);
            FindCShItemRet = FindCShItem(thisPidl);
            Marshal.FreeCoTaskMem(thisPidl);
            return FindCShItemRet;
        }

        public static CShItem FindCShItem(IntPtr ptr)
        {
            CShItem FindCShItemRet = default;
            FindCShItemRet = null;     // avoid VB2005 Warning
            var BaseItem = GetDeskTop();
            CShItem CSI;
            bool FoundIt = false;      // True if we found item or an ancestor
            while (!FoundIt)
            {
                foreach (CShItem currentCSI in BaseItem.GetDirectories())
                {
                    CSI = currentCSI;
                    if (IsAncestorOf(CSI.PIDL, ptr))
                    {
                        if (IsEqual(CSI.PIDL, ptr))  // we found the desired item
                        {
                            return CSI;
                        }
                        else
                        {
                            BaseItem = CSI;
                            FoundIt = true;
                            break;
                        }
                    }
                }
                if (!FoundIt) return null; // didn't find an ancestor
                                 // The complication is that the desired item may not be a directory
                if (!IsAncestorOf(BaseItem.PIDL, ptr, true))  // Don't have immediate ancestor
                {
                    FoundIt = false;     // go around again
                }
                else
                {
                    foreach (CShItem currentCSI1 in BaseItem.GetItems())
                    {
                        CSI = currentCSI1;
                        if (IsEqual(CSI.PIDL, ptr))
                        {
                            return CSI;
                        }
                    }
                    // fall thru here means it doesn't exist or we can't find it because of funny PIDL from SHParseDisplayName
                    return null;
                }
            }

            return FindCShItemRet;
        }

        /// <summary>Computes the Sort key of this CShItem, based on its attributes</summary>
        private int ComputeSortFlag()
        {
            int rVal = 0;
            if (m_IsDisk) rVal = 0x100000;
            if (m_TypeName.Equals(strSystemFolder))
            {
                if (!m_IsBrowsable)
                {
                    rVal = rVal | 0x10000;
                    if (m_strMyDocuments.Equals(m_DisplayName)) rVal = rVal | 0x1;
                }
                else
                {
                    rVal = rVal | 0x1000;
                }
            }
            if (m_IsFolder) rVal = rVal | 0x100;
            return rVal;
        }

        /// <Summary> CompareTo(obj as object)
        /// Compares obj to this instance based on SortFlag-- obj must be a CShItem</Summary>
        /// <SortOrder>  (low)Disks,non-browsable System Folders,
        /// browsable System Folders, 
        /// Directories, Files, Nothing (high)</SortOrder>
        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1; // non-existant is always low
            CShItem Other = (CShItem)obj;
            if (!m_HasDispType) SetDispType();

            int cmp = Other.SortFlag - m_SortFlag; // Note the reversal
            if (cmp != 0) return cmp;
            else if (m_IsDisk) return string.Compare(m_Path, Other.Path);// implies that both are
            else return StringLogicalComparer.CompareStrings(m_DisplayName, Other.DisplayName);
        }

        public static string strMyComputer => m_strMyComputer;
        public static string strSystemFolder => m_strSystemFolder;
        public static string DesktopDirectoryPath => m_DeskTopDirectory.Path;
        public IntPtr PIDL => m_Pidl;
        public IShellFolder Folder => m_Folder;
        public string Path =>  m_Path;
        public CShItem Parent => m_Parent;

        /// <summary>
        /// This instance's Shell Attributes as returned by Folder.GetAttributesOf
        /// </summary>
        /// <returns>This instance's Shell Attributes as returned by Folder.GetAttributesOf</returns>
        /// <remarks>Internal use only</remarks>
        internal SFGAO SFGAO_Attributes => m_SFGAO_Attributes;
        public bool IsBrowsable => m_IsBrowsable;
        public bool IsFileSystem => m_IsFileSystem;
        public bool IsFolder => m_IsFolder;
        private bool m_HasSubFoldersSetup;

        /// <summary>
        /// True if item is a Folder and has sub-Folders
        /// </summary>
        /// <returns>True if item is a Folder and has sub-Folders, False otherwise</returns>
        /// <remarks>Modified to make this attribute behave (with respect to Remote Folders) like XP, even on Vista/Win7.
        /// That is, any Remote Folder is reported as HasSubFolders = True. Local Folders are tested with the API call.
        /// On Vista/Win7, Compressed files (eg - .Zip, .Cab, etc) are considered sub Folders by this Property.
        /// This behavior is NOT modified to behave like XP.</remarks>
        public bool HasSubFolders
        {
            get
            {
                // Return Me.Directories.Length > 0  'new code(12/11/09) since removed.
                if (m_HasSubFoldersSetup)
                {
                    return m_HasSubFolders;
                }
                else if (m_IsRemote)          // 4/14/2012
                {
                    m_HasSubFolders = true;      // 4/14/2012
                    m_HasSubFoldersSetup = true; // 4/14/2012
                }
                else
                {
                    var shfi = new SHFILEINFO();
                    shfi.dwAttributes = (int)SFGAO.HASSUBFOLDER;
                    SHGFI dwflag = SHGFI.PIDL | SHGFI.ATTRIBUTES | SHGFI.ATTR_SPECIFIED;

                    int dwAttr = 0;
                    IntPtr H = SHGetFileInfo(m_Pidl, dwAttr, ref shfi, cbFileInfo, (int)dwflag);
                    if (H.ToInt32() != NOERROR && H.ToInt32() != 1) Marshal.ThrowExceptionForHR(H.ToInt32());
                    m_HasSubFolders = Convert.ToBoolean(shfi.dwAttributes & (int)SFGAO.HASSUBFOLDER);
                    m_SFGAO_Attributes = (SFGAO)(m_SFGAO_Attributes | (SFGAO)shfi.dwAttributes & SFGAO.HASSUBFOLDER);
                    m_HasSubFoldersSetup = true;
                }
                return m_HasSubFolders;         // old code reverted to (12/12/09)
            }
        }
        public bool IsDisk => m_IsDisk;
        public bool IsLink => m_IsLink;
        public bool IsShared => m_IsShared;
        public bool IsHidden => m_IsHidden;
        public bool IsRemovable => m_IsRemovable;
        public bool IsRemote => m_IsRemote;

        public bool CanMove => m_CanMove;
        public bool CanCopy => m_CanCopy;
        public bool CanDelete => m_CanDelete;
        public bool CanLink => m_CanLink;
        public bool IsDropTarget => m_IsDropTarget;

        /// <summary>
        /// Set DisplayName, TypeName, and SortFlag when actually needed
        /// </summary>
        private void SetDispType()
        {
            // Get Displayname, TypeName
            var shfi = new SHFILEINFO();
            int dwflag = (int)(SHGFI.DISPLAYNAME | SHGFI.TYPENAME | SHGFI.PIDL);

            int dwAttr = 0;
            if (m_IsFileSystem & !m_IsFolder)
            {
                dwflag = dwflag | (int)SHGFI.USEFILEATTRIBUTES;
                dwAttr = FILE_ATTRIBUTE_NORMAL;
            }
            IntPtr H = SHGetFileInfo(m_Pidl, dwAttr, ref shfi, cbFileInfo, dwflag);
            m_DisplayName = shfi.szDisplayName;
            m_TypeName = shfi.szTypeName;

            if (m_DisplayName.Equals("")) m_DisplayName = m_Path;

            m_SortFlag = ComputeSortFlag();
            m_HasDispType = true;
        }

        public string DisplayName
        {
            get
            {
                if (!m_HasDispType) SetDispType();
                return m_DisplayName;
            }
        }

        private int SortFlag
        {
            get
            {
                if (!m_HasDispType) SetDispType();
                return m_SortFlag;
            }
        }

        public string TypeName
        {
            get
            {
                if (!m_HasDispType) SetDispType();
                return m_TypeName;
            }
        }

        public int IconIndexNormal
        {
            get
            {
                if (m_IconIndexNormal < 0)
                {
                    if (!m_HasDispType) SetDispType();
                    SHFILEINFO shfi = new SHFILEINFO();
                    int dwflag = (int)(SHGFI.PIDL | SHGFI.SYSICONINDEX);
                    int dwAttr = 0;
                    if (m_IsFileSystem & !m_IsFolder)
                    {
                        dwflag = dwflag | (int)SHGFI.USEFILEATTRIBUTES;
                        dwAttr = FILE_ATTRIBUTE_NORMAL;
                    }
                    IntPtr H = SHGetFileInfo(m_Pidl, dwAttr, ref shfi, cbFileInfo, dwflag);
                    m_IconIndexNormal = shfi.iIcon;
                }
                return m_IconIndexNormal;
            }
        }
        // IconIndexOpen is Filled on demand
        public int IconIndexOpen
        {
            get
            {
                if (m_IconIndexOpen < 0)
                {
                    if (!m_HasDispType) SetDispType();
                    if (!m_IsDisk & m_IsFileSystem & m_IsFolder)
                    {
                        if (OpenFolderIconIndex < 0)
                        {
                            int dwflag = (int)(SHGFI.SYSICONINDEX | SHGFI.PIDL);
                            var shfi = new SHFILEINFO();
                            IntPtr H = SHGetFileInfo(m_Pidl, 0, ref shfi, cbFileInfo, (dwflag | (int)SHGFI.OPENICON));

                            m_IconIndexOpen = shfi.iIcon;
                        }
                        else
                        {
                            m_IconIndexOpen = OpenFolderIconIndex;
                        }
                    }
                    else
                    {
                        m_IconIndexOpen = m_IconIndexNormal;
                    }
                }
                return m_IconIndexOpen;
            }
        }

        /// <summary>
        /// Obtains information available from FileInfo.
        /// </summary>
        private void FillDemandInfo()
        {
            if (!m_IsDisk & m_IsFileSystem & !m_IsFolder)
            {
                // in this case, it's a file
                if (File.Exists(m_Path))
                {
                    var fi = new FileInfo(m_Path);
                    m_LastWriteTime = fi.LastWriteTime;
                    m_LastAccessTime = fi.LastAccessTime;
                    m_CreationTime = fi.CreationTime;
                    m_Length = fi.Length;
                    m_Attributes = fi.Attributes;          // Added 10/09/2011
                    m_XtrInfo = true;
                }
            }
            else if (m_IsFileSystem & m_IsFolder)
            {
                if (Directory.Exists(m_Path))
                {
                    var di = new DirectoryInfo(m_Path);
                    m_LastWriteTime = di.LastWriteTime;
                    m_LastAccessTime = di.LastAccessTime;
                    m_CreationTime = di.CreationTime;
                    m_Attributes = di.Attributes;          // Added 10/09/2011
                    m_XtrInfo = true;
                }
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                if (!m_XtrInfo) FillDemandInfo();
                return m_LastWriteTime;
            }
        }
        public DateTime LastAccessTime
        {
            get
            {
                if (!m_XtrInfo) FillDemandInfo();
                return m_LastAccessTime;
            }
        }
        public DateTime CreationTime
        {
            get
            {
                if (!m_XtrInfo) FillDemandInfo();
                return m_CreationTime;
            }
        }
        public long Length
        {
            get
            {
                if (!m_XtrInfo) FillDemandInfo();
                return m_Length;
            }
        }
        /// <summary>
        /// This instance's Attributes as returned by FileSystemInfo
        /// </summary>
        /// <returns>This instance's Attributes as returned by FileSystemInfo</returns>
        /// <remarks></remarks>
        public FileAttributes Attributes
        {
            get
            {
                if (!m_XtrInfo) FillDemandInfo();
                return m_Attributes;
            }
        }
        public bool IsNetworkDrive
        {
            get
            {
                if (!m_XtrInfo) FillDemandInfo();
                return m_IsNetWorkDrive;
            }
        }

        public cPidl clsPidl
        {
            get
            {
                if (m_cPidl == null) m_cPidl = new cPidl(m_Pidl);
                return m_cPidl;
            }
        }

        /// <Summary>The IsReadOnly attribute causes an annoying access to any floppy drives
        /// on the system. To postpone this (or avoid, depending on user action),
        /// the attribute is only queried when asked for</Summary>
        public bool IsReadOnly
        {
            get
            {
                if (m_IsReadOnlySetup)
                {
                    return m_IsReadOnly;
                }
                else
                {
                    var shfi = new SHFILEINFO();
                    shfi.dwAttributes = (int)SFGAO.RDONLY;
                    int dwflag = (int)(SHGFI.PIDL | SHGFI.ATTRIBUTES | SHGFI.ATTR_SPECIFIED);

                    int dwAttr = 0;
                    IntPtr H = SHGetFileInfo(m_Pidl, dwAttr, ref shfi, cbFileInfo, dwflag);
                    if (H.ToInt32() != NOERROR && H.ToInt32() != 1) Marshal.ThrowExceptionForHR(H.ToInt32());
                    m_IsReadOnly = Convert.ToBoolean(shfi.dwAttributes & (int)SFGAO.RDONLY);

                    m_IsReadOnlySetup = true;
                    return m_IsReadOnly;
                }
            }
        }

        private bool _IsSystem_HaveSysInfo = default;
        private bool _IsSystem_m_IsSystem = default;
        /// <Summary>The IsSystem attribute is seldom used, but required by DragDrop operations.
        /// Since there is no way of getting ONLY the System attribute without getting
        /// the RO attribute (which forces a reference to the floppy drive), we pay
        /// the price of getting its own File/DirectoryInfo for this purpose alone.
        /// </Summary>
        public bool IsSystem
        {
            get // true once we have gotten this attr
            { // the value of this attr once we have it
                if (!_IsSystem_HaveSysInfo)
                {
                    try
                    {
                        _IsSystem_m_IsSystem = (File.GetAttributes(m_Path) & FileAttributes.System) == FileAttributes.System;
                        _IsSystem_HaveSysInfo = true;
                    }
                    catch (Exception ex)
                    {
                        _IsSystem_HaveSysInfo = true;
                    }
                }
                Debug.WriteLine("In IsSystem -- Path = " + m_Path + " IsSystem = " + _IsSystem_m_IsSystem);
                return _IsSystem_m_IsSystem;
            }
        }

        /// <summary>
        /// If not initialized, then build DesktopBase
        /// once done, or if initialized already,
        /// </summary>
        /// <returns>The DesktopBase CShItem representing the desktop</returns>
        public static CShItem GetDeskTop()
        {
            if (DesktopBase == null) DesktopBase = new CShItem();
            return DesktopBase;
        }

        /// <Summary>IsAncestorOf returns True if CShItem ancestor is an ancestor of CShItem current
        /// if OS is Win2K or above, uses the ILIsParent API, otherwise uses the
        /// cPidl function StartsWith.  This is necessary since ILIsParent in only available
        /// in Win2K or above systems AND StartsWith fails on some folders on XP systems (most
        /// obviously some Network Folder Shortcuts, but also Control Panel. Note, StartsWith
        /// always works on systems prior to XP.
        /// NOTE: if ancestor and current reference the same Item, both
        /// methods return True</Summary>
        public static bool IsAncestorOf(CShItem ancestor, CShItem current, bool fParent = false) => 
            IsAncestorOf(ancestor.PIDL, current.PIDL, fParent);

        /// <Summary> Compares a candidate Ancestor PIDL with a Child PIDL and
        /// returns True if Ancestor is an ancestor of the child.
        /// if fParent is True, then only return True if Ancestor is the immediate
        /// parent of the Child</Summary>
        public static bool IsAncestorOf(IntPtr AncestorPidl, IntPtr ChildPidl, bool fParent = false)
        {
            bool IsAncestorOfRet = default;
            if (Is2KOrAbove())
            {
                return ILIsParent(AncestorPidl, ChildPidl, fParent);
            }
            else
            {
                cPidl Child = new cPidl(ChildPidl);
                cPidl Ancestor = new cPidl(AncestorPidl);
                IsAncestorOfRet = Child.StartsWith(Ancestor);

                if (!IsAncestorOfRet) return IsAncestorOfRet;
                if (fParent) // check for immediate ancestor, if desired
                {
                    var oAncBytes = Ancestor.Decompose();
                    var oChildBytes = Child.Decompose();
                    if (oAncBytes.Length != oChildBytes.Length - 1) IsAncestorOfRet = false;
                }
            }

            return IsAncestorOfRet;
        }

        /// <Summary>The WalkAllCallBack delegate defines the signature of 
        /// the routine to be passed to DirWalker
        /// Usage:  dim d1 as new CshItem.WalkAllCallBack(addressof yourroutine)
        ///   Callback function receives a CShItem for each file and Directory in
        ///   Starting Directory and each sub-directory of this directory and
        ///   each sub-dir of each sub-dir ....
        /// </Summary>
        public delegate bool WalkAllCallBack(CShItem info, int UserLevel, int Tag);


        /// <Summary>
        /// AllFolderWalk recursively walks down directories from cStart, calling its
        ///   callback routine, WalkAllCallBack, for each Directory and File encountered, including those in
        ///   cStart.  UserLevel is incremented by 1 for each level of dirs that DirWalker
        /// recurses thru.  Tag in an Integer that is simply passed, unmodified to the 
        /// callback, with each CShItem encountered, both File and Directory CShItems.
        /// </Summary>
        /// <param name="cStart"></param>
        /// <param name="cback"></param>
        /// <param name="UserLevel"></param>
        /// <param name="Tag"></param>
        public static bool AllFolderWalk(CShItem cStart, WalkAllCallBack cback, int UserLevel, int Tag)
        {
            if (!(cStart == null) && cStart.IsFolder)
            {
                CShItem cItem;
                // first processes all files in this directory
                foreach (CShItem currentCItem in cStart.GetFiles())
                {
                    cItem = currentCItem;
                    if (!cback(cItem, UserLevel, Tag)) return false; // user said stop
                }
                // then process all dirs in this directory, recursively
                foreach (CShItem currentCItem1 in cStart.GetDirectories())
                {
                    cItem = currentCItem1;
                    if (!cback(cItem, UserLevel + 1, Tag)) return false; // user said stop
                    else if (!AllFolderWalk(cItem, cback, UserLevel + 1, Tag)) return false;
                }
                return true;
            }
            else // Invalid call
            {
                throw new ApplicationException("AllFolderWalk -- Invalid Start Directory");
            }
        }

        public bool Equals(CShItem other)
        {
            bool EqualsRet = Path.Equals(other.Path);
            return EqualsRet;
        }

        /// <summary>
        /// Returns the Directories of this sub-folder as an ArrayList of CShitems
        /// </summary>
        /// <param name="doRefresh">Optional, default=True, Refresh the directories</param>
        /// <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
        /// <remarks>revised to alway return an up-to-date list unless 
        /// specifically instructed not to (useful in constructs like:
        /// if CSI.RefreshDirectories then
        ///     Dirs = CSI.GetDirectories(False)  ' just did a Refresh </remarks>
        public ArrayList GetDirectories(bool doRefresh = true)
        {
            if (m_IsFolder)
            {
                if (doRefresh) RefreshDirectories();   // return an up-to-date List
                else if (m_Directories is null) RefreshDirectories();
                return m_Directories;
            }
            else // if it is not a Folder, then return empty arraylist
            {
                return new ArrayList();
            }
        }

        /// <summary>
        /// Returns the Files of this sub-folder as an
        ///   ArrayList of CShitems
        /// Note: we do not keep the arraylist of files, Generate it each time
        /// </summary>
        /// <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
        public ArrayList GetFiles() => (m_IsFolder ? GetContents(SHCONTF.NONFOLDERS | SHCONTF.INCLUDEHIDDEN) : new ArrayList());

        /// <summary>
        /// Returns the Files of this sub-folder, filtered by a filtering string, as an
        ///   ArrayList of CShitems
        /// Note: we do not keep the arraylist of files, Generate it each time
        /// </summary>
        /// <param name="Filter">A filter string (for example: *.Doc)</param>
        /// <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
        public ArrayList GetFiles(string Filter)
        {
            if (m_IsFolder)
            {
                ArrayList dummy = new ArrayList();
                string[] fileentries = Directory.GetFiles(m_Path, Filter);
                foreach (var vFile in fileentries) dummy.Add(new CShItem(vFile));
                return dummy;
            }
            else
            {
                return new ArrayList();
            }
        }

        /// <summary>
        /// Returns the Directories and Files of this sub-folder as an
        ///   ArrayList of CShitems
        /// Note: we do not keep the arraylist of files, Generate it each time
        /// </summary>
        /// <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
        public ArrayList GetItems()
        {
            var rVal = new ArrayList();
            if (m_IsFolder)
            {
                rVal.AddRange(GetDirectories());
                rVal.AddRange(this.GetContents(SHCONTF.NONFOLDERS | SHCONTF.INCLUDEHIDDEN));
                rVal.Sort();
                return rVal;
            }
            else
            {
                return rVal;
            }
        }

        /// <Summary>GetFileName returns the Full file name of this item.
        /// Specifically, for a link file (xxx.txt.lnk for example) the
        /// DisplayName property will return xxx.txt, this method will
        /// return xxx.txt.lnk.  In most cases this is equivalent of
        /// System.IO.Path.GetFileName(m_Path).  However, some m_Paths
        /// actually are GUIDs.  In that case, this routine returns the
        /// DisplayName</Summary>
        public string GetFileName()
        {
            if (m_Path.StartsWith("::{")) return DisplayName;
            else if (m_IsDisk) return m_Path.Substring(0, 1);
            else return System.IO.Path.GetFileName(m_Path);
        }

        /// <Summary> A lower cost way of Refreshing the Directories of this CShItem</Summary>
        /// <returns> Returns True if there were any changes</returns>
        public bool RefreshDirectories()
        {
            bool RefreshDirectoriesRet = default;
            RefreshDirectoriesRet = false;      // value unless there were changes
            if (m_IsFolder)          // if not a folder, then return false
            {
                var InvalidDirs = new ArrayList();  // holds CShItems of not found dirs
                if (m_Directories == null)
                {
                    m_Directories = GetContents(SHCONTF.FOLDERS | SHCONTF.INCLUDEHIDDEN);
                    RefreshDirectoriesRet = true;     // changed from unexamined to examined
                }
                else
                {
                    // Get relative PIDLs from current directory items
                    var curPidls = GetContents(SHCONTF.FOLDERS | SHCONTF.INCLUDEHIDDEN, true);
                    IntPtr iptr;      // used below
                    if (curPidls.Count < 1)
                    {
                        if (m_Directories.Count > 0)
                        {
                            m_Directories = new ArrayList(); // nothing there anymore
                            RefreshDirectoriesRet = true;    // Changed from had some to have none
                        }
                        else
                        {

                        } // Empty before, Empty now, do nothing -- just a logic marker
                    }
                    else // still has some. Are they the same?
                    {
                        if (m_Directories.Count < 1) // didn't have any before, so different
                        {
                            m_Directories = GetContents(SHCONTF.FOLDERS | SHCONTF.INCLUDEHIDDEN);
                            RefreshDirectoriesRet = true;     // changed from had none to have some
                        }
                        else    // some before, some now. Same?  This is the complicated part
                        {
                            // Firstly, build ArrayLists of Relative Pidls
                            var compList = new ArrayList(curPidls);
                            // Since we are only comparing relative PIDLs, build a list of 
                            // the relative PIDLs of the old content -- saving repeated building
                            int iOld;
                            var OldRel = new IntPtr[m_Directories.Count];
                            var loopTo = m_Directories.Count - 1;
                            for (iOld = 0; iOld <= loopTo; iOld++)
                                // GetLastID returns a ptr into an EXISTING IDLIST -- never release that ptr
                                // and never release the EXISTING IDLIST before thru with OldRel
                                OldRel[iOld] = GetLastID(((CShItem)m_Directories[iOld]).PIDL);
                            int iNew;
                            var loopTo1 = m_Directories.Count - 1;
                            for (iOld = 0; iOld <= loopTo1; iOld++)
                            {
                                var loopTo2 = compList.Count - 1;
                                for (iNew = 0; iNew <= loopTo2; iNew++)
                                {
                                    if (IsEqual((IntPtr)compList[iNew], OldRel[iOld]))
                                    {
                                        compList.RemoveAt(iNew);  // Match, don't look at this one again
                                        goto NXTOLD;    // content item exists in both
                                    }
                                }
                                // falling thru here means couldn't find iOld entry
                                InvalidDirs.Add(m_Directories[iOld]); // save off the unmatched CShItem
                                RefreshDirectoriesRet = true;
                                // any not found should be removed from m_Directories
                                NXTOLD:
                                ;
                            }
                            foreach (CShItem csi in InvalidDirs)
                                m_Directories.Remove(csi);
                            // anything remaining in compList is a new entry
                            if (compList.Count > 0)
                            {
                                RefreshDirectoriesRet = true;
                                foreach (IntPtr currentIptr in compList)
                                {
                                    iptr = currentIptr;   // these are relative PIDLs
                                    try                 // ASUS Fix
                                    {
                                        m_Directories.Add(new CShItem(m_Folder, iptr, this));
                                    }
                                    catch (InvalidCastException ex)
                                    {
                                    }    // ASUS Fix
                                         // ASUS Fix
                                }
                            }
                            if (RefreshDirectoriesRet) // something added or removed, resort
                            {
                                m_Directories.Sort();
                            }
                        }
                        // we obtained some new relative PIDLs in curPidls, so free them
                        foreach (IntPtr currentIptr1 in curPidls)
                        {
                            iptr = currentIptr1;
                            Marshal.FreeCoTaskMem(iptr);
                        }
                    }
                }  // end of content comparison
                // end of IsNothing test
            } // end of IsFolder test

            return RefreshDirectoriesRet;
        }

        /// <summary>
        /// Returns the DisplayName as the normal ToString value
        /// </summary>
        public override string ToString() => m_DisplayName;

        /// <summary>
        /// Summary of DebugDump.
        /// </summary>
        public void DebugDump()
        {
            Debug.WriteLine("DisplayName = " + m_DisplayName);
            Debug.WriteLine("PIDL        = " + m_Pidl.ToString());
            Debug.WriteLine(Constants.vbTab + "Path        = " + m_Path);
            Debug.WriteLine(Constants.vbTab + "TypeName    = " + TypeName);
            Debug.WriteLine(Constants.vbTab + "iIconNormal = " + m_IconIndexNormal);
            Debug.WriteLine(Constants.vbTab + "iIconSelect = " + m_IconIndexOpen);
            Debug.WriteLine(Constants.vbTab + "IsBrowsable = " + m_IsBrowsable);
            Debug.WriteLine(Constants.vbTab + "IsFileSystem= " + m_IsFileSystem);
            Debug.WriteLine(Constants.vbTab + "IsFolder    = " + m_IsFolder);
            Debug.WriteLine(Constants.vbTab + "IsLink    = " + m_IsLink);
            Debug.WriteLine(Constants.vbTab + "IsDropTarget = " + m_IsDropTarget);
            Debug.WriteLine(Constants.vbTab + "IsReadOnly   = " + IsReadOnly);
            Debug.WriteLine(Constants.vbTab + "CanCopy = " + CanCopy);
            Debug.WriteLine(Constants.vbTab + "CanLink = " + CanLink);
            Debug.WriteLine(Constants.vbTab + "CanMove = " + CanMove);
            Debug.WriteLine(Constants.vbTab + "CanDelete = " + CanDelete);

            if (m_IsFolder)
            {
                if (!(m_Directories == null)) Debug.WriteLine(Constants.vbTab + "Directory Count = " + m_Directories.Count);
                else Debug.WriteLine(Constants.vbTab + "Directory Count Not yet set");
            }
        }

        public ShellDll.IDropTarget GetDropTargetOf(Control tn)
        {
            if (Folder == null) return default;

            IntPtr pInterface = new IntPtr();
            return Folder.CreateViewObject(tn.Handle, ref ShellDll.IID_IDropTarget, ref pInterface) == S_OK ?
                (ShellDll.IDropTarget)Marshal.GetTypedObjectForIUnknown(pInterface, typeof(ShellDll.IDropTarget)) : default;
        }

        /// <Summary>
        /// Returns the requested Items of this Folder as an ArrayList of CShitems
        /// unless the IntPtrOnly flag is set.  If IntPtrOnly is True, return an
        /// ArrayList of PIDLs.
        /// </Summary>
        /// <param name="flags">A set of one or more SHCONTF flags indicating which items to return</param>
        /// <param name="IntPtrOnly">True to suppress generation of CShItems, returning only an
        /// ArrayList of IntPtrs to RELATIVE PIDLs for the contents of this Folder</param>
        private ArrayList GetContents(SHCONTF flags, bool IntPtrOnly = false)
        {
            var rVal = new ArrayList();
            int HR;
            IEnumIDList IEnum = default;
            // UPDATE: Always get all items. Use flags param only to indicate what the caller wants
            HR = m_Folder.EnumObjects(0, SHCONTF.INCLUDEHIDDEN | SHCONTF.FOLDERS | SHCONTF.NONFOLDERS, IEnum); // UPDATE
            if (HR == NOERROR)
            {
                var item = IntPtr.Zero;
                var itemCnt = default(int);
                HR = IEnum.GetNext(1, ref item, itemCnt);
                if (HR == NOERROR)
                {
                    while (itemCnt > 0 && !item.Equals(IntPtr.Zero))
                    {
                        // UPDATE: Changed logic of testing for Compressed(zip, etc) files
                        // use the flags param to see what user wanted, but always Enumerate thru all items
                        // Vista and above really obey SHCONTF.FOLDERS and SHCONTF.NONFOLDERS, and we are
                        // defining compressed files as files rather than Folders as XP and above do
                        bool ItemIsFolder;
                        SFGAO attrFlag = SFGAO.FOLDER | SFGAO.STREAM;
                        var aPidl = new IntPtr[] { item };
                        m_Folder.GetAttributesOf(1, aPidl, attrFlag);
                        if (XPorAbove) ItemIsFolder = Convert.ToBoolean(attrFlag & SFGAO.FOLDER) && !Convert.ToBoolean(attrFlag & SFGAO.STREAM);
                        else ItemIsFolder = Convert.ToBoolean(attrFlag & SFGAO.FOLDER);

                        if (ItemIsFolder & !Convert.ToBoolean(flags & SHCONTF.FOLDERS)) goto SKIPONE; // if folders not wanted then skip this
                        if (!ItemIsFolder & !Convert.ToBoolean(flags & SHCONTF.NONFOLDERS)) goto SKIPONE; // not a folder, if nonfolders not wanted, skip
                                          // END UPDATE
                        if (IntPtrOnly)   // just relative pidls for fast look, no CShITem overhead
                        {
                            rVal.Add(PIDLClone(item));   // caller must free
                        }
                        else
                        {
                            try
                            {
                                rVal.Add(new CShItem(m_Folder, item, this));
                            }
                            catch (InvalidCastException ex)
                            {

                            }
                        }

                        SKIPONE:;
                        Marshal.FreeCoTaskMem(item); // if New kept it, it kept a copy
                        item = IntPtr.Zero;
                        itemCnt = 0;
                        // Application.DoEvents()
                        HR = IEnum.GetNext(1, item, itemCnt);
                    }
                }
                else if (HR != 1) goto HRError; // 1 means no more
            }
            else
            {
                goto HRError;
            }
            // Normal Exit
            NORMAL:;

            if (!(IEnum == null)) Marshal.ReleaseComObject(IEnum);
            rVal.TrimToSize();
            return rVal;

            // Error Exit for all Com errors
            HRError:;
            if (!(IEnum == null)) Marshal.ReleaseComObject(IEnum);

            rVal = new ArrayList(); // sometimes it is a non-fatal error,ignored
            goto NORMAL;
        }

        /// <Summary>
        /// Get Size in bytes of the first (possibly only)
        /// SHItem in an ID list.  Note: the full size of
        ///   the item is the sum of the sizes of all SHItems
        ///   in the list!!
        /// </Summary>
        /// <param name="pidl"></param>
        private static int ItemIDSize(IntPtr pidl)
        {
            if (!pidl.Equals(IntPtr.Zero))
            {
                var b = new byte[2];
                Marshal.Copy(pidl, b, 0, 2);
                return b[1] * 256 + b[0];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// computes the actual size of the ItemIDList pointed to by pidl
        /// </summary>
        /// <param name="pidl">The pidl pointing to an ItemIDList</param>
        /// <returns> Returns actual size of the ItemIDList, less the terminating nulnul</returns>
        public static int ItemIDListSize(IntPtr pidl)
        {
            if (!pidl.Equals(IntPtr.Zero))
            {
                int i = ItemIDSize(pidl);
                int b = Marshal.ReadByte(pidl, i) + Marshal.ReadByte(pidl, i + 1) * 256;
                while (b > 0)
                {
                    i += b;
                    b = Marshal.ReadByte(pidl, i) + Marshal.ReadByte(pidl, i + 1) * 256;
                }
                return i;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Counts the total number of SHItems in input pidl
        /// </summary>
        /// <param name="pidl">The pidl to obtain the count for</param>
        /// <returns> Returns the count of SHItems pointed to by pidl</returns>
        public static int PidlCount(IntPtr pidl)
        {
            if (!pidl.Equals(IntPtr.Zero))
            {
                int cnt = 0;
                int i = 0;
                int b = Marshal.ReadByte(pidl, i) + Marshal.ReadByte(pidl, i + 1) * 256;
                while (b > 0)
                {
                    cnt += 1;
                    i += b;
                    b = Marshal.ReadByte(pidl, i) + Marshal.ReadByte(pidl, i + 1) * 256;
                }
                return cnt;
            }
            else
            {
                return 0;
            }
        }

        /// <Summary>GetLastId -- returns a pointer to the last ITEMID in a valid
        /// ITEMIDLIST. Returned pointer SHOULD NOT be released since it
        /// points to place within the original PIDL</Summary>
        /// <returns>IntPtr pointing to last ITEMID in ITEMIDLIST structure,
        /// Returns IntPtr.Zero if given a null pointer.
        /// If given a pointer to the Desktop, will return same pointer.</returns>
        /// <remarks>This is what the API ILFindLastID does, however IL... 
        /// functions are not supported before Win2K.</remarks>
        public static IntPtr GetLastID(IntPtr pidl)
        {
            if (!pidl.Equals(IntPtr.Zero))
            {
                int prev = 0;
                int i = 0;
                int b = Marshal.ReadByte(pidl, i) + Marshal.ReadByte(pidl, i + 1) * 256;
                while (b > 0)
                {
                    prev = i;
                    i += b;
                    b = Marshal.ReadByte(pidl, i) + Marshal.ReadByte(pidl, i + 1) * 256;
                }
                return new IntPtr(pidl.ToInt32() + prev);
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        public static IntPtr[] DecomposePIDL(IntPtr pidl)
        {
            int lim = ItemIDListSize(pidl);
            var PIDLs = new IntPtr[(PidlCount(pidl))];
            int i = 0;
            var curB = default(int);
            int offSet = 0;
            while (curB < lim)
            {
                var thisPtr = new IntPtr(pidl.ToInt32() + curB);
                offSet = Marshal.ReadByte(thisPtr) + Marshal.ReadByte(thisPtr, 1) * 256;
                PIDLs[i] = Marshal.AllocCoTaskMem(offSet + 2);
                var b = new byte[offSet + 1 + 1];
                Marshal.Copy(thisPtr, b, 0, offSet);
                b[offSet] = 0;
                b[offSet + 1] = 0;
                Marshal.Copy(b, 0, PIDLs[i], offSet + 2);
                curB += offSet;
                i += 1;
            }
            return PIDLs;
        }

        private static IntPtr PIDLClone(IntPtr pidl)
        {
            IntPtr PIDLCloneRet = default;
            int cb = ItemIDListSize(pidl);
            var b = new byte[cb + 1 + 1];
            Marshal.Copy(pidl, b, 0, cb); // not including terminating nulnul
            b[cb] = 0;
            b[cb + 1] = 0; // force to nulnul
            PIDLCloneRet = Marshal.AllocCoTaskMem(cb + 2);
            Marshal.Copy(b, 0, PIDLCloneRet, cb + 2);
            return PIDLCloneRet;
        }

        public static bool IsEqual(IntPtr Pidl1, IntPtr Pidl2)
        {
            if (Win2KOrAbove)
            {
                return ILIsEqual(Pidl1, Pidl2);
            }
            else // do hard way, may not work for some folders on XP
            {

                int cb1;
                int cb2;
                cb1 = ItemIDListSize(Pidl1);
                cb2 = ItemIDListSize(Pidl2);
                if (cb1 != cb2)
                    return false;
                int lim32 = cb1 / 4;

                int i;
                var loopTo = lim32 - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (Marshal.ReadInt32(Pidl1, i) != Marshal.ReadInt32(Pidl2, i))
                    {
                        return false;
                    }
                }
                int limB = cb1 % 4;
                int offset = lim32 * 4;
                var loopTo1 = limB - 1;
                for (i = 0; i <= loopTo1; i++)
                {
                    if (Marshal.ReadByte(Pidl1, offset + i) != Marshal.ReadByte(Pidl2, offset + i))
                    {
                        return false;
                    }
                }
                return true;
            } // made it to here, so they are equal
        }

        /// <summary>
        /// Concatenates the contents of two pidls into a new Pidl (ended by 00)
        /// allocating CoTaskMem to hold the result,
        /// placing the concatenation (followed by 00) into the
        /// allocated Memory,
        /// and returning an IntPtr pointing to the allocated mem
        /// </summary>
        /// <param name="pidl1">IntPtr to a well formed SHItemIDList or IntPtr.Zero</param>
        /// <param name="pidl2">IntPtr to a well formed SHItemIDList or IntPtr.Zero</param>
        /// <returns>Returns a ptr to an ItemIDList containing the 
        ///   concatenation of the two (followed by the req 2 zeros
        ///   Caller must Free this pidl when done with it</returns>
        public static IntPtr concatPidls(IntPtr pidl1, IntPtr pidl2)
        {
            int cb1;
            int cb2;
            cb1 = ItemIDListSize(pidl1);
            cb2 = ItemIDListSize(pidl2);
            int rawCnt = cb1 + cb2;
            if (rawCnt > 0)
            {
                var b = new byte[rawCnt + 1 + 1];

                if (cb1 > 0) Marshal.Copy(pidl1, b, 0, cb1);
                if (cb2 > 0) Marshal.Copy(pidl2, b, cb1, cb2);

                var rVal = Marshal.AllocCoTaskMem(cb1 + cb2 + 2);
                b[rawCnt] = 0;
                b[rawCnt + 1] = 0;
                Marshal.Copy(b, 0, rVal, rawCnt + 2);
                return rVal;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Returns an ItemIDList with the last ItemID trimed off
        /// This is necessary since I cannot get SHBindToParent to work 
        /// It's purpose is to generate an ItemIDList for the Parent of a
        /// Special Folder which can then be processed with DesktopBase.BindToObject,
        /// yeilding a Folder for the parent of the Special Folder
        /// It also creates and passes back a RELATIVE pidl for this item
        /// </summary>
        /// <param name="pidl">A pointer to a well formed ItemIDList. The PIDL to trim</param>
        /// <param name="relPidl">BYREF IntPtr which will point to a new relative pidl
        ///        containing the contents of the last ItemID in the ItemIDList
        ///        terminated by the required 2 nulls.</param>
        /// <returns> an ItemIDList with the last element removed.
        /// Caller must Free this item when through with it
        /// Also returns the new relative pidl in the 2nd parameter
        ///   Caller must Free this pidl as well, when through with it
        /// </returns>
        public static IntPtr TrimPidl(IntPtr pidl, ref IntPtr relPidl)
        {
            int cb = ItemIDListSize(pidl);
            var b = new byte[cb + 1 + 1];
            Marshal.Copy(pidl, b, 0, cb);
            int prev = 0;
            int i = b[0] + b[1] * 256;

            while (i > 0 && i < cb)
            {
                prev = i;
                i += b[i] + b[i + 1] * 256;
            }
            if (prev + 1 < cb)
            {
                // first set up the relative pidl
                b[cb] = 0;
                b[cb + 1] = 0;
                int cb1 = b[prev] + b[prev + 1] * 256;
                relPidl = Marshal.AllocCoTaskMem(cb1 + 2);
                Marshal.Copy(b, prev, relPidl, cb1 + 2);
                b[prev] = 0;
                b[prev + 1] = 0;
                var rVal = Marshal.AllocCoTaskMem(prev + 2);
                Marshal.Copy(b, 0, rVal, prev + 2);
                return rVal;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Dumps, to the Debug output, the contents of the mem block pointed to by
        /// a PIDL. Depends on the internal structure of a PIDL
        /// </summary>
        /// <param name="pidl">The IntPtr(a PIDL) pointing to the block to dump</param>
        public static void DumpPidl(IntPtr pidl)
        {
            int cb = ItemIDListSize(pidl);
            Debug.WriteLine("PIDL " + pidl.ToString() + " contains " + cb + " bytes");
            if (cb > 0)
            {
                var b = new byte[cb + 1 + 1];
                Marshal.Copy(pidl, b, 0, cb + 1);
                int pidlCnt = 1;
                int i = b[0] + b[1] * 256;
                int curB = 0;
                while (i > 0)
                {
                    Debug.Write("ItemID #" + pidlCnt + " Length = " + i);
                    DumpHex(b, curB, curB + i - 1);
                    pidlCnt += 1;
                    curB += i;
                    i = b[curB] + b[curB + 1] * 256;
                }
            }
        }

        /// <Summary>Dump a portion or all of a Byte Array to Debug output</Summary>
        /// <param name = "b">A single dimension Byte Array</param>
        /// <param name = "sPos">Optional start index of area to dump (default = 0)</param>
        /// <param name = "epos">Optional last index position to dump (default = end of array)</param>
        /// <Remarks>
        /// </Remarks>
        public static void DumpHex(byte[] b, int sPos = 0, int ePos = 0)
        {
            if (ePos == 0) ePos = b.Length - 1;
            int j;
            int curB = sPos;
            string sTmp;
            char ch;
            var SBH = new StringBuilder();
            var SBT = new StringBuilder();
            var loopTo = ePos - sPos;
            for (j = 0; j <= loopTo; j++)
            {
                if (j % 16 == 0)
                {
                    Debug.WriteLine(SBH.ToString() + SBT.ToString());
                    SBH = new StringBuilder();
                    SBT = new StringBuilder("          ");
                    SBH.Append(HexNum(j + sPos, 4) + "). ");
                }

                if (b[curB] < 16) sTmp = "0" + Conversion.Hex(b[curB]);
                else sTmp = Conversion.Hex(b[curB]);

                SBH.Append(sTmp);
                SBH.Append(" ");
                ch = Strings.Chr(b[curB]);

                if (char.IsControl(ch)) SBT.Append(".");
                else SBT.Append(ch);
                curB += 1;
            }

            int fill = j % 16;
            if (fill != 0) SBH.Append(' ', 48 - 3 * (j % 16));
            Debug.WriteLine(SBH.ToString() + SBT.ToString());
        }

        public static string HexNum(int num, int nrChrs)
        {
            string h = Conversion.Hex(num);
            var SB = new StringBuilder();
            int i;
            var loopTo = nrChrs - h.Length;
            for (i = 1; i <= loopTo; i++) SB.Append("0");
            SB.Append(h);
            return SB.ToString();
        }

        /// <Summary> It is sometimes useful to sort a list of TreeNodes,
        /// ListViewItems, or other objects in an order based on CShItems in their Tag
        /// use this Icomparer for that Sort</Summary>
        public partial class TagComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                CShItem xTag = (((TreeNode)(x)).Tag as CShItem);
                CShItem yTag = (((TreeNode)(y)).Tag as CShItem);
                return xTag.CompareTo(yTag);
            }
        }

        /// <Summary>cPidl class contains a Byte() representation of a PIDL and
        /// certain Methods and Properties for comparing one cPidl to another</Summary>
        public partial class cPidl : IEnumerable
        {
            private byte[] m_bytes;   // The local copy of the PIDL
            private int m_ItemCount;      // the # of ItemIDs in this ItemIDList (PIDL)
            private int m_OffsetToRelative; // the index of the start of the last itemID in m_bytes

            public cPidl(IntPtr pidl)
            {
                int cb = ItemIDListSize(pidl);
                if (cb > 0)
                {
                    m_bytes = new byte[cb + 1 + 1];
                    Marshal.Copy(pidl, m_bytes, 0, cb);
                }
                // DumpPidl(pidl)
                else
                {
                    m_bytes = new byte[2];
                }  // This is the DeskTop (we hope)
                   // ensure nulnul
                m_bytes[m_bytes.Length - 2] = 0;
                m_bytes[m_bytes.Length - 1] = 0;
                m_ItemCount = PidlCount(pidl);
            }

            public byte[] PidlBytes => m_bytes;
            public int Length => m_bytes.Length;
            public int ItemCount => m_ItemCount;

            /// <Summary> Copy the contents of a byte() containing a pidl to
            /// CoTaskMemory, returning an IntPtr that points to that mem block
            /// Assumes that this cPidl is properly terminated, as all New 
            /// cPidls are.
            /// Caller must Free the returned IntPtr when done with mem block.
            /// </Summary>
            public IntPtr ToPIDL()
            {
                IntPtr ToPIDLRet = default;
                ToPIDLRet = BytesToPidl(m_bytes);
                return ToPIDLRet;
            }

            /// <Summary>Returns an object containing a byte() for each of this cPidl's
            /// ITEMIDs (individual PIDLS), in order such that obj(0) is
            /// a byte() containing the bytes of the first ITEMID, etc.
            /// Each ITEMID is properly terminated with a nulnul
            /// </Summary>
            public object[] Decompose()
            {
                var bArrays = new object[ItemCount];
                ICPidlEnumerator eByte = (ICPidlEnumerator)GetEnumerator();
                var i = default(int);
                while (eByte.MoveNext())
                {
                    bArrays[i] = eByte.Current;
                    i += 1;
                }
                return bArrays;
            }

            /// <Summary>Returns True if input cPidl's content exactly match the 
            /// contents of this instance</Summary>
            public bool IsEqual(cPidl other)
            {
                bool IsEqualRet = default;
                IsEqualRet = false;     // assume not
                if (other.Length != Length)
                    return IsEqualRet;
                var ob = other.PidlBytes;
                int i;
                var loopTo = Length - 1;
                for (i = 0; i <= loopTo; i++)  // note: we look at nulnul also
                {
                    if (ob[i] != m_bytes[i])
                        return IsEqualRet;
                }
                return true;         // all equal on fall thru
            }

            /// <Summary> Join two byte arrays containing PIDLS, returning a 
            /// Byte() containing the resultant ITEMIDLIST. Both Byte() must
            /// be properly terminated (nulnul)
            /// Returns NOTHING if error
            /// </Summary>
            public static byte[] JoinPidlBytes(byte[] b1, byte[] b2)
            {
                if (IsValidPidl(b1) & IsValidPidl(b2))
                {
                    var b = new byte[b1.Length + b2.Length - 3 + 1]; // allow for leaving off first nulnul
                    Array.Copy(b1, b, b1.Length - 2);
                    Array.Copy(b2, 0, b, b1.Length - 2, b2.Length);
                    if (IsValidPidl(b))
                    {
                        return b;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            /// <Summary> Copy the contents of a byte() containing a pidl to
            /// CoTaskMemory, returning an IntPtr that points to that mem block
            /// Caller must free the IntPtr when done with it
            /// </Summary>
            public static IntPtr BytesToPidl(byte[] b)
            {
                IntPtr BytesToPidlRet = default;
                BytesToPidlRet = IntPtr.Zero;       // assume failure
                if (IsValidPidl(b))
                {
                    int bLen = b.Length;
                    BytesToPidlRet = Marshal.AllocCoTaskMem(bLen);
                    if (BytesToPidlRet.Equals(IntPtr.Zero))
                        return BytesToPidlRet; // another bad error
                    Marshal.Copy(b, 0, BytesToPidlRet, bLen);
                }

                return BytesToPidlRet;
            }

            /// <Summary>returns True if the beginning of pidlA matches PidlB exactly for pidlB's entire length</Summary>
            public static bool StartsWith(IntPtr pidlA, IntPtr pidlB) => StartsWith(new cPidl(pidlA), new cPidl(pidlB));

            /// <Summary>returns True if the beginning of A matches B exactly for B's entire length</Summary>
            public static bool StartsWith(cPidl A, cPidl B) => A.StartsWith(B);

            /// <Summary>Returns true if the CPidl input parameter exactly matches the
            /// beginning of this instance of CPidl</Summary>
            public bool StartsWith(cPidl cp)
            {
                var b = cp.PidlBytes;
                if (b.Length > m_bytes.Length)
                    return false; // input is longer
                int i;
                var loopTo = b.Length - 3;
                for (i = 0; i <= loopTo; i++) // allow for nulnul at end of cp.PidlBytes
                {
                    if (b[i] != m_bytes[i])
                        return false;
                }
                return true;
            }

            public IEnumerator GetEnumerator() => new ICPidlEnumerator(m_bytes);

            private partial class ICPidlEnumerator : IEnumerator
            {
                private int m_sPos;   // the first index in the current PIDL
                private int m_ePos;   // the last index in the current PIDL
                private byte[] m_bytes;   // the local copy of the PIDL
                private bool m_NotEmpty = false; // the desktop PIDL is zero length

                public ICPidlEnumerator(byte[] b)
                {
                    m_bytes = b;
                    if (b.Length > 0) m_NotEmpty = true;
                    m_sPos = -1;
                    m_ePos = -1;
                }

                public object Current
                {
                    get
                    {
                        if (m_sPos < 0)
                            throw new InvalidOperationException("ICPidlEnumerator --- attempt to get Current with invalidated list");
                        var b = new byte[m_ePos - m_sPos + 2 + 1];    // room for nulnul
                        Array.Copy(m_bytes, m_sPos, b, 0, b.Length - 2);
                        b[b.Length - 2] = 0;
                        b[b.Length - 1] = 0; // add nulnul
                        return b;
                    }
                }

                public bool MoveNext()
                {
                    if (m_NotEmpty)
                    {
                        if (m_sPos < 0)
                        {
                            m_sPos = 0;
                            m_ePos = -1;
                        }
                        else
                        {
                            m_sPos = m_ePos + 1;
                        }

                        if (m_bytes.Length < m_sPos + 1) throw new InvalidCastException("Malformed PIDL");

                        int cb = m_bytes[m_sPos] + m_bytes[m_sPos + 1] * 256;
                        if (cb == 0) return false; // have passed all back
                        else m_ePos += cb;
                    }
                    else
                    {
                        m_sPos = 0;
                        m_ePos = 0;
                        return false; // in this case, we have exhausted the list of 0 ITEMIDs
                    }
                    return true;
                }

                public void Reset()
                {
                    m_sPos = -1;
                    m_ePos = -1;
                }
            }
        }
    }
}
