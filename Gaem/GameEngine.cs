using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

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

                    sw.Restart();

                    GameTick?.Invoke(now / UpdateFrequency);
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