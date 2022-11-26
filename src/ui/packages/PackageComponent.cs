using Godot;

public partial class PackageComponent : Control {

    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public Label PackagesLabel { get; private set; }

    private Tween tween;

    public override void _EnterTree() {
        PackagesLabel.Text = "0 / 20";
        this.EventBus().WarehouseCapacity += UpdatePackagesLabel;
    }

    public override void _ExitTree() {
        this.EventBus().WarehouseCapacity -= UpdatePackagesLabel;
    }

    private void UpdatePackagesLabel(int packages) {
        // if (tween != null) {
        //     tween.Kill();
        // }
        // tween = GetTree().CreateTween().SetParallel(true);
        // tween.TweenProperty(PackagesLabel, "scale", new Vector2(2.5f, 2.5f), 0.1f);
        // tween.TweenProperty(PackagesLabel, "position", new Vector2(-22, -80), 0.1f);
        // tween.Chain().TweenCallback(new Callable(this, nameof(OnTweenComplete)));
        PackagesLabel.Text = $"{packages} /20";
    }

    private void OnTweenComplete() {
        tween = GetTree().CreateTween().SetParallel(true);
        tween.TweenProperty(PackagesLabel, "scale", Vector2.One, 0.3f);
        tween.TweenProperty(PackagesLabel, "position", Vector2.Zero, 0.3f);
    }
}