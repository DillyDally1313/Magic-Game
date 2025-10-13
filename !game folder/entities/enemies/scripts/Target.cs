using Godot;

public partial class Target : StaticBody2D {
	[Export] PlayerController player;
	float speed = 0;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
		Position = Position.MoveToward(player.Position, speed * (float) delta);
	}
}