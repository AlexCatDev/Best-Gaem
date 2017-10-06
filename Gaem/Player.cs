using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class Player : IGameObject
{
    Game game;
    RectangleF rect;
    float speed;

    float fireRate;
    float lastFire;

    float guideLength = 200f;

    public RectangleF Rect {
        get {
            return rect;
        }
    }

    public Player()
    {
        rect = new RectangleF(100,200,48,48);
        speed = 16f;
        lastFire = 0;
        fireRate = 15;
    }


    public void OnRender(Graphics g)
    {
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.FillRectangle(Brushes.LimeGreen, rect);
        PointF point1 = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);

        float angle = (float)Math.Atan2(game.CursorPosition.Y - rect.Y, game.CursorPosition.X - rect.X);

        PointF point2 = new PointF(rect.X + (float)Math.Cos(angle) * guideLength, rect.Y + (float)Math.Sin(angle) * guideLength);
        g.DrawLine(Pens.Blue, point1, point2);
    }

    float elapsed = 0;

    public void OnUpdate(float delta)
    {
        elapsed += delta;

        if (Input.GetKey(Keys.W))
            rect.Y -= speed * delta;

        if (Input.GetKey(Keys.S))
            rect.Y += speed * delta;

        if (Input.GetKey(Keys.D))
            rect.X += speed * delta;

        if (Input.GetKey(Keys.A))
            rect.X -= speed * delta;

        if (Input.GetKey(Keys.H))
            HUD.Instance.AddHealth(1);

        if (Input.GetKey(Keys.LButton)) {
            if(elapsed - lastFire >= fireRate){
                if (HUD.Instance.GetAmmo() > 0) {
                        game.SpawnObject(new Bullet(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f));
                    lastFire = elapsed;
                    HUD.Instance.RemoveAmmo(1);
                }
            }
        }

        var monInsect = game.GetObjects<Monster>();

        if (monInsect.Count > 0) {
            if(monInsect[0].Rect.IntersectsWith(rect))
            HUD.Instance.SubtractHealth(3f*delta);
        }

        rect.X = Game.Clamp(rect.X, 0, game.GameArea.Width - rect.Width);
        rect.Y = Game.Clamp(rect.Y, 0, game.GameArea.Height - rect.Height);
    }

    public void OnSpawn(Game game)
    {
        this.game = game; 
    }

    public void OnDestroy()
    {
        
    }
}