using Godot;

public partial class EventBus : Node {

    [Signal]
    public delegate void WarehouseCapacityEventHandler(int packageAmount);

    [Signal]
    public delegate void WarehouseCapacityExceededEventHandler();

    [Signal]
    public delegate void BoxCollectedEventHandler();

    [Signal]
    public delegate void GameTimerTickEventHandler(int newTime);

    [Signal]
    public delegate void ShowNotificationEventHandler(DropOff dropOff);

    [Signal]
    public delegate void HideNotificationEventHandler(DropOff dropOff);

    [Signal]
    public delegate void PackageExpiredEventHandler(DropOff dropOff);

    [Signal]
    public delegate void PackageDeliveredEventHandler(DropOff dropOff, bool anyoneHome);

    [Signal]
    public delegate void MoneyChangedEventHandler(int newMoney);

    [Signal]
    public delegate void GameEndEventHandler();

    [Signal]
    public delegate void PauseChangedEventHandler(bool newState);
    
    [Signal]
    public delegate void CarNotificationEventHandler(string text, Color color);

    [Signal]
    public delegate void GameStartEventHandler();

    [Signal]
    public delegate void ResetDropOffTimersEventHandler();
}