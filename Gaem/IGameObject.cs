using System.Drawing;

public interface IGameObject
{
    RectangleF Rect { get; }

    void OnSpawn(Game game);
    void OnDestroy();
    void OnRender(Graphics g);
    void OnUpdate(float delta);
}