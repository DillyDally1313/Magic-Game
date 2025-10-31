using Godot;

public partial class EnemyStunnedState : State {
    private EnemyController enemy;
    private float stunTimer = 0f;

    public override void Enter() {
        enemy = character as EnemyController;

        enemy.Modulate = enemy.stunColor;
        stunTimer = enemy.stunTime;
        enemy.isStunned = true;
    } // Enter

    public override void PhysicsProcess(float delta) {
        stunTimer -= delta;

        if (stunTimer <= 0) {
            stateMachine.ChangeState("EnemyIdleState");
            return;
        } // if
    } // PhysicsProcess

    public override void Exit() {
        enemy.Modulate = Color.Color8(255, 255, 255);
        enemy.isStunned = false;
    } // Exit
} // EnemyStunnedState