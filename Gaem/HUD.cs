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