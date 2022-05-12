using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Game
{
    public class Game
    {
        List<GameObject> gameObjects = new List<GameObject>();

        public RectangleF GameArea => form.DisplayRectangle;

        private BufferedGraphics bf;

        private static Game instance;
        public static Game Instance => instance;

        private Form form;
        private bool running;
        private Thread gameThread;
        private Stopwatch gameWatch;

        public Game()
        {
            form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(1280, 720);
            form.MinimumSize = form.Size;
            form.MaximumSize = form.Size;

            form.Text = "Game!";

            form.FormClosing += (s, e) =>
            {
                running = false;
                gameThread.Join();
            };

            form.Load += (s, e) =>
            {
                running = true;
                gameThread.Start();
            };

            bf = BufferedGraphicsManager.Current.Allocate(form.CreateGraphics(), form.DisplayRectangle);

            gameThread = new Thread(gameLoop);

            Control.CheckForIllegalCrossThreadCalls = false;
            ColorPalette.Initialize();

            instance = this;
        }


        float elapsedTime = 0;
        int frames = 0;

        private void gameLoop()
        {
            gameWatch = Stopwatch.StartNew();

            SpawnObject(new Player());
            SpawnObject(new Monster());
            SpawnObject(new HUD());

            while (running)
            {
                float delta = ((float)gameWatch.ElapsedTicks / Stopwatch.Frequency);
                gameWatch.Restart();

                elapsedTime += delta;
                frames++;
                if (elapsedTime >= 1)
                {
                    SetTitle("FPS: " + frames + " Objects: " + gameObjects.Count);
                    frames = 0;
                    elapsedTime = 0;
                }


                bf.Graphics.Clear(Color.FromArgb(255, 32, 32, 32));
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    var obj = gameObjects[i];
                    obj.OnUpdate(delta);
                    obj.OnRender(bf.Graphics);
                }

                bf.Render();
            }
        }

        public Point CursorPosition => form.PointToClient(Cursor.Position);

        public void GetObjects<T>(Action<T> action) where T : GameObject
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                T obj = gameObjects[i] as T;
                if (obj != null)
                    action?.Invoke(obj);
            }
        }

        public T GetObject<T>() where T : GameObject
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                T obj = gameObjects[i] as T;
                if (obj != null)
                    return obj;
            }

            return null;
        }

        public void SetTitle(string text)
        {
            form.Text = text;
        }

        public void DestroyObject(GameObject obj)
        {
            gameObjects.Remove(obj);
        }

        public void SpawnObject(GameObject obj)
        {
            gameObjects.Add(obj);
        }

        public void Run()
        {
            if (!running)
            {
                Application.Run(form);
            }
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
            form.Close();
        }
    }
}