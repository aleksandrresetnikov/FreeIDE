using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace FreeIDE.Controls.TabControl
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    internal class FlatTabControl : System.Windows.Forms.TabControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private TabControlSubClass scUpDown = null;
        private bool bUpDown; // true when the button UpDown is required
        private ImageList leftRightImages = null;
        private const int nMargin = 5;
        private Color mBackColor = SystemColors.Control;

        private Image arrow_Forward;
        private Image arrow_Back;

        public bool DrawBorders { get; set; } = false;
        public bool DrawTopCurtain { get; set; } = false;
        public int BorderSize { get; set; } = 1;
        public Color BorderColor { get; set; } = Color.Red;

        public FlatTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // double buffering
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            bUpDown = false;

            this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(FlatTabControl_SelectedIndexChanged);

            leftRightImages = new ImageList();

            //System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FlatTabControl));
            /*if (resources == null) return;
            arrow_Back = (Bitmap)resources.GetObject("arrow_Back");
            arrow_Forward = (Bitmap)resources.GetObject("arrow_Forward");*/

            /*leftRightImages.Images.Add(arrow_Forward);
            leftRightImages.Images.Add(arrow_Back);
            leftRightImages.Images.Add(arrow_Forward);
            leftRightImages.Images.Add(arrow_Back);*/
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                leftRightImages.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            if (!base.Visible) return;

            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            // fill client area
            Brush br = new SolidBrush(mBackColor);
            g.FillRectangle(br, TabControlArea);
            br.Dispose();

            // draw the top curtain
            if (this.DrawTopCurtain && this.BorderSize > 0)
            {
                using (Pen topCurtain = new Pen(BorderColor, this.BorderSize))
                {
                    //g.DrawRectangle(topCurtain, new Rectangle(TabArea.X, TabArea.Y, TabArea.Width, this.BorderSize));
                    g.DrawLine(topCurtain, 1, 1, 10, 10);
                }
            }

            // draw border
            if (this.DrawBorders && this.BorderSize > 0)
            {
                int nDelta = /*this.BorderSize;*/SystemInformation.BorderSize.Width;

                using (Pen border = new Pen(BorderColor, this.BorderSize))
                {
                    TabArea.Inflate(nDelta, nDelta);
                    //TabArea.Size = new Size(TabArea.Size.Width - this.BorderSize*2, TabArea.Size.Height - this.BorderSize*2);
                    //TabArea.Location = new Point(TabArea.Location.X + this.BorderSize, TabArea.Location.Y + this.BorderSize);
                    g.DrawRectangle(border, TabArea);
                }
            }

            // clip region for drawing tabs
            Region rsaved = g.Clip;
            Rectangle rreg;

            int nWidth = TabArea.Width + nMargin;
            if (bUpDown)
            {
                // exclude updown control for painting
                if (TabControlWin32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    TabControlWin32.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);
                    nWidth = rupdown2.X;
                }
            }

            rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);
            //g.SetClip(rreg);

            // draw tabs
            for (int i = 0; i < this.TabCount; i++) DrawTab(g, this.TabPages[i], i);

            g.Clip = rsaved;

            // draw background to cover flat border areas
            /*if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                border = new Pen(BorderColor);

                TabArea.Offset(1, 1);
                TabArea.Width -= 2;
                TabArea.Height -= 2;

                g.DrawRectangle(border, TabArea);
                TabArea.Width -= 1;
                TabArea.Height -= 1;
                g.DrawRectangle(border, TabArea);

                border.Dispose();
            }*/
        }

        internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
        {
            Rectangle recBounds = this.GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);

            bool bSelected = (this.SelectedIndex == nIndex);

            // fill this tab with background color
            Brush br = new SolidBrush(tabPage.BackColor);
            //g.FillPolygon(br, pt);
            g.FillRectangle(br, recBounds);
            br.Dispose();

            // draw border
            //g.DrawRectangle(BorderColor, recBounds);
            //g.DrawPolygon(BorderColor, pt);

            if (bSelected)
            {
                // clear bottom lines
                Pen pen = new Pen(/*tabPage.BackColor*/BorderColor);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, recBounds.Left + BorderSize, recBounds.Bottom, recBounds.Right - BorderSize, recBounds.Bottom);
                        g.DrawLine(pen, recBounds.Left + BorderSize, recBounds.Bottom + BorderSize, recBounds.Right - BorderSize, recBounds.Bottom + BorderSize);
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + BorderSize, recBounds.Top, recBounds.Right - BorderSize, recBounds.Top);
                        g.DrawLine(pen, recBounds.Left + BorderSize, recBounds.Top - BorderSize, recBounds.Right - BorderSize, recBounds.Top - BorderSize);
                        g.DrawLine(pen, recBounds.Left + BorderSize, recBounds.Top - (BorderSize+1), recBounds.Right - BorderSize, recBounds.Top - (BorderSize+1));
                        break;
                }

                pen.Dispose();
            }

            // draw tab's icon
            if ((tabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[tabPage.ImageIndex] != null))
            {
                int nLeftMargin = 8;
                int nRightMargin = 2;

                Image img = ImageList.Images[tabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

                // adjust rectangles
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
                tabTextArea.Width -= nAdj;

                // draw icon
                g.DrawImage(img, rimage);
            }

            // draw string
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            br = new SolidBrush(tabPage.ForeColor);

            g.DrawString(tabPage.Text, Font, br, tabTextArea, stringFormat);
        }

        internal void DrawIcons(Graphics g)
        {
            if ((leftRightImages == null) || (leftRightImages.Images.Count != 4))
                return;

            // calc positions
            Rectangle TabControlArea = this.ClientRectangle;

            Rectangle r0 = new Rectangle();
            TabControlWin32.GetClientRect(scUpDown.Handle, ref r0);

            Brush br = new SolidBrush(myBackColor);
            g.FillRectangle(br, r0);
            br.Dispose();

            Pen border = new Pen(BorderColor);
            Rectangle rborder = r0;
            rborder.Inflate(-1, -1);
            //g.DrawRectangle(border, rborder);
            border.Dispose();

            int nMiddle = (r0.Width / 2);
            int nTop = (r0.Height - 16) / 2 + 2;
            int nLeft = (nMiddle - 16) / 2 - 1;

            Rectangle r1 = new Rectangle(nLeft, nTop, 16, 16);
            Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop, 16, 16);

            // draw buttons
            Image img = leftRightImages.Images[1];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(0);
                    if (r3.Left < TabControlArea.Left)
                        g.DrawImage(img, r1);
                    else
                    {
                        img = leftRightImages.Images[3];
                        if (img != null)
                            g.DrawImage(img, r1);
                    }
                }
            }

            img = leftRightImages.Images[0];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(this.TabCount - 1);
                    if (r3.Right > (TabControlArea.Width - r0.Width)) g.DrawImage(img, r2);
                    else
                    {
                        img = leftRightImages.Images[2];
                        if (img != null)
                            g.DrawImage(img, r2);
                    }
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            FindUpDown();
        }

        private void FlatTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUpDown();
            Invalidate();   // we need to update border and background colors
        }

        private void FindUpDown()
        {
            bool bFound = false;

            // find the UpDown control
            IntPtr pWnd = TabControlWin32.GetWindow(this.Handle, TabControlWin32.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                // Get the window class name
                char[] className = new char[33];

                int length = TabControlWin32.GetClassName(pWnd, className, 32);
                if (new string(className, 0, length) == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        // Subclass it
                        this.scUpDown = new TabControlSubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new TabControlSubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);
                        
                        bUpDown = true;
                    }
                    break;
                }

                pWnd = TabControlWin32.GetWindow(pWnd, TabControlWin32.GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown)) bUpDown = false;
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (TabControlWin32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();

                    TabControlWin32.GetClientRect(scUpDown.Handle, ref rect);
                    TabControlWin32.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }


        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case TabControlWin32.WM_PAINT:
                    // redraw
                    IntPtr hDC = TabControlWin32.GetWindowDC(scUpDown.Handle);
                    Graphics g = Graphics.FromHdc(hDC);

                    DrawIcons(g);

                    g.Dispose();
                    TabControlWin32.ReleaseDC(scUpDown.Handle, hDC);

                    m.Result = IntPtr.Zero;

                    // validate current rect
                    Rectangle rect = new Rectangle();

                    TabControlWin32.GetClientRect(scUpDown.Handle, ref rect);
                    TabControlWin32.ValidateRect(scUpDown.Handle, ref rect);
                    return 1;
                default:
                    return 0;
            }
        }


        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }


        [Editor(typeof(TabpageExCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }

        new public TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                TabAlignment ta = value;
                if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                    ta = TabAlignment.Top;

                base.Alignment = ta;
            }
        }

        [Browsable(false)]
        new public bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; }
        }

        [Browsable(true)]
        new public Color myBackColor
        {
            get { return mBackColor; }
            set { mBackColor = value; this.Invalidate(); }
        }


        internal class TabpageExCollectionEditor : CollectionEditor
        {
            public TabpageExCollectionEditor(Type type) : base(type)
            {
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(TabPage);
            }
        }
    }
}
