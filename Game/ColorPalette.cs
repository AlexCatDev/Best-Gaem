using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class ColorPalette
    {
        public static Color ColorLightRed => Color.FromArgb(231, 76, 60);
        public static Color ColorLightGreen => Color.FromArgb(46, 204, 113);
        public static Color ColorLightBlue => Color.FromArgb(52, 152, 219);

        public static Color ColorDarkBlue => Color.FromArgb(41, 128, 185);

        public static Color ColorFlatDark => Color.FromArgb(32, 32, 32);

        public static Color ColorYellow => Color.FromArgb(241, 196, 15);

        public static SolidBrush BrushLightRed { get; private set; }
        public static SolidBrush BrushLightGreen { get; private set; }
        public static SolidBrush BrushLightBlue { get; private set; }
        public static SolidBrush BrushDarkBlue { get; private set; }
        public static SolidBrush BrushYellow { get; private set; }

        public static void Initialize() {
            BrushYellow = new SolidBrush(ColorYellow);
            BrushLightRed = new SolidBrush(ColorLightRed);
            BrushLightGreen = new SolidBrush(ColorLightGreen);
            BrushLightBlue = new SolidBrush(ColorLightBlue);
            BrushDarkBlue = new SolidBrush(ColorDarkBlue);
        }
    }
}
