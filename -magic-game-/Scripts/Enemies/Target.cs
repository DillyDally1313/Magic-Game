using Godot;
using System;

public partial class Target : StaticBody2D
{
	float speed = 0;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position -= new Vector2(speed * (float)delta, 0);

		if (Position.X < 0) {
			Position = new Vector2(1000, Position.Y);
		}
	}
}
