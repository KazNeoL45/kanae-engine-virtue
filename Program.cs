using Kanae;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

class Program
{
    static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(500, 400),
            Title = "Kanae Engine Launcher",
            WindowBorder = WindowBorder.Fixed,
            StartVisible = true,
            APIVersion = new Version(4, 1),
            Profile = ContextProfile.Core,
            Flags = ContextFlags.ForwardCompatible,
            NumberOfSamples = 0,
            IsEventDriven = true,
            WindowState = WindowState.Normal,
            Vsync = VSyncMode.On
        };

        var gameWindowSettings = GameWindowSettings.Default;
        gameWindowSettings.UpdateFrequency = 60.0;

        try
        {
            using var launcherWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
            ImGuiController? imGuiController = null;
            LauncherWindow? launcher = null;

            launcherWindow.Load += () =>
            {
                Console.WriteLine($"OpenGL Version: {GL.GetString(StringName.Version)}");
                Console.WriteLine($"OpenGL Vendor: {GL.GetString(StringName.Vendor)}");
                Console.WriteLine($"OpenGL Renderer: {GL.GetString(StringName.Renderer)}");

                // Initialize OpenGL state
                GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
                
                // Basic OpenGL settings
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Lequal);
                
                // Initialize ImGui
                try
                {
                    imGuiController = new ImGuiController(launcherWindow.ClientSize.X, launcherWindow.ClientSize.Y);
                    launcher = new LauncherWindow((width, height, projectName) =>
                    {
                        launcherWindow.Close();
                        using var engine = new KanaeCore(width, height, projectName);
                        engine.Run();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing ImGui: {ex}");
                    throw;
                }

                launcherWindow.CenterWindow();
            };

            launcherWindow.RenderFrame += (e) =>
            {
                if (imGuiController == null || launcher == null) return;

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
                // Save OpenGL state
                int lastProgram = GL.GetInteger(GetPName.CurrentProgram);
                bool lastBlendEnabled = GL.IsEnabled(EnableCap.Blend);
                bool lastDepthTestEnabled = GL.IsEnabled(EnableCap.DepthTest);
                
                try
                {
                    // Set state for ImGui rendering
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    GL.Disable(EnableCap.DepthTest);
                    GL.Disable(EnableCap.CullFace);
                    
                    imGuiController.Update(launcherWindow, (float)e.Time);
                    launcher.Render();
                    imGuiController.Render();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during render: {ex}");
                }
                finally
                {
                    // Restore OpenGL state
                    if (!lastBlendEnabled) GL.Disable(EnableCap.Blend);
                    if (lastDepthTestEnabled) GL.Enable(EnableCap.DepthTest);
                    GL.UseProgram(lastProgram);
                }
                
                launcherWindow.SwapBuffers();

                if (!launcher.IsOpen)
                {
                    launcherWindow.Close();
                }
            };

            launcherWindow.Resize += (e) =>
            {
                if (imGuiController == null) return;

                GL.Viewport(0, 0, launcherWindow.ClientSize.X, launcherWindow.ClientSize.Y);
                imGuiController.WindowResized(launcherWindow.ClientSize.X, launcherWindow.ClientSize.Y);
            };

            launcherWindow.TextInput += (e) =>
            {
                if (imGuiController == null) return;
                imGuiController.PressChar((char)e.Unicode);
            };

            launcherWindow.MouseWheel += (e) =>
            {
                if (imGuiController == null) return;
                imGuiController.MouseScroll(e.Offset);
            };

            launcherWindow.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running window: {ex}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
} 