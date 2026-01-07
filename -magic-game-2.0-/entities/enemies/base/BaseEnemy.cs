using Godot;
using Core.Types;
using Core.Utilities;

public partial class BaseEnemy : CharacterBody2D, IDamageable, IParryable {
    // health settings
    public float maxHealth = 50f;
    public float currentHealth;
    public bool isDead = false;

    // movement settings
    public float gravity = 1500f;
    public float friction = 3000f;
    public float maxFallSpeed = 1200.0f;

    // attack settings
    public float attackDamage = 15f;
    public float attackKnockback = 150f;
    public float attackWindup = 2f;
    public float attackDuration = 0.5f;
    public ActionDirection attackDirection;

    // stunned settings
    public bool isStunned = false;
    public float stunTime = 5f;
    public Color stunColor = Color.Color8(0, 255, 0);
    bool IDamageable.isStunned {
        get => isStunned;
        set => isStunned = value;
    } // isStunned

    // helper properties
    public int facingDirection = 1;

    public override void _Ready() {
        currentHealth = maxHealth;
    } // _Ready

    public override void _PhysicsProcess(double delta) {
        // apply gravity
        if (!IsOnFloor()) {
            float currentGravity = (Velocity.Y > 0) ? gravity * 1.5f : gravity;
            Velocity = new Vector2(Velocity.X, Mathf.Min(Velocity.Y + currentGravity * (float)delta, maxFallSpeed));
        } // if

        if (isDead) return;

        MoveAndSlide();
    } // _PhysicsProcess

    public void ApplyFriction(float delta) {
        Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, friction * delta), Velocity.Y);
    } // ApplyFriction

    public void ApplyDamage(DamageInfo info) {
        if (isDead) return;

        // deal damage
        currentHealth -= info.damage;

        // show damage
        Utilities.CreateDamageNumbers(this, info.damage);

        // check if no health left
        if (currentHealth <= 0) {
            Die();
        } // if
    } // ApplyDamage

    public void OnParried() {
        GetNodeOrNull<StateMachine>("StateMachine").CallDeferred("ChangeState", "EnemyStunnedState");
        return;
    } // OnParried

    private void Die() {
        isDead = true;

        // disable collider
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);

        // remove from tree
        QueueFree();    
    } // Die
} // BaseEnemy