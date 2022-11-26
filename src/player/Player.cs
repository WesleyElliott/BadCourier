using Godot;
using System;

public partial class Player : Node {

	[Export]
	public Node3D Rig { get; private set; }

	[Export]
	public Node3D Container { get; private set; }

    public override void _PhysicsProcess(double delta) {
		Rig.Position = Rig.Position.Lerp(Container.Position, (float) delta * 5);
    }

}
