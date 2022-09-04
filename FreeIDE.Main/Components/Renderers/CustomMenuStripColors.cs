using System.Drawing;
using System.Windows.Forms;

namespace FreeIDE.Components.Renderers
{
    public class CustomMenuStripColors : ProfessionalColorTable
    {
        private Color MainColor;

        public CustomMenuStripColors(Color MainColor) => this.MainColor = MainColor;

        public override Color ToolStripDropDownBackground => MainColor;
        public override Color ToolStripContentPanelGradientBegin => MainColor;
        public override Color ToolStripContentPanelGradientEnd => MainColor;
        public override Color MenuItemSelected => MainColor;
        public override Color MenuItemBorder => MainColor;
        public override Color MenuItemSelectedGradientBegin => MainColor;
        public override Color MenuItemSelectedGradientEnd => MainColor;
        public override Color MenuBorder => MainColor;
    }
}
