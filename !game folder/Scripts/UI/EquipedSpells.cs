using System.Collections.Generic;
using Godot;

public partial class EquipedSpells : PanelContainer {
	List<Panel> spellBacks;
	List<TextureRect> spellImgs;

	Panel bossSpellBack;
	TextureRect bossSpellImg;
	

	Player player;
	List<string> equipedSpells;

	Color activeColor = Color.Color8(60, 20, 120);
	Color inactiveColor = Color.Color8(37, 124, 126);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		spellBacks = new List<Panel>() {
			GetNode<Panel>("MainSpells/SpellSlots/Slots/Slot1"),
			GetNode<Panel>("MainSpells/SpellSlots/Slots/Slot2"),
			GetNode<Panel>("MainSpells/SpellSlots/Slots/Slot3")
		};

		spellImgs = new List<TextureRect>() {
			GetNode<TextureRect>("MainSpells/SpellSlots/Slots/Slot1/Spell"),
			GetNode<TextureRect>("MainSpells/SpellSlots/Slots/Slot2/Spell"),
			GetNode<TextureRect>("MainSpells/SpellSlots/Slots/Slot3/Spell")
		};

		bossSpellBack = GetNode<Panel>("BossSpell/SpellSlot/Slot");
		bossSpellImg = GetNode<TextureRect>("BossSpell/SpellSlot/Slot/Spell");

		player = GetNode<Player>("/root/Main/Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		equipedSpells = player.useableSpells;
		
		// update spell slot image to reflect current spells
		for (int i = 0; i < 3; i++) {
			if (equipedSpells.Count >= i + 1) {
				spellImgs[i].Texture = GD.Load<Texture2D>("res://Assets/Spells/" + equipedSpells[i].Capitalize() + "/" + equipedSpells[i] + "_icon.png");
			} else {
				spellImgs[i].Texture = null;
			}
		}
	}
}