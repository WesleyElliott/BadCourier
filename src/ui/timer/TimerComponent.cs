using Godot;
using System;

public partial class TimerComponent : Control {

    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public Label TimerLabel { get; private set; }

    private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    private Tween tween;

    public override void _EnterTree() {
        UpdateTimer(LevelData.GameTime);
        this.EventBus().GameTimerTick += UpdateTimer;
    }

    public override void _ExitTree() {
        this.EventBus().GameTimerTick -= UpdateTimer;
    }

    private void UpdateTimer(int newTime) {
        var time = TimeSpan.FromSeconds(newTime);
        var text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        TimerLabel.Text = $"{text}";
    }
}