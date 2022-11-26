using Godot;

public partial class WarehouseComponent : Control {
    
    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public TextureProgressBar ProgressBar { get; private set; }

    private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    private Tween tween;

    public override void _EnterTree() {
        RenderProgress(0);
        this.EventBus().WarehouseCapacity += RenderProgress;
    }

    public override void _ExitTree() {
        RenderProgress(0);
        this.EventBus().WarehouseCapacity -= RenderProgress;
    }

    private void RenderProgress(int newBoxCount) {
        var percent = newBoxCount / (float) LevelData.WarehouseCapacity * 100f;
        var color = GetProgressColor(percent);

        if (tween != null) {
            tween.Kill();
        }
        tween = GetTree().CreateTween();
        tween.TweenProperty(ProgressBar, "value", percent, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quint);

        ProgressBar.TintProgress = color;
    }

    private Color GetProgressColor(float percent) {
        if (percent <= 30) {
            return GameController.SafeColor;
        } else if (percent < 60) {
            return GameController.WarningColor;
        } else {
            return GameController.CriticalColor;
        }
    }
}