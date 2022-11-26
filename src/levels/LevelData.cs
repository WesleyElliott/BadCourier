using Godot;
using MonoCustomResourceRegistry;

[RegisteredType("LevelData")]
public partial class LevelData : Resource {

    [Export]
    public int GameTime;

    [Export]
    public int WarehouseCapacity;

    [Export]
    public int WarehouseStartingPackageCount = 4;

    [Export]
    public Vector2i PackageSpawnRange;

    [Export]
    public int PackageExpiryTime;

    [Export]
    public int PlayerPackageCapacity = 5;

    [Export]
    public int PackageDeliveredTimeAward;

    [Export]
    public int PackageDeliveredMoneyAward;

    [Export]
    public int PackageExpiredMoneyCost;

    [Export]
    public Vector2i DropOffHomeStateChangeRange;
}