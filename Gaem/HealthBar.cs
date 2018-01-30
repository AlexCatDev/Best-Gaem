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
        private TransformFloat transform;
        private float health;

        public float Health {
            get {
                return health;
            }
            set {
                if (transform.IsFinished) {
                    transform.Duration = 0.75f;
                    transform.StartValue = health;
                }

                if (value > MaxHealth)
                    health = MaxHealth;
                else if (value < 0)
                    health = 0;
                else
                    health = value;
                
                transform.EndValue = health;
            }
        }
        public float MaxHealth = 100f;

        public HealthBar() {
            transform = new TransformFloat();
            transform.Easing = EasingTypes.OutCirc;
            transform.Duration = 0.75f;
            transform.StartValue = MaxHealth;
        }

        public void OnDestroy() {
            
        }

        public override void OnRender(Graphics g) {
            g.FillRectangle(ColorPalette.BrushLightRed, Rect);
            float width = (Rect.Width / MaxHealth) * Health;
            g.FillRectangle(ColorPalette.BrushLightGreen, new RectangleF(Rect.X, Rect.Y, width, Rect.Height));
            g.FillRectangle(ColorPalette.BrushYellow, new RectangleF(Rect.X + width, Rect.Y, (Rect.Width / MaxHealth) * transform.CurrentProgress - width, Rect.Height));
            g.DrawRectangle(new Pen(Color.White), Rect.X, Rect.Y, Rect.Width, Rect.Height);
        }

        public override void OnUpdate(float delta) {
            transform.Update(delta);
        }
    }
}
