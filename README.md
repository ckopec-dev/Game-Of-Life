
# Game of Life

A Conway's Game of Life implementation in C# This will be a console-based version with a simple interface to watch the evolution of cellular automata.This C# implementation of Conway's Game of Life includes:

**Features:**
- **50x30 grid** with bordered display using Unicode characters
- **Multiple initial patterns**: Glider, Blinker, Block (still life), Toad oscillator, and random cells
- **Real-time animation** with generation counter
- **Interactive controls**: Press any key to start, ESC to exit during simulation
- **Visual representation**: Living cells shown as solid blocks (█), dead cells as spaces

**Game Rules Implemented:**
1. Live cell with 2-3 neighbors survives
2. Live cell with <2 neighbors dies (underpopulation)
3. Live cell with >3 neighbors dies (overpopulation)  
4. Dead cell with exactly 3 neighbors becomes alive (reproduction)

**How to use:**
1. Compile and run the program
2. Press any key to start the simulation
3. Watch the patterns evolve over generations
4. Press ESC during simulation to exit

The program starts with several interesting patterns including a glider that moves across the screen, oscillators that cycle between states, and some random cells that create unpredictable evolution. You can modify the `InitializeGrid()` method to add your own starting patterns or make the grid larger by changing the `Width` and `Height` constants.

Here are several ways to compile and run the C# Game of Life program:

## Method 1: Using .NET CLI (Recommended)

**Prerequisites:** Install .NET SDK from https://dotnet.microsoft.com/download

1. **Save the code** to a file named `Program.cs`

2. **Open terminal/command prompt** in the same directory

3. **Create a new console project:**
```bash
dotnet new console -n GameOfLife
cd GameOfLife
```

4. **Replace the default Program.cs** with the Game of Life code

5. **Run the program:**
```bash
dotnet run
```

## Method 2: Quick Single-File Compilation

1. **Save the code** as `GameOfLife.cs`

2. **Compile directly:**
```bash
dotnet new console -n temp
cd temp
# Replace Program.cs with your code, then:
dotnet run
```

## Method 3: Using Visual Studio

1. **Open Visual Studio**
2. **Create new project:** File → New → Project → Console App (.NET)
3. **Replace Program.cs** with the Game of Life code
4. **Press F5** or click "Start" to run

## Method 4: Using Visual Studio Code

1. **Install C# extension** in VS Code
2. **Save code** as `Program.cs`
3. **Open terminal in VS Code** (Ctrl+`)
4. **Run:**
```bash
dotnet new console -n GameOfLife
cd GameOfLife
# Replace Program.cs, then:
dotnet run
```

## Method 5: Direct Compilation (Advanced)

If you have .NET SDK installed:
```bash
csc Program.cs
Program.exe
```

**Most users should use Method 1** - it's the modern, standard approach for .NET development. The program will run in your console/terminal window and display the animated Game of Life simulation.
