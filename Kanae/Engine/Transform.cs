using OpenTK.Mathematics;
using System;

namespace Kanae
{
    public class Transform
    {
        private Vector3 _position = Vector3.Zero;
        private Vector3 _eulerAngles = Vector3.Zero;
        private Vector3 _scale = Vector3.One;
        private Matrix4 _matrix = Matrix4.Identity;
        private bool _isDirty = true;

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                _isDirty = true;
            }
        }

        public Vector3 EulerAngles
        {
            get => _eulerAngles;
            set
            {
                _eulerAngles = value;
                _isDirty = true;
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _isDirty = true;
            }
        }

        public Matrix4 Matrix
        {
            get
            {
                if (_isDirty)
                {
                    UpdateMatrix();
                    _isDirty = false;
                }
                return _matrix;
            }
        }

        public void Update(double deltaTime)
        {
            // Transform update logic can be added here if needed
            // Currently, the transform is updated automatically when properties change
            // through the _isDirty flag and UpdateMatrix()
        }

        private void UpdateMatrix()
        {
            var rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_eulerAngles.X)) *
                          Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_eulerAngles.Y)) *
                          Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_eulerAngles.Z));

            _matrix = Matrix4.CreateScale(_scale) *
                     rotation *
                     Matrix4.CreateTranslation(_position);
        }
    }
} 