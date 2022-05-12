using System.Runtime.InteropServices;
using System.Windows.Forms;

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