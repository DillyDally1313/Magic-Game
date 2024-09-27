using Godot;
using System.Diagnostics;

public partial class PlayerWalking : State {
	Player player;

    public override void Ready() {
        player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Walking State");

        player.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://Assets/Player/player_sprite.png");
    }

    public override void PhysicsUpdate(float delta){
        if (player.Velocity == Vector2.Zero) {
			fsm.ChangeState("idle");
		}
    }

    public override void HandleInput(InputEvent @event){
        if (@event.IsActionPressed("use_sword") || @event.IsActionPressed("use_spell")) {
            fsm.ChangeState("attacking");
        }
    }

    public override void Exit() {
        player.Modulate = Color.Color8(255, 255, 255);
    }
}