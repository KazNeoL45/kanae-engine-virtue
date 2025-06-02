using OpenTK.Mathematics;
using System;

namespace Kanae
{
    public class GameObject : IDisposable
    {
        public string Name { get; set; } = "GameObject";
        public Mesh Mesh { get; set; }
        public Material? Material { get; set; }
        public Transform Transform { get; }

        public GameObject(Mesh mesh)
        {
            Mesh = mesh;
            Transform = new Transform();
        }

        public virtual void Update(double deltaTime)
        {
            Transform.Update(deltaTime);
        }

        public virtual void Draw(Material material)
        {
            material.Use();
            material.SetMatrix4("model", Transform.Matrix);
            Mesh.Draw();
        }

        public void Dispose()
        {
            Material?.Dispose();
            Mesh?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
} 