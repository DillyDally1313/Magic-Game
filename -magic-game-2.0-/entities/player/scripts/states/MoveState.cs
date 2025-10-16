using Godot;

public partial class MoveState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // play move animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // check if idle
        if (player.InputDirection == 0 && Mathf.Abs(player.Velocity.X) < 10f) {
            stateMachine.ChangeState("IdleState");
            return;
        } // if

        // check if jumping
        if (player.JumpJustPressed && player.IsOnFloor()) {
            stateMachine.ChangeState("JumpState");
            return;
        } // if

        // apply movement
        float targetSpeed = player.InputDirection * player.maxSpeed;
        if (player.IsSprinting) { targetSpeed *= player.sprintMultipler; }

        // accelerate toward target speed
        player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, targetSpeed, player.acceleration * delta), player.Velocity.Y);

        // update facing direction
        if (player.InputDirection != 0) {
            player.facingDirection = Mathf.Sign(player.InputDirection);
        } // if
    } // PhysicsProcess
} // MoveState