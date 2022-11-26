using Godot;

public partial class Countdown : Control {

    [Export]
    public Label Label { get; private set; }

    [Export]
    public Timer Timer { get; private set; }

    [Export]
    public AudioStreamPlayer AudioPlayer { get; private set; }

    private int time = 3;

    public override void _EnterTree() {
        Timer.Timeout += OnTimeout;
    }

    public override void _ExitTree() {
        Timer.Timeout -= OnTimeout;
    }

    public override void _Ready() {
        AudioPlayer.Play();
    }

    private void OnTimeout() {
        time -= 1;

        if (time == -1) {
            Timer.Stop();
            AudioPlayer.Stop();
            Hide();
            return;
        }
        
        if (time == 0) {
            Label.Text = "GO!";
            GD.Print("GO!");
            this.EventBus().EmitSignal(EventBus.SignalName.GameStart);
            AudioPlayer.PitchScale += 0.3f;
            AudioPlayer.VolumeDb += 3;
            AudioPlayer.Play();
        } else {
            Label.Text = $"{time}";
            AudioPlayer.Play();
        }
    }
}