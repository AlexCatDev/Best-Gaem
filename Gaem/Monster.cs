using Gaem;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class Monster : IGameObject
{
    RectangleF rect;

    public RectangleF Rect {
        get {
            return rect;
        }
    }

    float velX;
    Random r = new Random();
    Game game;
    Font font;
    HPBar healthBar;
    float lastFire;


    public Monster()
    {
        healthBar = new HPBar();
        healthBar.MaxHealth = 100f;
        healthBar.Health = 100f;
        velX = 10;
        r = new Random();
        rect = new RectangleF(0, 120, 48, 48);

        font = new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12);
    }

    public void OnDestroy()
    {
        
    }

    public void OnRender(Graphics g)
    {
        healthBar.OnRender(g);
        g.FillRectangle(ColorPalette.BrushLightRed, rect);
    }

    public void Hit(float damage)
    {
        healthBar.Health -= damage;
    }

    public void OnSpawn(Game game)
    {
        this.game = game;   
    }

    public void OnUpdate(float delta)
    {
        lastFire += delta;
        if(lastFire > 4) {
            lastFire = 0;
            EnemyBullet b = new EnemyBullet(rect.X, rect.Y, r.Next(4, 12));
            b.OnSpawn(game);
            game.SpawnObject(b);
        }

        healthBar.rect = new RectangleF(rect.X, rect.Y - 20, rect.Width, 12);

        if (healthBar.Health == 0f) {
            game.DestroyObject(this);
            return;
        }

        if (rect.X >= game.GameArea.Width - rect.Width)
            velX = -r.Next(8, 18);
        else if (rect.X <= 0)
            velX = r.Next(8, 18);

        rect.X += velX * delta;
    }
}