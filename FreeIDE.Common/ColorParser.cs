using System;
using System.Drawing;

namespace FreeIDE.Common
{
    public class ColorParser
    {
        public static readonly Exception NotCompetentTypeException =
            new Exception("The original color type has the wrong appearance, or is not competent in this context.");

        public static Color ParseColor(string Type, string Value)
        {
            try
            {
                switch (Type.ToLower())
                {
                    case "dec": return ParseColorFromDecimal(Value);
                    case "hex": return ParseColorFromHexdecimal(Value);
                    case "rgb": return ParseColorFromRgbScheme(Value);
                    case "str": return ParseColorFromName(Value);
                    default: throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw NotCompetentTypeException;
            }
        }

        public static Color ParseColorFromDecimal(string ValueContext)
        {
            return Color.FromArgb(255, Color.FromArgb(Convert.ToInt32(ValueContext)));
        }

        public static Color ParseColorFromHexdecimal(string ValueContext)
        {
            return ColorTranslator.FromHtml(ValueContext);
        }

        public static Color ParseColorFromRgbScheme(string ValueContext)
        {
            string[] colorValueItems = ValueContext.Replace(" ", "").Split(',');
            return Color.FromArgb(Convert.ToByte(colorValueItems[0]),
                Convert.ToByte(colorValueItems[1]), Convert.ToByte(colorValueItems[2]));
        }

        public static Color ParseColorFromName(string ValueContext)
        {
            return Color.FromName(ValueContext);
        }
    }
}
