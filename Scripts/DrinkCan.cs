using Godot;

public partial class DrinkCan : Area2D
{
	[Export]
	public float SpeedBoostMultiplier { get; set; } = 2.0f;
	
	[Export]
	public float SpeedBoostDuration { get; set; } = 5.0f;

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Character player)
		{
			player.ApplySpeedBoost(SpeedBoostMultiplier, SpeedBoostDuration);
			// Optionally, remove the drink can after use
			QueueFree();
		}
	}
}
