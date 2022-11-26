using Godot;

public partial class DropOffComponent : Control {

    [Export]
    public TextureRect DropOffMarker { get; private set; }
    
    [Export]
    public TextureProgressBar TimerProgress { get; private set; }

    [Export]
    public Texture2D SomeoneHomeTexture { get; private set; }

    [Export]
    public Texture2D NobodyHomeTexture { get; private set; }

    public DropOff DropOff;
    public GameController GameController;
    
    private Tween tween;

    public override void _EnterTree() {
        DropOff.HomeStateChanged += UpdateAtHomeStatus;
        DropOff.ExpirationTimerTick += UpdateTimeProgress;
        Render(DropOff.DropOffColor);
        Position = new Vector2(-200, Position.y);
    }

    public override void _ExitTree() {
        DropOff.HomeStateChanged -= UpdateAtHomeStatus;
        DropOff.ExpirationTimerTick -= UpdateTimeProgress;
    }

    public void UpdateAtHomeStatus(bool atHome) {
        if (atHome) {
            DropOffMarker.Texture = SomeoneHomeTexture;
        } else {
            DropOffMarker.Texture = NobodyHomeTexture;
        }
    }

    public void Render(Color color) {
        DropOffMarker.Modulate = color;
    }

    public void ShowNotification() {
        if (tween != null) {
            tween.Kill();
        }
        tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", new Vector2(0, Position.y), 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
    }

    public void MoveNotification(Vector2 position) {
        if (tween != null) {
            tween.Kill();
        }
        tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position",position, 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
    }

    public void HideNotification(Callable callback) {
        if (tween != null) {
            tween.Kill();
        }
        tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", new Vector2(-200, Position.y), 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
        tween.TweenCallback(callback);
    }

    private void UpdateTimeProgress(int timeRemaining, int expirationTime) {
        var percent = timeRemaining / (float) expirationTime * 100f;
        var color = GetProgressColor(percent);

        TimerProgress.TintProgress = color;
        if (tween != null) {
            tween.Kill();
        }
        tween = GetTree().CreateTween();
        tween.TweenProperty(TimerProgress, "value", percent, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quint);
    }

    private Color GetProgressColor(float percent) {
        if (percent <= 30) {
            return GameController.CriticalColor;
        } else if (percent < 60) {
            return GameController.WarningColor;
        } else {
            return GameController.SafeColor;
        }
    }

}