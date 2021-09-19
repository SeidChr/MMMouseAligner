﻿using System;
using System.Collections.Generic;
using System.Threading;
using LowLevelInput.Hooks;
using MMMouseAligner;
using MMMouseAligner.Interop;
using MMMouseAligner.Models;
using static MMMouseAligner.Interop.User32;

var leftScreen = new Screen(0, 0.75, ScreenPosition.Left, "L");

var rightScreen = new Screen(2560, 0.75, ScreenPosition.Right, "R");

var screens = new List<Screen> { leftScreen, rightScreen };

var inputManager = new InputManager(true);

var history = new History<Point>(5);

history.Enqueue(User32.CursorPosition);

inputManager.OnMouseEvent += (code, state, x, y) =>
{
    ////Console.WriteLine($". ME Code:{code} State:{state} X:{x} Y:{y}");

    var newPos = new User32.Point { X = x, Y = y };
    if (code == VirtualKeyCode.Invalid && state == KeyState.None)
    {
        HandlePositionChange(newPos);
    }
};

void HandlePositionChange(User32.Point currentPosition)
{
    history.Enqueue(currentPosition);

    foreach (var screen in screens)
    {
        var (newPoint, transition) = screen.GetNewCursorPosition<Point>(history, User32.Point.Create);
        if (transition != Transition.None) 
        {
            var directionMark = ((transition == Transition.In && screen.IsLeftScreen) || (transition == Transition.Out && screen.IsRightScreen)) ? "<" : ">";
            var transitionMark = transition == Transition.In ? "I" : "O";
            User32.CursorPosition = newPoint;
            Console.WriteLine($"{directionMark} {transitionMark}{screen.Name} ({history[-1].X:+0000;-0000}) => ({history[0].X:+0000;-0000}, {history[0].Y:+0000;-0000} => {newPoint.Y:+0000;-0000})");
            break;
        }
    }

    ////string GetTransitionStringB(IPoint newPoint)
    ////    => $"({history[-1].X:+0000;-0000}) => ({history[0].X:+0000;-0000}, {history[0].Y:+0000;-0000} => {newPoint.Y:+0000;-0000})";

    ////if (leftScreen.HasMovedIn(history))
    ////{
    ////    var newPoint = new User32.Point { X = history[0].X, Y = leftScreen.ScaleIn(history[0].Y) };
    ////    Console.WriteLine("< IL " + GetTransitionStringB(newPoint));
    ////    User32.CursorPosition = newPoint;
    ////}
    ////else if (leftScreen.HasMovedOut(history))
    ////{
    ////    var newPoint = new User32.Point { X = history[0].X, Y = leftScreen.ScaleOut(history[0].Y) };
    ////    Console.WriteLine("> OL " + GetTransitionStringB(newPoint));
    ////    User32.CursorPosition = newPoint;
    ////}
    ////else if (rightScreen.HasMovedOut(history))
    ////{
    ////    var newPoint = new User32.Point { X = history[0].X, Y = rightScreen.ScaleOut(history[0].Y) };
    ////    Console.WriteLine("< OR " + GetTransitionStringB(newPoint));
    ////    User32.CursorPosition = newPoint;
    ////}
    ////else if (rightScreen.HasMovedIn(history))
    ////{
    ////    var newPoint = new User32.Point { X = history[0].X, Y = rightScreen.ScaleIn(history[0].Y) };
    ////    Console.WriteLine("> IR " + GetTransitionStringB(newPoint));
    ////    User32.CursorPosition = newPoint;
    ////}
}

while (true)
{
    Thread.Sleep(100);
}