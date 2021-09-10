namespace MMMouseAligner.Interop
{
    using System.Runtime.InteropServices;

    public class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point point);

        public static Point CursorPosition
        {
            get 
            { 
                GetCursorPos(out var point);
                return point;
            }

            set => SetCursorPos(value.X, value.Y);
        }
    }
}
