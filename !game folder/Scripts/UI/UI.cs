using Godot;

public partial class UI : Control {
	[Export] Shop shop;

    public override void _Input(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
			if (eventKey.Keycode == Key.Tab && eventKey.Pressed) {
				shop.Visible = !shop.Visible;
			}
		}
    }
}