using Godot;
using System.Linq;

public partial class Player : Node {

	[Export]
	public Node3D Rig { get; private set; }

	[Export]
	public Node3D Container { get; private set; }

	[Export]
	public Node3D Model { get; private set; }

	private Node boxes;

    public override void _Ready() {
        boxes = GetNode<Node>("Boxes");
    }

    public override void _PhysicsProcess(double delta) {
		Rig.Position = Rig.Position.Lerp(Container.Position, (float) delta * 5);

		var newRotationY = Mathf.Lerp(Rig.Rotation.y, Model.Rotation.y, (float) delta * 0.8f);
		Rig.Rotation = new Vector3(Rig.Rotation.x, newRotationY, Rig.Rotation.z);
    }

	public void OnBoxEntered(Node3D other) {
		if (other.Owner is Box) {
			Box box = (Box) other.Owner;
			if (boxes.GetChildren().Contains(box)) {
				return;
			}
			var parent = box.GetParent();
			parent.RemoveChild(box);
			box.Visible = false;
			boxes.AddChild(box);	
		}
	}

	public void OnAreaEntered(Area3D other) {
		if (other.Owner is DropOff) {
			DropOff dropOff = (DropOff) other.Owner;
			if (dropOff.CanDropOff) {
				DropOffPackage(dropOff);
			} else {
				GD.Print("NO ONE IS HOME!");
			}
		} 
	}

	private void DropOffPackage(DropOff dropOff) {
		var box = boxes.GetChildren().Cast<Box>().Where(box => box.DropOff == dropOff).FirstOrDefault();
		if (box == null) {
			GD.Print("No box found for this drop off zone!");
			return;
		}

		GD.Print($"Dropping off box: {box.Name}");
		boxes.RemoveChild(box);
		box.QueueFree();
		dropOff.CallDeferred("Disable");
	}

}
