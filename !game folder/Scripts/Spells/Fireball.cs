using Godot;

public partial class Fireball : RigidBody2D {
	float speed = 300;

	public Vector2 target;

	// runs once when object is created
	public override void _Ready() {
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