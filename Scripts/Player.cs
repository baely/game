using Godot;
using System;

public partial class Player : Character {
	private const string UiLeft = "ui_left";
	private const string UiRight = "ui_right";
	private const string UiUp = "ui_up";
	private const string UiDown = "ui_down";

	public override void _PhysicsProcess(float delta)
	{
		Vector2 inputVector = GetInputVector();
		if (inputVector != Vector2.Zero)
		{
			InputVector = GetInputVector();
		}
		
		base._PhysicsProcess(delta);
	}
	
	private Vector2 GetInputVector()
	{
		var inputVector = Vector2.Zero;
		
		if (Input.IsActionPressed(UiLeft))
			inputVector.x -= 1;

		if (Input.IsActionPressed(UiRight))
			inputVector.x += 1;
		
		if (Input.IsActionPressed(UiUp))
			inputVector.y -= 1;
		
		if (Input.IsActionPressed(UiDown))
			inputVector.y += 1;

		ShiftMultiplier = 1.0f;
		if (Input.IsActionPressed("ui_shift"))
			ShiftMultiplier = 4.0f;
		
		if (inputVector.Length() > 1)
			return Vector2.Zero;
		
		return inputVector;
	}
}
