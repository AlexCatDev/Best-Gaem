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
        rect = new RectangleF(100,200,64,64);
        speed = 8f;
        lastFire = 0;
        //Every 375 milliseconds
        fireRate = 10;
    }


    public void OnRender(Graphics g)
    {
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.FillRectangle(Brushes.Red, rect);
    }

    float elapsed = 0;

    public void OnUpdate(float delta)
    {
        elapsed += delta;

        if (Input.GetKey(Keys.Up))
            rect.Y -= speed * delta;

        if (Input.GetKey(Keys.Down))
            rect.Y += speed * delta;

        if (Input.GetKey(Keys.Right))
            rect.X += speed * delta;

        if (Input.GetKey(Keys.Left))
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

        var monInsect = game.GetSpecific<Monster>();

        if (monInsect.Count > 0) {
            if(monInsect[0].Rect.IntersectsWith(rect))
            HUD.Instance.SubtractHealth(1f);
        }

    }

    public void OnSpawn(Game game)
    {
        this.game = game; 
    }

    public void OnDestroy()
    {
        
    }
}