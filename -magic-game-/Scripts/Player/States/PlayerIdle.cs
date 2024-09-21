using Godot;
using System.Diagnostics;

public partial class PlayerIdle : State {
	Player player;

    public override void Ready() {
		player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Idle State");

        player.Modulate = Color.Color8(255, 0, 0);
    }

    public override void HandleInput(InputEvent @event){
        if (@event.IsActionPressed("move_up") || @event.IsActionPressed("move_down") || @event.IsActionPressed("move_left") ||  @event.IsActionPressed("move_right")) {
			fsm.ChangeState("walking");
		}
        if (player.attacking) {
            fsm.ChangeState("attacking");
        }
    }

    public override void Exit() {
		player.Modulate = Color.Color8(255, 255, 255);
    }
}