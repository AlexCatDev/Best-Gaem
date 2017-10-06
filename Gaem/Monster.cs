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

    float health;
    float velX, velY;
    Random r = new Random();
    Game game;

    public Monster()
    {
        health = 1000;
        velX = 20;
        velY = 20;
        r = new Random();
        rect = new RectangleF(200,200,48,48);
    }

    public void OnDestroy()
    {
        
    }

    public void OnRender(Graphics g)
    {
        g.DrawString("HP: " + Math.Floor(health), new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12), Brushes.White, new PointF(rect.X + 2, rect.Y - 34));
        g.FillRectangle(Brushes.Red, rect);
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health < 1) {
            game.DestroyObject(this);
            MessageBox.Show("Eyy u won skrrrt");
            game.Quit();
        }
    }

    public void OnSpawn(Game game)
    {
        this.game = game;   
    }

    public void OnUpdate(float delta)
    {
        if (rect.X < game.GameArea.Left)
            velX = r.Next(8, 30);
        else if(rect.X > game.GameArea.Width-rect.Width)
            velX = -r.Next(8, 30);

        if (rect.Y <= 0)
            velY = r.Next(8, 30);
        else if (rect.Y > game.GameArea.Height - rect.Height)
            velY = -r.Next(8, 30);

        rect.Y += velY * delta;
        rect.X += velX * delta;
    }
}