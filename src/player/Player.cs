using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class Player : Node {

	[Export]
	public Node3D Rig { get; private set; }

	[Export]
	public Node3D Container { get; private set; }

	[Export]
	public Node3D Model { get; private set; }

	private Node boxes;
	private List<Color> dropOffColors = new List<Color>();

    public override void _Ready() {
        boxes = GetNode<Node>("Boxes");
		dropOffColors.Add(new Color(1, 0, 0));
		dropOffColors.Add(new Color(0, 1, 0));
		dropOffColors.Add(new Color(0, 0, 1));
		dropOffColors.Add(new Color(1, 1, 0));
		dropOffColors.Add(new Color(0, 1, 1));
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
			if (boxes.GetChildCount() == 5) {
				GD.Print("Cannot carry anymore boxes");
				return;
			}
			var parent = box.GetParent();
			parent.RemoveChild(box);
			box.Visible = false;
			boxes.AddChild(box);
			var dropOffColor = dropOffColors[boxes.GetChildCount()];
			box.DropOff.DropOffColor = dropOffColor;
		}
	}

	public void OnAreaEntered(Area3D other) {
		if (other.Owner is DropOff) {
			DropOff dropOff = (DropOff) other.Owner;
			DropOffPackage(dropOff);
		} 
	}

	private void DropOffPackage(DropOff dropOff) {
		var box = boxes.GetChildren().Cast<Box>().Where(box => box.DropOff == dropOff).FirstOrDefault();
		if (box == null) {
			GD.Print("[Delivery] No packages for this drop off");
			return;
		}

		GD.Print($"[Delivery] Dropping off package: {box.Name}");
		boxes.RemoveChild(box);
		box.QueueFree();
		dropOff.CallDeferred("Disable");

		if (dropOff.SomeoneHome) {
			GD.Print("[Delivery] Some was home, no bonus!");
		} else {
			GD.Print("[Delivery] Nobody home! BONUS $$$");
		}	
	}

}
