using Godot;

public partial class DropOff : Node3D {

    public delegate void HomeStateChangedEventHandler(bool atHome);
    public delegate void ExpirationTimerTickEventHandler(int newTime, int expirationTime);

    public event HomeStateChangedEventHandler HomeStateChanged;
    public event ExpirationTimerTickEventHandler ExpirationTimerTick;

    public MeshInstance3D Model { get; private set; }
    public Timer HomeStateTimer { get; private set; }
    public Timer ExpirationTimer { get; private set; }
    public CollisionShape3D CollisionShape { get; private set; }
    public WayPoint WayPoint { get; private set; }

    public bool SomeoneHome { get; private set; } = true;
    public bool HasOrder { get; set; } = false;
    public Color DropOffColor { 
        get {
            return dropOffColor;
        } 
        set {
            dropOffColor = value;
            WayPoint.Modulate = dropOffColor;
        }
    }

    private Color dropOffColor;
    private RandomNumberGenerator rng;
    private int timeLeft;

    public override void _EnterTree() {
        this.EventBus().GameTimerTick += OnTickTimeout;
    }

    public override void _ExitTree() {
        this.EventBus().GameTimerTick -= OnTickTimeout;
    }

    public override void _Ready() {
        Model = GetNode<MeshInstance3D>("Model");
        HomeStateTimer = GetNode<Timer>("HomeStateTimer");
        ExpirationTimer = GetNode<Timer>("ExpirationTimer");
        CollisionShape = GetNode<CollisionShape3D>("Collider/CollisionShape");
        WayPoint = GetNode<WayPoint>("WayPoint");

        rng = new RandomNumberGenerator();
        rng.Randomize();

        
    }

    public void Enable() {
        Show();
        WayPoint.Show();
        CollisionShape.Disabled = false;
        Reset();
    }

    public void Disable() {
        Hide();
        WayPoint.Hide();
        CollisionShape.Disabled = true;
        ExpirationTimer.Stop();
        HomeStateTimer.Stop();
    }

    private void Reset() {
        timeLeft = 20; //rng.RandiRange(45, 150);
        ExpirationTimer.WaitTime = timeLeft;
        ExpirationTimer.Start();
        OnHomeStateTimeout();
    }

    private void OnTickTimeout(int newTime) {
        timeLeft -= 1;
        ExpirationTimerTick?.Invoke(timeLeft, (int) ExpirationTimer.WaitTime);
    }

    private void OnHomeStateTimeout() {
        if (!Visible) {
            return;
        }
        HomeStateTimer.WaitTime = rng.RandiRange(5, 8);
        HomeStateTimer.Start();
        SomeoneHome = !SomeoneHome;
        HomeStateChanged?.Invoke(SomeoneHome);
    }

    private void OnExpirationTimeout() {
        this.EventBus().EmitSignal(EventBus.SignalName.HideNotification, this);
        this.EventBus().EmitSignal(EventBus.SignalName.PackageExpired, this);
    }
}