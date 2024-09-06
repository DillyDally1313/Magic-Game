using Godot;

public partial class Lightning : RigidBody2D {
	float timeAlive = 1;
	float distance;

	public float rotation;
	public Node2D target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// rotate towards the target
		Rotation = rotation;

		GetNode<AnimationPlayer>("AnimationPlayer").Play("animate");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta) {
		// deletes itself after certain amount of time
		await ToSignal(GetTree().CreateTimer(timeAlive), "timeout");
		QueueFree();
	}
}