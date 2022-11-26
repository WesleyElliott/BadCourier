using Godot;

public partial class GameOver : Control {

    [Export]
    public Panel Panel;

    [Export]
    public Label Score;

    [Export]
    public Label Heading;

    [Export]
    public Button RestartButton;

    [Export]
    public Button QuitButton;

    private Tween tween;

    public override void _EnterTree() {
        RestartButton.Pressed += OnRestartPressed;
        QuitButton.Pressed += OnQuitPressed;
    }

    public override void _ExitTree() {
        RestartButton.Pressed -= OnRestartPressed;
        QuitButton.Pressed -= OnQuitPressed;
    }

    public void Render() {
        if (tween != null) {
            tween.Kill();
        }

        Panel.SelfModulate = new Color(1, 1, 1, 0);
        Heading.Modulate = new Color(1, 1, 1, 0);
        Score.Modulate = new Color(1, 1, 1, 0);
        RestartButton.Modulate = new Color(1, 1, 1, 0);
        QuitButton.Modulate = new Color(1, 1, 1, 0);

        tween = GetTree().CreateTween().SetParallel().SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(Panel, "self_modulate:a", 1.0f, 0.4f);
        tween.TweenProperty(Heading, "modulate:a", 1f, 0.6f).SetDelay(0.8f);
        tween.TweenProperty(Score, "modulate:a", 1f, 0.6f).SetDelay(1.3f);
        tween.TweenProperty(RestartButton, "modulate:a", 1f, 0.4f).SetDelay(1.8f);
        tween.TweenProperty(QuitButton, "modulate:a", 1f, 0.4f).SetDelay(1.8f);
    }

    private void OnRestartPressed() {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnQuitPressed() {
        GetTree().Quit();
    }
}