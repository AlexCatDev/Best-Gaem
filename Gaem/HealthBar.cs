using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaem
{
    public class HealthBar : GameObject
    {
        private float health;

        public float Health {
            get {
                return health;
            }
            set {
                if (value > MaxHealth)
                    health = MaxHealth;
                else if (value < 0)
                    health = 0;
                else
                    health = value;
            }
        }
        public float MaxHealth = 100f;

        public void OnDestroy() {
            
        }

        public override void OnRender(Graphics g) {
            g.FillRectangle(ColorPalette.BrushLightRed, Rect);
            g.FillRectangle(ColorPalette.BrushLightGreen, new RectangleF(Rect.X, Rect.Y, (Rect.Width / MaxHealth) * Health, Rect.Height));
            g.DrawRectangle(new Pen(Color.White), Rect.X, Rect.Y, Rect.Width, Rect.Height);
        }

        public override void OnUpdate(float delta) {
           
        }
    }
}
