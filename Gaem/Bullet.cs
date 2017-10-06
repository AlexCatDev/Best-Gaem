using System;
using System.Drawing;

public class Bullet : IGameObject
{
    RectangleF rect;
    Game game;
    public RectangleF Rect {
        get {
            return rect;
        }
    }

    public void OnDestroy()
    {
        
    }

    public Bullet(float x, float y)
    {
        rect = new RectangleF(x, y, 16, 16);
    }

    public void OnRender(Graphics g)
    {
        g.FillEllipse(Brushes.Brown, rect);
        g.FillEllipse(Brushes.Brown, rect.X + 16, rect.Y, 16, 16);
        g.FillRectangle(Brushes.Brown, rect.X + 12, rect.Y - 32, 8, 32);
        g.FillRectangle(Brushes.Pink, rect.X + 12, rect.Y - 32, 8, 6);
    }

    public void OnSpawn(Game game)
    {
        this.game = game;
        angle = Math.Atan2(game.CursorPosition.Y - Rect.Y, game.CursorPosition.X - Rect.X);
    }

    private double angle = 0;

    public void OnUpdate(float delta)
    {
        var mon = game.GetObjects<Monster>();

        //rect.Y -= 16f * delta;
        if (rect.Y < game.GameArea.Top) {
            game.DestroyObject(this);
            return;
        }

        if (mon.Count > 0) {
            Monster first = mon[0];

            //double angle = Math.Atan2(first.Rect.Y - Rect.Y, first.Rect.X - Rect.X);
            game.SetTitle($"Angle: " + angle);
            float dirX = (float)Math.Cos(angle);
            float dirY = (float)Math.Sin(angle);
            rect.X += (dirX * 20) * delta;
            rect.Y += (dirY * 20) * delta;

            if (rect.IntersectsWith(first.Rect)) {
                first.Hit(20f);
                game.DestroyObject(this);
            }
        }
            
    }
}
