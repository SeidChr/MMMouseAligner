namespace MMMouseAligner.Models
{
    using System;

    public readonly struct Screen
    {
        public readonly int BorderX;

        public readonly double Scale;

        public readonly ScreenPosition ScreenPosition;

        public readonly string Name;

        public Screen(int borderX, double scale, ScreenPosition screenPosition, string name)
        {
            this.BorderX = borderX;
            this.Scale = scale;
            this.ScreenPosition = screenPosition;
            this.Name = name;
        }

        public bool IsLeftScreen
            => this.ScreenPosition == ScreenPosition.Left;

        public bool IsRightScreen
            => this.ScreenPosition == ScreenPosition.Right;

        public bool IsInScreen<TPoint>(TPoint point)
            where TPoint : IPoint 
            => this.ScreenPosition switch
            {
                ScreenPosition.Left => point.X < this.BorderX,
                ScreenPosition.Right => point.X > this.BorderX,
                _ => false,
            };

        public (TPoint NewPoint, Transition RelativeTransition) GetNewCursorPosition<TPoint>(
            History<TPoint> history,
            Func<int, int, TPoint> pointFactory)
            where TPoint : IPoint
        {
            var relativeTransition = this.TestMove(history);
            var result = relativeTransition switch
            {
                Transition.In => pointFactory(history[0].X, this.ScaleIn(history[0].Y)),
                Transition.Out => pointFactory(history[0].X, this.ScaleOut(history[0].Y)),
                _ => history[0],
            };

            return (result, relativeTransition);
        }

        public Transition TestMove<T>(History<T> history)
            where T : IPoint
            => this.ScreenPosition switch
            {
                ScreenPosition.Left => this.HasMovedLtr(history) 
                    ? Transition.Out 
                    : this.HasMovedRtl(history) 
                        ? Transition.In 
                        : Transition.None,

                ScreenPosition.Right => this.HasMovedRtl(history)
                    ? Transition.Out
                    : this.HasMovedLtr(history)
                        ? Transition.In
                        : Transition.None,

                _ => Transition.None,
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
