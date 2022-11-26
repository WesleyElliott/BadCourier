using Godot;

public partial class CarNotification : Label3D {

    private Tween tween;

    public void ShowNotification(string text, Color color) {
        Text = text;
        Modulate = color;

        tween = GetTree().CreateTween().SetParallel();
        
        var targetPosition = new Vector3(Position.x, Position.y + 4.5f, Position.z);
        tween.TweenProperty(this, "position", targetPosition, 0.7f);
        tween.TweenProperty(this, "modulate:a", 0f, 0.4f).SetDelay(0.3f);
        tween.TweenProperty(this, "outline_modulate:a", 0f, 0.4f).SetDelay(0.3f);
        tween.Chain().TweenCallback(Callable.From(() => NotificationFinished()));
    }

    private void NotificationFinished() {
        QueueFree();
        GetParent().RemoveChild(this);
    }
}