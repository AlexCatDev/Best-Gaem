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
        //Every 375 milliseconds
        fireRate = 10;
    }


    public void OnRender(Graphics g)
    {
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.FillRectangle(Brushes.LimeGreen, rect);
        g.DrawLine(Pens.Blue, new PointF(Rect.X - Rect.Width / 2f, Rect.Y - Rect.Width / 2f), new PointF(Rect.X + 10, Rect.Y + 10));
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

        if (Input.GetKey(Keys.Space)) {
            if(elapsed - lastFire >= fireRate){
                if (HUD.Instance.GetAmmo() > 0) {
                        game.SpawnObject(new Bullet(rect.X + 16, rect.Y));
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