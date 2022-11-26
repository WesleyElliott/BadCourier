using Godot;
using System;

public partial class Player : Node {

	[Export]
	public Node3D Rig { get; private set; }

	[Export]
	public Node3D Container { get; private set; }

	[Export]
	public Node3D Model { get; private set; }

    public override void _PhysicsProcess(double delta) {
		Rig.Position = Rig.Position.Lerp(Container.Position, (float) delta * 5);

		var newRotationY = Mathf.Lerp(Rig.Rotation.y, Model.Rotation.y, (float) delta * 0.8f);
		Rig.Rotation = new Vector3(Rig.Rotation.x, newRotationY, Rig.Rotation.z);
    }

}
