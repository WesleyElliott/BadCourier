using Godot;

public partial class PackageComponent : Control {

    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public Label PackagesLabel { get; private set; }

    private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    public override void _EnterTree() {
        PackagesLabel.Text = $"0 /{LevelData.WarehouseCapacity}";
        this.EventBus().WarehouseCapacity += UpdatePackagesLabel;
    }

    public override void _ExitTree() {
        this.EventBus().WarehouseCapacity -= UpdatePackagesLabel;
    }

    private void UpdatePackagesLabel(int packages) {
        PackagesLabel.Text = $"{packages} /{LevelData.WarehouseCapacity}";
    }
}