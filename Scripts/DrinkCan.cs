using Godot;

public partial class DrinkCan : Area2D
{
	[Export]
	public float SpeedBoostMultiplier { get; set; } = 2.0f;

	[Export]
	public float SpeedBoostDuration { get; set; } = 5.0f;

	private Tween _tween;
	private Sprite _sprite;

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
		_tween = GetNode<Tween>("Tween");
		_sprite = GetNode<Sprite>("Sprite");
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("OnBodyEntered called");
		if (body is Character player)
		{
			GD.Print("Character entered");
			player.ApplySpeedBoost(SpeedBoostMultiplier, SpeedBoostDuration);
			FlashAndJiggle();
		}
	}

	private void FlashAndJiggle()
	{
		GD.Print("FlashAndJiggle called");
		// Flash animation
		_tween.InterpolateProperty(_sprite, "modulate:a", 1.0f, 0.0f, 0.1f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.0f);
		_tween.InterpolateProperty(_sprite, "modulate:a", 0.0f, 1.0f, 0.1f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.1f);

		// Jiggle animation
		_tween.InterpolateProperty(_sprite, "scale", new Vector2(1, 1), new Vector2(1.2f, 0.8f), 0.1f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.0f);
		_tween.InterpolateProperty(_sprite, "scale", new Vector2(1.2f, 0.8f), new Vector2(0.8f, 1.2f), 0.1f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.1f);
		_tween.InterpolateProperty(_sprite, "scale", new Vector2(0.8f, 1.2f), new Vector2(1, 1), 0.1f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.2f);

		_tween.Start();
	}
}
