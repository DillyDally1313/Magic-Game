using System;
using System.Diagnostics;
using Godot;

public partial class PlayerAttacking : State {
	Player player;

    public override void Ready() {
        player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Attacking State");

        player.animationPlayer.Play("attack_right");
    }

    public override void Update(float delta) {
    }

    public void _OnAnimationFinished(String animation) {
        if (player.Velocity == Vector2.Zero) {
            fsm.ChangeState("idle");
        }
        else {
            fsm.ChangeState("walking");
        }
    }

    public override void Exit() {
        player.Modulate = Color.Color8(255, 255, 255);
    }
}