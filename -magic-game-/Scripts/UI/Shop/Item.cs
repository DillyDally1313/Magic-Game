using Godot;

public partial class Item : Button {

	[Export] public Texture2D icon;
	[Export] public string name;
	[Export] string price;
	[Export(PropertyHint.MultilineText)] public string description;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GetNode<TextureRect>("ItemMargin/Title/Item").Texture = icon;
		GetNode<RichTextLabel>("ItemMargin/Title/Name").Text = name;
		GetNode<RichTextLabel>("ItemMargin/Price/Price").Text = price;
	}

	// set larger info when pressed
	public void _OnPressed() {
		if (Owner is Shop s) {
			s.SetInfo(this);
		}
	}
}