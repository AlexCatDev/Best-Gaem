using System.Windows.Forms;

public static class Input
{
    public static bool GetKey(Keys key)
    {
        return Win32.IsKeyDown(key);
    }
}