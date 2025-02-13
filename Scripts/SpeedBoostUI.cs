using Godot;

public partial class SpeedBoostUI : CanvasLayer
{
	private ProgressBar progressBar;
	private Label durationLabel;
	private Character player;
	private float currentDuration;

	public override void _Ready()
	{
		progressBar = GetNode<ProgressBar>("ProgressBar");
		durationLabel = GetNode<Label>("ProgressBar/Label");
		// Hide the bar initially
		progressBar.Visible = false;
	}
	
	public override void _Process(float delta)
	{
		if (progressBar.Visible)
		{
			// Update the label text with remaining time rounded to 1 decimal place
			durationLabel.Text = $"Zone Energy remaining: {currentDuration:F1}s";
		}
	}

	public void SetPlayer(Character playerNode)
	{
		player = playerNode;
	}

	public void ShowBoostTimer(float duration)
	{
		currentDuration = duration;
		progressBar.MaxValue = duration;
		progressBar.Value = duration;
		progressBar.Visible = true;
		durationLabel.Text = $"Zone Energy remaining: {duration:F1}s";
	}

	public void UpdateBoostTimer(float timeLeft)
	{
		currentDuration = timeLeft;
		// Use Godot's built-in tweening for smooth progress bar updates
		progressBar.Value = timeLeft;
		
		if (timeLeft <= 0)
		{
			progressBar.Visible = false;
		}
	}
}
