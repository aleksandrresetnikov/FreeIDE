using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using FreeIDE.Common.WinShell;

namespace FreeIDE.Controls
{
    public partial class ColorView : UserControl
    {
        private ToolTip toolTip = new ToolTip();

        private Color _BorderColor = Color.White;
        private Color _ElementColor = Color.DimGray;
        private bool _ShowCopyMessage = true;
        private int _CopyMessageTime = 1000;
        private bool _CopyColor = true;

        [Browsable(true)]
        [Category("Appearance")]
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                _BorderColor = value;
                this.BackColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color ElementColor
        {
            get => _ElementColor;
            set
            {
                _ElementColor = value;
                colorViever.BackColor = value;
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public bool ShowCopyMessage
        {
            get => _ShowCopyMessage;
            set => _ShowCopyMessage = value;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public int CopyMessageTime
        {
            get => _CopyMessageTime;
            set
            {
                _CopyMessageTime = value;
                timerHideToolTip.Interval = _CopyMessageTime;
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public bool CopyColor
        {
            get => _CopyColor;
            set => _CopyColor = value;
        }

        public ColorView()
        {
            InitializeComponent();


            this.toolTip = new ToolTip();

            this.toolTip.AutoPopDelay = 0;
            this.toolTip.InitialDelay = 0;
            this.toolTip.ReshowDelay = 0;

            this.toolTip.ShowAlways = true;
        }

        private void LPI_ColorView_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_CopyColor) return;

            copyColor(e);
        }
        private void colorViever_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_CopyColor) return;

            copyColor(e);
        }

        private void copyColor(MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Keys.ControlKey))
                copyColor_HSB(e);
            else
                copyColor_RGB(e);
        }

        private void copyColor_RGB(MouseEventArgs e)
        {
            Clipboard.SetText($"{_ElementColor.R.ToString()};{_ElementColor.G.ToString()};{_ElementColor.B.ToString()}");

            if (_ShowCopyMessage)
            {
                this.toolTip.Show("Цвет скопирован (RGB)", this, e.X, e.Y);
                timerHideToolTip.Enabled = true;
            }
        }
        private void copyColor_HSB(MouseEventArgs e)
        {
            Clipboard.SetText($"{((int)_ElementColor.GetHue()).ToString()};{((int)_ElementColor.GetSaturation()).ToString()};{((int)_ElementColor.GetBrightness()).ToString()}");

            if (_ShowCopyMessage)
            {
                this.toolTip.Show("Цвет скопирован (HSB)", this, e.X, e.Y);
                timerHideToolTip.Enabled = true;
            }
        }

        private void timerHideToolTip_Tick(object sender, EventArgs e)
        {
            this.toolTip.Hide(this);
            timerHideToolTip.Enabled = false;
        }
    }
}
