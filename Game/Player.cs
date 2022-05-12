using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Game
{
    public class Player : GameObject
    {
        float speed;

        float fireRate;
        float shootTimer;

        float dashTimer = 0;

        float guideLength = 150f;

        public Player()
        {
            Rect = new RectangleF(100, 200, 48, 48);
            speed = 800f;
            shootTimer = 0;
            fireRate = 0.4f;
        }


        public override void OnRender(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(ColorPalette.BrushLightBlue, Rect);
            PointF point1 = new PointF(Rect.X + Rect.Width / 2f, Rect.Y + Rect.Height / 2f);

            float angle = (float)Math.Atan2(Game.Instance.CursorPosition.Y - Rect.Y, Game.Instance.CursorPosition.X - Rect.X);

            PointF point2 = new PointF(Rect.X + (float)Math.Cos(angle) * guideLength, Rect.Y + (float)Math.Sin(angle) * guideLength);
            g.DrawLine(new Pen(ColorPalette.BrushLightBlue), point1, point2);

            g.DrawString("Speed: " + speed, HUD.Instance.font, Brushes.White, new PointF(200, 200));
        }

        public override void OnUpdate(float delta)
        {
            shootTimer += delta;
            dashTimer += delta;

            if (Input.GetKey(Keys.W))
                Rect.Y -= speed * delta;

            if (Input.GetKey(Keys.S))
                Rect.Y += speed * delta;

            if (Input.GetKey(Keys.D))
                Rect.X += speed * delta;

            if (Input.GetKey(Keys.A))
                Rect.X -= speed * delta;

            if (Input.GetKey(Keys.H))
                HUD.Instance.AddHealth(1);

            if (Input.GetKey(Keys.LButton) || Input.GetKey(Keys.Space))
            {
                if (shootTimer >= fireRate)
                {
                    Game.Instance.SpawnObject(new Bullet(Rect.X + Rect.Width / 2f, Rect.Y + Rect.Height / 2f));
                    shootTimer = 0;
                }
            }

            if (Input.GetKey(Keys.LShiftKey))
            {
                if (dashTimer >= 2)
                {

                    dashTimer = 0;
                }
            }

            Game.Instance.GetObjects<Monster>((monster) =>
            {
                if (monster.Rect.IntersectsWith(Rect))
                    HUD.Instance.SubtractHealth(20f * delta);
            });

            Game.Instance.GetObjects<EnemyBullet>((enemyBullet) =>
            {
                if (enemyBullet.Rect.IntersectsWith(Rect))
                {
                    HUD.Instance.SubtractHealth(5f);
                    Game.Instance.DestroyObject(enemyBullet);
                }
            });

            Rect.X = Game.Clamp(Rect.X, 0, Game.Instance.GameArea.Width - Rect.Width);
            Rect.Y = Game.Clamp(Rect.Y, 0, Game.Instance.GameArea.Height - Rect.Height);
        }
    }
}