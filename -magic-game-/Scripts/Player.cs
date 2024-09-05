using System.Collections.Generic;
using Godot;

public partial class Player : CharacterBody2D {
	float moveSpeed = 300;
	string currentSpell = null;

	// all available spells
	List<string> availableSpells = new();

	// main physics loop, runs every frame
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");	
		
		// move the player and slow down if they stop moving			
		if (direction != Vector2.Zero) {
			velocity = direction * moveSpeed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, moveSpeed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, moveSpeed);
		}

		// use the spell
		if (Input.IsActionJustPressed("use_spell")) {
			if (availableSpells.Count > 0){
				UseSpell();
			}
		}

		// reset position and velocity, then move player
		Velocity = velocity;
		MoveAndSlide();
	}

	// when player collides with spell, call PickUpSpell
	public void _OnArea2DAreaEntered(Area2D area) {
		if (area.IsInGroup("SpellPickups")) {
			PickUpSpell(area.Name);
			area.QueueFree();
		}
	}

	// add spell to available spells
	public void PickUpSpell(string spellName) {
		if (!availableSpells.Contains(spellName.ToLower())) {
			availableSpells.Add(spellName.ToLower());
			currentSpell = spellName.ToLower();
		}
	}

	// instantiates spell
	public async void UseSpell() {
		switch (currentSpell) {
			case "fireball": {
				Fireball fireball = GD.Load<PackedScene>("res://Prefabs/Spells/Fireball/fireball.tscn").Instantiate<Fireball>();
				fireball.Position = Position;
				GetNode("/root/Main").AddChild(fireball);
				break;
			}
			case "airblast": {
				for (int i = 0; i < 3; i++) {
					Airblast airblast = GD.Load<PackedScene>("res://Prefabs/Spells/Airblast/airblast" + i + ".tscn").Instantiate<Airblast>();
					airblast.Position = Position;
					GetNode("/root/Main").AddChild(airblast);

					await ToSignal(GetTree().CreateTimer(.05), "timeout");
				}
				break;
			}
		}
	}
}