using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class HealthBar : GameObject
    {
        public const float AnimationDelayDefault = 0.5f;
        public const float MaxHealthDefault = 100f;
        public const float AnimationDurationDefault = 0.6f;

        private TransformFloat transform;
        private float health;
        private float animationDelayElapsed;

        public float Health {
            get {
                return health;
            }

            set {
                if (animationDelayElapsed >= AnimationDelayDefault)
                    transform.StartValue = health;

                transform.Duration = AnimationDurationDefault;

                if (value > MaxHealth)
                    health = MaxHealth;
                else if (value < 0)
                    health = 0;
                else
                    health = value;

                animationDelayElapsed = 0f;
                transform.EndValue = health;
            }
        }

        public float MaxHealth = MaxHealthDefault;

        public HealthBar() {
            transform = new TransformFloat();
            transform.Easing = EasingTypes.OutQuart;
            transform.Duration = AnimationDurationDefault;
            transform.StartValue = MaxHealth;
            transform.EndValue = MaxHealth;
        }

        public override void OnRender(Graphics g) {
            g.FillRectangle(ColorPalette.BrushLightRed, Rect);
            float width = (Rect.Width / MaxHealth) * Health;
            g.FillRectangle(ColorPalette.BrushLightGreen, new RectangleF(Rect.X, Rect.Y, width, Rect.Height));
            g.FillRectangle(ColorPalette.BrushYellow, new RectangleF(Rect.X + width, Rect.Y, (Rect.Width / MaxHealth) * transform.CurrentProgress - width, Rect.Height));
            g.DrawRectangle(new Pen(Color.White), Rect.X, Rect.Y, Rect.Width, Rect.Height);
        }

        public override void OnUpdate(float delta) {
            animationDelayElapsed += delta;

            if (animationDelayElapsed >= AnimationDelayDefault)
                transform.Update(delta);
        }
    }
}
