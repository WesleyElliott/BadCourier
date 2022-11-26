using Godot;

public partial class EventBus : Node {

    [Signal]
    public delegate void WarehouseCapacityEventHandler(int packageAmount);

    [Signal]
    public delegate void PlayerCollectPackageEventHandler(int packageCount);
}