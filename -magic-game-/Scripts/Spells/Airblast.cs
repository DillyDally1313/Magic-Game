using Godot;

public partial class Airblast : RigidBody2D {
	float speed = 600;

	public Node2D target;

	// runs once when object is created
	public override void _Ready() {
		// rotate towards the target
		Rotation = (target.Position - Position).Normalized().Angle();
	}

	// main physics loop, runs once every frame
	public override void _Process(double delta) {
		LinearVelocity = Transform.X * speed;
	}

	// when it exits the screen, delete it
	public void _OnScreenExited() {
		QueueFree();
	}
}