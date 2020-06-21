using Gaem;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class HUD : GameObject
{
    static HUD instance;

    public static HUD Instance => instance;

    HealthBar healthBar;
    public Font font;

    public double TotalTime;

    public HUD()
    {
        healthBar = new HealthBar();
        healthBar.Health = 100;
        Rect = new RectangleF(20, 20, 200, 32);
        healthBar.Rect = Rect;
        font = new Font(FontFamily.Families.Where((o) => o.Name == "Segoe UI").First(), 12);
        instance = this;
    }

    public override void OnRender(Graphics g)
    {
        healthBar.OnRender(g);
        g.DrawString($"HitPoints: {(int)healthBar.Health}", font, Brushes.White, new PointF(Rect.X-2, Rect.Y+34));

        string text = $"Time: {(int)TotalTime}";
        SizeF textMeasure = g.MeasureString(text, font);
        g.DrawString(text, font, ColorPalette.BrushLightBlue, Game.Instance.GameArea.Width - textMeasure.Width, Game.Instance.GameArea.Height - textMeasure.Height);
    }

    public override void OnUpdate(float delta)
    {
        TotalTime += delta;
        healthBar.OnUpdate(delta);
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
            //game.Quit();
        }
    }
}