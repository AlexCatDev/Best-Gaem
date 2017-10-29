using Gaem;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class Monster : GameObject
{   
    float velX;
    Random r = new Random();
    Font font;
    HealthBar healthBar;
    float lastFire;


    public Monster()
    {
        healthBar = new HealthBar();
        healthBar.MaxHealth = 100f;
        healthBar.Health = 100f;
        velX = 10;
        r = new Random();
        Rect = new RectangleF(0, 120, 48, 48);

        font = new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12);
    }

    public override void OnRender(Graphics g)
    {
        healthBar.OnRender(g);
        g.FillRectangle(ColorPalette.BrushLightRed, Rect);
    }

    public void Hit(float damage)
    {
        healthBar.Health -= damage;
    }

    public override void OnUpdate(float delta)
    {
        lastFire += delta;
        if(lastFire > 0.1) {
            lastFire = 0;
            EnemyBullet b = new EnemyBullet(Rect.X, Rect.Y, r.Next(500, 1000));
            Game.Instance.SpawnObject(b);
        }

        healthBar.Rect = new RectangleF(Rect.X, Rect.Y - 20, Rect.Width, 12);

        if (healthBar.Health == 0f) {
            Game.Instance.DestroyObject(this);
            return;
        }

        if (Rect.X >= Game.Instance.GameArea.Width - Rect.Width)
            velX = -r.Next(600, 1100);
        else if (Rect.X <= 0)
            velX = r.Next(600, 1100);

        Rect.X += velX * delta;
    }
}