using Godot;
using System.Linq;

public partial class Warehouse : Node3D {

    [Export]
    public PackedScene BoxTemplate;

    [Export]
    public Timer OrderGenerator { get; private set; }

    [Export]
    public Node DropOffPoints { get; private set; }

    [Export]
    public WayPoint WayPoint { get; private set; }

    [Export]
    public Texture2D WarehouseIcon { get; private set; }

    public OrderManager OrderManager { get; private set; }

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    public override void _EnterTree() {
        this.EventBus().GameStart += OnGameStart;
        OrderGenerator.Timeout += OnOrderGeneratorTimeout;
    }

    public override void _ExitTree() {
        this.EventBus().GameStart -= OnGameStart;
        OrderGenerator.Timeout -= OnOrderGeneratorTimeout;
    }

    public override void _Ready() {
        rng.Randomize();
        OrderManager = GetNode<OrderManager>("OrderManager");

        OrderGenerator.WaitTime = rng.RandiRange(LevelData.PackageSpawnRange.x, LevelData.PackageSpawnRange.y);
        WayPoint.SetColor(new Color("#1e90ff"));
        WayPoint.SetIcon(WarehouseIcon);
    }

    public void OnPlayerEntered(Area3D body) {
        if (body.Owner is Player) {
            Player player = (Player) body.Owner;
            var boxesToCollectCount = Mathf.Min(player.AvailableSpace(), Mathf.Min(LevelData.PlayerPackageCapacity, TotalBoxes()));
            if (boxesToCollectCount == 0) {
                return;
            }
            GD.Print($"[Warehouse] Collecting {boxesToCollectCount} boxes");
            for (int i = 0; i < boxesToCollectCount; i++) {
                var box = GetBox();
                if (box == null) {
                    continue;
                }
                var randomOffset = new Vector3(rng.RandfRange(-1f, 1f), rng.RandfRange(0f, 0.7f), rng.RandfRange(-1f, 1f));
                var randomRotation = new Vector3(rng.RandfRange(-1.5f, 1.5f), rng.RandfRange(-1.5f, 1.5f), rng.RandfRange(-1.5f, 1.5f));
                box.Position += randomOffset;
                box.Rotation += randomRotation;
                box.Visible = true;
                box.Player = player;
            }
        }
    }

    public int TotalBoxes() {
        return OrderManager.GetChildCount();
    }

    private Box GetBox() {
        return OrderManager.GetChildren().Cast<Box>().Where(box => box.Visible == false).FirstOrDefault();
    }

    private void OnOrderGeneratorTimeout() {
        OrderGenerator.WaitTime = rng.RandiRange(LevelData.PackageSpawnRange.x, LevelData.PackageSpawnRange.y);
        OrderManager.GenerateOrder();
    }

    private void OnGameStart() {
        if (LevelData.GameStartGeneratesOrder) {
            OrderGenerator.Start();
        }
        // Generate LevelData.WarehouseStartingPackageCount orders to begin with
        for (int i = 0; i < LevelData.WarehouseStartingPackageCount; i++) {
            OrderManager.GenerateOrder();
        }
    }
}