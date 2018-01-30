using Gaem;
using System;
using System.Collections.Generic;
using System.Drawing;

public class Bullet : GameObject
{
    public Bullet(float x, float y)
    {
        Rect = new RectangleF(x - 12 , y - 12, 24, 24);
        angle = Math.Atan2(Game.Instance.CursorPosition.Y - Rect.Y, Game.Instance.CursorPosition.X - Rect.X);
    }

    public override void OnRender(Graphics g)
    {
        g.FillEllipse(ColorPalette.BrushLightBlue, Rect);
    }

    private double angle = 0;

    public override void OnUpdate(float delta)
    {
        if (!Rect.IntersectsWith(Game.Instance.GameArea))
            Game.Instance.DestroyObject(this);

        float dirX = (float)Math.Cos(angle);
        float dirY = (float)Math.Sin(angle);
        Rect.X += (dirX * 1200) * delta;
        Rect.Y += (dirY * 1200) * delta;

        Game.Instance.GetObjects<Monster>((monster) => {
            if (monster.Rect.IntersectsWith(Rect)) {
                monster.Hit(10f);
                Game.Instance.DestroyObject(this);
            }
        });

    }
}
