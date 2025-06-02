using ImGuiNET;
using OpenTK.Mathematics;
using System;
using System.IO;
using System.Numerics;

namespace Kanae
{
    public class LauncherWindow
    {
        private bool _isOpen = true;
        private int _width = 1280;
        private int _height = 720;
        private string _projectName = "New Project";
        private string _projectPath = "";
        private bool _showFileDialog = false;
        private Action<int, int, string>? _onLaunch;
        private System.Numerics.Vector4 _accentColor = new System.Numerics.Vector4(0.2f, 0.6f, 1.0f, 1.0f);
        private System.Numerics.Vector4 _bgColor = new System.Numerics.Vector4(0.1f, 0.1f, 0.1f, 1.0f);
        private System.Numerics.Vector4 _buttonHoverColor = new System.Numerics.Vector4(0.25f, 0.65f, 1.0f, 1.0f);

        public bool IsOpen => _isOpen;

        public LauncherWindow(Action<int, int, string> onLaunch)
        {
            _onLaunch = onLaunch;
            _projectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "KanaeProjects");
            
            // Set global ImGui style
            var style = ImGui.GetStyle();
            style.WindowRounding = 12.0f;
            style.FrameRounding = 6.0f;
            style.PopupRounding = 6.0f;
            style.ScrollbarRounding = 6.0f;
            style.GrabRounding = 6.0f;
            style.TabRounding = 6.0f;
            
            // Set colors
            style.Colors[(int)ImGuiCol.WindowBg] = _bgColor;
            style.Colors[(int)ImGuiCol.TitleBg] = _bgColor;
            style.Colors[(int)ImGuiCol.TitleBgActive] = _bgColor;
            style.Colors[(int)ImGuiCol.Button] = _accentColor;
            style.Colors[(int)ImGuiCol.ButtonHovered] = _buttonHoverColor;
            style.Colors[(int)ImGuiCol.ButtonActive] = _accentColor * new System.Numerics.Vector4(0.8f, 0.8f, 0.8f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBg] = new System.Numerics.Vector4(0.15f, 0.15f, 0.15f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new System.Numerics.Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new System.Numerics.Vector4(0.25f, 0.25f, 0.25f, 1.0f);
        }

        public void Render()
        {
            if (!_isOpen) return;

            // Center the window on screen
            var io = ImGui.GetIO();
            var windowSize = new System.Numerics.Vector2(600, 500);
            var center = io.DisplaySize / 2;
            ImGui.SetNextWindowPos(center - windowSize / 2);
            ImGui.SetNextWindowSize(windowSize);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(20, 20));
            ImGui.Begin("Kanae Engine Launcher", ref _isOpen, 
                ImGuiWindowFlags.NoResize | 
                ImGuiWindowFlags.NoMove | 
                ImGuiWindowFlags.NoCollapse);

            // Title with custom styling
            ImGui.PushFont(ImGui.GetIO().Fonts.Fonts[0]);
            var title = "KANAE ENGINE";
            var titleSize = ImGui.CalcTextSize(title);
            ImGui.SetCursorPosX((windowSize.X - titleSize.X) * 0.5f);
            ImGui.PushStyleColor(ImGuiCol.Text, _accentColor);
            ImGui.Text(title);
            ImGui.PopStyleColor();
            ImGui.PopFont();

            ImGui.Spacing();
            ImGui.Spacing();
            
            // Subtitle
            var subtitle = "Create Something Amazing";
            var subtitleSize = ImGui.CalcTextSize(subtitle);
            ImGui.SetCursorPosX((windowSize.X - subtitleSize.X) * 0.5f);
            ImGui.TextColored(new System.Numerics.Vector4(0.7f, 0.7f, 0.7f, 1.0f), subtitle);
            
            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            ImGui.Spacing();

            // New Project Section
            ImGui.PushStyleColor(ImGuiCol.Text, _accentColor);
            ImGui.Text("CREATE NEW PROJECT");
            ImGui.PopStyleColor();
            ImGui.Spacing();
            
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new System.Numerics.Vector2(10, 8));
            
            // Project Name
            ImGui.TextColored(new System.Numerics.Vector4(0.8f, 0.8f, 0.8f, 1.0f), "Project Name");
            ImGui.SetNextItemWidth(-1);
            ImGui.InputText("##projectname", ref _projectName, 100);
            ImGui.Spacing();

            // Project Path
            ImGui.TextColored(new System.Numerics.Vector4(0.8f, 0.8f, 0.8f, 1.0f), "Project Location");
            ImGui.SetNextItemWidth(-120);
            ImGui.InputText("##projectpath", ref _projectPath, 260);
            ImGui.SameLine();
            if (ImGui.Button("Browse##path", new System.Numerics.Vector2(100, 0)))
            {
                _showFileDialog = true;
            }
            ImGui.Spacing();
            ImGui.Spacing();

            // Resolution with better layout
            ImGui.TextColored(new System.Numerics.Vector4(0.8f, 0.8f, 0.8f, 1.0f), "Resolution");
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(10, 8));
            
            ImGui.SetNextItemWidth(windowSize.X / 2 - 30);
            ImGui.InputInt("Width##width", ref _width);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(-1);
            ImGui.InputInt("Height##height", ref _height);
            
            ImGui.PopStyleVar(); // ItemSpacing

            // Clamp resolution values
            _width = Math.Clamp(_width, 800, 3840);
            _height = Math.Clamp(_height, 600, 2160);

            ImGui.PopStyleVar(); // FramePadding
            
            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            ImGui.Spacing();

            // Open Project Section
            ImGui.PushStyleColor(ImGuiCol.Text, _accentColor);
            ImGui.Text("OPEN EXISTING PROJECT");
            ImGui.PopStyleColor();
            ImGui.Spacing();
            
            if (ImGui.Button("Browse Projects", new System.Numerics.Vector2(150, 35)))
            {
                _showFileDialog = true;
            }

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            ImGui.Spacing();

            // Create/Launch button
            var buttonSize = new System.Numerics.Vector2(200, 40);
            ImGui.SetCursorPosX((windowSize.X - buttonSize.X) * 0.5f);
            if (ImGui.Button("CREATE PROJECT", buttonSize))
            {
                string fullPath = Path.Combine(_projectPath, _projectName);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                _onLaunch?.Invoke(_width, _height, _projectName);
                _isOpen = false;
            }

            // Simple file dialog with styling
            if (_showFileDialog)
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(500, 400));
                ImGui.Begin("Select Project Directory", ref _showFileDialog, 
                    ImGuiWindowFlags.NoCollapse);

                if (Directory.Exists(_projectPath))
                {
                    foreach (var dir in Directory.GetDirectories(_projectPath))
                    {
                        if (ImGui.Selectable(Path.GetFileName(dir), false, ImGuiSelectableFlags.None, new System.Numerics.Vector2(0, 30)))
                        {
                            _projectName = Path.GetFileName(dir);
                            _showFileDialog = false;
                            _onLaunch?.Invoke(_width, _height, _projectName);
                            _isOpen = false;
                        }
                    }
                }

                ImGui.End();
            }

            ImGui.PopStyleVar(); // WindowPadding
            ImGui.End();
        }
    }
} 