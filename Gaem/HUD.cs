using Gaem;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

    HPBar healthBar;
    Font font;

    public double TotalTime;

    public HUD()
    {
        healthBar = new HPBar();
        healthBar.Health = 100;
        rect = new RectangleF(20, 20, 200, 32);
        healthBar.rect = rect;
        font = new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12);
        instance = this;
    }

    public void OnDestroy()
    {

    }

    public void OnRender(Graphics g)
    {
        healthBar.OnRender(g);
        g.DrawString($"HitPoints: {(int)healthBar.Health}", font, Brushes.White, new PointF(rect.X-2,rect.Y+34));

        string text = $"Time: {(int)TotalTime}";
        SizeF textMeasure = g.MeasureString(text, font);
        g.DrawString(text, font, ColorPalette.BrushLightBlue, game.GameArea.Width - textMeasure.Width, game.GameArea.Height - textMeasure.Height);
    }

    public void OnSpawn(Game game)
    {
        this.game = game;
    }

    public void OnUpdate(float delta)
    {
        TotalTime += delta;
    }

    public void SetHealth(float value)
    {
        healthBar.Health = value;
    }

    public void AddHealth(float value)
    {
        healthBar.Health += value;
    }

    public void SubtractHealth(float value)
    {
        healthBar.Health -= value;

        if(healthBar.Health <= 1) {
            MessageBox.Show("You have lost :(");
            game.Quit();
        }
    }
}