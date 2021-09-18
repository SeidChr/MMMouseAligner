namespace MMMouseAligner.Interop
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    public class User32
    {
        public static Point CursorPosition
        {
            get
            {
                GetCursorPos(out var point);
                return point;
            }

            set => SetCursorPos(value.X, value.Y);
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point point);

        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "ConvertToAutoProperty", Justification = "Do not break structural layout.")]
        public struct Point : IPoint
        {
            private int valueX;
            
            private int valueY;

            public int X
            {
                get => this.valueX;
                set => this.valueX = value;
            }

            public int Y
            {
                get => this.valueY;
                set => this.valueY = value;
            }
        }
    }
}
