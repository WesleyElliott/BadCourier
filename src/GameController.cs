using Godot;

struct GameState {

    public GameState() {
        GameTime = 120;
    }

    public int GameTime; // Seconds
}

public partial class GameController : Node {

    [Export]
    public Color SafeColor { get; private set; }

    [Export]
    public Color WarningColor { get; private set; }

    [Export]
    public Color CriticalColor { get; private set; }

    private Timer gameTimer;
    private GameState gameState = new GameState();

    public override void _Ready() {
        gameTimer = GetNode<Timer>("GameTimer");
        OnTick();

        this.EventBus().PackageDelivered += OnPackageDelivered;
        this.EventBus().PackageExpired += OnPackageExpired;
    }

    public void OnTick() {
        gameTimer.Start();
        this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
        gameState.GameTime -= 1;
    }

    private void OnPackageDelivered(DropOff dropOff, bool anyoneHome) {
        if (!anyoneHome) {
            gameState.GameTime += 10;
            this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
        }
    }

    private void OnPackageExpired(DropOff dropOff) {
        gameState.GameTime -= 10;
        this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
    }
}