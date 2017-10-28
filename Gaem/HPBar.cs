using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaem
{
    public class HPBar : IGameObject
    {
        public RectangleF rect;

        public RectangleF Rect => rect;

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

        public void OnRender(Graphics g) {
            g.FillRectangle(ColorPalette.BrushLightRed, rect);
            g.FillRectangle(ColorPalette.BrushLightGreen, new RectangleF(rect.X, rect.Y, (rect.Width / MaxHealth) * Health, rect.Height));
            g.DrawRectangle(new Pen(Color.White), rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void OnSpawn(Game game) {
         
        }

        public void OnUpdate(float delta) {
           
        }
    }
}
