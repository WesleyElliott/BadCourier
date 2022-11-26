using Godot;

public partial class Pause : Control {

    [Export]
    public Panel Panel;

    [Export]
    public Label Heading;

    [Export]
    public Button ResumeButton;

    [Export]
    public Button RestartButton;

    [Export]
    public Button QuitButton;

    private Tween tween;

    public override void _EnterTree() {
        ResumeButton.Pressed += OnResumePressed;
        RestartButton.Pressed += OnRestartPressed;
        QuitButton.Pressed += OnQuitPressed;
    }

    public override void _ExitTree() {
        ResumeButton.Pressed -= OnResumePressed;
        RestartButton.Pressed -= OnRestartPressed;
        QuitButton.Pressed -= OnQuitPressed;
    }

    public void Render() {
        if (tween != null) {
            tween.Kill();
        }

        Panel.SelfModulate = new Color(1, 1, 1, 0);
        Heading.Modulate = new Color(1, 1, 1, 0);
        ResumeButton.Modulate = new Color(1, 1, 1, 0);
        RestartButton.Modulate = new Color(1, 1, 1, 0);
        QuitButton.Modulate = new Color(1, 1, 1, 0);

        tween = GetTree().CreateTween().SetParallel().SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(Panel, "self_modulate:a", 1.0f, 0.2f);
        tween.TweenProperty(Heading, "modulate:a", 1f, 0.4f).SetDelay(0.2f);
        tween.TweenProperty(ResumeButton, "modulate:a", 1f, 0.4f).SetDelay(0.4f);
        tween.TweenProperty(RestartButton, "modulate:a", 1f, 0.4f).SetDelay(0.4f);
        tween.TweenProperty(QuitButton, "modulate:a", 1f, 0.4f).SetDelay(0.4f);
    }

    private void OnResumePressed() {
        this.EventBus().EmitSignal(EventBus.SignalName.PauseChanged, false);
    }

    private void OnRestartPressed() {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnQuitPressed() {
        GetTree().Quit();
    }
}