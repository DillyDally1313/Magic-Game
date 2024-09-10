using System.Collections.Generic;
using Godot;

public partial class Player : CharacterBody2D {
	float moveSpeed = 300;
	public int currentSpell;

	// all available spells
	List<string> availableSpells = new();
	public List<string> useableSpells = new();

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

		// switch spells
		if (Input.IsActionJustPressed("spell_slot1") && useableSpells.Count >= 1) { currentSpell = 0; }
		if (Input.IsActionJustPressed("spell_slot2") && useableSpells.Count >= 2) { currentSpell = 1; }
		if (Input.IsActionJustPressed("spell_slot3") && useableSpells.Count >= 3) { currentSpell = 2; }

		// use the spell
		if (Input.IsActionJustPressed("use_spell")) {
			if (useableSpells.Count > 0){
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
			AddSpell(area.Name.ToString().ToLower());
			area.QueueFree();
		}
	}

	// add spell to available spells
	public void AddSpell(string spellName) {
		// add to total available spells
		if (!availableSpells.Contains(spellName)) {
			availableSpells.Add(spellName);
		}

		// add to useable spell slots if there is room
		if (useableSpells.Count < 3) {
			useableSpells.Add(spellName);
		}
	}

	// instantiates spell
	public async void UseSpell() {
		float spawnDistance = 50;

		Vector2 enemyPos = GetNode<Node2D>("/root/Main/target").Position;

		switch (useableSpells[currentSpell]) {
			case "fireball": {
				Fireball fireball = GD.Load<PackedScene>("res://Prefabs/Spells/Fireball/fireball.tscn").Instantiate<Fireball>();
				fireball.Position = Position + new Vector2(spawnDistance, spawnDistance) * Position.DirectionTo(enemyPos);
				fireball.target = enemyPos;
				GetNode("/root/Main").AddChild(fireball);
				break;
			}
			case "airblast": {
				Vector2 start = Position + new Vector2(spawnDistance, spawnDistance) * Position.DirectionTo(enemyPos);

				for (int i = 0; i < 3; i++) {
					Airblast airblast = GD.Load<PackedScene>("res://Prefabs/Spells/Airblast/airblast" + i + ".tscn").Instantiate<Airblast>();
					airblast.Position = start;
					airblast.target = enemyPos;
					GetNode("/root/Main").AddChild(airblast);

					if (i == 0) {
						airblast.GetNode<CpuParticles2D>("ParticlesTop").Emitting = true;
						airblast.GetNode<CpuParticles2D>("ParticlesBottom").Emitting = true;
					}

					await ToSignal(GetTree().CreateTimer(.05), "timeout");
				}
				break;
			}
			case "lightning": {
				Vector2 start = Position + new Vector2(spawnDistance, spawnDistance) * Position.DirectionTo(enemyPos);

				Lightning lightning = GD.Load<PackedScene>("res://Prefabs/Spells/Lightning/lightning.tscn").Instantiate<Lightning>();
				GetNode("/root/Main").AddChild(lightning);

				lightning.Initialize(6);
				lightning.DrawLightning(start, enemyPos);

				break;
			}

			case "earthblock": {
				Earthblock earthblock = GD.Load<PackedScene>("res://Prefabs/Spells/Earthblock/earthblock.tscn").Instantiate<Earthblock>();
				earthblock.Position = Position + new Vector2(0, -spawnDistance);
				GetNode("/root/Main").AddChild(earthblock);

				break;
			}
		}
	}
}