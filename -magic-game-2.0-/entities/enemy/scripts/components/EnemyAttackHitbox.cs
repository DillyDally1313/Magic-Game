using Godot;
using Core.Types;

public partial class EnemyAttackHitbox : Area2D {
    private EnemyController enemy;

    public override void _Ready() {
        enemy = GetParent<EnemyController>();

        // connect area entered signals
        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;

        // start disabled
        Monitoring = false;
        Visible = false;
    } // _Ready

    private void OnAreaEntered(Area2D area) => HandleHit(area);
    private void OnBodyEntered(Node2D body) => HandleHit(body);
    
    private void HandleHit(Node2D node) {
        if (node is IDamageable target && node != enemy) {
            DamageInfo info = new DamageInfo(enemy.attackDamage, Vector2.Zero);

            target.ApplyDamage(info);

            SetDeferred("monitoring", false);
            return;
        } // if
    } // HandleHit
} // PlayerAttackHitbox