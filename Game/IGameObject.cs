using System.Drawing;

public abstract class GameObject
{
    public RectangleF Rect;

    public abstract void OnRender(Graphics g);
    public abstract void OnUpdate(float delta);
}