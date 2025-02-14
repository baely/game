using Godot;
using System;

/// <summary>
/// Represents a player character that can move on a grid-based system using MoveAndCollide
/// </summary>
public partial class Character : KinematicBody2D
{
	private const float POSITION_THRESHOLD = 1.0f;
	private const string DIRECTION_DOWN = "down";
	private const string DIRECTION_UP = "up";
	private const string DIRECTION_SIDE = "side";

	[Export]
	public int GridSize { get; set; } = 16;

	[Export]
	public float Speed { get; set; } = 32.0f;

	private bool isMoving = false;
	private Vector2 targetPosition = Vector2.Zero;
	private Vector2 velocity = Vector2.Zero;
	private string lastDirection = DIRECTION_DOWN;
	private float originalSpeed;
	private float speedMultiplier = 1.0f;
	private float speedBoostTimeLeft = 0.0f;
	private AnimatedSprite sprite;
	private SpeedBoostUI speedBoostUI;

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		speedBoostUI = GetNode<SpeedBoostUI>("../SpeedBoostUI"); // Adjust the path as needed
		speedBoostUI.SetPlayer(this);
		originalSpeed = Speed;
		Position = Position.Snapped(Vector2.One * GridSize);
		targetPosition = Position;
		PlayIdleAnimation();
	}

	/// <summary>
	/// Applies a temporary speed boost to the character
	/// </summary>
	/// <param name="multiplier">Speed multiplier to apply</param>
	/// <param name="duration">Duration of the speed boost in seconds</param>
	public void ApplySpeedBoost(float multiplier, float duration)
	{
		speedMultiplier = multiplier;
		speedBoostTimeLeft = duration;
		Speed = originalSpeed * speedMultiplier;
		sprite.SpeedScale = speedMultiplier;
		speedBoostUI.ShowBoostTimer(duration);
	}

	public override void _Process(float delta)
	{
		ProcessSpeedBoost(delta);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (isMoving)
		{
			ProcessMovement(delta);
		}
		else
		{
			ProcessInput();
		}
	}

	private void ProcessSpeedBoost(float delta)
	{
		if (speedBoostTimeLeft > 0)
		{
			speedBoostTimeLeft -= delta;
			speedBoostUI.UpdateBoostTimer(speedBoostTimeLeft);
			if (speedBoostTimeLeft <= 1)
			{
				Speed = originalSpeed * ((speedMultiplier - 1) * speedBoostTimeLeft + 1);
				sprite.SpeedScale = (speedMultiplier - 1) * speedBoostTimeLeft + 1;
			}
			if (speedBoostTimeLeft <= 0)
			{
				ResetSpeedBoost();
			}
		}
	}

	private void ResetSpeedBoost()
	{
		speedMultiplier = 1.0f;
		Speed = originalSpeed;
		sprite.SpeedScale = 1.0f;
	}

	private void ProcessMovement(float delta)
	{
		Vector2 moveDirection = (targetPosition - Position).Normalized();
		velocity = moveDirection * Speed;
		
		KinematicCollision2D collision = MoveAndCollide(velocity * delta);
		
		if (collision != null)
		{
			// Handle collision - stop movement and reset target position
			targetPosition = Position;
			isMoving = false;
			PlayIdleAnimation();
		}
		else if (Position.DistanceTo(targetPosition) < POSITION_THRESHOLD)
		{
			Position = targetPosition;
			isMoving = false;
			PlayIdleAnimation();
		}
	}

	private void ProcessInput()
	{
		Vector2 direction = GetInputDirection();
		
		if (direction != Vector2.Zero)
		{
			// Check if the next position is valid before moving
			Vector2 nextPosition = Position + (direction * GridSize);
			var testMove = TestMove(GlobalTransform, direction * GridSize);
			
			if (!testMove)
			{
				targetPosition = nextPosition;
				isMoving = true;
				PlayWalkAnimation();
			}
		}
	}

	private Vector2 GetInputDirection()
	{
		Vector2 direction = Vector2.Zero;
		
		if (Input.IsActionPressed("ui_right"))
		{
			direction.x += 1;
			sprite.FlipH = false;
			lastDirection = DIRECTION_SIDE;
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			direction.x -= 1;
			sprite.FlipH = true;
			lastDirection = DIRECTION_SIDE;
		}
		else if (Input.IsActionPressed("ui_up"))
		{
			direction.y -= 1;
			lastDirection = DIRECTION_UP;
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			direction.y += 1;
			lastDirection = DIRECTION_DOWN;
		}

		return direction;
	}

	private void PlayIdleAnimation()
	{
		string nextAnimation = lastDirection switch
		{
			DIRECTION_DOWN => "idle",
			DIRECTION_UP => "idle_back",
			DIRECTION_SIDE => "idle_side",
			_ => "idle"
		};

		sprite.Animation = nextAnimation;
	}

	private void PlayWalkAnimation()
	{
		string animation = lastDirection switch
		{
			DIRECTION_DOWN => "walk",
			DIRECTION_UP => "walk_back",
			DIRECTION_SIDE => "walk_side",
			_ => "walk"
		};
		
		sprite.Animation = animation;
	}
}
