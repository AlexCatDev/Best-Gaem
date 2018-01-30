using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaem
{
    public class EnemyBullet : GameObject
    {
        private float dirX;
        private float dirY;
        private float speed;

        public EnemyBullet(float x, float y, float speed) {
            Rect = new RectangleF(x - 9, y - 9, 18, 18);
            this.speed = speed;

            Player player = Game.Instance.GetObject<Player>();

            float angle = (float)Math.Atan2(player.Rect.Y - Rect.Y, player.Rect.X - Rect.X);
            dirX = (float)Math.Cos(angle);
            dirY = (float)Math.Sin(angle);
        }

        public override void OnRender(Graphics g) {
            g.FillEllipse(ColorPalette.BrushLightRed, Rect);
        }

        public override void OnUpdate(float delta) {
         if(!Game.Instance.GameArea.IntersectsWith(Rect)) {
                Game.Instance.DestroyObject(this);
                return;
            }

            Rect.X += dirX * speed * delta;
            Rect.Y += dirY * speed * delta;

        }
    }
}
