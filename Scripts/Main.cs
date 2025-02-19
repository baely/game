using Godot;
using System;

/// <summary>
/// Main game scene controller
/// </summary>
public partial class Main : Node2D
{
	public override void _Ready()
	{
		var controller = GetNode<GameController>("GameController");
		Global.Controller = controller;
	}
}
