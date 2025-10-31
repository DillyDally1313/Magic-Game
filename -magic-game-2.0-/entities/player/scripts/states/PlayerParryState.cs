using Godot;
using Core.Types;

public partial class PlayerParryState : State {
    private PlayerController player;
    private ParryHandler parryHandler;
    private ActionDirection parryDirection;

    private Area2D parryHitbox;

    public override void Enter() {
        player = character as PlayerController;
        parryHitbox = player.GetNodeOrNull<Area2D>("ParryHitbox");

        // get parry direction
        parryHandler = player.parryHandler;
        GetParryDirection();
        PositionHitbox(parryHitbox, parryDirection);
        parryHitbox.Visible = true;
    } // Enter

    public override void PhysicsProcess(float delta) {
        if (parryHandler.hasParry) {
            Node2D enemy = parryHandler.enemy;

            if (parryHandler.parryDirection == parryDirection &&
                Mathf.Sign(enemy.GlobalPosition.X - player.GlobalPosition.X) == player.facingDirection) {
                OnParrySuccess(enemy);
                return;
            } // if
        } // if

        stateMachine.ChangeState("PlayerIdleState");
    } // PhysicsProcess

    private void OnParrySuccess(Node2D enemy) {
        parryHandler.ClearParry();

        if (enemy is IParryable) {
            enemy.CallDeferred("OnParried");
        } // if

        stateMachine.ChangeState("PlayerIdleState");
    } // OnParrySuccess

    private void GetParryDirection() {
        if (Input.IsActionPressed("move_up")) {
            parryDirection = ActionDirection.Up;
        } else if (Input.IsActionPressed("move_down")) {
            parryDirection = ActionDirection.Down;
        } else {
            parryDirection = ActionDirection.Forward;
        } // if
    } // GetParryDirection

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

    public override void Exit() {
        parryHitbox.Visible = false;
    } // Exit
} // PlayerParryState