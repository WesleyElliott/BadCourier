using Godot;
using System.Linq;

public partial class DropOffNotifications : Control {

    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public PackedScene NotificationScene { get; private set; }

    private int notificationCount;

    public override void _Ready() {
        this.EventBus().PlayerPickupPackage += OnPlayerPickupPackage;
        this.EventBus().PlayerDropOffPackage += OnPlayerDropOffPackage;
    }
    
    private void OnPlayerPickupPackage(DropOff dropOff) {
        DropOffComponent notification = NotificationScene.Instantiate<DropOffComponent>();
        var yOffset = 112 * notificationCount;
        notification.LayoutMode = 0;
        notification.GameController = GameController;
        notification.DropOff = dropOff;
        AddChild(notification);
        notification.Position = new Vector2(-200, yOffset);
        notification.ShowNotification();
        notificationCount += 1;
    }
    
    private void OnPlayerDropOffPackage(DropOff dropOff) {
        var notification = GetChildren().Cast<DropOffComponent>().FirstOrDefault(child => child.DropOff.Name == dropOff.Name);
        if (notification != null) {
            notification.HideNotification(Callable.From(() => {
                notification.QueueFree();
                RemoveChild(notification);
                notificationCount -= 1;
                ReorderNotifications();
            }));
        }
    }

    private void ReorderNotifications() {
        var index = 0;
        foreach (DropOffComponent notification in GetChildren()) {
             var yOffset = 112 * index;
             if (notification.Position.y != yOffset) {
                notification.MoveNotification(new Vector2(0, yOffset));
             }
             index++;
        }
    }
}