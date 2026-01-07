using Godot;

public partial class PlayerFallState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // play fall animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // apply fall gravity
        float speedY = Mathf.Min(player.Velocity.Y + player.fallGravity * delta, player.maxFallSpeed);
        player.Velocity = new Vector2(player.Velocity.X, speedY);

        // handle movement in air
        if (player.HorizontalInput != 0) {
            float targetSpeed = player.HorizontalInput * player.maxSpeed;
            if (player.IsSprinting) {
                targetSpeed *= player.sprintMultipler;
            } // if

            // accelerate
            player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, targetSpeed, player.acceleration * delta), player.Velocity.Y);
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
} // PlayerFallState