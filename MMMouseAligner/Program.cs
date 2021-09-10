using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using LowLevelInput.Hooks;
using MMMouseAligner;
using MMMouseAligner.Interop;

var leftScreenScale = 0.75;
var leftScreenBorderX = 0;

var rightScreenScale = 0.75;
var rightScreenBorderX = 2560;

var inputManager = new InputManager(true);

var history = new History<User32.Point>(5);
history.Enqueue(User32.CursorPosition);

inputManager.OnMouseEvent += (code, state, x, y) =>
{
    var newPos = new User32.Point {X = x, Y = y};
    HandlePositionChange(newPos);
};

void HandlePositionChange(User32.Point currentPosition) 
{
    history.Enqueue(currentPosition);

    if (history[0].X < leftScreenBorderX && history[-1].X >= leftScreenBorderX)
    {
        var newY = (int)(history[0].Y * leftScreenScale);
        Console.WriteLine($"< RTL ({history[-1].X:+0000;-0000}) => ({history[0].X:+0000;-0000}, {history[0].Y:+0000;-0000} => {newY:+0000;-0000})");
        User32.CursorPosition = new User32.Point { X = history[0].X, Y = newY };
    }
    else if (history[0].X >= leftScreenBorderX && history[-1].X < leftScreenBorderX)
    {
        var newY = (int)(history[0].Y / leftScreenScale);
        Console.WriteLine($"> LTR ({history[-1].X:+0000;-0000}) => ({history[0].X:+0000;-0000}, {history[0].Y:+0000;-0000} => {newY:+0000;-0000})");
        User32.CursorPosition = new User32.Point { X = history[0].X, Y = newY };
    }
    else if (history[0].X < rightScreenBorderX && history[-1].X >= rightScreenBorderX)
    {
        var newY = (int)(history[0].Y / rightScreenScale);
        Console.WriteLine($"< RTL ({history[-1].X:+0000;-0000}) => ({history[0].X:+0000;-0000}, {history[0].Y:+0000;-0000} => {newY:+0000;-0000})");
        User32.CursorPosition = new User32.Point { X = history[0].X, Y = newY };
    }
    else if (history[0].X >= rightScreenBorderX && history[-1].X < rightScreenBorderX)
    {
        var newY = (int)(history[0].Y * rightScreenScale);
        Console.WriteLine($"> LTR ({history[-1].X:+0000;-0000}) => ({history[0].X:+0000;-0000}, {history[0].Y:+0000;-0000} => {newY:+0000;-0000})");
        User32.CursorPosition = new User32.Point { X = history[0].X, Y = newY };
    }
    //Console.WriteLine($". MV ({history[0].X},{history[0].Y})");
};

while (true)
{
    Thread.Sleep(100);
}