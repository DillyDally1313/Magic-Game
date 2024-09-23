using System.Diagnostics;
using Godot;

public partial class PlayerAttacking : State {
	Player player;

    public override void Ready() {
        player = Owner as Player;
    }

    public override void Enter() {
        player.Modulate = Color.Color8(0, 0, 255);

		Debug.Print("Attacking State");
    }

    public override void Update(float delta) {
        if (!player.attacking) {
			fsm.ChangeState("idle");
		}
    }

    public override void Exit() {
        player.Modulate = Color.Color8(255, 255, 255);
    }
}