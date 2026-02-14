using Godot;
using Core.Types;

public partial class PlayerAttackHitbox : Area2D {
    private PlayerController player;

    public override void _Ready() {
        player = GetParent<PlayerController>();

        // connect area entered signals
        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;

        // start disabled
        Monitoring = false;
        Visible = false;
    } // _Ready

    private void OnAreaEntered(Area2D area) {
        HandleHit(area);
    } // OnAreaEntered

    private void OnBodyEntered(Node2D body) {
        HandleHit(body);
    } // OnBodyEntered

    private void HandleHit(Node2D node) {
        if (node is IDamageable target && node != player) {
            DamageInfo info = new DamageInfo(
                target.isStunned ? player.attackDamage * player.parryMultiplier : player.attackDamage,
                Vector2.Zero
            );

            target.ApplyDamage(info);

            SetDeferred("monitoring", false);
            return;
        } // if
    } // HandleHit
} // PlayerAttackHitbox