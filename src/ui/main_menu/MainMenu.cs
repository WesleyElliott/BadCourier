using Godot;

public partial class MainMenu : Control {

    [Export]
    public Button NewGameButton { get; private set; }
    
    [Export]
    public Button TutorialButton { get; private set; }

    [Export]
    public Button OptionsButton { get; private set; }

    [Export]
    public Button QuitButton { get; private set; }

    [Export]
    public AudioStreamPlayer ButtonAudio { get; private set; }

    [Export]
    public ColorRect Panel { get; private set; }

    [Export]
    public Label Heading { get; private set; }

    public override void _EnterTree() {
        NewGameButton.Pressed += OnNewGamePressed;
        TutorialButton.Pressed += OnTutorialPressed;
        OptionsButton.Pressed += OnOptionsPressed;
        QuitButton.Pressed += OnQuitPressed;
    }

    public override void _ExitTree() {
        NewGameButton.Pressed -= OnNewGamePressed;
        TutorialButton.Pressed -= OnTutorialPressed;
        OptionsButton.Pressed -= OnOptionsPressed;
        QuitButton.Pressed -= OnQuitPressed;
    }

    public override void _Ready() {

        Heading.Modulate = new Color(1, 1, 1, 0);
        NewGameButton.Modulate = new Color(1, 1, 1, 0);
        TutorialButton.Modulate = new Color(1, 1, 1, 0);
        OptionsButton.Modulate = new Color(1, 1, 1, 0);
        QuitButton.Modulate = new Color(1, 1, 1, 0);

        var enterTween = GetTree().CreateTween().SetParallel();
        var time = 1.8f;

        enterTween.TweenProperty(Panel, "color:a", 0.5f, time)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);

        enterTween.TweenProperty(Heading, "modulate:a", 1, time)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out)
            .SetDelay(0.8f);
        enterTween.TweenProperty(NewGameButton, "modulate:a", 1, time)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out)
            .SetDelay(1.2f);
        enterTween.TweenProperty(TutorialButton, "modulate:a", 1, time)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out)
            .SetDelay(1.35f);
        enterTween.TweenProperty(OptionsButton, "modulate:a", 1, time)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out)
            .SetDelay(1.5f);
        enterTween.TweenProperty(QuitButton, "modulate:a", 1, time)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out)
            .SetDelay(1.65f);
    }

    private void OnNewGamePressed() {
        ButtonAudio.Play();
        GetTree().ChangeSceneToFile("res://src/MainWorld.tscn");
    }

    private void OnTutorialPressed() {
        ButtonAudio.Play();
        GetTree().ChangeSceneToFile("res://src/tutorial/TutorialWorld.tscn");
    }

    private void OnOptionsPressed() {
        ButtonAudio.Play();
    }

    private void OnQuitPressed() {
        ButtonAudio.Play();
        GetTree().Quit();
    }
}