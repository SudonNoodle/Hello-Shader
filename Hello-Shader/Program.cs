using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace GettingStarted
{
    class Program
    {
        private static GraphicsDevice _graphicsDevice;

        static void Main()
        {
            WindowCreateInfo windowCI = new WindowCreateInfo()
            {
                X = 100,
                Y = 100,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "Veldrid Tutorial"
            };
            Sdl2Window window = VeldridStartup.CreateWindow(ref windowCI);

            GraphicsDeviceOptions options = new GraphicsDeviceOptions
            {
                PreferStandardClipSpaceYDirection = true,
                PreferDepthRangeZeroToOne = true
            };
            _graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, options);

            while (window.Exists)
            {
                window.PumpEvents();
            }
        }
    }
}