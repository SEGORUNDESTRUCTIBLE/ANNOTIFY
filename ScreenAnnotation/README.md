# Screen Annotation

A simple and modern desktop screen annotation tool for Windows 10/11, built with C# and WPF. This application allows you to draw directly on your screen, making it perfect for presentations, screen recordings, and online teaching.

## Features

- **Transparent Overlay**: A fully transparent window that sits on top of all other applications, allowing you to draw over anything.
- **Global Hotkey**: Toggle annotation mode on and off with a global hotkey (**Ctrl+Alt+A**).
- **Drawing Tools**:
    - **Pen**: A freehand pen tool for drawing.
    - **Highlighter**: A semi-transparent highlighter to emphasize content.
    - **Eraser**: A stroke-based eraser to remove annotations.
- **Customization**:
    - **Color Palette**: Choose from a selection of colors.
    - **Thickness Slider**: Adjust the thickness of the pen and highlighter.
- **Undo/Redo**: Step backward and forward through your annotations.
- **Clear All**: Instantly remove all drawings from the screen.
- **Screenshot**: Capture your screen with annotations and save it as a PNG file.
- **Multi-Monitor Support**: The annotation window automatically appears on your active monitor.

## Requirements

- Windows 10 or Windows 11
- .NET 6.0 SDK (or later)

## How to Build

1.  **Clone the repository** (or download the source code).
2.  **Open a terminal** (like Command Prompt or PowerShell) and navigate to the `ScreenAnnotation` project directory.
3.  **Run the build command**:
    ```sh
    dotnet build --configuration Release
    ```
4.  The compiled application will be located in the `bin/Release/net6.0-windows/` directory.

## How to Run

After building the application, you can run it directly from the output folder:

```sh
./bin/Release/net6.0-windows/ScreenAnnotation.exe
```

Alternatively, you can use the `dotnet run` command from the project directory:

```sh
dotnet run --project ScreenAnnotation.csproj
```

## How to Use

1.  **Launch the application**. The annotation window will be hidden by default.
2.  **Press `Ctrl+Alt+A`** to show the annotation overlay and the toolbar.
3.  **Use the toolbar** to select your desired tool, color, and thickness.
4.  **Draw on the screen**.
5.  **Press `Ctrl+Alt+A`** again to hide the annotations and interact with the desktop underneath.
6.  Use the **Screenshot** button to save your work or the **Exit** button to close the application.

## Creating an Installer (MSIX)

To package this application into an MSIX installer for easy distribution, you can add a "Windows Application Packaging Project" to the solution in Visual Studio.

1.  Open the `ScreenAnnotation.csproj` in Visual Studio.
2.  Right-click the solution in the Solution Explorer and go to **Add > New Project...**.
3.  Search for and select the **Windows Application Packaging Project** template. Give it a name (e.g., `ScreenAnnotation.Package`).
4.  In the new packaging project, right-click the **Dependencies** node (or Applications node) and select **Add Project Reference...**.
5.  Check the `ScreenAnnotation` project and click **OK**.
6.  Set the packaging project as the startup project.
7.  To create the package, right-click the packaging project and select **Publish > Create App Packages...**. Follow the wizard to create the MSIX installer. You can choose to sign it with a temporary certificate for testing.

This will produce an `.msix` file that can be installed on Windows 10/11 machines.
