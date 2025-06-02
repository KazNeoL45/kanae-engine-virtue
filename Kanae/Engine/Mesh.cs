using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Kanae;

public class Mesh : IDisposable
{
    private readonly int _vao;
    private readonly int _vbo;
    private readonly int _ebo;
    private readonly int _indexCount;

    public Mesh(float[] vertices, uint[] indices)
    {
        _indexCount = indices.Length;

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        // Position attribute
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Normal attribute
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(BeginMode.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
        GC.SuppressFinalize(this);
    }

    public static Mesh CreateCube()
    {
        float[] vertices = {
            // Front face
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Bottom-left
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Bottom-right
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Top-right
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Top-left
            // Back face
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Bottom-left
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Bottom-right
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Top-right
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Top-left
            // Top face
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Back-left
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Back-right
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f, // Front-right
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f, // Front-left
            // Bottom face
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Back-left
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Back-right
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f, // Front-right
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f, // Front-left
            // Right face
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f, // Bottom-back
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f, // Top-back
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Top-front
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Bottom-front
            // Left face
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f, // Bottom-back
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f, // Top-back
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Top-front
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f  // Bottom-front
        };

        uint[] indices = {
            0,  1,  2,  2,  3,  0,  // Front
            4,  5,  6,  6,  7,  4,  // Back
            8,  9,  10, 10, 11, 8,  // Top
            12, 13, 14, 14, 15, 12, // Bottom
            16, 17, 18, 18, 19, 16, // Right
            20, 21, 22, 22, 23, 20  // Left
        };

        return new Mesh(vertices, indices);
    }

    public static Mesh CreatePlane()
    {
        float[] vertices = {
            // Position          Normal
            -0.5f, 0.0f,  0.5f, 0.0f, 1.0f, 0.0f, // Top-left
             0.5f, 0.0f,  0.5f, 0.0f, 1.0f, 0.0f, // Top-right
             0.5f, 0.0f, -0.5f, 0.0f, 1.0f, 0.0f, // Bottom-right
            -0.5f, 0.0f, -0.5f, 0.0f, 1.0f, 0.0f  // Bottom-left
        };

        uint[] indices = {
            0, 1, 2,
            2, 3, 0
        };

        return new Mesh(vertices, indices);
    }

    public static Mesh CreateSphere(int segments = 32)
    {
        var vertices = new List<float>();
        var indices = new List<uint>();

        for (int y = 0; y <= segments; y++)
        {
            float ySegment = (float)y / segments;
            float yPos = MathF.Cos(ySegment * MathF.PI);
            float yRadius = MathF.Sin(ySegment * MathF.PI);

            for (int x = 0; x <= segments; x++)
            {
                float xSegment = (float)x / segments;
                float xPos = MathF.Cos(xSegment * MathF.PI * 2.0f) * yRadius;
                float zPos = MathF.Sin(xSegment * MathF.PI * 2.0f) * yRadius;

                // Position
                vertices.Add(xPos * 0.5f);
                vertices.Add(yPos * 0.5f);
                vertices.Add(zPos * 0.5f);

                // Normal
                vertices.Add(xPos);
                vertices.Add(yPos);
                vertices.Add(zPos);
            }
        }

        for (int y = 0; y < segments; y++)
        {
            for (int x = 0; x < segments; x++)
            {
                uint current = (uint)(y * (segments + 1) + x);
                uint next = current + 1;
                uint bottom = (uint)((y + 1) * (segments + 1) + x);
                uint bottomNext = bottom + 1;

                indices.Add(current);
                indices.Add(next);
                indices.Add(bottom);

                indices.Add(next);
                indices.Add(bottomNext);
                indices.Add(bottom);
            }
        }

        return new Mesh(vertices.ToArray(), indices.ToArray());
    }
} 