namespace MMMouseAligner.Models
{
    public struct CursorTransitionResult<TPoint>
    {
        public TPoint NewPoint { get; set; }

        public Transition RelativeTransition { get; set; }
    }
}
