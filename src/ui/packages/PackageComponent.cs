using Godot;

public partial class PackageComponent : Control {

    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public Label PackagesLabel { get; private set; }

    private Tween tween;

    public override void _Ready() {
        PackagesLabel.Text = "0";
        this.EventBus().PlayerCollectPackage += UpdatePackagesLabel;
    }

    private void UpdatePackagesLabel(int packages) {
        if (tween != null) {
            tween.Kill();
        }
        tween = GetTree().CreateTween().SetParallel(true);
        tween.TweenProperty(PackagesLabel, "scale", new Vector2(2.5f, 2.5f), 0.1f);
        tween.TweenProperty(PackagesLabel, "position", new Vector2(-15, -40), 0.1f);
        tween.Chain().TweenCallback(new Callable(this, nameof(OnTweenComplete)));
        PackagesLabel.Text = $"{packages}";
    }

    private void OnTweenComplete() {
        tween = GetTree().CreateTween().SetParallel(true);
        tween.TweenProperty(PackagesLabel, "scale", Vector2.One, 0.3f);
        tween.TweenProperty(PackagesLabel, "position", Vector2.Zero, 0.3f);
    }
}