using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Kanae
{
    public class Material : IDisposable
    {
        private readonly Shader _shader;
        private readonly Dictionary<string, object> _properties = new();

        public Material(Shader shader)
        {
            _shader = shader;
        }

        public void Use()
        {
            _shader.Use();
            foreach (var (name, value) in _properties)
            {
                switch (value)
                {
                    case Matrix4 matrix:
                        _shader.SetMatrix4(name, matrix);
                        break;
                    case Vector3 vector:
                        _shader.SetVector3(name, vector);
                        break;
                    case float scalar:
                        _shader.SetFloat(name, scalar);
                        break;
                }
            }
        }

        public void SetMatrix4(string name, Matrix4 value)
        {
            _properties[name] = value;
        }

        public void SetVector3(string name, Vector3 value)
        {
            _properties[name] = value;
        }

        public void SetFloat(string name, float value)
        {
            _properties[name] = value;
        }

        public void Dispose()
        {
            _shader.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}