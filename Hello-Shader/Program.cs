// See https://aka.ms/new-console-template for more information
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

WindowCreateInfo wci = new WindowCreateInfo()
{
    X = 100, Y = 100,
    WindowWidth = 800, WindowHeight = 600,
    WindowTitle = "Hello Shader",
};
Sdl2Window window = VeldridStartup.CreateWindow(wci);
GraphicsDeviceOptions gdo = new GraphicsDeviceOptions
{
    PreferDepthRangeZeroToOne = true,
    PreferStandardClipSpaceYDirection = true,
};
GraphicsDevice gd = VeldridStartup.CreateGraphicsDevice(window, gdo);

UInt64 ticks = 0;
while (window.Exists)
{
    ticks++;
    Console.WriteLine($"Ticks: {ticks}");
    window.PumpEvents();
    Thread.Sleep(30);
}