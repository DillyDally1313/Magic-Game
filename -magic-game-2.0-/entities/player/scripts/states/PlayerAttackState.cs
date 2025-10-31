using Godot;
using Core.Types;

public partial class PlayerAttackState : State {
    private PlayerController player;
    private Area2D attackHitbox;

    private float attackTimer = 0f;

    // if player had successful parry
    private bool isCounter = false;

    public override void Enter() {
        player = character as PlayerController;
        attackHitbox = player.GetNodeOrNull<Area2D>("AttackHitbox");

        // position and activate hitbox
        PositionHitbox(attackHitbox, ActionDirection.Forward);
        attackHitbox.Visible = true;
        attackHitbox.Monitoring = true;
    } // Enter

    public override void PhysicsProcess(float delta) {
        attackTimer -= delta;

        player.ApplyFriction(delta);
        ApplyActionMovement(delta);

        if (attackTimer <= 0) {
            ReturnToPreviousState();
        } // if
    } // PhysicsProcess

    private void ApplyActionMovement(float delta) {
        // add small dash forward
        float movementX = 100f * player.facingDirection * delta;
        player.Velocity = new Vector2(movementX, player.Velocity.Y);
    } // ApplyActionMovement

    private void PositionHitbox(Area2D hitbox, ActionDirection direction) {
        switch (direction) {
            case ActionDirection.Up:
                hitbox.RotationDegrees = -30;
                break;
            case ActionDirection.Down:
                hitbox.RotationDegrees = 30;
                break;
            case ActionDirection.Forward:
                hitbox.RotationDegrees = 0;
                break;
        } // switch
    } // PositionHitbox

    private void ReturnToPreviousState() {
        if (!player.IsOnFloor()) {
            stateMachine.ChangeState("PlayerJumpState");
        } else if (player.HorizontalInput != 0) {
            stateMachine.ChangeState("PlayerMoveState");
        } else {
            stateMachine.ChangeState("PlayerIdleState");
        } // if
    } // ReturnToPreviousState

    public override void Exit() {
        attackHitbox.Monitoring = false;
        attackHitbox.Visible = false;
    } // Exit
} // PlayerAttackState