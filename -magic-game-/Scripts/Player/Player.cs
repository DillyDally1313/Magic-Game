using System.Collections.Generic;
using Godot;

public partial class Player : CharacterBody2D {
	float moveSpeed = 150;
	public int currentSpell;
	public bool attacking = false;
	string facing = "down";

	// all available spells
	List<string> availableSpells = new();
	public List<string> useableSpells = new();

	[Export] CollisionShape2D feetCollider;

	// main physics loop, runs every frame
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (Input.IsActionJustPressed("move_up")) { facing = "up"; }
		if (Input.IsActionJustPressed("move_down")) { facing = "down"; }
		if (Input.IsActionJustPressed("move_left")) { facing = "left"; }
		if (Input.IsActionJustPressed("move_right")) { facing = "right"; }
		
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
				attacking = true;
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
		float spawnDistance = 40;

		Vector2 enemyPos = GetNode<Node2D>("/root/Main/target").Position;

		switch (useableSpells[currentSpell]) {
			case "fireball": {
				Fireball fireball = GD.Load<PackedScene>("res://Prefabs/Spells/Fireball/fireball.tscn").Instantiate<Fireball>();
				fireball.Position = feetCollider.GlobalPosition + new Vector2(spawnDistance, spawnDistance) * feetCollider.GlobalPosition.DirectionTo(enemyPos);
				fireball.target = enemyPos;
				GetNode("/root/Main").AddChild(fireball);
				break;
			}
			case "airblast": {
				Vector2 start = feetCollider.GlobalPosition + new Vector2(spawnDistance, spawnDistance) * feetCollider.GlobalPosition.DirectionTo(enemyPos);

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
				/*
				Vector2 start = Position + new Vector2(spawnDistance, spawnDistance) * Position.DirectionTo(enemyPos);

				Lightning lightning = GD.Load<PackedScene>("res://Prefabs/Spells/Lightning/lightning.tscn").Instantiate<Lightning>();
				GetNode("/root/Main").AddChild(lightning);

				lightning.Initialize(6);
				lightning.DrawLightning(start, enemyPos);
				*/

				Lightning lightning = GD.Load<PackedScene>("res://Prefabs/Spells/Lightning/lightning.tscn").Instantiate<Lightning>();
				lightning.start = feetCollider.GlobalPosition + new Vector2(spawnDistance, spawnDistance) * feetCollider.GlobalPosition.DirectionTo(enemyPos);
				lightning.target = enemyPos;
				GetNode("/root/Main").AddChild(lightning);

				break;
			}

			case "crystalwall": {
				Crystalwall crystalwall = GD.Load<PackedScene>("res://Prefabs/Spells/Crystalwall/crystalwall.tscn").Instantiate<Crystalwall>();
				
				// change where wall is spawned based on direction player is facing
				if (facing.Equals("down")) {
					crystalwall.Position = feetCollider.GlobalPosition + new Vector2(0, spawnDistance);
				} else {
					crystalwall.Position = feetCollider.GlobalPosition + new Vector2(0, -spawnDistance);
				}

				GetNode("/root/Main").AddChild(crystalwall);

				break;
			}
		}
		attacking = false;
	}

    public override void _Draw() {
		DrawCircle(feetCollider.Position, 40, Color.Color8(255, 0, 0), false, 2);
    }
}