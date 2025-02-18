using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using LowLevelInput.Hooks;
using MMMouseAligner.Interop;
using MMMouseAligner.Models;
using static MMMouseAligner.Interop.User32;

var leftScreenBorder = 0;

var rightScreenBorder = 2560;

var scaleFactor = 0.75;

var leftScreen = new Screen(leftScreenBorder, scaleFactor, ScreenPosition.Left, "L");

var rightScreen = new Screen(rightScreenBorder, scaleFactor, ScreenPosition.Right, "R");

var screens = new List<Screen> { leftScreen, rightScreen };

////var lastMouseArea = MouseArea.Unknown;

var inputManager = new InputManager(true);

var history = new History<Point>(5);

const string left = "<-";

const string right = "->";

const string neutral = "  ";

history.Enqueue(User32.CursorPosition);

Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;

int GetMonitorIndex(Point position)
    => position.X < leftScreenBorder
    ? 0 
    : position.X > rightScreenBorder
        ? 2 
        : 0;

inputManager.OnMouseEvent += (code, state, x, y) =>
{
    // cursor may skip a few positions while moving
    // a updated cursor position will also be registered here
    var newPosition = User32.Point.Create(x, y);
    ////Console.WriteLine($"RECEIVED {x} {y}");

    var oldScreenIndex = GetMonitorIndex(history[0]);
    var newScreenIndex = GetMonitorIndex(newPosition);
    
    if (code == VirtualKeyCode.Invalid && state == KeyState.None && (oldScreenIndex != newScreenIndex))
    {
        HandlePositionChangeQuick(newPosition, oldScreenIndex, newScreenIndex);
        ////HandlePositionChange(newPos);
    }
};

[MethodImpl(MethodImplOptions.AggressiveInlining)]
void HandlePositionChangeQuick(Point newPoint, int oldScreenIndex, int newScreenIndex) 
{
    int ScaleIn(int oldY)
        => (int)(oldY * scaleFactor);

    int ScaleOut(int oldY)
        => (int)(oldY / scaleFactor);

    history.Enqueue(newPoint);

    var newY = newScreenIndex switch
    {
        <= 0 or >= 2 => ScaleIn(newPoint.Y),
        _ => ScaleOut(newPoint.Y),
    };

    User32.CursorPosition = User32.Point.Create(newPoint.X, newY);
}

////void AlterPosition(Screen screen, User32.Point currentPosition)
////{

////}

////void ResetPosition(User32.Point currentPosition)
////{
////    var (newPoint, transition) = screen.GetNewCursorPosition(history, User32.Point.Create);
////    User32.CursorPosition = newPoint;

////    var dirL = screen.IsLeftScreen
////        ? transition == Transition.In
////            ? left
////            : right
////        : neutral;

////    var dirR = screen.IsRightScreen
////        ? transition == Transition.In
////            ? right
////            : left
////        : neutral;

////    Console.WriteLine($"[L] {dirL} [C] {dirR} [R] ({history[-1].X:+0000;-0000}, {history[-1].Y:+0000;-0000}) => ({history[0].X:+0000;-0000}, [{history[0].Y:+0000;-0000} >> {newPoint.Y:+0000;-0000}])");
////    history.Enqueue(newPoint);
////}

////void HandlePositionChangeV2(User32.Point currentPosition)
////{
////    var currentMouseArea = MouseArea.Unknown;

////    currentMouseArea = leftScreen.IsInScreen(currentPosition) 
////        ? MouseArea.Left 
////        : rightScreen.IsInScreen(currentPosition) 
////            ? MouseArea.Right 
////            : MouseArea.Center;    

////    if (lastMouseArea != currentMouseArea) 
////    {
////        lastMouseArea = currentMouseArea;
////        switch (currentMouseArea)
////        {
////            case MouseArea.Left:
////                AlterPosition(leftScreen, currentPosition); 
////                break;

////            case MouseArea.Right:
////                AlterPosition(rightScreen, currentPosition);
////                break;

////            default:
////                ResetPosition(currentPosition);
////                break;
////        }
////    }
////}

void HandlePositionChange(User32.Point currentPosition)
{
    if (Math.Abs(history[0].Y - currentPosition.Y) > 5 /*&& history[0].Y == currentPosition.Y*/)
    {
        Console.WriteLine($"Jump from {history[0]} to {currentPosition}");
    }

    history.Enqueue(currentPosition);

    foreach (var screen in screens)
    {
        var (newPoint, transition) = screen.GetNewCursorPosition(history, User32.Point.Create);

        if (transition != Transition.None) 
        {
            // updating mouse position
            // problem: seems not to affect position all the time. maybe send
            // to late so that another normal update is in the way?
            User32.CursorPosition = newPoint;

            ////Console.WriteLine($"SET      {newPoint.X} {newPoint.Y}");

            var dirL = screen.IsLeftScreen 
                ? transition == Transition.In 
                    ? left 
                    : right 
                : neutral;

            var dirR = screen.IsRightScreen 
                ? transition == Transition.In 
                    ? right 
                    : left 
                : neutral;
            
            Console.WriteLine($"[L] {dirL} [C] {dirR} [R] ({history[-1].X:+0000;-0000}, {history[-1].Y:+0000;-0000}) => ({history[0].X:+0000;-0000}, [{history[0].Y:+0000;-0000} >> {newPoint.Y:+0000;-0000}])");

            history.Enqueue(newPoint);

            break;
        }
    }
}

while (true)
{
    Thread.Sleep(1000);
}