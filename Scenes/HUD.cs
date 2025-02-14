using Godot;
using System;

public class HUD : CanvasLayer
{
    private LineEdit nameInput;

    public override void _Ready()
    {
        nameInput = GetNode<LineEdit>("LineEdit");
        nameInput.Connect("text_entered", this, nameof(OnNameEntered));
    }

    private void OnNameEntered(string text)
    {
        GD.Print("User entered name: " + text);
        // Store the entered name in the Global singleton
        Global.PlayerName = text;
    }
}