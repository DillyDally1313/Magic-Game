using Godot;
using System.Diagnostics;

public partial class PlayerIdle : State {
	Player player;

    public override void Ready() {
		player = Owner as Player;
    }

    public override void Enter() {
        Debug.Print("Idle State");

        player.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://Assets/Player/player_sprite.png");
    }

    public override void HandleInput(InputEvent @event){
        // movement input
        if (@event.IsActionPressed("move_up") || @event.IsActionPressed("move_down") || @event.IsActionPressed("move_left") ||  @event.IsActionPressed("move_right")) {
			fsm.ChangeState("walking");
		}
        
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