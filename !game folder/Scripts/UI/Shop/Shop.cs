using Godot;

public partial class Shop : NinePatchRect{
	[Export] RichTextLabel displayName;
	[Export] TextureRect displayIcon;
	[Export] RichTextLabel displayDescription;

	[Export] VBoxContainer itemList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// set larger info to first item in stock
		if (itemList.GetChild(0) is Item item) {
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