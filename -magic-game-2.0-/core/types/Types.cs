using Godot;

namespace Core.Types {
    public enum ActionDirection {
        Up,
        Down,
        Forward
    } // ActionDirection

    public struct DamageInfo {
        public float damage;
        public Vector2 knockback;

        public DamageInfo(float damage, Vector2 knockback) {
            this.damage = damage;
            this.knockback = knockback;
        } // DamageInfo
    } // DamageInfo

    public interface IDamageable {
        bool isStunned { get; set; }
        void ApplyDamage(DamageInfo info);
    } // IDamageable

    public interface IParryable {
        void OnParried();
    } // IParryable
} // Core.Types