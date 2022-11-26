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
    
    private Tween showHideTween;
    private Tween timerTween;
    private Tween moveTween;

    public override void _EnterTree() {
        DropOff.HomeStateChanged += UpdateAtHomeStatus;
        DropOff.ExpirationTimerTick += UpdateTimeProgress;
        Render(DropOff.DropOffColor);
        Position = new Vector2(30, Position.y);
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
        if (showHideTween != null) {
            showHideTween.Kill();
        }
        showHideTween = GetTree().CreateTween();
        showHideTween.TweenProperty(this, "position", new Vector2(-124, Position.y), 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
    }

    public void MoveNotification(Vector2 position) {
        if (moveTween != null) {
            moveTween.Kill();
        }
        moveTween = GetTree().CreateTween();
        moveTween.TweenProperty(this, "position", position, 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
    }

    public void HideNotification(Callable callback) {
        if (showHideTween != null) {
            showHideTween.Kill();
        }
        showHideTween = GetTree().CreateTween();
        showHideTween.TweenProperty(this, "position", new Vector2(30, Position.y), 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
        showHideTween.TweenCallback(callback);
    }

    private void UpdateTimeProgress(int timeRemaining, int expirationTime) {
        var percent = timeRemaining / (float) expirationTime * 100f;
        var color = GetProgressColor(percent);

        TimerProgress.TintProgress = color;
        if (timerTween != null) {
            timerTween.Kill();
        }
        timerTween = GetTree().CreateTween();
        timerTween.TweenProperty(TimerProgress, "value", percent, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quint);
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