using Godot;

public partial class PlayerIdleState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // play idle animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // check if moving
        if (player.HorizontalInput != 0) {
            stateMachine.ChangeState("PlayerMoveState");
            return;
        } // if

        // check if jumping
        if (player.JumpJustPressed && player.canJump) {
            stateMachine.ChangeState("PlayerJumpState");
            return;
        } // if

        // check if attacking
        if (player.AttackJustPressed) {
            stateMachine.ChangeState("PlayerAttackState");
            return;
        } // if

        // check if parrying
        if (player.ParryJustPressed) {
            stateMachine.ChangeState("PlayerParryState");
            return;
        } // if

        // apply friction
        player.ApplyFriction(delta);
    } // PhyisicsProcess
} // PlayerIdleState