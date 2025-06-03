# Kanae Engine Virtue Documentation
© 2024 Kyon Soft - Maxim Alexander Lamas

## Overview
Kanae Engine Virtue is a modern game engine built using C# and OpenGL through OpenTK. It provides a robust foundation for game development with an intuitive user interface powered by ImGui.NET.

## System Requirements

### Minimum Requirements
- Operating System: Windows 10/11 (recommended) or macOS 12+
- .NET SDK 9.0 or later
- Graphics: OpenGL 4.1 compatible GPU
- RAM: 4GB minimum
- Storage: 100MB for engine installation

### Recommended Requirements
- Operating System: Windows 10/11
- .NET SDK 9.0 or later
- Graphics: Dedicated GPU with OpenGL 4.1+ support
- RAM: 8GB or more
- Storage: 1GB or more for engine and projects

## Engine Architecture

### Core Components

#### Transform System
- Position, rotation, and scale manipulation
- Parent-child hierarchy support
- World and local space transformations

#### GameObject System
- Component-based architecture
- Dynamic object creation and destruction
- Material and mesh handling

#### Scene Management
- Scene loading and saving
- Primitive object creation (cube, sphere, plane)
- Scene hierarchy management

### Graphics Pipeline

#### Rendering System
- OpenGL 4.1 based rendering
- Forward rendering pipeline
- Material system with shader support
- Basic lighting system

#### Shader System
- GLSL shader support
- Vertex and fragment shader compilation
- Uniform management
- Built-in shader library

### User Interface

#### Launcher Window
- Project creation and management
- Resolution settings (800x600 to 3840x2160)
- Project name configuration
- Quick launch functionality

#### Editor Interface
- ImGui-based UI system
- Scene hierarchy window
- Properties window with transform controls
- 3D viewport with camera controls
- Main menu system

## Controls

### Camera Controls
- WASD: Camera movement
- Mouse: Camera rotation
- Mouse Wheel: Zoom in/out

### Interface Controls
- Left Click: Select objects
- Right Click: Context menus
- Drag & Drop: Hierarchy management

## Project Structure

### Directory Layout
```
Kanae/
├── Core/
│   ├── Engine/
│   └── Components/
├── Engine/
│   ├── UI/
│   ├── Shader/
│   └── Components/
└── Resources/
```

### Key Files
- `Program.cs`: Engine entry point
- `KanaeCore.cs`: Core engine functionality
- `LauncherWindow.cs`: Project launcher interface
- `ImGuiController.cs`: UI system integration

## Platform-Specific Considerations

### Windows
- Full OpenGL support
- Native graphics driver integration
- Optimal performance

### macOS
- Metal-based OpenGL implementation
- Some performance limitations
- Requires specific OpenGL context configuration

## Best Practices

### Project Organization
1. Use meaningful project names
2. Organize assets in appropriate folders
3. Follow the component-based architecture
4. Maintain clean scene hierarchies

### Performance Optimization
1. Minimize draw calls
2. Use appropriate primitive types
3. Implement proper object pooling
4. Manage memory efficiently

### Development Workflow
1. Create new project through launcher
2. Set up scene hierarchy
3. Add and configure game objects
4. Implement game logic through components
5. Test and iterate

## Troubleshooting

### Common Issues

#### Rendering Issues
- Ensure graphics drivers are up to date
- Verify OpenGL version compatibility
- Check shader compilation errors
- Validate texture formats and sizes

#### Performance Problems
- Monitor draw call count
- Check object hierarchy complexity
- Verify memory usage
- Profile CPU and GPU usage

#### Project Loading Issues
- Verify project file integrity
- Check file permissions
- Validate asset references
- Ensure correct project structure

### Error Messages
- Shader compilation errors: Check GLSL version and syntax
- OpenGL context errors: Verify graphics driver compatibility
- Memory allocation errors: Monitor resource usage
- File access errors: Check permissions and paths

## Support and Contact

### Technical Support
For technical issues and bug reports, please contact:
- Ninesstrife416c78@outlook.com


### License Information
Kanae Engine Virtue
Copyright © 2025 Kyon Soft
All rights reserved.
Developed by Maxim Alexander Lamas

## Version History

### Current Version
- Version: 1.0.0
- Release Date: 2025
- Platform: Windows, macOS

### Key Features
- Modern OpenGL rendering
- ImGui-based user interface
- Scene management system
- Component-based architecture
- Cross-platform support

### Aditional info
---------- Execution info -----------
- once in the root folder of the project, run the following command to build:

"dotnet build"

- afterwards, run:
"dotnet run"

-- the engine launcher window will open once that done, youre good to go, happy development!

-------------------------------------


### Engine news 

### Version 1.0.1
- improved stability and memory management 
-improved interface 
- build 1.0.1


*This documentation is subject to updates and changes as the engine evolves. For the latest information, please check the official documentation or contact Kyon Soft support.* 