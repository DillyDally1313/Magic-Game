using Godot;

public partial class PlayerMoveState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // play move animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // check if idle
        if (player.HorizontalInput == 0 && Mathf.Abs(player.Velocity.X) < 10f) {
            stateMachine.ChangeState("PlayerIdleState");
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

        // apply movement
        float targetSpeed = player.HorizontalInput * player.maxSpeed;
        if (player.IsSprinting) { targetSpeed *= player.sprintMultipler; }

        // accelerate toward target speed
        player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, targetSpeed, player.acceleration * delta), player.Velocity.Y);

        // update facing direction
        if (player.HorizontalInput != 0) {
            player.facingDirection = Mathf.Sign(player.HorizontalInput);
        } // if
    } // PhysicsProcess
} // PlayerMoveState