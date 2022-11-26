using Godot;

public partial class CarNotifications : Node3D {

    [Export]
    public PackedScene NotificationTemplate { get; private set; }

    public override void _EnterTree() {
        this.EventBus().CarNotification += ShowNotification;
    }

    public override void _ExitTree() {
        this.EventBus().CarNotification -= ShowNotification;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }

    private void ShowNotification(string text, Color color) {
        if (GetChildCount() > 0) {
            ScheduleNotification(text, color);
        } else {
            var notification = NotificationTemplate.Instantiate<CarNotification>();
            AddChild(notification);
            notification.ShowNotification(text, color);
        }
    }

    private async void ScheduleNotification(string text, Color color) {
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        var notification = NotificationTemplate.Instantiate<CarNotification>();
        AddChild(notification);
        notification.ShowNotification(text, color);
    }
}