using Godot;

public partial class IdleState : State {
    PlayerController player;

    public override void Enter() {
        player = character as PlayerController;

        // play idle animation
    } // Enter

    public override void PhysicsProcess(float delta) {
        // check if moving
        if (player.InputDirection != 0) {
            stateMachine.ChangeState("MoveState");
            return;
        } // if

        // check if jumping
        if (player.JumpJustPressed && player.IsOnFloor()) {
            stateMachine.ChangeState("JumpState");
            return;
        } // if

        // apply friction
        player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, 0, player.friction * delta), player.Velocity.Y);
    } // PhyisicsProcess
} // IdleState