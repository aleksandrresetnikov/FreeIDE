using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FreeIDE.Common
{
    public partial class ShellDll
    {
        public const int MAX_PATH = 260;
        public const int FILE_ATTRIBUTE_NORMAL = 0x80;
        public const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
        public const int NOERROR = 0;
        public const int S_OK = 0;
        public const int S_FALSE = 1;
        
        [Flags()]
        public enum SFGAO
        {
            CANCOPY = 0x1,                    // Objects can be copied    
            CANMOVE = 0x2,                    // Objects can be moved     
            CANLINK = 0x4,                    // Objects can be linked    
            STORAGE = 0x8,                    // supports BindToObject(IID_IStorage)
            CANRENAME = 0x10,                 // Objects can be renamed
            CANDELETE = 0x20,                 // Objects can be deleted
            HASPROPSHEET = 0x40,              // Objects have property sheets
            DROPTARGET = 0x100,               // Objects are drop target
            CAPABILITYMASK = 0x177,           // This flag is a mask for the capability flags.
            ENCRYPTED = 0x2000,               // object is encrypted (use alt color)
            ISSLOW = 0x4000,                  // 'slow' object
            GHOSTED = 0x8000,                 // ghosted icon
            LINK = 0x10000,                   // Shortcut (link)
            SHARE = 0x20000,                  // shared
            RDONLY = 0x40000,                 // read-only
            HIDDEN = 0x80000,                 // hidden object
            DISPLAYATTRMASK = 0xFC000,        // This flag is a mask for the display attributes.
            FILESYSANCESTOR = 0x10000000,     // may contain children with FILESYSTEM
            FOLDER = 0x20000000,              // support BindToObject(IID_IShellFolder)
            FILESYSTEM = 0x40000000,          // is a win32 file system object (file/folder/root)
            HASSUBFOLDER = int.MinValue + 0x00000000,        // may contain children with FOLDER
            CONTENTSMASK = int.MinValue + 0x00000000,        // This flag is a mask for the contents attributes.
            VALIDATE = 0x1000000,             // invalidate cached information
            REMOVABLE = 0x2000000,            // is this removeable media?
            COMPRESSED = 0x4000000,           // Object is compressed (use alt color)
            BROWSABLE = 0x8000000,            // supports IShellFolder but only implements CreateViewObject() (non-folder view)
            NONENUMERATED = 0x100000,         // is a non-enumerated object
            NEWCONTENT = 0x200000,            // should show bold in explorer tree
            CANMONIKER = 0x400000,            // defunct
            HASSTORAGE = 0x400000,            // defunct
            STREAM = 0x400000,                // supports BindToObject(IID_IStream)
            STORAGEANCESTOR = 0x800000,       // may contain children with STORAGE or STREAM
            STORAGECAPMASK = 0x70C50008       // for determining storage capabilities ie for open/save semantics
        }
        
        [Flags()]
        public enum SHGFI
        {
            ICON = 0x100,                // get icon 
            DISPLAYNAME = 0x200,         // get display name 
            TYPENAME = 0x400,            // get type name 
            ATTRIBUTES = 0x800,          // get attributes 
            ICONLOCATION = 0x1000,       // get icon location 
            EXETYPE = 0x2000,            // return exe type 
            SYSICONINDEX = 0x4000,       // get system icon index 
            LINKOVERLAY = 0x8000,        // put a link overlay on icon 
            SELECTED = 0x10000,          // show icon in selected state 
            ATTR_SPECIFIED = 0x20000,    // get only specified attributes 
            LARGEICON = 0x0,             // get large icon 
            SMALLICON = 0x1,             // get small icon 
            OPENICON = 0x2,              // get open icon 
            SHELLICONSIZE = 0x4,         // get shell size icon 
            PIDL = 0x8,                  // pszPath is a pidl 
            USEFILEATTRIBUTES = 0x10,    // use passed dwFileAttribute 
            ADDOVERLAYS = 0x20,          // apply the appropriate overlays
            OVERLAYINDEX = 0x40          // Get the index of the overlay
        }
        
        public enum CSIDL : int
        {
            DESKTOP = 0x0,
            INTERNET = 0x1,
            PROGRAMS = 0x2,
            CONTROLS = 0x3,
            PRINTERS = 0x4,
            PERSONAL = 0x5,
            FAVORITES = 0x6,
            STARTUP = 0x7,
            RECENT = 0x8,
            SENDTO = 0x9,
            BITBUCKET = 0xA,
            STARTMENU = 0xB,
            MYDOCUMENTS = 0xC,
            MYMUSIC = 0xD,
            MYVIDEO = 0xE,
            DESKTOPDIRECTORY = 0x10,
            DRIVES = 0x11,
            NETWORK = 0x12,
            NETHOOD = 0x13,
            FONTS = 0x14,
            TEMPLATES = 0x15,
            COMMON_STARTMENU = 0x16,
            COMMON_PROGRAMS = 0x17,
            COMMON_STARTUP = 0x18,
            COMMON_DESKTOPDIRECTORY = 0x19,
            APPDATA = 0x1A,
            PRINTHOOD = 0x1B,
            LOCAL_APPDATA = 0x1C,
            ALTSTARTUP = 0x1D,
            COMMON_ALTSTARTUP = 0x1E,
            COMMON_FAVORITES = 0x1F,
            INTERNET_CACHE = 0x20,
            COOKIES = 0x21,
            HISTORY = 0x22,
            COMMON_APPDATA = 0x23,
            WINDOWS = 0x24,
            SYSTEM = 0x25,
            PROGRAM_FILES = 0x26,
            MYPICTURES = 0x27,
            PROFILE = 0x28,
            SYSTEMX86 = 0x29,
            PROGRAM_FILESX86 = 0x2A,
            PROGRAM_FILES_COMMON = 0x2B,
            PROGRAM_FILES_COMMONX86 = 0x2C,
            COMMON_TEMPLATES = 0x2D,
            COMMON_DOCUMENTS = 0x2E,
            COMMON_ADMINTOOLS = 0x2F,
            ADMINTOOLS = 0x30,
            CONNECTIONS = 0x31,
            COMMON_MUSIC = 0x35,
            COMMON_PICTURES = 0x36,
            COMMON_VIDEO = 0x37,
            RESOURCES = 0x38,
            RESOURCES_LOCALIZED = 0x39,
            COMMON_OEM_LINKS = 0x3A,
            CDBURN_AREA = 0x3B,
            COMPUTERSNEARME = 0x3D,
            FLAG_PER_USER_INIT = 0x800,
            FLAG_NO_ALIAS = 0x1000,
            FLAG_DONT_VERIFY = 0x4000,
            FLAG_CREATE = 0x8000,
            FLAG_MASK = 0xFF00
        }
        
        [Flags()]
        private enum E_STRRET
        {
            @int,
            WSTR = 0x0,          // Use STRRET.pOleStr
            OFFSET = 0x1,        // Use STRRET.uOffset to Ansi
            C_STR = 0x2          // Use STRRET.cStr
        }
        
        [Flags()]
        public enum SHCONTF
        {
            EMPTY = 0,                      // used to zero a SHCONTF variable
            FOLDERS = 0x20,                 // only want folders enumerated (FOLDER)
            NONFOLDERS = 0x40,              // include non folders
            INCLUDEHIDDEN = 0x80,           // show items normally hidden
            INIT_ON_FIRST_NEXT = 0x100,     // allow EnumObject() to return before validating enum
            NETPRINTERSRCH = 0x200,         // hint that client is looking for printers
            SHAREABLE = 0x400,              // hint that client is looking sharable resources (remote shares)
            STORAGE = 0x800                 // include all items with accessible storage and their ancestors
        }
        
        [Flags()]
        public enum SHGDN
        {
            NORMAL = 0,
            INFOLDER = 1,
            FORADDRESSBAR = 16384,
            FORPARSING = 32768
        }
        
        // /// <summary>
        // /// Flags controlling how the Image List item is 
        // /// drawn
        // /// </summary>
        // [Flags]	
        // Public Enum ImageListDrawItemConstants : int
        // {
        // /// <summary>
        // /// Draw item normally.
        // /// </summary>
        // ILD_NORMAL = 0x0,
        // /// <summary>
        // /// Draw item transparently.
        // /// </summary>
        // ILD_TRANSPARENT = 0x1,
        // /// <summary>
        // /// Draw item blended with 25% of the specified foreground colour
        // /// or the Highlight colour if no foreground colour specified.
        // /// </summary>
        // ILD_BLEND25 = 0x2,
        // /// <summary>
        // /// Draw item blended with 50% of the specified foreground colour
        // /// or the Highlight colour if no foreground colour specified.
        // /// </summary>
        // ILD_SELECTED = 0x4,
        // /// <summary>
        // /// Draw the icon's mask
        // /// </summary>
        // ILD_MASK = 0x10,
        // /// <summary>
        // /// Draw the icon image without using the mask
        // /// </summary>
        // ILD_IMAGE = 0x20,
        // /// <summary>
        // /// Draw the icon using the ROP specified.
        // /// </summary>
        // ILD_ROP = 0x40,
        // /// <summary>
        // /// Preserves the alpha channel in dest. XP only.
        // /// </summary>
        // ILD_PRESERVEALPHA = 0x1000,
        // /// <summary>
        // /// Scale the image to cx, cy instead of clipping it.  XP only.
        // /// </summary>
        // ILD_SCALE = 0x2000,
        // /// <summary>
        // /// Scale the image to the current DPI of the display. XP only.
        // /// </summary>
        // ILD_DPISCALE = 0x4000
        // /// <summary>
        // /// Flags controlling how the Image List item is 
        // /// drawn
        // /// </summary>
        [Flags()]
        public enum ILD
        {
            NORMAL = 0x0,
            TRANSPARENT = 0x1,
            BLEND25 = 0x2,
            SELECTED = 0x4,
            MASK = 0x10,
            IMAGE = 0x20,
            ROP = 0x40,
            PRESERVEALPHA = 0x1000,
            SCALE = 0x2000,
            DPISCALE = 0x4000
        }

        // /// <summary>
        // /// Enumeration containing XP ImageList Draw State options
        // /// </summary>

        // /// <summary>
        // /// The image state is not modified. 
        // /// </summary>
        // ILS_NORMAL = (0x00000000),
        // /// <summary>
        // /// Adds a glow effect to the icon, which causes the icon to appear to glow 
        // /// with a given color around the edges. (Note: does not appear to be
        // /// implemented)
        // /// </summary>
        // ILS_GLOW = (0x00000001), //The color for the glow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
        // /// <summary>
        // /// Adds a drop shadow effect to the icon. (Note: does not appear to be
        // /// implemented)
        // /// </summary>
        // ILS_SHADOW = (0x00000002), //The color for the drop shadow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
        // /// <summary>
        // /// Saturates the icon by increasing each color component 
        // /// of the RGB triplet for each pixel in the icon. (Note: only ever appears
        // /// to result in a completely unsaturated icon)
        // /// </summary>
        // ILS_SATURATE = (0x00000004), // The amount to increase is indicated by the frame member in the IMAGELISTDRAWPARAMS method. 
        // /// <summary>
        // /// Alpha blends the icon. Alpha blending controls the transparency 
        // /// level of an icon, according to the value of its alpha channel. 
        // /// (Note: does not appear to be implemented).
        // /// </summary>
        // ILS_ALPHA = (0x00000008) //The value of the alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255, with 0 being completely transparent, and 255 being completely opaque. 
        public enum ILS
        {
            NORMAL = 0x0,      // The image state is not modified.
            GLOW = 0x1,        // The color for the glow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
            SHADOW = 0x2,      // The color for the drop shadow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
            SATURATE = 0x4,    // The amount to increase is indicated by the frame member in the IMAGELISTDRAWPARAMS method. 
            ALPHA = 0x8        // The value of the alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255 with 0 being completely transparent and 255 being completely opaque. 
        }

        [Flags()]
        public enum SLR
        {
            NO_UI = 0x1,
            ANY_MATCH = 0x2,
            UPDATE = 0x4,
            NOUPDATE = 0x8,
            NOSEARCH = 0x10,
            NOTRACK = 0x20,
            NOLINKINFO = 0x40,
            INVOKE_MSI = 0x80,
            NO_UI_WITH_MSG_PUMP = 0x101
        }

        [Flags()]
        public enum SLGP
        {
            SHORTPATH = 0x1,
            UNCPRIORITY = 0x2,
            RAWPATH = 0x4
        }

        [Flags()]
        public enum SHGNLI
        {
            PIDL = 1,        // pszLinkTo is a pidl
            PREFIXNAME = 2,  // Make name "Shortcut to xxx"
            NOUNIQUE = 4,    // don't do the unique name generation
            NOLNK = 8        // don't add ".lnk" extension (Win2k or higher,IE5 or higher)
        }

        public static Guid IID_IMalloc = new Guid("{00000002-0000-0000-C000-000000000046}");
        public static Guid IID_IShellFolder = new Guid("{000214E6-0000-0000-C000-000000000046}");
        public static Guid IID_IFolderFilterSite = new Guid("{C0A651F5-B48B-11d2-B5ED-006097C686F6}");
        public static Guid IID_IFolderFilter = new Guid("{9CC22886-DC8E-11d2-B1D0-00C04F8EEB3E}");
        public static Guid DesktopGUID = new Guid("{00021400-0000-0000-C000-000000000046}");
        public static Guid CLSID_ShellLink = new Guid("{00021401-0000-0000-C000-000000000046}");
        public static Guid CLSID_InternetShortcut = new Guid("{FBF23B40-E3F0-101B-8488-00AA003E56F8}");
        public static Guid IID_IDropTarget = new Guid("{00000122-0000-0000-C000-000000000046}");
        public static Guid IID_IDataObject = new Guid("{0000010e-0000-0000-C000-000000000046}");


        ///<Summary>
        /// SHFILEINFO structure for VB.Net
        ///</Summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public partial struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        private static SHFILEINFO shfitmp;   // just used for the following
        public static int cbFileInfo = Marshal.SizeOf(shfitmp.GetType());

        // both of these formats work in main thread, neither in worker thread
        // <StructLayout(LayoutKind.Sequential)> _
        // Public Structure STRRET
        // Public uType As Integer
        // Public pOle As IntPtr
        // End Structure
        [StructLayout(LayoutKind.Explicit)]
        public partial struct STRRET
        {
            [FieldOffset(0)]
            public int uType; // One of the STRRET_* values
            [FieldOffset(4)]
            public int pOleStr; // must be freed by caller of GetDisplayNameOf
            [FieldOffset(4)]
            public int uOffset; // Offset into SHITEMID
            [FieldOffset(4)]
            public int pStr; // NOT USED
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        public partial struct WIN32_FIND_DATA
        {
            public int dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;      // update to remove warning messages 2/2/2011
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;    // update to remove warning messages 2/2/2011
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;     // update to remove warning messages 2/2/2011
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]

            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]

            public string cAlternateFileName;
        }


        /// <Summary>
        /// Get an Imalloc Interface
        /// Not required for .Net apps, use Marshal class
        /// </Summary>
        [DllImport("shell32", CharSet = CharSet.Auto)]
        public static extern int SHGetMalloc(ref IMalloc pMalloc);


        /// <Summary>
        /// Retrieves the IShellFolder interface for the desktop folder, 
        /// which is the root of the Shell's namespace. 
        /// <param>
        /// ppshf -- Recieves the IShellFolder interface for the desktop folder
        /// </param>
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHGetDesktopFolder(ref IShellFolder ppshf);

        [DllImport("Shell32")]
        public static extern int SHGetSpecialFolderLocation(int hWndOwner, int csidl, ref IntPtr ppidl);


        // SHGetFileInfo
        // Retrieves information about an object in the file system,
        // such as a file, a folder, a directory, or a drive root.

        /// <Summary>
        /// SHGetFileInfo  - for a given Path as a string
        /// </Summary>
        [DllImport("shell32", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO sfi, int cbsfi, int uFlags);


        /// <Summary>
        /// SHGetFileInfo  - for a given ItemIDList as IntPtr
        /// </Summary>
        [DllImport("shell32", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(IntPtr ppidl, int dwFileAttributes, ref SHFILEINFO sfi, int cbsfi, int uFlags);


        /// <Summary>Despite its name, the API returns a filename
        /// for a link to be copied/created in a Target directory,
        /// with a specific LinkTarget. It will create a unique name
        /// unless instructed otherwise (SHGLNI_NOUNIQUE).  And add
        /// the ".lnk" extension, unless instructed otherwise(SHGLNI.NOLNK)
        /// </Summary>
        [DllImport("shell32", EntryPoint = "SHGetNewLinkInfoA", CharSet = CharSet.Ansi)]
        public static extern int SHGetNewLinkInfo(string pszLinkTo, string pszDir, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pszName, ref bool pfMustCopy, SHGNLI uFlags);


        /// <Summary> Same function using a PIDL as the pszLinkTo.
        /// SHGNLI.PIDL must be set.
        /// </Summary>
        [DllImport("shell32", EntryPoint = "SHGetNewLinkInfoA", CharSet = CharSet.Ansi)]
        public static extern int SHGetNewLinkInfo(IntPtr pszLinkTo, string pszDir, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pszName, ref bool pfMustCopy, SHGNLI uFlags);

        [DllImport("shell32", EntryPoint = "#23", CharSet = CharSet.Auto)]
        public static extern bool ILIsParent(IntPtr pidlParent, IntPtr pidlBelow, bool fImmediate);

        [DllImport("shell32", EntryPoint = "#21", CharSet = CharSet.Auto)]
        public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);


        // Accepts a STRRET structure returned by IShellFolder::GetDisplayNameOf that contains or points to a 
        // string, and then returns that string as a BSTR.
        /// <param>
        /// Pointer to a STRRET structure.
        /// Pointer to an ITEMIDLIST uniquely identifying a file object or subfolder relative
        /// Pointer to a variable of type BSTR that contains the converted string.
        /// </param>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern int StrRetToBSTR(ref STRRET pstr, IntPtr pidl, [MarshalAs(UnmanagedType.BStr)] ref string pbstr);


        /// <Summary>
        /// Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf, 
        /// converts it to a string, and 
        /// places the result in a buffer. 
        /// <param>
        /// Pointer to a STRRET structure.
        /// Pointer to an ITEMIDLIST uniquely identifying a file object or subfolder relative
        /// Pointer to a Buffer to hold the display name. It will be returned as a null-terminated
        /// string. If cchBuf is too small, 
        /// the name will be truncated to fit. 
        /// Size of pszBuf, in characters. 
        /// </param>
        /// </Summary>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern int StrRetToBuf(IntPtr pstr, IntPtr pidl, StringBuilder pszBuf, [MarshalAs(UnmanagedType.U4)] int cchBuf);


        /// <Summary>
        /// Sends a message to some Window
        /// </Summary>
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);


        // <Summary>
        // Gets an IconSize from a ImagelistHandle
        // </Summary>
        [DllImport("comctl32")]
        public static extern int ImageList_GetIconSize(IntPtr himl, ref int cx, ref int cy);

        [DllImport("comctl32", CharSet = CharSet.Auto)]
        public static extern int ImageList_ReplaceIcon(IntPtr hImageList, int IconIndex, IntPtr hIcon);

        [DllImport("comctl32")]
        public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int flags);

        [DllImport("comctl32")]
        public static extern int ImageList_Draw(IntPtr hIml, int indx, IntPtr hdcDst, int x, int y, int fStyle);

        [DllImport("user32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential)]
        private partial struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        public partial struct POINT
        {
            public int x;
            public int y;
        }
        [ComImport()]
        [Guid("00000000-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public partial interface IUnknown
        {
            [PreserveSig()]
            int QueryInterface(ref Guid riid, ref IntPtr pVoid);
            [PreserveSig()]
            int AddRef();
            [PreserveSig()]
            int Release();
        }

        // Not needed in .Net - use Marshal Class
        [ComImport()]
        [Guid("00000002-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public partial interface IMalloc
        {
            // Allocates a block of memory.
            // Return value: a pointer to the allocated memory block.
            [PreserveSig()]
            IntPtr Alloc(int cb);
            // Size, in bytes, of the memory block to be allocated.

            // Changes the size of a previously allocated memory block.
            // Return value:  Reallocated memory block 
            [PreserveSig()]
            IntPtr Realloc(IntPtr pv, int cb);

            // Frees a previously allocated block of memory.
            [PreserveSig()]
            void Free(IntPtr pv); // Pointer to the memory block to be freed.

            // This method returns the size (in bytes) of a memory block previously allocated with 
            // IMalloc::Alloc or IMalloc::Realloc.
            // Return value: The size of the allocated memory block in bytes 
            [PreserveSig()]
            int GetSize(IntPtr pv); // Pointer to the memory block for which the size is requested.

            // This method determines whether this allocator was used to allocate the specified block of memory.
            // Return value: 1 - allocated 0 - not allocated by this IMalloc instance. 
            [PreserveSig()]
            short DidAlloc(IntPtr pv);
            // Pointer to the memory block

            // This method minimizes the heap as much as possible by releasing unused memory to the operating system, 
            // coalescing adjacent free blocks and committing free pages.
            [PreserveSig()]
            void HeapMinimize();
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        public partial interface IShellFolder
        {
            [PreserveSig()]
            int ParseDisplayName(int hwndOwner, IntPtr pbcReserved, [MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName, ref int pchEaten, ref IntPtr ppidl, ref int pdwAttributes);

            [PreserveSig()]
            int EnumObjects(int hwndOwner, [MarshalAs(UnmanagedType.U4)] SHCONTF grfFlags, ref IEnumIDList ppenumIDList);

            [PreserveSig()]
            int BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, ref IShellFolder ppvOut);

            [PreserveSig()]
            int BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);

            [PreserveSig()]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

            [PreserveSig()]
            int CreateViewObject(IntPtr hwndOwner, ref Guid riid, ref IntPtr ppvOut);

            // ByRef ppvOut As IUnknown) As Integer 'Old Version
            [PreserveSig()]
            int GetAttributesOf(int cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref SFGAO rgfInOut);

            [PreserveSig()]
            int GetUIObjectOf(IntPtr hwndOwner, int cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref Guid riid, ref int prgfInOut, ref IUnknown ppvOut);

            // ByRef ppvOut As IDropTarget) As Integer
            [PreserveSig()]
            int GetDisplayNameOf(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] SHGDN uFlags, IntPtr lpName);

            [PreserveSig()]
            int SetNameOf(int hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, [MarshalAs(UnmanagedType.U4)] SHCONTF uFlags, ref IntPtr ppidlOut);
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        public partial interface IEnumIDList
        {
            [PreserveSig()]
            int GetNext(int celt, ref IntPtr rgelt, ref int pceltFetched);

            [PreserveSig()]
            int Skip(int celt);

            [PreserveSig()]
            int Reset();

            [PreserveSig()]
            int Clone(ref IEnumIDList ppenum);
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("0000010B-0000-0000-C000-000000000046")]
        public partial interface IPersistFile
        {
            void GetClassID(out Guid pClassID); // Inheirited from Ipersist

            // IPersistFile Interfaces
            [PreserveSig()]
            int IsDirty();

            int Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);
            int Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
            int SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
            int GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
        }

        // We define the Ansi version since all Win OSs (95 thru XP) support it
        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214EE-0000-0000-C000-000000000046")]
        public partial interface IShellLink
        {
            int GetPath([MarshalAs(UnmanagedType.LPStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATA pfd, SLGP fFlags);
            int GetIDList(ref IntPtr ppidl);
            int SetIDList(IntPtr pidl);
            int GetDescription([MarshalAs(UnmanagedType.LPStr)] StringBuilder pszName, int cchMaxName);
            int SetDescription([MarshalAs(UnmanagedType.LPStr)] string pszName);
            int GetWorkingDirectory([MarshalAs(UnmanagedType.LPStr)] StringBuilder pszDir, int cchMaxPath);
            int SetWorkingDirectory([MarshalAs(UnmanagedType.LPStr)] string pszDir);
            int GetArguments([MarshalAs(UnmanagedType.LPStr)] StringBuilder pszArgs, int cchMaxPath);
            int SetArguments([MarshalAs(UnmanagedType.LPStr)] string pszArgs);
            int GetHotkey(ref short pwHotkey);
            int SetHotkey(short wHotkey);
            int GetShowCmd(ref int piShowCmd);
            int SetShowCmd(int iShowCmd);
            int GetIconLocation([MarshalAs(UnmanagedType.LPStr)] StringBuilder pszIconPath, int cchIconPath, ref int piIcon);
            int SetIconLocation([MarshalAs(UnmanagedType.LPStr)] string pszIconPath, int iIcon);
            int SetRelativePath([MarshalAs(UnmanagedType.LPStr)] string pszPathRel, int dwReserved);
            int Resolve(IntPtr hwnd, SLR fFlags);
            int SetPath([MarshalAs(UnmanagedType.LPStr)] string pszFile);
        }

        // UINT RegisterClipboardFormat(LPCTSTR lpszFormat)
        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern int RegisterClipboardFormat(string lpszFormat);

        [DllImport("ole32.dll", CharSet = CharSet.Auto)]
        public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

        [DllImport("ole32.dll", CharSet = CharSet.Auto)]
        public static extern int RegisterDragDrop(IntPtr hWnd, IDropTarget IdropTgt);

        [DllImport("ole32.dll", CharSet = CharSet.Auto)]
        public static extern int RevokeDragDrop(IntPtr hWnd);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int DragQueryFile(IntPtr hDrop, int iFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFile, int cch);

        public enum CF
        {
            TEXT = 1,
            BITMAP = 2,
            METAFILEPICT = 3,
            SYLK = 4,
            DIF = 5,
            TIFF = 6,
            OEMTEXT = 7,
            DIB = 8,
            PALETTE = 9,
            PENDATA = 10,
            RIFF = 11,
            WAVE = 12,
            UNICODETEXT = 13,
            ENHMETAFILE = 14,
            HDROP = 15,
            LOCALE = 16,
            MAX = 17,
            OWNERDISPLAY = 0x80,
            DSPTEXT = 0x81,
            DSPBITMAP = 0x82,
            DSPMETAFILEPICT = 0x83,
            DSPENHMETAFILE = 0x8E,
            PRIVATEFIRST = 0x200,
            PRIVATELAST = 0x2FF,
            GDIOBJFIRST = 0x300,
            GDIOBJLAST = 0x3FF
        }

        [Flags()]
        public enum DVASPECT
        {
            CONTENT = 1,
            THUMBNAIL = 2,
            ICON = 4,
            DOCPRINT = 8
        }

        [Flags()]
        public enum TYMED
        {
            HGLOBAL = 1,
            FILE = 2,
            ISTREAM = 4,
            ISTORAGE = 8,
            GDI = 16,
            MFPICT = 32,
            ENHMF = 64,
            NULL = 0
        }

        [Flags()]
        public enum ADVF
        {
            NODATA = 1,
            PRIMEFIRST = 2,
            ONLYONCE = 4,
            DATAONSTOP = 64,
            CACHE_NOHANDLER = 8,
            CACHE_FORCEBUILTIN = 16,
            CACHE_ONSAVE = 32
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public partial struct FORMATETC
        {
            public CF cfFormat;
            public IntPtr ptd;
            public DVASPECT dwAspect;
            public int lindex;
            public TYMED Tymd;
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct STGMEDIUM
        {
            public int tymed;
            public IntPtr hGlobal;
            public IntPtr pUnkForRelease;
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct DROPFILES
        {
            public int pFiles;
            public POINT pt;
            public bool fNC;
            public bool fWide;
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("00000103-0000-0000-C000-000000000046")]
        public partial interface IEnumFORMATETC
        {
            [PreserveSig()]
            int GetNext(int celt, ref FORMATETC rgelt, ref int pceltFetched);

            [PreserveSig()]
            int Skip(int celt);

            [PreserveSig()]
            int Reset();

            [PreserveSig()]
            int Clone(ref IEnumFORMATETC ppenum);
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("0000010e-0000-0000-C000-000000000046")]
        public partial interface IDataObject
        {
            [PreserveSig()]
            int GetData(ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium);
            [PreserveSig()]
            int GetDataHere(ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium);
            [PreserveSig()]
            int QueryGetData(ref FORMATETC pformatetc);

            [PreserveSig()]
            int GetCanonicalFormatEtc(FORMATETC pformatetc, ref FORMATETC pformatetcout);
            [PreserveSig()]
            int SetData(ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium, bool frelease);

            [PreserveSig()]
            int EnumFormatEtc(int dwDirection, ref IEnumFORMATETC ppenumFormatEtc);
            [PreserveSig()]
            int DAdvise(ref FORMATETC pformatetc, ADVF advf, IAdviseSink pAdvSink, ref int pdwConnection);

            [PreserveSig()]
            int DUnadvise(int dwConnection);

            [PreserveSig()]
            int EnumDAdvise(ref object ppenumAdvise);
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("0000010f-0000-0000-C000-000000000046")]
        public partial interface IAdviseSink
        {
            void OnDataChange(ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium);
            void OnViewChange(int dwAspect, int lindex);
            void OnRename(IntPtr pmk);
            void OnSave();
            void OnClose();
        }

        [ComImport()]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("00000122-0000-0000-C000-000000000046")]
        public partial interface IDropTarget
        {
            [PreserveSig()]
            int DragEnter(IntPtr pDataObj, int grfKeyState, POINT pt, ref int pdwEffect);

            [PreserveSig()]
            int DragOver(int grfKeyState, POINT pt, ref int pdwEffect);

            [PreserveSig()]
            int DragLeave();

            [PreserveSig()]
            int DragDrop(IntPtr pDataObj, int grfKeyState, POINT pt, ref int pdwEffect);
        }

        public static string GetSpecialFolderPath(IntPtr hWnd, int csidl)
        {
            IntPtr ppidl;
            ppidl = GetSpecialFolderLocation(hWnd, csidl);
            var shfi = new SHFILEINFO();
            int uFlags = (int)(SHGFI.PIDL | SHGFI.DISPLAYNAME | SHGFI.TYPENAME);
            int dwAttr = 0;
            IntPtr res = SHGetFileInfo(ppidl, dwAttr, ref shfi, cbFileInfo, uFlags);
            Marshal.FreeCoTaskMem(ppidl);
            return shfi.szDisplayName + "  (" + shfi.szTypeName + ")";
        }

        public static IntPtr GetSpecialFolderLocation(IntPtr hWnd, int csidl)
        {
            IntPtr rVal = new IntPtr();
            int res = SHGetSpecialFolderLocation(0, csidl, ref rVal);
            return rVal;
        }

        public static bool IsXpOrAbove()
        {
            /*bool rVal = false;
            if (Environment.OSVersion.Version.Major > 5) 
                rVal = true;
            else if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
                rVal = true;
            // if none of the above tests succeed, then return false
            return rVal;*/
            return (Environment.OSVersion.Version.Major > 5) ||
                (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1);
        }
        public static bool Is2KOrAbove()
        {
            return (Environment.OSVersion.Version.Major >= 5);
        }
    }
}
