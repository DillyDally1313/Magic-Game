using Godot;
using System.Diagnostics;

public partial class PlayerWalking : State {
	Player player;

    public override void Ready() {
        player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Walking State");

        player.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://Assets/Player/player_sprite.png");
    }

    public override void PhysicsUpdate(float delta){
        // switch to idle when player is not moving
        if (player.Velocity == Vector2.Zero) {
			fsm.ChangeState("idle");
		}
    }

    public override void HandleInput(InputEvent @event){
        // spell casting input
        if (@event.IsActionPressed("use_spell1") && player.useableSpells.Count >= 1) { player.UseSpell(0); fsm.ChangeState("attacking");}
		if (@event.IsActionPressed("use_spell2") && player.useableSpells.Count >= 2) { player.UseSpell(1); fsm.ChangeState("attacking");}
		if (@event.IsActionPressed("use_spell3") && player.useableSpells.Count >= 3) { player.UseSpell(2); fsm.ChangeState("attacking");}
		if (@event.IsActionPressed("use_boss_spell") && player.bossSpell != null) { player.UseBossSpell(); fsm.ChangeState("attacking");}

        // sword attacking input    
        if (@event is InputEventMouseButton mouse) {
            if (mouse.ButtonIndex == MouseButton.Left && mouse.Pressed) {
                fsm.ChangeState("attacking");
            }
        }
    }

    public override void Exit() {
        player.Modulate = Color.Color8(255, 255, 255);
    }
}