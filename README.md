# MMMouseAligner
The Multi Monitor Mouse Aligner aligns the mouse on the transition between two monitors

## Status
The this project is currently developed for only one purpose: shift the mouse of my work-pc to the correct position when it moves accross screen borders.

Thus it is working exactly for my machine.

You can use it as a template for your own setup.

## Functionality
Setup:
- 3 Monitors of the same size.
- Two of the monitors (left and right) are running a reduced resolution.
- All Monitors are aligned at the top border in windows settings
- Center screen width and resolution reductions are set up in code (top of Programm.cs)

When moving the mosue accross the left or right border of the center screen into one of the other screens, the Y coordinate of the mouse is updated according to the size ratio of the other monitor, so that it appears to be moving smothly accross the border.

# Copyright
Whatever `¯\_(ツ)_/¯`. Have fun.