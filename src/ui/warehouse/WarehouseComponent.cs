using Godot;

public partial class WarehouseComponent : Control {
    
    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public TextureProgressBar ProgressBar { get; private set; }

    [Export]
    public Color SafeColor { get; private set; }

    [Export]
    public Color WarningColor { get; private set; }

    [Export]
    public Color CriticalColor { get; private set; }

    public override void _Ready() {
        RenderProgress(0);
        this.EventBus().WarehouseCapacity += RenderProgress;
    }

    private void RenderProgress(int newBoxCount) {
        var percent = newBoxCount / 20 * 100f;
        var color = GetProgressColor(percent);

        ProgressBar.Value = percent;
        ProgressBar.TintProgress = color;
    }

    private Color GetProgressColor(float percent) {
        if (percent <= 30) {
            return SafeColor;
        } else if (percent < 60) {
            return WarningColor;
        } else {
            return CriticalColor;
        }
    }
}