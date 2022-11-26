using Godot;
using System.Linq;

public partial class OrderManager : Node {

    [Export]
    public Node DropOffPoints { get; private set; }

    private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready() {
        rng.Randomize();
        // Disable all of them for now
        foreach (DropOff dropOff in DropOffPoints.GetChildren()) {
            dropOff.Disable();
        }
    }

    public override void _UnhandledKeyInput(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed && eventKey.Keycode == Key.Space) {
                GenerateOrder();
            }
        }
    }

    public void GenerateOrder() {
        if (GetChildCount() == LevelData.WarehouseCapacity) {
            // Game has ended!
            GD.Print("[Order] Warehouse full! Game ending...");
            this.EventBus().EmitSignal(EventBus.SignalName.WarehouseCapacityExceeded);
            return;
        }

        GD.Print("[Order] Generating order...");
        var boxScene = GD.Load<PackedScene>("res://src/world/misc/box/Box.tscn");
        Box box = (Box) boxScene.Instantiate();
        box.Visible = false;

        AddChild(box);
        this.EventBus().EmitSignal(EventBus.SignalName.WarehouseCapacity, GetChildCount());
    }

    public DropOff GetRandomDropOff() {
         var eligibleDropOffPoints = DropOffPoints.GetChildren()
            .Cast<DropOff>()
            .Where(dropOff => !dropOff.HasOrder);

        if (eligibleDropOffPoints.Count() == 0) {
            GD.PrintErr("[Order] No more drop of points...");
            return null;
        }
        var dropOffPoint = eligibleDropOffPoints.ElementAt(rng.RandiRange(0, eligibleDropOffPoints.Count() - 1));
        GD.Print($"[Order] Drop off @ {dropOffPoint.Name}");
        return dropOffPoint;
    }
}