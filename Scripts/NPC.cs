using Godot;
using System;
using System.Collections.Generic;

public class NPC : KinematicBody2D
{
	private float textTimeLeft = 0.0f;

	private Control control;
	
	public override void _Ready()
	{
		control = GetNode<Control>("Control");
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		
		if (textTimeLeft > 0)
		{
			textTimeLeft -= delta;
			if (textTimeLeft <= 0)
			{
				control.Visible = false;
			}
		}
	}

	public void Bump()
	{
		
		List<string> items = new List<string> { "Watch out mate!", "I'm walking here!", "OI" };
		Random random = new Random();
		string text = items[random.Next(items.Count)];
		textTimeLeft = 3.0f;

		control.Visible = true;
		control.GetNode<Label>("Label").Text = text;
	}
	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
