using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class Player : Node3D {

	[Export]
	public Node3D Rig { get; private set; }

	[Export]
	public Node3D Container { get; private set; }

	[Export]
	public Node3D Model { get; private set; }

	[Export]
	public Van Van { get; private set; }

	[Export]
	public OrderColorData OrderColors;

	private Node boxes;

	private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    public override void _EnterTree() {
        this.EventBus().PackageExpired += OnPackageExpired;
		this.EventBus().GameEnd += OnGameEnd;
		this.EventBus().GameStart += OnGameStart;
    }

    public override void _ExitTree() {
        this.EventBus().PackageExpired -= OnPackageExpired;
		this.EventBus().GameEnd -= OnGameEnd;
		this.EventBus().GameStart -= OnGameStart;
    }

    public override void _Ready() {
        boxes = GetNode<Node>("Boxes");
    }

    public override void _PhysicsProcess(double delta) {
		Rig.Position = Rig.Position.Lerp(Container.Position - new Vector3(0, 1, 0), (float) delta * 2);

		var newRotationY = Mathf.LerpAngle(Rig.Rotation.y, Model.Rotation.y, (float) delta * 2f);
		Rig.Rotation = new Vector3(Rig.Rotation.x, newRotationY, Rig.Rotation.z);
    }

	public void OnBoxEntered(Node3D other) {
		if (other.Owner is Box) {
			Box box = (Box) other.Owner;
			if (boxes.GetChildren().Contains(box)) {
				return;
			}
			if (TotalBoxes() == LevelData.PlayerPackageCapacity) {
				GD.Print("Cannot carry anymore boxes");
				return;
			}
			OrderManager parent = (OrderManager) box.GetParent();
			var dropOffPoint = parent.GetRandomDropOff();
			dropOffPoint.HasOrder = true;
			parent.RemoveChild(box);
			box.DropOff = dropOffPoint;
			box.Visible = false;
			boxes.AddChild(box);
			var dropOffColor = OrderColors.OrderColorOptions[boxes.GetChildCount()];
			box.DropOff.DropOffColor = dropOffColor;
			box.DropOff.CallDeferred("Enable");

			this.EventBus().EmitSignal(EventBus.SignalName.BoxCollected);
			this.EventBus().EmitSignal(EventBus.SignalName.ShowNotification, box.DropOff);
			this.EventBus().EmitSignal(EventBus.SignalName.WarehouseCapacity, parent.GetChildCount());
		}
	}

	public void OnAreaEntered(Area3D other) {
		if (other.Owner is DropOff) {
			DropOff dropOff = (DropOff) other.Owner;
			DropOffPackage(dropOff);
		} 
	}

	public int TotalBoxes() {
		return boxes.GetChildCount();
	}

	public int AvailableSpace() {
		return LevelData.PlayerPackageCapacity - TotalBoxes();
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
		dropOff.HasOrder = false;
		dropOff.CallDeferred("Disable");

		if (dropOff.SomeoneHome) {
			GD.Print("[Delivery] Some was home, no bonus!");
		} else {
			GD.Print("[Delivery] Nobody home! BONUS $$$");
		}

		this.EventBus().EmitSignal(EventBus.SignalName.HideNotification, dropOff);
		this.EventBus().EmitSignal(EventBus.SignalName.PackageDelivered, dropOff, dropOff.SomeoneHome);
	}

	private void OnPackageExpired(DropOff dropOff) {
		var box = boxes.GetChildren().Cast<Box>().Where(box => box.DropOff == dropOff).FirstOrDefault();
		if (box == null) {
			GD.Print("[Expiration] No packages for this drop off... BUG?");
			return;
		}

		GD.Print($"[Expiration] Removing package: {box.Name}");
		boxes.RemoveChild(box);
		box.QueueFree();
		dropOff.HasOrder = false;
		dropOff.CallDeferred("Disable");
	}

	private void OnGameEnd() {
		Van.CanDrive = false;
	}

	private void OnGameStart() {
		Van.CanDrive = true;
		if (LevelData.GameStartGeneratesOrder) {
			Van.OnGameStart();
        }
	}
}
