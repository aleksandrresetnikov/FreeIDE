using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using Microsoft.VisualBasic;
using static FreeIDE.Common.ShellDll;

namespace FreeIDE.Common
{
    public partial class SystemImageListManager
    {
        // For ImageList manipulation
        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETIMAGELIST = LVM_FIRST + 3;

        private const int LVSIL_NORMAL = 0;
        private const int LVSIL_SMALL = 1;
        private const int LVSIL_STATE = 2;

        private const int TV_FIRST = 0x1100;
        private const int TVM_SETIMAGELIST = TV_FIRST + 9;

        private const int TVSIL_NORMAL = 0;
        private const int TVSIL_STATE = 2;
        

        private static bool m_Initialized = false;
        private static IntPtr m_smImgList = IntPtr.Zero; // Handle to System Small ImageList
        private static IntPtr m_lgImgList = IntPtr.Zero; // Handle to System Large ImageList
        private static Hashtable m_Table = new Hashtable(128);

        private static Mutex m_Mutex = new Mutex();
        

        /// <summary>
        /// Summary of Initializer.
        /// </summary>
        private static void Initializer()
        {
            if (m_Initialized) return;

            var shfi = new SHFILEINFO();
            m_smImgList = SHGetFileInfo(".txt", FILE_ATTRIBUTE_NORMAL, ref shfi, cbFileInfo, 
                (int)(SHGFI.USEFILEATTRIBUTES | SHGFI.SYSICONINDEX | SHGFI.SMALLICON));


            Debug.Assert(!m_smImgList.Equals(IntPtr.Zero), "Failed to create Image Small ImageList");
            if (m_smImgList.Equals(IntPtr.Zero)) throw new Exception("Failed to create Small ImageList");

            m_lgImgList = SHGetFileInfo(".txt", FILE_ATTRIBUTE_NORMAL, ref shfi, cbFileInfo, 
                (int)(SHGFI.USEFILEATTRIBUTES | SHGFI.SYSICONINDEX | SHGFI.LARGEICON));


            Debug.Assert(!m_lgImgList.Equals(IntPtr.Zero), "Failed to create Image Small ImageList");
            if (m_lgImgList.Equals(IntPtr.Zero)) throw new Exception("Failed to create Large ImageList");

            m_Initialized = true;
        }


        public static IntPtr hSmallImageList => m_smImgList;
        public static IntPtr hLargeImageList => m_lgImgList;


        private static int mCnt;
        private static int bCnt;

        /// <summary>
        /// Summary of GetIconIndex.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="GetOpenIcon"></param>
        public static int GetIconIndex(ref CShItem item, bool GetOpenIcon = false, bool GetSelectedIcon = false)
        {
            Initializer();
            bool HasOverlay = false; // true if it's an overlay
            int rVal; // The returned Index

            int dwflag = (int)(SHGFI.SYSICONINDEX | SHGFI.PIDL | SHGFI.ICON);
            int dwAttr = 0;
            // build Key into HashTable for this Item
            int Key = Convert.ToInt32(Interaction.IIf(!GetOpenIcon, item.IconIndexNormal * 256, item.IconIndexOpen * 256));
            if (item.IsLink)
            {
                Key = Key | 1;
                dwflag = (int)(dwflag | (int)SHGFI.LINKOVERLAY);
                HasOverlay = true;
            }
            if (item.IsShared)
            {
                Key = Key | 2;
                dwflag = (int)(dwflag | (int)SHGFI.ADDOVERLAYS);
                HasOverlay = true;
            }
            if (GetSelectedIcon)
            {
                Key = Key | 4;
                dwflag = (int)(dwflag | (int)SHGFI.SELECTED);
                HasOverlay = true; // not really an overlay, but handled the same
            }
            if (m_Table.ContainsKey(Key))
            {
                rVal = Convert.ToInt32(m_Table[Key]);
                mCnt += 1;
            }
            else if (!HasOverlay) // for non-overlay icons, we already have
            {
                rVal = Key / 256; // the right index -- put in table
                m_Table[Key] = rVal;
                bCnt += 1;
            }
            else // don't have iconindex for an overlay, get it. 
            {
                // This is the tricky part -- add overlaid Icon to systemimagelist
                // use of SmallImageList from Calum McLellan
                var shfi = new SHFILEINFO();
                var shfi_small = new SHFILEINFO();
                IntPtr HR;
                IntPtr HR_SMALL;

                if (item.IsFileSystem & !item.IsDisk & !item.IsFolder)
                {
                    dwflag = dwflag | (int)SHGFI.USEFILEATTRIBUTES;
                    dwAttr = FILE_ATTRIBUTE_NORMAL;
                }

                HR = SHGetFileInfo(item.PIDL, dwAttr, ref shfi, cbFileInfo, dwflag);
                HR_SMALL = SHGetFileInfo(item.PIDL, dwAttr, ref shfi_small, cbFileInfo, dwflag | (int)SHGFI.SMALLICON);
                m_Mutex.WaitOne();
                rVal = ImageList_ReplaceIcon(m_smImgList, -1, shfi_small.hIcon);
                Debug.Assert(rVal > -1, "Failed to add overlaid small icon");
                int rVal2;
                rVal2 = ImageList_ReplaceIcon(m_lgImgList, -1, shfi.hIcon);
                Debug.Assert(rVal2 > -1, "Failed to add overlaid large icon");
                Debug.Assert(rVal == rVal2, "Small & Large IconIndices are Different");
                m_Mutex.ReleaseMutex();
                DestroyIcon(shfi.hIcon);
                DestroyIcon(shfi_small.hIcon);
                if (rVal < 0 || rVal != rVal2) throw new ApplicationException("Failed to add Icon for " + item.DisplayName);
                m_Table[Key] = rVal;
            }
            return rVal;
        }
        // Private Shared Sub DebugShowImages(ByVal useSmall As Boolean, ByVal iFrom As Integer, ByVal iTo As Integer)
        // Dim RightIcon As Icon = GetIcon(iFrom, Not useSmall)
        // Dim rightIndex As Integer = iFrom
        // Do While iFrom <= iTo
        // Dim ico As Icon = GetIcon(iFrom, useSmall)
        // Dim fShow As New frmDebugShowImage(rightIndex, RightIcon, ico, IIf(useSmall, "Small ImageList", "Large ImageList"), iFrom)
        // fShow.ShowDialog()
        // fShow.Dispose()
        // iFrom += 1
        // Loop
        // End Sub

        /// <summary>
        /// Returns a GDI+ copy of the icon from the ImageList
        /// at the specified index.</summary>
        /// <param name="Index">The IconIndex of the desired Icon</param>
        /// <param name="smallIcon">Optional, default = False. If True, return the
        ///   icon from the Small ImageList rather than the Large.</param>
        /// <returns>The specified Icon or Nothing</returns>
        public static Icon GetIcon(int Index, bool smallIcon = false)
        {
            Initializer();
            Icon icon = default;
            var hIcon = IntPtr.Zero;

            // Customisation to return a small image
            if (smallIcon) hIcon = ImageList_GetIcon(m_smImgList, Index, 0); 
            else hIcon = ImageList_GetIcon(m_lgImgList, Index, 0);

            if (!((object)hIcon == null)) icon = Icon.FromHandle(hIcon);
            return icon;
        }

        // No longer used. Retained for information purposes
        // <summary>
        // public int GetDeskTopIconIndex()
        // Returns the Icon Index for the Desktop. This is not
        // available using the normal methods and the image
        // itself is not placed into the ImageList unless this
        // call is made.
        // </summary>
        // <returns>Returns the Icon Index for the Desktop
        // </returns>
        // Public Shared Function GetDeskTopIconIndex() As Integer
        // Dim ppidl As IntPtr
        // Dim hDum As Integer = 0
        // Dim rVal As Integer

        // rVal = SHGetSpecialFolderLocation(hDum, CSIDL.DESKTOP, ppidl)
        // If rVal = 0 Then
        // Dim dwFlags As Integer = SHGFI.SYSICONINDEX _
        // Or SHGFI.PIDL
        // Dim dwAttr As Integer = 0

        // Dim shfi As SHFILEINFO = New SHFILEINFO()
        // Dim resp As IntPtr
        // resp = SHGetFileInfo(ppidl, _
        // dwAttr, _
        // shfi, _
        // cbFileInfo, _
        // dwFlags)
        // Marshal.FreeCoTaskMem(ppidl)  ' free the pointer
        // If resp.Equals(IntPtr.Zero) Then
        // Debug.Assert(Not resp.Equals(IntPtr.Zero), "Failed to get icon index")
        // rVal = -1  'Failed to get IconIndex
        // Else
        // rVal = shfi.iIcon  'got it, return it
        // End If
        // Else        'failed to get DesktopLocation
        // rVal = -1
        // End If
        // If rVal > -1 Then
        // Return rVal
        // Else
        // Throw New ApplicationException("Failed to get Desktop Icon Index")
        // Return -1
        // End If
        // End Function

        // <summary>
        // Associates a SysImageList with a ListView control
        // </summary>
        // <param name="listView">ListView control to associate ImageList with</param>
        // <param name="forLargeIcons">True=Set Large Icon List
        // False=Set Small Icon List</param>
        // <param name="forStateImages">Whether to add ImageList as StateImageList</param>
        /// <summary>
        /// Summary of SetListViewImageList.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="forLargeIcons"></param>
        /// <param name="forStateImages"></param>
        public static void SetListViewImageList(ListView listView, bool forLargeIcons, bool forStateImages)
        {
            Initializer();
            int wParam = LVSIL_NORMAL;
            var HImageList = m_lgImgList;
            if (!forLargeIcons)
            {
                wParam = LVSIL_SMALL;
                HImageList = m_smImgList;
            }
            if (forStateImages)
            {
                wParam = LVSIL_STATE;
            }
            SendMessage(listView.Handle, LVM_SETIMAGELIST, wParam, HImageList);
        }

        // /// <summary>
        // /// Associates a SysImageList with a TreeView control
        // /// </summary>
        // /// <param name="treeView">TreeView control to associated ImageList with</param>
        // /// <param name="forStateImages">Whether to add ImageList as StateImageList</param>
        /// <summary>
        /// Summary of SetTreeViewImageList.
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="forStateImages"></param>
        public static void SetTreeViewImageList(TreeView treeView, bool forStateImages)
        {
            Initializer();
            int HR = SendMessage(treeView.Handle, TVM_SETIMAGELIST, (forStateImages ? LVSIL_STATE : LVSIL_NORMAL), m_smImgList);
        }
    }
}
