using Godot;

public partial class PlayerController : CharacterBody2D {
    // movement settings
    public float maxSpeed = 600.0f;
    public float sprintMultipler = 1.5f;
    public float acceleration = 3000.0f;
    public float friction = 2000.0f;

    // jump settings
    public float jumpSpeed = -700.0f;
    public float gravity = 1500.0f;
    public float fallGravity = 2300.0f;
    public float maxFallSpeed = 1200.0f;

    // input properties
    public float InputDirection => Input.GetAxis("move_left", "move_right");
    public bool IsSprinting => Input.IsActionPressed("sprint");
    public bool JumpJustPressed => Input.IsActionJustPressed("jump");

    // helper properties
    public int facingDirection = 1;
    public bool isFalling => Velocity.Y > 0 && !IsOnFloor();

    public override void _PhysicsProcess(double delta) {
        // apply gravity
        if (!IsOnFloor()) {
            float currentGravity = (Velocity.Y > 0) ? gravity * 1.5f : gravity;
            Velocity = new Vector2(Velocity.X, Mathf.Min(Velocity.Y + currentGravity * (float)delta, maxFallSpeed));
        } // if
        
        MoveAndSlide();
    } // _PhysicsProcess
} // PlayerController