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

    float velX, velY;
    Random r = new Random();
    Game game;
    Font font;
    HPBar healthBar;

    public Monster()
    {
        healthBar = new HPBar();
        healthBar.MaxHealth = 100f;
        healthBar.Health = 100f;
        velX = 20;
        velY = 20;
        r = new Random();
        rect = new RectangleF(200,200,48,48);

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
        healthBar.rect = new RectangleF(rect.X, rect.Y - 20, rect.Width, 12);

        Player player = game.GetObjects<Player>().First();

        float angleFromPlayer = (float)Math.Atan2(player.Rect.Y - rect.Y, player.Rect.X - rect.X);
        float cos = (float)Math.Cos(angleFromPlayer);

        if (rect.X < game.GameArea.Left) {
            if (r.Next(0, 10) > 4) {
                velX = r.Next(8, 30);
            }else {
                if (cos > 0.6)
                    velX = cos * 20f * delta;
                else
                    velX = r.Next(8, 30);
            }
        } else if (rect.X > game.GameArea.Width - rect.Width) {
            velX = -r.Next(8, 30);
        }

        if (rect.Y <= 0) {
            velY = r.Next(8, 30);
        } else if (rect.Y > game.GameArea.Height - rect.Height) {
            velY = -r.Next(8, 30);
        }
            
        rect.Y += velY * delta;
        rect.X += velX * delta;
    }
}