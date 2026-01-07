using Godot;
using Core.Types;
using Core.Utilities;

public partial class PlayerController : CharacterBody2D, IDamageable {
    // health settings
    public float maxHealth = 100f;
    public float currentHealth;

    // movement settings
    public float maxSpeed = 600.0f;
    public float sprintMultipler = 1.5f;
    public float acceleration = 3000.0f;
    public float friction = 3000.0f;

    // jump settings
    public bool canJump = false;
    public float jumpSpeed = -700.0f;
    public float jumpGravity = 1500.0f;
    public float fallGravity = 2300.0f;
    public float maxFallSpeed = 1200.0f;

    // attack settings
    public float attackDamage = 10.0f;
    public float attackKnockback = 200.0f;

    // parry settings
    public float parryMultiplier = 2.0f;
    public ParryHandler parryHandler;

    // stunned settings
    public bool isStunned = false;

    // input properties
    public float HorizontalInput => Input.GetAxis("move_left", "move_right");
    public float VerticalInput => Input.GetAxis("move_up", "move_down");
    public bool IsSprinting => Input.IsActionPressed("sprint");
    public bool JumpJustPressed => Input.IsActionJustPressed("jump");
    public bool AttackJustPressed => Input.IsActionJustPressed("attack");
    public bool ParryJustPressed => Input.IsActionJustPressed("parry");

    // helper properties
    public int facingDirection = 1;

    bool IDamageable.isStunned {
        get => isStunned;
        set => isStunned = value;
    } // isStunned

    public override void _Ready() {
        parryHandler = GetNodeOrNull<ParryHandler>("ParryHandler");
    } // _Ready

    public override void _PhysicsProcess(double delta) {
        // check if on ground
        if (IsOnFloor()) {
            canJump = true;
        } // if

        // handle facing direction
        GD.Print(Scale.X + ", " + Scale.Y);
        if (HorizontalInput != 0) {
            int newDirection = Mathf.Sign(HorizontalInput);
            if (facingDirection != newDirection) {
                facingDirection = newDirection;
                Scale = new Vector2(facingDirection, 1);
            } // if
        } // if

        MoveAndSlide();
    } // _PhysicsProcess

    public void ApplyFriction(float delta) {
        Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, friction * delta), Velocity.Y);
    } // ApplyFriction

    public void ApplyDamage(DamageInfo info) {
        Utilities.CreateDamageNumbers(this, info.damage);
    } // TakeDamage
} // PlayerController