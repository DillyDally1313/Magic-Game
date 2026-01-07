using Godot;
using Core.Types;
using System;

public partial class EnemyAttackState : State {
    private BaseEnemy enemy;
    private Area2D attackHitbox;

    private float windupTimer = 0f;
    private float parryTimer = 0f;
    private float attackTimer = 0f;

    private float parryWindow = 3f;

    // 0 = windup, 1 = parry window, 2 = attack
    private int phase = 0;

    // DEBUG STUFF
    [Export] Label attackDirectionLabel;
    [Export] Label parryWindowLabel;

    public override void Enter() {
        enemy = character as BaseEnemy;
        attackHitbox = enemy.GetNodeOrNull<Area2D>("AttackHitbox");

        // choose random attack direction
        RandomNumberGenerator rng = new RandomNumberGenerator();
        enemy.attackDirection = (ActionDirection)rng.RandiRange(0, 2);

        // show attack direction for parry
        attackDirectionLabel.Text = enemy.attackDirection.ToString();

        windupTimer = enemy.attackWindup;
        parryTimer = parryWindow;
        attackTimer = enemy.attackDuration;
        phase = 0;

        attackHitbox.Monitoring = false;
        attackHitbox.Visible = false;
    } // Enter

    public override void PhysicsProcess(float delta) {
        // windup phase
        switch (phase) {
            case 0:
                attackDirectionLabel.Visible = true;
                windupTimer -= delta;
                if (windupTimer <= 0) {
                    phase = 1;
                    parryTimer = parryWindow;

                    attackDirectionLabel.Visible = false;
                    parryWindowLabel.Visible = true;

                    PlayerController player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
                    ParryHandler parryHandler = player.parryHandler;
                    parryHandler.Register(enemy, enemy.attackDirection, parryTimer, attackHitbox);
                } // if
                break;
            case 1:
                parryTimer -= delta;
                parryWindowLabel.Text = Math.Round(parryTimer, 2).ToString();
                if (parryTimer <= 0) {
                    PlayerController player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
                    player.parryHandler.UnregisterByEnemy(enemy);

                    phase = 2;
                    parryWindowLabel.Visible = false;
                } // if
                break;
            case 2:
                attackTimer -= delta;
                PositionHitbox(attackHitbox, enemy.attackDirection);
                attackHitbox.Monitoring = true;
                attackHitbox.Visible = true;

                if (attackTimer <= 0) {
                    stateMachine.ChangeState("EnemyIdleState");
                    return;
                } // if
                break;
        } // switch
    } // PhysicsProcess

    private void PositionHitbox(Area2D hitbox, ActionDirection direction) {
        switch (direction) {
            case ActionDirection.Up:
                hitbox.RotationDegrees = 30;
                break;
            case ActionDirection.Down:
                hitbox.RotationDegrees = -30;
                break;
            case ActionDirection.Forward:
                hitbox.RotationDegrees = 0;
                break;
        } // switch
    } // PositionHitbox

    public override void Exit() {
        // remove registered parry
        PlayerController player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        player.parryHandler.UnregisterByEnemy(enemy);
                    
        attackHitbox.Monitoring = false;
        attackHitbox.Visible = false;

        attackDirectionLabel.Visible = false;
        parryWindowLabel.Visible = false;
    } // Exit
} // EnemyAttackState