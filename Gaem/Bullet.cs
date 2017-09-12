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
    }

    public void OnUpdate(float delta)
    {
        rect.Y -= 8f * delta;

        if (rect.Y < game.GameArea.Top) {
            game.DestroyObject(this);
            return;
        }
        var mon = game.GetSpecific<Monster>();
        if (mon.Count > 0) {
            if (rect.IntersectsWith(mon[0].Rect)) {
                mon[0].Hit(20f);
                game.DestroyObject(this);
            }
        }
            
    }
}
