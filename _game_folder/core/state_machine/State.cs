using Godot;

public partial class State : Node {
    public StateMachine stateMachine;
    public Node character;

    public void Initialize(StateMachine stateMachine, Node character) {
        this.stateMachine = stateMachine;
        this.character = character;
    } // Initialize

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Process(float delta) { }
    public virtual void PhysicsProcess(float delta) { }
    public virtual void HandleInput(InputEvent @event) { }
} // State