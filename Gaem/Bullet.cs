using Gaem;
using System;
using System.Collections.Generic;
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
        rect = new RectangleF(x - 12 , y - 12, 24, 24);
    }

    public void OnRender(Graphics g)
    {
        g.FillEllipse(ColorPalette.BrushLightBlue, rect);
    }

    public void OnSpawn(Game game)
    {
        this.game = game;
        angle = Math.Atan2(game.CursorPosition.Y - Rect.Y, game.CursorPosition.X - Rect.X);
    }

    private double angle = 0;

    public void OnUpdate(float delta)
    {
        List<Monster> monsters = game.GetObjects<Monster>();
        if (!rect.IntersectsWith(game.GameArea))
            game.DestroyObject(this);

        float dirX = (float)Math.Cos(angle);
        float dirY = (float)Math.Sin(angle);
        rect.X += (dirX * 30) * delta;
        rect.Y += (dirY * 30) * delta;

        for (int i = 0; i < monsters.Count; i++) {
            if (rect.IntersectsWith(monsters[i].Rect)) {
                monsters[i].Hit(30f*delta);
                game.DestroyObject(this);
            }
        }

    }
}
