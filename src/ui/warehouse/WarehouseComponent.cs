using Godot;

public partial class WarehouseComponent : Control {
    
    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public TextureProgressBar ProgressBar { get; private set; }

    public override void _Ready() {
        RenderProgress(0);
        this.EventBus().WarehouseCapacity += RenderProgress;
    }

    private void RenderProgress(int newBoxCount) {
        var percent = newBoxCount / 20f * 100f;
        var color = GetProgressColor(percent);

        ProgressBar.Value = percent;
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