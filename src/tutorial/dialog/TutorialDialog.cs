using Godot;

public partial class TutorialDialog : Control {

    [Export]
    public Label MessageLabel;

    [Export]
    public Label HelpText;

    public bool TweenFinished = false;

    private Tween tween;
    private bool isShown = false;

    public override void _UnhandledKeyInput(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed ) {
                if (eventKey.Keycode == Key.Key1) {
                    if (isShown) {
                        HideDialog();
                        isShown = false;
                    } else {
                        ShowDialog();
                        isShown = true;
                    }
                } else if (eventKey.Keycode == Key.Key2) {
                    SetMessage("This is an animation test when changing the text", true);
                }
                
            }
        }
    }

    public void SetMessage(string message, bool animate = false) {
        
        if (animate) {
            if (tween != null) {
                tween.Kill();
            }

            tween = GetTree().CreateTween();
            tween.SetPauseMode(Tween.TweenPauseMode.Process);
            tween.TweenProperty(MessageLabel, "modulate:a", 0, 0.4f)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);
            tween.TweenCallback(Callable.From(() => {
                MessageLabel.Text = message;

                var tween2 = GetTree().CreateTween();
                tween2.SetPauseMode(Tween.TweenPauseMode.Process);
                tween2.TweenProperty(MessageLabel, "modulate:a", 1, 0.4f)
                    .SetTrans(Tween.TransitionType.Quint)
                    .SetEase(Tween.EaseType.Out);
            }));
        } else {
            MessageLabel.Text = message;
        }
    }

    public void SetHelpText(string text) {
        HelpText.Text = text;
    }

    public void ShowDialog() {
        if (tween != null) {
            tween.Kill();
        }

        TweenFinished = false;
        tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(this, "position", new Vector2(1188, Position.y), 1.2f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);
        tween.TweenCallback(Callable.From(() => TweenFinished = true));
    }

    public void HideDialog() {
        if (tween != null) {
            tween.Kill();
        }

        TweenFinished = false;
        tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(this, "position", new Vector2(2000, Position.y), 1.2f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);
        tween.TweenCallback(Callable.From(() => TweenFinished = true));
    }
    
    public bool SkipPositionTween(bool show) {
        bool animationSkipped;
        tween.Kill();
        if (show) {
            animationSkipped = Position.x != 1188;
            Position = new Vector2(1188, Position.y);
        } else {
            animationSkipped = Position.x != 2000;
            Position = new Vector2(2000, Position.y);
        }

        return animationSkipped;
    }
}