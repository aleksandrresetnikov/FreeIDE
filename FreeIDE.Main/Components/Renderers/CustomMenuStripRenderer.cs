using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FreeIDE.Components.Renderers
{
    internal class CustomMenuStripRenderer : ToolStripProfessionalRenderer
    {
        // Define titlebar colors.
        private protected Color TitlebarColor = Color.FromArgb(89, 135, 214);
        private protected Color MainColor = Color.FromArgb(39, 40, 34);
        private protected Color ItemSelectedColor = Color.FromArgb(24, 25, 19);
        private protected Color ItemForeColor = Color.White;

        public CustomMenuStripRenderer() 
            : base(new CustomMenuStripColors(Color.FromArgb(39, 40, 34))) 
        { 

        }
        public CustomMenuStripRenderer(Color TitlebarColor, Color MainColor, Color ItemSelectedColor, Color ItemForeColor) 
            : base(new CustomMenuStripColors(MainColor)) 
        { 
            this.TitlebarColor = TitlebarColor; 
            this.MainColor = MainColor;
            this.ItemSelectedColor = ItemSelectedColor;
            this.ItemForeColor = ItemForeColor;
        }

        private void DrawTitleBar(Graphics g, Rectangle rect)
        {
            using (Pen underliningLinePen = new Pen(TitlebarColor))
                for (int step = 0; step < 2; step++)
                    g.DrawLine(underliningLinePen, rect.X, rect.Y + (rect.Height - step),
                        rect.X + rect.Width, rect.Y + (rect.Height - step));
        }

        // This method handles the RenderGrip event.
        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            DrawTitleBar(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
        }

        // This method handles the RenderToolStripBorder event.
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            DrawTitleBar(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
        }

        // This method handles the RenderButtonBackground event.
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

            Color gradientBegin = Color.FromArgb(203, 225, 252);
            Color gradientEnd = Color.FromArgb(125, 165, 224);

            ToolStripButton button = e.Item as ToolStripButton;
            if (button.Pressed || button.Checked)
            {
                gradientBegin = Color.FromArgb(254, 128, 62);
                gradientEnd = Color.FromArgb(255, 223, 154);
            }
            else if (button.Selected)
            {
                gradientBegin = Color.FromArgb(255, 255, 222);
                gradientEnd = Color.FromArgb(255, 203, 136);
            }

            using (Brush b = new LinearGradientBrush(bounds, gradientBegin, gradientEnd, LinearGradientMode.Vertical))
                g.FillRectangle(b, bounds);

            e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, bounds);

            g.DrawLine(SystemPens.ControlDarkDark, bounds.X, bounds.Y, bounds.Width - 1, bounds.Y);
            g.DrawLine(SystemPens.ControlDarkDark, bounds.X, bounds.Y, bounds.X, bounds.Height - 1);

            ToolStrip toolStrip = button.Owner;
            ToolStripButton nextItem = button.Owner.GetItemAt(button.Bounds.X, button.Bounds.Bottom + 1) as ToolStripButton;

            if (nextItem == null)
                g.DrawLine(SystemPens.ControlDarkDark, bounds.X, bounds.Height - 1, bounds.X + bounds.Width - 1, bounds.Height - 1);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

            if (e.Item.Selected)
            {
                Color color = this.ItemSelectedColor;
                using (SolidBrush brush = new SolidBrush(color))
                    e.Graphics.FillRectangle(brush, rect);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(MainColor))
                    e.Graphics.FillRectangle(brush, rect);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.Item.ForeColor = this.ItemForeColor;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
        }
    }
}
