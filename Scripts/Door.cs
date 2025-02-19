using Godot;
using System;

public class Door : Area2D
{
	[Export] public string Destination;

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			Global.Controller.Goto(Destination);
		}
	}
}
