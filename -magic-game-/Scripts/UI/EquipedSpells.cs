using System.Collections.Generic;
using Godot;

public partial class EquipedSpells : PanelContainer {
	List<TextureRect> spellSlotImages;
	List<Panel> spellSlots;

	Player player;
	List<string> equipedSpells;

	int currentSpell;

	Color activeColor = Color.Color8(60, 20, 120);
	Color inactiveColor = Color.Color8(37, 124, 126);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		spellSlots = new List<Panel>() {
			GetNode<Panel>("SpellSlots/Slot1"),
			GetNode<Panel>("SpellSlots/Slot2"),
			GetNode<Panel>("SpellSlots/Slot3")
		};

		spellSlotImages = new List<TextureRect>() {
			GetNode<TextureRect>("EquipedSpells/Margin1/Spell1"),
			GetNode<TextureRect>("EquipedSpells/Margin2/Spell2"),
			GetNode<TextureRect>("EquipedSpells/Margin3/Spell3")
		};

		player = GetNode<Player>("/root/Main/Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		equipedSpells = player.useableSpells;
		currentSpell = player.currentSpell;
		
		// update spell slot image to reflect current spells
		for (int i = 0; i < 3; i++) {
			if (equipedSpells.Count >= i + 1) {
				spellSlotImages[i].Texture = GD.Load<Texture2D>("res://Assets/Spells/" + equipedSpells[i].Capitalize() + "/" + equipedSpells[i] + "_icon.png");
			} else {
				spellSlotImages[i].Texture = null;
			}
			
			// outline which spell is currently being used
			if (i == currentSpell) {
				StyleBox stylebox = spellSlots[i].GetThemeStylebox("panel");
				stylebox.Set("bg_color", activeColor);
				spellSlots[i].AddThemeStyleboxOverride("panel", stylebox);
			} else {
				StyleBox stylebox = spellSlots[i].GetThemeStylebox("panel");
				stylebox.Set("bg_color", inactiveColor);
				spellSlots[i].AddThemeStyleboxOverride("panel", stylebox);
			}
		}
	}
}