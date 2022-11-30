using Godot;
using System.Threading.Tasks;

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

    private async void ShowNotification(Godot.Collections.Array<string> text, Color color) {
        var notification = NotificationTemplate.Instantiate<CarNotification>();
        AddChild(notification);
        if (GetChildCount() > 1)
            await Task.Delay(300 * GetChildCount());
        notification.ShowNotification(text[0], color);
    }
}