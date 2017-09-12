using System;
using System.Data;
using System.Drawing;
using System.Linq;

public class Monster : IGameObject
{
    RectangleF rect;

    public RectangleF Rect {
        get {
            return rect;
        }
    }

    float health;
    float velX;
    Game game;

    public Monster()
    {
        health = 100;
        velX = 13;
        rect = new RectangleF(200,200,72,72);
    }

    public void OnDestroy()
    {
        
    }

    public void OnRender(Graphics g)
    {
        g.DrawString("HP: " + Math.Floor(health), new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12), Brushes.Blue, new PointF(rect.X + 2, rect.Y - 34));
        g.FillRectangle(Brushes.Blue, rect);
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health < 1)
            game.DestroyObject(this);
    }

    public void OnSpawn(Game game)
    {
        this.game = game;   
    }

    public void OnUpdate(float delta)
    {
        if (rect.X < game.GameArea.Left)
            velX = 13f;
        else if(rect.X > game.GameArea.Width-rect.Width)
            velX = -13f;

        rect.X += velX * delta;
    }
}