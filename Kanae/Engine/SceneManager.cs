using OpenTK.Mathematics;
using ImGuiNET;
using System.Collections.Generic;
using System;

namespace Kanae
{
    public enum PrimitiveType
    {
        Cube,
        Sphere,
        Plane
    }

    public class SceneManager : IDisposable
    {
        private readonly List<GameObject> _objects = new();
        private GameObject? _selectedObject;
        private readonly Dictionary<PrimitiveType, Mesh> _primitiveMeshes = new();
        private readonly Shader _defaultShader;

        public SceneManager()
        {
            _defaultShader = new Shader("Shaders/basic.vert", "Shaders/basic.frag");
            InitializePrimitiveMeshes();
        }

        private void InitializePrimitiveMeshes()
        {
            _primitiveMeshes[PrimitiveType.Cube] = Mesh.CreateCube();
            _primitiveMeshes[PrimitiveType.Sphere] = Mesh.CreateSphere();
            _primitiveMeshes[PrimitiveType.Plane] = Mesh.CreatePlane();
        }

        public void NewScene()
        {
            foreach (var obj in _objects)
            {
                obj.Dispose();
            }
            _objects.Clear();
            _selectedObject = null;
        }

        public GameObject CreatePrimitive(PrimitiveType type)
        {
            var mesh = _primitiveMeshes[type];
            var obj = new GameObject(mesh)
            {
                Name = $"{type}_{_objects.Count}",
                Material = new Material(_defaultShader)
            };
            _objects.Add(obj);
            return obj;
        }

        public void RenderSceneHierarchy()
        {
            foreach (var obj in _objects)
            {
                bool isSelected = obj == _selectedObject;
                if (ImGui.Selectable(obj.Name, isSelected))
                {
                    _selectedObject = obj;
                }
            }
        }

        public void RenderProperties()
        {
            if (_selectedObject == null) return;

            var obj = _selectedObject;

            // Name
            var name = obj.Name;
            if (ImGui.InputText("Name", ref name, 100))
            {
                obj.Name = name;
            }

            // Transform
            var position = obj.Transform.Position.ToSystem();
            if (ImGui.DragFloat3("Position", ref position, 0.1f))
            {
                obj.Transform.Position = position.ToOpenTK();
            }

            var rotation = obj.Transform.EulerAngles.ToSystem();
            if (ImGui.DragFloat3("Rotation", ref rotation, 1.0f))
            {
                obj.Transform.EulerAngles = rotation.ToOpenTK();
            }

            var scale = obj.Transform.Scale.ToSystem();
            if (ImGui.DragFloat3("Scale", ref scale, 0.1f, 0.01f, 100f))
            {
                obj.Transform.Scale = scale.ToOpenTK();
            }
        }

        public void RenderScene(Camera camera)
        {
            foreach (var obj in _objects)
            {
                if (obj.Material != null)
                {
                    obj.Material.Use();
                    obj.Material.SetMatrix4("model", obj.Transform.Matrix);
                    obj.Material.SetMatrix4("view", camera.ViewMatrix);
                    obj.Material.SetMatrix4("projection", camera.ProjectionMatrix);
                    obj.Mesh.Draw();
                }
            }
        }

        public void Dispose()
        {
            foreach (var obj in _objects)
            {
                obj.Dispose();
            }
            foreach (var mesh in _primitiveMeshes.Values)
            {
                mesh.Dispose();
            }
            _defaultShader.Dispose();
            GC.SuppressFinalize(this);
        }
    }
} 