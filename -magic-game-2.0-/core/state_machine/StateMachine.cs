using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node {
    [Export] public State initialState;
    [Export] public Node character;

    private Dictionary<string, State> states = new();
    private State currentState;

    public override void _Ready() {
        // get all child states
        foreach (Node node in GetChildren()) {
            if (node is State state) {
                states[state.Name] = state;
                state.Initialize(this, character);
                state.SetProcess(false);
                state.SetPhysicsProcess(false);
            } // if
        } // foreach

        // start with initial state
        ChangeState(initialState.Name);
    } // _Ready

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
