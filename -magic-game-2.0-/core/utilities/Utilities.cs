using Godot;

namespace Core.Utilities {
    public static class Utilities {
        public static void CreateDamageNumbers(Node2D parent, float damage) {
            // create a simple damage number display
            Label damageNumber = new Label();
            damageNumber.Text = damage.ToString();
            damageNumber.Position = new Vector2(0, -50);
            
            // add to scene and tween for fade out/float up effect
            parent.AddChild(damageNumber);
            
            Tween tween = parent.CreateTween();
            tween.TweenProperty(damageNumber, "position", new Vector2(0, -80), 0.8f);
            tween.TweenProperty(damageNumber, "modulate", new Color(1, 1, 1, 0), 0.5f);
            tween.TweenCallback(Callable.From(() => damageNumber.QueueFree()));
        } // CreateDamageNumbers
    } // DamageUtils
} // Core.Utils