using Godot;

public partial class PlayerJumpState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // apply jump velocity
        if (player.canJump) {
            player.Velocity = new Vector2(player.Velocity.X, player.jumpSpeed);
            player.canJump = false;
        } // if
        // play jump animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // handle movement in air
        if (player.HorizontalInput != 0) {
            float targetSpeed = player.HorizontalInput * player.maxSpeed;
            if (player.IsSprinting) {
                targetSpeed *= player.sprintMultipler;
            } // if

            // accelerate
            player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, targetSpeed, player.acceleration * delta), player.Velocity.Y);

            // update facing direction
            if (player.HorizontalInput != 0) {
                player.facingDirection = Mathf.Sign(player.HorizontalInput);
            } // if
        } else {
            player.ApplyFriction(delta);
        } // if

        // check if attacking
        if (player.AttackJustPressed) {
            stateMachine.ChangeState("PlayerAttackState");
            return;
        } // if

        // check if landed
        if (player.IsOnFloor()) {
            if (player.HorizontalInput != 0) {
                stateMachine.ChangeState("PlayerMoveState");
                return;
            } else {
                stateMachine.ChangeState("PlayerIdleState");
                return;
            } // if
        } // if
    } // PhysicsProcess
} // PlayerJumpState