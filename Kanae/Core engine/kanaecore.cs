using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4; 
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ImGuiNET;
using Kanae;

namespace Kanae
{
    public class KanaeCore : GameWindow
    {
        private ImGuiController? _imGuiController;
        private SceneManager? _sceneManager;
        private Camera? _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        public KanaeCore(int width = 1280, int height = 720, string title = "Kanae Engine") 
            : base(GameWindowSettings.Default, 
                   new NativeWindowSettings
                   {
                       ClientSize = new Vector2i(width, height),
                       Title = title,
                       APIVersion = new Version(3, 3),
                       Profile = ContextProfile.Core
                   })
        {
            CenterWindow();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _imGuiController = new ImGuiController(ClientSize.X, ClientSize.Y);
            _sceneManager = new SceneManager();
            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorState = CursorState.Normal;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _imGuiController?.Update(this, (float)args.Time);

            // Begin ImGui window
            ImGui.Begin("Scene Hierarchy");
            _sceneManager?.RenderSceneHierarchy();
            ImGui.End();

            ImGui.Begin("Properties");
            _sceneManager?.RenderProperties();
            ImGui.End();

            // Main menu bar
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New Scene")) _sceneManager?.NewScene();
                    if (ImGui.MenuItem("Exit")) Close();
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Create"))
                {
                    if (ImGui.MenuItem("Cube")) _sceneManager?.CreatePrimitive(PrimitiveType.Cube);
                    if (ImGui.MenuItem("Sphere")) _sceneManager?.CreatePrimitive(PrimitiveType.Sphere);
                    if (ImGui.MenuItem("Plane")) _sceneManager?.CreatePrimitive(PrimitiveType.Plane);
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            // Render scene
            if (_sceneManager != null && _camera != null)
            {
                _sceneManager.RenderScene(_camera);
            }

            _imGuiController?.Render();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused || _camera == null)
                return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
                Close();

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.S))
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.A))
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.D))
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.Space))
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.LeftShift))
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;

            // Handle mouse input for camera rotation
            if (MouseState.IsButtonDown(MouseButton.Right))
            {
                if (_firstMove)
                {
                    _lastPos = new Vector2(MouseState.X, MouseState.Y);
                    _firstMove = false;
                }
                else
                {
                    var deltaX = MouseState.X - _lastPos.X;
                    var deltaY = MouseState.Y - _lastPos.Y;
                    _lastPos = new Vector2(MouseState.X, MouseState.Y);

                    _camera.Yaw += deltaX * sensitivity;
                    _camera.Pitch -= deltaY * sensitivity;
                }
            }
            else
            {
                _firstMove = true;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            if (_camera != null)
            {
                _camera.AspectRatio = ClientSize.X / (float)ClientSize.Y;
            }
            _imGuiController?.WindowResized(ClientSize.X, ClientSize.Y);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            _imGuiController?.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _imGuiController?.MouseScroll(e.Offset);
        }

        protected override void OnUnload()
        {
            _imGuiController?.Dispose();
            _sceneManager?.Dispose();
            base.OnUnload();
        }
    }

    public static class VectorExtensions
    {
        public static System.Numerics.Vector3 ToSystem(this Vector3 v) => new(v.X, v.Y, v.Z);
        public static Vector3 ToOpenTK(this System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
    }
}