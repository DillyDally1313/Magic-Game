using Godot;

public partial class EnemyIdleState : State {
    private EnemyController enemy;
    private PlayerController player;

    public float attackRange = 150f;
    public float attackCooldown = 2.0f; 
    public float detectionRange = 300f;

    private float cooldownTimer = 0f;

    public override void Enter() {
        enemy = character as EnemyController;
        player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        cooldownTimer = 0f;
    } // Enter

    public override void PhysicsProcess(float delta) {
        // update cooldown timer
        if (cooldownTimer > 0) {
            cooldownTimer -= delta;
        } // if

        // check if player is in attack range and cooldown is ready
        float distanceToPlayer = enemy.GlobalPosition.DistanceTo(player.GlobalPosition);
        if (distanceToPlayer <= attackRange && cooldownTimer <= 0) {
            stateMachine.ChangeState("EnemyAttackState");
            return;
        } // if
    } // PhysicsProcess
} // IdleState