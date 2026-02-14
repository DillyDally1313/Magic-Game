using Godot;
using Core.Types;

public partial class ParryHandler : Node {
    public bool hasParry = false;
    public Node2D enemy = null;
    public ActionDirection parryDirection = ActionDirection.Forward;
    public Area2D hitbox = null;
    private float timeRemaining = 0f;

    public bool Register(Node2D enemy, ActionDirection direction, float windowSeconds, Area2D hitbox = null) {
        if (hasParry) return false;
        this.hasParry = true;
        this.enemy = enemy;
        this.parryDirection = direction;
        this.hitbox = hitbox;
        this.timeRemaining = windowSeconds;
        return true;
    } // Register

    public void UnregisterByEnemy(Node2D enemy) {
        if (!hasParry) return;
        if (enemy == this.enemy) ClearParry();
    } // UnregisterByEnemy

    public void ClearParry() {
        hasParry = false;
        enemy = null;
        hitbox = null;
        timeRemaining = 0f;
    } // ClearParry

    public override void _PhysicsProcess(double delta) {
        if (!hasParry) return;

        timeRemaining -= (float)delta;
        if (timeRemaining <= 0f) ClearParry();
    } // _PhysicsProcess
} // ParryHandler