using Godot;

public partial class SoundController : Node {
    
    [Export]
    public AudioStreamPlayer ButtonAudio;

    [Export]
    public AudioStreamPlayer PackageDeliveredAudio;

    public override void _EnterTree() {
        var buttons = GetTree().GetNodesInGroup("Button");
        foreach (Button button in buttons) {
            button.Pressed += OnButtonPressed;
        }
    }

    public override void _ExitTree() {
        var buttons = GetTree().GetNodesInGroup("Button");
        foreach (Button button in buttons) {
            button.Pressed -= OnButtonPressed;
        }
    }

    public void PlayPackageDelivered() {
        PackageDeliveredAudio.Play();
    }

    private void OnButtonPressed() {
        ButtonAudio.Play();
    }
}