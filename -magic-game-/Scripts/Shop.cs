using System.Collections.Generic;
using Godot;

public partial class Shop : NinePatchRect{
	int shroomBalance;
	RichTextLabel balance;

	RichTextLabel displayName;
	TextureRect displayIcon;
	RichTextLabel displayDescription;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		displayName = GetNode<RichTextLabel>("ItemInfo/InfoMargin/Details/Item/Name");
		displayIcon = GetNode<TextureRect>("ItemInfo/InfoMargin/Details/Item/Icon");
		displayDescription = GetNode<RichTextLabel>("ItemInfo/InfoMargin/Details/Info/Description");

		// set larger info to first item in stock
		if (GetNode("Stock/ItemMargin/ItemList").GetChild(0) is Item item) {
			SetInfo(item);
		}
	}

	// set larger info to item info when item is selected
	public void SetInfo(Item item) {
		displayName.Text = item.name;
		displayIcon.Texture = item.icon;
		displayDescription.Text = item.description;
	}
}