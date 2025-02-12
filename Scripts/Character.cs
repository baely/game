using Godot;
using System;

public partial class Character : AnimatedSprite2D
{
	[Export]
	public int GridSize = 16;
	[Export]
	public float Speed = 200.0f;

	public float stamina = 100.0f;
	private float staminaRegenRate = 10.0f;
	private float staminaUseRate = 20.0f;

	private bool isMoving = false;
	private Vector2 targetPosition = Vector2.Zero;
	private string lastDirection = "down";

	public override void _Ready()
	{
		// Snap position to grid
		Position = Position.Snapped(Vector2.One * GridSize);
		targetPosition = Position;
		PlayIdleAnimation();
	}

	private void PlayIdleAnimation()
	{
		switch (lastDirection)
		{
			case "down":
				Play("idle");
				break;
			case "up":
				Play("idle_back");
				break;
			case "side":
				Play("idle_side");
				break;
		}
	}

	private void PlayWalkAnimation()
	{
		switch (lastDirection)
		{
			case "down":
				Play("walk");
				break;
			case "up":
				Play("walk_back");
				break;
			case "side":
				Play("walk_side");
				break;
		}
	}

	public override void _Process(double delta)
	{
		// Regenerate stamina when not moving
		if (!isMoving)
		{
			stamina = Mathf.Min(stamina + (staminaRegenRate * (float)delta), 100.0f);
		}

		if (isMoving)
		{
			Vector2 moveDirection = (targetPosition - Position).Normalized();
			Position += moveDirection * Speed * (float)delta;

			// Use stamina while moving
			stamina = Mathf.Max(stamina - (staminaUseRate * (float)delta), 0.0f);

			if (Position.DistanceTo(targetPosition) < 1)
			{
				Position = targetPosition;
				isMoving = false;
				PlayIdleAnimation();
			}
		}
		else
		{
			Vector2 direction = Vector2.Zero;
			// Only allow movement if we have stamina
			if (stamina > 0)
			{
				if (Input.IsActionPressed("ui_right"))
				{
					direction.X += 1;
					FlipH = false;
					lastDirection = "side";
				}
				else if (Input.IsActionPressed("ui_left"))
				{
					direction.X -= 1;
					FlipH = true;
					lastDirection = "side";
				}
				else if (Input.IsActionPressed("ui_up"))
				{
					direction.Y -= 1;
					lastDirection = "up";
				}
				else if (Input.IsActionPressed("ui_down"))
				{
					direction.Y += 1;
					lastDirection = "down";
				}
			}

			if (direction != Vector2.Zero)
			{
				targetPosition = Position + (direction * GridSize);
				isMoving = true;
				PlayWalkAnimation();
			}
		}
	}
}
