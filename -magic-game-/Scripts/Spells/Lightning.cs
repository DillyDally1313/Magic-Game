using System;
using System.Collections.Generic;
using Godot;

public partial class Lightning : RigidBody2D {
	float timeAlive = 0.25f;
	float distance;

	Sprite2D sprite;
	List<Texture2D> textures = new() {
		GD.Load<Texture2D>("res://Assets/Spells/Lightning/lightning0.png"),
		GD.Load<Texture2D>("res://Assets/Spells/Lightning/lightning1.png"),
		GD.Load<Texture2D>("res://Assets/Spells/Lightning/lightning2.png")
	};

	public float rotation;
	public Node2D target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Texture = textures[new Random().Next(textures.Count)];

		// rotate towards the target
		Rotation = rotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta) {
		// deletes itself after certain amount of time
		await ToSignal(GetTree().CreateTimer(timeAlive), "timeout");
		QueueFree();
	}
}