using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node {

	[Export] public State initialState;

	Dictionary<string, State> states;
	State currentState;

 	public override void _Ready() {
		states = new Dictionary<string, State>();
		foreach (Node node in GetChildren()) {
			if (node is State s) {
				states[node.Name] = s;
				s.fsm =  this;
				s.Ready();
				s.Exit();
			}
		}
		currentState = initialState;
		currentState.Enter();
	}

	public override void _Process(double delta) {
		currentState.Update((float) delta);
	}

    public override void _PhysicsProcess(double delta) {
        currentState.PhysicsUpdate((float) delta);
    }

	public override void _UnhandledInput(InputEvent @event) {
		currentState.HandleInput(@event);
	}

	public void ChangeState(string key) {
		if (!states.ContainsKey(key) || currentState == states[key]) {
			return;
		}

		currentState.Exit();
		currentState = states[key];
		currentState.Enter();
	}
}