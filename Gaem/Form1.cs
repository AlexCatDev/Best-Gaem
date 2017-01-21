using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaem
{
    public partial class Form1 : Form
    {
        Game game;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            game?.Quit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            DoubleBuffered = true;

            game = new Game(this);
            game.Init();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < game.GameObjects.Count; i++) {
                game.GameObjects[i].OnRender(e.Graphics);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            game.GameArea = this.ClientRectangle;
        }
    }
}

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

    public Game(Control parentContainer)
    {
        this.parentContainer = parentContainer;
        GameArea = parentContainer.ClientRectangle;
        //Update 60 times a second
        engine = new GameEngine(parentContainer, 1000f / 60f);
        engine.GameTick += Engine_GameTick;
    }

    //Delta is how our accuracy of time
    private void Engine_GameTick(float delta)
    {
        SetTitle("Delta: " + delta + " Objects: " + gameObjects.Count);
        for (int i = 0; i < gameObjects.Count; i++) {
            var obj = gameObjects[i];
            obj.OnUpdate(delta);;
        }
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

public interface IGameObject
{
    RectangleF Rect { get; }

    void OnSpawn(Game game);
    void OnDestroy();
    void OnRender(Graphics g);
    void OnUpdate(float delta);
}

public class HUD : IGameObject
{
    static HUD instance;

    public static HUD Instance {
        get {
            if (instance == null)
                instance = new HUD();

            return instance;
        }
    }

    RectangleF rect;
    Game game;

    public RectangleF Rect {
        get {
            return rect;
        }
    }

    float health;
    float minHealth;
    float maxHealth;
    float ammo;

    public HUD()
    {
        health = 100f;
        maxHealth = 100f;
        minHealth = 100f;
        ammo = 5;
        rect = new RectangleF(50, 50, 200, 32);
        instance = this;
    }

    public void OnDestroy()
    {

    }

    public void OnRender(Graphics g)
    {
        g.FillRectangle(Brushes.Gray, rect.X, rect.Y, rect.Width, rect.Height);
        g.FillRectangle(new SolidBrush(Color.FromArgb((int)(200-health*2), (int)health*2, 0)), rect.X, rect.Y, health * 2, rect.Height);
        g.DrawRectangle(Pens.White, rect.X, rect.Y, rect.Width, rect.Height);
        g.DrawString("HitPoints: " + Math.Floor(health), new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12), Brushes.Gray, new PointF(rect.X-2,rect.Y+34));
        g.DrawString("Ammo: " + ammo, new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12), Brushes.Gray, game.GameArea.Right-96, game.GameArea.Bottom-32);
    }

    public void OnSpawn(Game game)
    {
        this.game = game;
    }

    public void OnUpdate(float delta)
    {
        
    }

    public float GetAmmo()
    {
        return ammo;
    }

    public void RemoveAmmo(float value)
    {
        if(ammo >=  value)
        ammo -= value;
    }

    public void SetHealth(float value)
    {
        health = Game.Clamp(value, minHealth, maxHealth);
    }

    public void AddHealth(float value)
    {
        if(health<100)
        health += value;
    }

    public void SubtractHealth(float value)
    {
        if(health > 0 && value <= health)
        health -= value;

        if(health == 0) {
            MessageBox.Show("You have lost :(");
            game.Quit();
        }
    }
}

public class Player : IGameObject
{
    Game game;
    RectangleF rect;
    float speed;

    float fireRate;
    float lastFire;

    public RectangleF Rect {
        get {
            return rect;
        }
    }

    public Player()
    {
        rect = new RectangleF(100,200,64,64);
        speed = 8f;
        lastFire = 0;
        //Every 375 milliseconds
        fireRate = 375;
    }


    public void OnRender(Graphics g)
    {
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.FillRectangle(Brushes.Red, rect);
    }

    public void OnUpdate(float delta)
    {
        if (Input.GetKey(Keys.Up))
            rect.Y -= speed * delta;

        if (Input.GetKey(Keys.Down))
            rect.Y += speed * delta;

        if (Input.GetKey(Keys.Right))
            rect.X += speed * delta;

        if (Input.GetKey(Keys.Left))
            rect.X -= speed * delta;

        if (Input.GetKey(Keys.H))
            HUD.Instance.AddHealth(1);

        if (Input.GetKey(Keys.Space)) {
            if(game.Engine.ElapsedTime - lastFire >= fireRate){
                if (HUD.Instance.GetAmmo() > 0) {
                    game.SpawnObject(new Bullet(rect.X + 16, rect.Y));
                    lastFire = game.Engine.ElapsedTime;
                    HUD.Instance.RemoveAmmo(1);
                }
            }
        }

        if (rect.IntersectsWith(HUD.Instance.Rect))
            HUD.Instance.SubtractHealth(1);

        var monInsect = game.GetSpecific<Monster>();

        if (monInsect.Count > 0) {
            if(monInsect[0].Rect.IntersectsWith(rect))
            HUD.Instance.SubtractHealth(1f);
        }

    }

    public void OnSpawn(Game game)
    {
        this.game = game; 
    }

    public void OnDestroy()
    {
        
    }
}

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

        game.AppendTitle(" X: " + velX);

        rect.X += velX * delta;
    }
}



public class GameEngine
{
    public delegate void GameTickEventHandler(float delta);

    public event GameTickEventHandler GameTick;

    Control ParentContainer;
    float updateFrequency;

    bool running;
    bool throttled;

    public float ElapsedTime { get; private set; }

    public bool Throttled {
        get {
            return throttled;
        }
        set {
            throttled = value;
        }
    }

    public bool Running {
        get {
            return running;
        }
    }

    public float UpdateFrequency {
        get {
            return updateFrequency;
        }
    }

    public GameEngine(Control parentContainer, float updateFrequency)
    {
        this.ParentContainer = parentContainer;
        this.updateFrequency = updateFrequency;
    }

    public void Init()
    {
        if (!running) {
            running = true;
            Thread thread = new Thread(() => {
                float now;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (running) {
                    now = ((float)sw.ElapsedTicks / Stopwatch.Frequency) * 1000f;

                    if (now >= updateFrequency) {
                        ElapsedTime += now;
                        sw.Restart();
                        GameTick?.Invoke(now / updateFrequency);
                    }

                    ParentContainer.Invalidate();

                    if (throttled)
                        Thread.Sleep(1);
                }
            });

            thread.Start();
        }
    }

    public void Quit()
    {
        if (running)
            running = false;
    }
}

public static class Input
{
    public static bool GetKey(Keys key)
    {
        return Win32.IsKeyDown(key);
    }
}

public static class Win32
{
    [DllImport("USER32.dll")]
    static extern short GetKeyState(Keys key);

    public static bool IsKeyDown(Keys key)
    {
        switch (GetKeyState(key)) {
            case -128:
                return true;
            case -127:
                return true;
            default:
                return false;
        }
    }
}