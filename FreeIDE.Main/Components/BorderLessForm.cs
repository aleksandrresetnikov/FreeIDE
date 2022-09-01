using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;

namespace FreeIDE.Components
{
    public class BorderLessForm : Form, IThemeDesigner
    {
        const UInt32 WM_NCHITTEST = 0x0084;
        const UInt32 WM_MOUSEMOVE = 0x0200;

        const UInt32 HTLEFT = 10;
        const UInt32 HTRIGHT = 11;
        const UInt32 HTBOTTOMRIGHT = 17;
        const UInt32 HTBOTTOM = 15;
        const UInt32 HTBOTTOMLEFT = 16;
        const UInt32 HTTOP = 12;
        const UInt32 HTTOPLEFT = 13;
        const UInt32 HTTOPRIGHT = 14;

        private Int32 _RoundingValue;
        private protected readonly System.Timers.Timer timer = new System.Timers.Timer(1);

        /// <summary>
        /// Border color
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Border color")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color BorderColor { get; set; }

        /// <summary>
        /// Header back color
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Header back color")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color HeaderBackColor { get; set; }

        /// <summary>
        /// Width of area where user can handle window border
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Width of area where user can handle window border")]
        [DefaultValue(4)]
        public int ResizeHandleSize { get; set; }

        /// <summary>
        /// Width of area where user can change window size (right bottom corner)
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Width of area where user can change window size (right bottom corner)")]
        [DefaultValue(10)]
        public int ResizeHandleSizeBottomRight { get; set; }

        /// <summary>
        /// Window can be resized
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Window can be resized")]
        [DefaultValue(true)]
        public bool Resizeable { get; set; }

        /// <summary>
        /// Window can be moved
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Window can be moved")]
        [DefaultValue(true)]
        public bool Moveable { get; set; }

        /// <summary>
        /// Draws grip at right bottom corner
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Draws grip at right bottom corner")]
        [DefaultValue(true)]
        public bool GripVisible { get; set; }

        /// <summary>
        /// Height of top window header
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Height of top window header")]
        [DefaultValue(20)]
        public int HeaderHeight { get; set; }

        /// <summary>
        /// Top header icon height
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Top header icon height")]
        [DefaultValue(20)]
        public int IconHeight { get; set; }

        /// <summary>
        /// Allows to drag form by any part of the window
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Allows to drag form by any part of the window")]
        [DefaultValue(false)]
        public bool MoveOnWholeForm { get; set; }

        /// <summary>
        /// Rounding value
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Rounding value")]
        [DefaultValue(0)]
        public Int32 RoundingValue
        {
            get => this._RoundingValue;
            set
            {
                this._RoundingValue = value;
                SettingRegions();
            }
        }

        /// <summary>
        /// Icon indent
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Icon indent")]
        [DefaultValue(1)]
        public Int32 IconPadding { get; set; }

        /// <summary>
        /// Will there be a line drawn at the bottom of the header
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Will there be a line drawn at the bottom of the header")]
        [DefaultValue(false)]
        public bool ShowHeaderUnderline { get; set; }

        /// <summary>
        /// Line width at the bottom of the header
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Line width at the bottom of the header")]
        [DefaultValue(2)]
        public float WidthHeaderUnderline { get; set; }

        /// <summary>
        /// Header line color
        /// </summary>
        [Browsable(true)]
        [Category("FreeIDE.Dev")]
        [Description("Header line color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ColorHeaderUnderline { get; set; }

        public BorderLessForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(2, 2, 2, 2);
            this.ResizeHandleSize = 4;
            this.ResizeHandleSizeBottomRight = 10;
            this.Resizeable = true;
            this.Moveable = true;
            this.HeaderHeight = 20;
            this.HeaderBackColor = Color.Transparent;
            this.BorderColor = Color.Transparent;
            this.MoveOnWholeForm = false;
            this.IconHeight = 20;
            this.RoundingValue = 0;
            this.IconPadding = 1;
            this.ShowHeaderUnderline = false;
            this.WidthHeaderUnderline = 2;
            this.ColorHeaderUnderline = Color.Black;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Load += Form_Load;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.OnTimerElapsed(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (Resizeable && e.Y < HeaderHeight && ControlBox)
                this.OnHeaderMouseDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (ControlBox)
            {
                if (this.HeaderBackColor != Color.Transparent)
                {
                    using (var brush = new SolidBrush(this.HeaderBackColor))
                        e.Graphics.FillRectangle(brush, 0, 0, this.Width, this.HeaderHeight);
                }

                if (this.ShowIcon)
                    e.Graphics.DrawIcon(this.Icon, new Rectangle(this.ResizeHandleSize + this.IconPadding,
                        this.IconPadding, this.IconHeight, this.IconHeight));

                if (this.ShowHeaderUnderline)
                    e.Graphics.DrawLine(CreateHeaderUnderlinePen(), 0, this.HeaderHeight, this.Width,
                        this.HeaderHeight);
            }

            if (BorderColor != Color.Transparent)
            {
                using (var pen = new Pen(BorderColor))
                    e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);

                if (this.SizeGripStyle != SizeGripStyle.Hide && this.Resizeable)
                    using (var pen = new Pen(this.BorderColor, 5))
                        e.Graphics.DrawLine(pen, this.Width - 4, this.Height, this.Width, this.Height - 4);
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.OnMouseMoveMethod(e);
            base.OnMouseMove(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (!Resizeable)
            {
                base.WndProc(ref m);
                return;
            }

            bool handled = false;
            if (m.Msg == WM_NCHITTEST || m.Msg == WM_MOUSEMOVE)
            {
                Size formSize = this.Size;
                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = this.PointToClient(screenPoint);

                var boxes = new Dictionary<UInt32, Rectangle>() {
                    { HTBOTTOMRIGHT, new Rectangle(formSize.Width - ResizeHandleSizeBottomRight, formSize.Height - ResizeHandleSizeBottomRight, ResizeHandleSizeBottomRight, ResizeHandleSizeBottomRight) },
                    { HTBOTTOMLEFT, new Rectangle(0, formSize.Height - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize) },
                    { HTBOTTOM, new Rectangle(ResizeHandleSize, formSize.Height - ResizeHandleSize, formSize.Width - 2*ResizeHandleSize, ResizeHandleSize) },
                    { HTRIGHT, new Rectangle(formSize.Width - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize, formSize.Height - 2*ResizeHandleSize) },
                    { HTTOPRIGHT, new Rectangle(formSize.Width - ResizeHandleSize, 0, ResizeHandleSize, ResizeHandleSize) },
                    { HTTOP, new Rectangle(ResizeHandleSize, 0, formSize.Width - 2*ResizeHandleSize, ResizeHandleSize) },
                    { HTTOPLEFT, new Rectangle(0, 0, ResizeHandleSize, ResizeHandleSize) },
                    { HTLEFT, new Rectangle(0, ResizeHandleSize, ResizeHandleSize, formSize.Height - 2*ResizeHandleSize) }
                };

                foreach (KeyValuePair<UInt32, Rectangle> hitBox in boxes)
                {
                    if (hitBox.Value.Contains(clientPoint))
                    {
                        m.Result = (IntPtr)hitBox.Key;
                        handled = true;
                        break;
                    }
                }
            }

            if (!handled)
                base.WndProc(ref m);
        }

        private Pen CreateHeaderUnderlinePen() =>
            new Pen(this.ColorHeaderUnderline, this.WidthHeaderUnderline);

        private protected virtual void SettingRegions()
        {
            if (this.RoundingValue <= 0) return;
            this.Region = Region.FromHrgn(FormDisigner.CreateRoundRectRgn(0, 0, this.Width,
                this.Height, this.RoundingValue, this.RoundingValue));
        }

        private protected virtual void OnMouseMoveMethod(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                if (this.Moveable && (e.Y < this.HeaderHeight || this.MoveOnWholeForm))
                    this.OnHeaderMouseMove(e);
        }

        private protected virtual void OnHeaderMouseMove(MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            SettingRegions();
        }

        private protected virtual void OnHeaderMouseDoubleClick(MouseEventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Maximized ?
                this.WindowState = FormWindowState.Normal : this.WindowState = FormWindowState.Maximized);
        }

        private protected virtual void OnTimerElapsed(ElapsedEventArgs e)
        {
            BeginInvoke((Action)delegate {
                SettingRegions();
            });
        }

        public virtual void Clean()
        {
            this.timer.Stop();
            this.timer.Dispose();
            this.Dispose();
            GC.Collect();
        }

        public virtual void ApplyTheme(ThemeData themeData)
        {

        }

        ~BorderLessForm()
        {
            this.timer.Stop();
            this.timer.Dispose();
            this.Dispose();
            GC.Collect();
        }
    }

    public interface IThemeDesigner
    {
        void ApplyTheme(ThemeData themeData);
    }
}
