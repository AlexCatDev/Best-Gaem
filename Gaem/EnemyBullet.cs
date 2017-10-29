using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaem
{
    public class EnemyBullet : IGameObject
    {
        public RectangleF rect;
        public RectangleF Rect => rect;

        private Game game;

        private float dirX;
        private float dirY;
        private float speed;

        public EnemyBullet(float x, float y, float speed) {
            rect = new RectangleF(x - 9, y - 9, 18, 18);
            this.speed = speed;
        }

        public void OnDestroy() {
            
        }

        public void OnRender(Graphics g) {
            g.FillEllipse(ColorPalette.BrushLightRed, rect);
        }

        public void OnSpawn(Game game) {
            this.game = game;
            Player player = game.GetObjects<Player>().First();

            float angle = (float)Math.Atan2(player.Rect.Y - rect.Y, player.Rect.X - rect.X);
            dirX = (float)Math.Cos(angle);
            dirY = (float)Math.Sin(angle);
        }

        public void OnUpdate(float delta) {
         if(!game.GameArea.IntersectsWith(rect)) {
                game.DestroyObject(this);
                return;
            }

            rect.X += dirX * speed * delta;
            rect.Y += dirY * speed * delta;

        }
    }
}
