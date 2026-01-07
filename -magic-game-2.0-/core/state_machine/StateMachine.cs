using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node {
    [Export] public State initialState;
    [Export] public Node character;

    private Dictionary<string, State> states = new();
    private State currentState;

    public override void _Ready() {
        // Recursively find all states
        GetStates(this, states);

        // start with initial state
        ChangeState(initialState.Name);
    } // _Ready

    private void GetStates(Node parent, Dictionary<string, State> states) {
        foreach (Node child in parent.GetChildren()) {
            if (child is State state) {
                states[state.Name] = state;
                state.Initialize(this, character);
                state.SetProcess(false);
                state.SetPhysicsProcess(false);
            } else {
                GetStates(child, states);
            } // if
        } // foreach
    } // GetState

    public override void _Process(double delta) {
        currentState?.Process((float)delta);
    } // _Process

    public override void _PhysicsProcess(double delta) {
        currentState?.PhysicsProcess((float)delta);
    } // _PhysicsProcess

    public override void _Input(InputEvent @event) {
        currentState?.HandleInput(@event);
    } // _Input

    public void ChangeState(string stateName) {
        // exit current state
        currentState?.Exit();
        currentState?.SetProcess(false);
        currentState?.SetPhysicsProcess(false);

        // enter new state
        currentState = states[stateName];
        currentState.Enter();
        currentState.SetProcess(true);
        currentState.SetPhysicsProcess(true);
    } // ChangeState
} // StateMachine
