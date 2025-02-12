using Godot;
using System;

public class Main : Node2D
{
    public override void _Ready()
    {
        GD.Print("Game started!");
    }

    public override void _Process(float delta)
    {
        GD.Print("Game loop!");
        // Game loop logic here
    }
}
