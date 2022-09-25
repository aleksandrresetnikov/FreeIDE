namespace FreeIDE.Syntax
{
    internal class InvisibleCharsRenderer : FastColoredTextBoxNS.Style
    {
        System.Drawing.Pen pen;

        public InvisibleCharsRenderer(System.Drawing.Pen pen)
        {
            this.pen = pen;
        }

        public override void Draw(System.Drawing.Graphics gr, System.Drawing.Point position, FastColoredTextBoxNS.Range range)
        {
            var tb = range.tb;
            using (System.Drawing.Brush brush = new System.Drawing.SolidBrush(pen.Color))
                foreach (var place in range)
                {
                    switch (tb[place].c)
                    {
                        case ' ':
                            var point = tb.PlaceToPoint(place);
                            point.Offset(tb.CharWidth / 2, tb.CharHeight / 2);
                            gr.DrawLine(pen, point.X, point.Y, point.X + 1, point.Y);
                            break;
                    }

                    if (tb[place.iLine].Count - 1 == place.iChar)
                    {
                        var point = tb.PlaceToPoint(place);
                        point.Offset(tb.CharWidth, 0);
                        gr.DrawString("¶", tb.Font, brush, point);
                    }
                }
        }
    }
}
