namespace MMMouseAligner.Models
{
    using System;
    using Interop;

    public struct Screen
    {
        public int BorderX;

        public double Scale;

        public ScreenPosition ScreenPosition;

        public Screen(int borderX, double scale, ScreenPosition screenPosition)
        {
            this.BorderX = borderX;
            this.Scale = scale;
            this.ScreenPosition = screenPosition;
        }

        public IPoint TryApplyCursorPosition<TPoint>(History<IPoint> history, Func<int, int, TPoint> pointFactory)
            where TPoint : IPoint
        {
            IPoint result = null;
            if (this.HasMovedIn(history))
            {
                var newY = this.ScaleIn(history[0].Y);
                result = pointFactory(history[0].X, newY);
            }
            else if (this.HasMovedOut(history))
            {
                var newY = this.ScaleOut(history[0].Y);
                result = pointFactory(history[0].X, newY);
            }

            return result;
        }

        public bool HasMovedOut<T>(History<T> history)
            where T : IPoint
            => this.ScreenPosition switch
            {
                ScreenPosition.Left => this.HasMovedLtr(history),
                ScreenPosition.Right => this.HasMovedRtl(history),
                _ => throw new ArgumentOutOfRangeException(nameof(this.ScreenPosition)),
            };

        public bool HasMovedIn<T>(History<T> history)
            where T : IPoint
            => this.ScreenPosition switch
            {
                ScreenPosition.Left => this.HasMovedRtl(history),
                ScreenPosition.Right => this.HasMovedLtr(history),
                _ => throw new ArgumentOutOfRangeException(nameof(this.ScreenPosition)),
            };

        public int ScaleIn(int oldY)
            => (int)(oldY * this.Scale);

        public int ScaleOut(int oldY)
            => (int)(oldY / this.Scale);

        private bool HasMovedLtr<T>(History<T> history)
            where T : IPoint
            => history[-1].X < this.BorderX 
               && history[0].X >= this.BorderX;

        private bool HasMovedRtl<T>(History<T> history)
            where T : IPoint
            => history[-1].X >= this.BorderX 
               && history[0].X < this.BorderX;
    }
}
