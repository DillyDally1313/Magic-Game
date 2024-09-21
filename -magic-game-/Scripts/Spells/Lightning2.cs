using Godot;

public partial class Lightning2 : Line2D {

	public AnimationPlayer animationPlayer;
	public Vector2 start;
	public Vector2 target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		SetPointPosition(0, start);
		SetPointPosition(1, target);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta) {
		await ToSignal(GetTree().CreateTimer(animationPlayer.GetAnimation("vanish").Length), "timeout");
		QueueFree();
	}
}