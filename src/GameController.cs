using Godot;

struct GameState {

    public GameState() {
        GameTime = 15;
        Money = 0;
        Paused = false;
        GameOver = false;
    }

    public int GameTime; // Seconds
    public int Money;
    public bool Paused;
    public bool GameOver;
}

public partial class GameController : Node {

    [Export]
    public Color SafeColor { get; private set; }

    [Export]
    public Color WarningColor { get; private set; }

    [Export]
    public Color CriticalColor { get; private set; }

    [Export]
    public GameOver GameOver { get; private set; }

    private Timer gameTimer;
    private GameState gameState = new GameState();

    public override void _EnterTree() {
        this.EventBus().PackageDelivered += OnPackageDelivered;
        this.EventBus().PackageExpired += OnPackageExpired;
    }

    public override void _ExitTree() {
        this.EventBus().PackageDelivered -= OnPackageDelivered;
        this.EventBus().PackageExpired -= OnPackageExpired;
    }

    public override void _Ready() {
        gameTimer = GetNode<Timer>("GameTimer");
        OnTick();
    }

    public override void _Process(double delta) {
        if (gameState.GameTime < 0 && !gameState.GameOver) {
            HandleGameOver();
        }
    }

    public void OnTick() {
        gameTimer.Start();
        this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
        gameState.GameTime -= 1;
    }

    private void OnPackageDelivered(DropOff dropOff, bool anyoneHome) {
        gameState.Money += 20;
        this.EventBus().EmitSignal(EventBus.SignalName.MoneyChanged, gameState.Money);
        if (!anyoneHome) {
            gameState.GameTime += 10;
            gameState.Money += 10;
            this.EventBus().EmitSignal(EventBus.SignalName.MoneyChanged, gameState.Money);
            this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
        }
    }

    private void OnPackageExpired(DropOff dropOff) {
        gameState.GameTime -= 10;
        this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
    }

    private void HandleGameOver() {
        GD.Print("[GameController] GAME OVER");
        this.EventBus().EmitSignal(EventBus.SignalName.GameEnd);
        gameState.GameOver = true;
        GameOver.Visible = true;
        GameOver.Render(gameState.Money);

        gameTimer.Stop();
    }
}