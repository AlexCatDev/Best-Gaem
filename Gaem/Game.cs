using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    public IList<IGameObject> GameObjects {
        get {
            return gameObjects.AsReadOnly();
        }
    }

    List<IGameObject> gameObjects = new List<IGameObject>();

    GameEngine engine;
    Control parentContainer;

    public GameEngine Engine {
        get { return engine; }
    }

    public RectangleF GameArea { get; set; }

    private BufferedGraphics bf;

    public Game(Control parentContainer)
    {
        this.parentContainer = parentContainer;
        BufferedGraphicsContext context = BufferedGraphicsManager.Current;
        bf = context.Allocate(parentContainer.CreateGraphics(), parentContainer.DisplayRectangle);
        GameArea = parentContainer.ClientRectangle;
        //Update 60 times a second
        engine = new GameEngine(parentContainer, 1000f / 60f);
        engine.GameTick += Engine_GameTick;
    }

    float elapsedTime = 0;
    int frames = 0;
    //Delta is how our accuracy of time
    private void Engine_GameTick(float delta)
    {
        elapsedTime += delta;
        frames++;
        if(elapsedTime >= 2000/60) {
            SetTitle("FPS: " + frames + " Objects: " + gameObjects.Count);
            frames = 0;
            elapsedTime = 0;
        }

        bf.Graphics.Clear(Color.Black);
        for (int i = 0; i < gameObjects.Count; i++) {
            var obj = gameObjects[i];
            obj.OnUpdate(delta);
        }

        for (int i = 0; i < GameObjects.Count; i++) {
            GameObjects[i].OnRender(bf.Graphics);
        }

        bf.Render();
    }

    public List<T> GetSpecific<T>()
        where T : IGameObject
    {
        List<T> temp = new List<T>();
        for (int i = 0; i < gameObjects.Count; i++) {
            if (gameObjects[i] is T)
                temp.Add((T)gameObjects[i]);
        }

        return temp;
    }

    public void SetTitle(string text)
    {
        parentContainer.Text = text;
    }

    public void AppendTitle(string text)
    {
        parentContainer.Text += text;
    }

    public void DestroyObject(IGameObject obj)
    {
        obj.OnDestroy();
        gameObjects.Remove(obj);
    }

    public void SpawnObject(IGameObject obj)
    {
        obj.OnSpawn(this);
        gameObjects.Add(obj);
    }

    public void Init()
    {
        SpawnObject(new Player());
        SpawnObject(new HUD());
        SpawnObject(new Monster());
        engine.Init();
    }

    public static float Clamp(float value, float min, float max)
    {
        if (value > max)
            return max;
        if (value < min)
            return min;

        return value;
    }

    public void Quit()
    {
        engine.Quit();
    }
}