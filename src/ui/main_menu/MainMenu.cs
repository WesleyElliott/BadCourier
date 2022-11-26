using Godot;

public partial class MainMenu : Control {

    [Export]
    public Button NewGameButton { get; private set; }

    [Export]
    public Button OptionsButton { get; private set; }

    [Export]
    public Button QuitButton { get; private set; }

    [Export]
    public AudioStreamPlayer ButtonAudio { get; private set; }

    public override void _EnterTree() {
        NewGameButton.Pressed += OnNewGamePressed;
        OptionsButton.Pressed += OnOptionsPressed;
        QuitButton.Pressed += OnQuitPressed;
    }

    public override void _ExitTree() {
        NewGameButton.Pressed -= OnNewGamePressed;
        OptionsButton.Pressed -= OnOptionsPressed;
        QuitButton.Pressed -= OnQuitPressed;
    }

    private void OnNewGamePressed() {
        ButtonAudio.Play();
        GetTree().ChangeSceneToFile("res://src/DemoScene.tscn");
    }

    private void OnOptionsPressed() {
        ButtonAudio.Play();
    }

    private void OnQuitPressed() {
        ButtonAudio.Play();
        GetTree().Quit();
    }
}