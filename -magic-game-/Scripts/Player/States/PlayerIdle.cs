using Godot;
using System.Diagnostics;

public partial class PlayerIdle : State {
	Player player;

    public override void Ready() {
		player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Idle State");

        player.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://Assets/Player/player_sprite.png");
    }

    public override void HandleInput(InputEvent @event){
        if (@event.IsActionPressed("move_up") || @event.IsActionPressed("move_down") || @event.IsActionPressed("move_left") ||  @event.IsActionPressed("move_right")) {
			fsm.ChangeState("walking");
		}

        if (@event.IsActionPressed("use_sword") || @event.IsActionPressed("use_spell")) {
            fsm.ChangeState("attacking");
        }
    }

    public override void Exit() {
		player.Modulate = Color.Color8(255, 255, 255);
    }
}