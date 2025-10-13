using System;
using System.Collections.Generic;
using Godot;

public partial class PlayerController : CharacterBody2D {
	float moveSpeed = 150;
	string facing = "down";
	public string currentAttack;

	// all available spells
	private List<string> availableSpells = new();
	public List<string> useableSpells = new();
	public String bossSpell = null;

	[Export] CollisionShape2D feetCollider;
	[Export] public AnimationPlayer animationPlayer;

	// main physics loop, runs every frame
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		// set player direction
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
	public async void UseSpell(int spellIndex) {
		currentAttack = useableSpells[spellIndex];

		float spawnDistance = 20;

		Vector2 mousePos = GetGlobalMousePosition();

		switch (currentAttack) {
			case "fireball": {
				Fireball fireball = GD.Load<PackedScene>("res://entities/spells/fireball/scenes/fireball.tscn").Instantiate<Fireball>();
				fireball.Position = feetCollider.GlobalPosition + new Vector2(spawnDistance, spawnDistance) * feetCollider.GlobalPosition.DirectionTo(mousePos);
				fireball.Rotation = (mousePos - Position).Normalized().Angle();
				GetNode("/root/Main").AddChild(fireball);
				break;
			}
			case "air_blast": {
				Vector2 start = feetCollider.GlobalPosition + new Vector2(spawnDistance, spawnDistance) * feetCollider.GlobalPosition.DirectionTo(mousePos);

				for (int i = 0; i < 3; i++) {
					AirBlast airblast = GD.Load<PackedScene>("res://entities/spells/air_blast/scenes/air_blast" + i + ".tscn").Instantiate<AirBlast>();
					airblast.Position = start;
					airblast.Rotation = (mousePos - Position).Normalized().Angle();
					GetNode("/root/Main").AddChild(airblast);

					await ToSignal(GetTree().CreateTimer(.05), "timeout");
				}
				break;
			}
			case "lightning": {
				Lightning lightning = GD.Load<PackedScene>("res://entities/spells/lightning/scenes/lightning.tscn").Instantiate<Lightning>();
				lightning.start = feetCollider.GlobalPosition + new Vector2(spawnDistance, spawnDistance) * feetCollider.GlobalPosition.DirectionTo(mousePos);
				lightning.mouse = mousePos;
				GetNode("/root/Main").AddChild(lightning);

				break;
			}

			case "crystal_wall": {
				CrystalWall crystalwall = GD.Load<PackedScene>("res://entities/spells/crystal_wall/scenes/crystal_wall.tscn").Instantiate<CrystalWall>();
				
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
	}

	// instantiates boss spell
	public void UseBossSpell(){

	}

    public override void _Draw() {
		DrawCircle(feetCollider.Position, 20, Color.Color8(255, 0, 0), false, 2);
    }
}