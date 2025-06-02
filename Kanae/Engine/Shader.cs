using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kanae;

public class Shader : IDisposable
{
    private readonly int _handle;
    private readonly Dictionary<string, int> _uniformLocations;

    public Shader(string vertPath, string fragPath, bool isPath = true)
    {
        int vertexShader, fragmentShader;

        if (isPath)
        {
            vertexShader = LoadShaderFromFile(vertPath, ShaderType.VertexShader);
            fragmentShader = LoadShaderFromFile(fragPath, ShaderType.FragmentShader);
        }
        else
        {
            vertexShader = LoadShaderFromString(vertPath, ShaderType.VertexShader);
            fragmentShader = LoadShaderFromString(fragPath, ShaderType.FragmentShader);
        }

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out var status);
        if (status == 0)
        {
            throw new Exception($"Program failed to link with error: {GL.GetProgramInfoLog(_handle)}");
        }

        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
        _uniformLocations = new Dictionary<string, int>();

        for (var i = 0; i < numberOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(_handle, i, out _, out _);
            var location = GL.GetUniformLocation(_handle, key);
            _uniformLocations.Add(key, location);
        }
    }

    private static int LoadShaderFromFile(string path, ShaderType type)
    {
        var source = File.ReadAllText(path);
        return LoadShaderFromString(source, type);
    }

    private static int LoadShaderFromString(string source, ShaderType type)
    {
        var shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
        if (status == 0)
        {
            throw new Exception($"Error compiling {type} shader: {GL.GetShaderInfoLog(shader)}");
        }

        return shader;
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void SetMatrix4(string name, Matrix4 data)
    {
        if (_uniformLocations.TryGetValue(name, out int location))
        {
            GL.UniformMatrix4(location, true, ref data);
        }
    }

    public void SetVector3(string name, Vector3 data)
    {
        if (_uniformLocations.TryGetValue(name, out int location))
        {
            GL.Uniform3(location, data);
        }
    }

    public void SetFloat(string name, float data)
    {
        if (_uniformLocations.TryGetValue(name, out int location))
        {
            GL.Uniform1(location, data);
        }
    }

    public void SetInt(string name, int data)
    {
        if (_uniformLocations.TryGetValue(name, out int location))
        {
            GL.Uniform1(location, data);
        }
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
        GC.SuppressFinalize(this);
    }
} 