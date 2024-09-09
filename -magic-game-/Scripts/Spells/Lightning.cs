using Godot;
using System;

public partial class Lightning : Node2D {
	public Line2D[] lines;

	float segmentLength = 80;
	float segmentWidth = 1.5f;
	int spread = 30;
	float timeAlive = 0.1f;

	public Node2D target;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta) {
		// remove after certain amount of time
		await ToSignal(GetTree().CreateTimer(timeAlive), "timeout");
		QueueFree();
	}

	public void Initialize(int lineCount) {
		lines = new Line2D[lineCount];
		// create all the Line2Ds and change their starting values
		for (int i = 0; i < lineCount; i++) {
			lines[i] = new Line2D();
			lines[i].Width = segmentWidth;
			lines[i].DefaultColor = Color.Color8(255, 255, 255, (byte)new Random().Next(100, 255));
			AddChild(lines[i]);
		}
	}

	public void DrawLightning(Vector2 initialPos, Vector2 target) {
		// calculate distance to target
		float distance = initialPos.DistanceTo(target);
		int segments;
		// divide distance into segments
		if (distance > segmentLength) {
			segments = Convert.ToInt32(Math.Floor(distance / segmentLength) + 2);
		} else {
			segments = 4;
		}

		for (int i = 0; i < lines.Length; i++) {
			Random random = new Random();

			// give each Line2D the correct amount of points for the amount of segments
			for(int seg = 0; seg < segments; seg++) {
				lines[i].AddPoint(initialPos);
			}


			Vector2 lastPos;
			for (int j = 1; j < segments - 1; j++) {
				// move next point toward target
				Vector2 temp = initialPos.MoveToward(target, distance / segments * j);

				// randomize the next point
				lastPos = new Vector2(temp.X + random.Next(-spread, spread), temp.Y + random.Next(-spread, spread));
				lines[i].SetPointPosition(j, lastPos);
			}
			// set last point to the target
			lines[i].SetPointPosition(segments - 1, target);
		}
	}
}