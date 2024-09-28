using System;
using System.Diagnostics;
using Godot;

public partial class PlayerAttacking : State {
	Player player;
    string currentAttack;

    public override void Ready() {
        player = Owner as Player;
    }

    public override void Enter() {
        currentAttack = player.currentAttack;
        Debug.Print("Attacking State");

        // player animation based on current player attack
        switch (currentAttack) {
            case "fireball": {
                player.animationPlayer.Play("magma_slash_right");
                break;
            }
            case "airblast": {
                player.animationPlayer.Play("slash_right");
                break;
            }
            case "crystalwall": {
                player.animationPlayer.Play("slash_right");
                break;
            }
            case "lightning": {
                player.animationPlayer.Play("slash_right");
                break;
            }
            default: {
                player.animationPlayer.Play("slash_right");
                break;
            }
        }
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
        player.currentAttack = null;
    }
}