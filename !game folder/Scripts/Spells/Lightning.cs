using Godot;
using System;
using System.Diagnostics;

public partial class Lightning : Line2D {
	float timeAlive = 0.4f;
	float maxLength = 150;
    public Vector2 start;
	public Vector2 mouse;

    public override void _Ready() {
		// make bolt go from player to direction of mouse
        SetPointPosition(0, start);

		// if the mouse is closer than max length, go to mouse
		if (start.DistanceTo(mouse) < maxLength) {
			SetPointPosition(1, mouse);
		} else {
			SetPointPosition(1, start + start.DirectionTo(mouse) * maxLength);
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override async void _Process(double delta) {
		// remove after certain amount of time
		await ToSignal(GetTree().CreateTimer(timeAlive), "timeout");
		QueueFree();
	}
}