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
    public int PackageDeliveredMoneyBonus;

    [Export]
    public int PackageExpiredTimeCost;

    [Export]
    public Vector2i DropOffHomeStateChangeRange;

    [Export]
    public bool GameStartPlaysMusic = true;

    [Export]
    public bool GameStartGeneratesOrder = true;

    [Export(hint: PropertyHint.Range, hintString: "0,1,0.05")]
    public float ResetTimerChancePercentage = 0.3f;
}