using Godot;

public partial class JumpState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // apply jump velocity
        player.Velocity = new Vector2(player.Velocity.X, player.jumpSpeed);

        // play jump animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // handle movement in air
        if (player.InputDirection != 0) {
            float targetSpeed = player.InputDirection * player.maxSpeed;
            if (player.IsSprinting) {
                targetSpeed *= player.sprintMultipler;
            } // if

            // accelerate
            player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, targetSpeed, player.acceleration * delta), player.Velocity.Y);

            // update facing direction
            if (player.InputDirection != 0) {
                player.facingDirection = Mathf.Sign(player.InputDirection);
            } // if
        } else {
            player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, 0, player.friction * delta), player.Velocity.Y);
        } // if

        // check if landed
        if (player.IsOnFloor()) {
            if (player.InputDirection != 0) {
                stateMachine.ChangeState("MoveState");
                return;
            } else {
                stateMachine.ChangeState("IdleState");
                return;
            } // if
        } // if
    } // PhysicsProcess
} // JumpState