using Godot;
using System.Diagnostics;

public partial class PlayerWalking : State {
	Player player;

    public override void Ready() {
        player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Walking State");

        player.Modulate = Color.Color8(0, 255, 0);
    }

    public override void PhysicsUpdate(float delta){
        if (player.Velocity == Vector2.Zero) {
			fsm.ChangeState("idle");
		}
        if (player.attacking) {
            fsm.ChangeState("attacking");
        }
    }

    public override void Exit() {
        player.Modulate = Color.Color8(255, 255, 255);
    }
}