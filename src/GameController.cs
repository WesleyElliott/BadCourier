using Godot;

struct GameState {

    public GameState() {
        GameTime = 120;
    }

    public int GameTime; // Seconds
}

public partial class GameController : Node {

    private Timer gameTimer;
    private GameState gameState = new GameState();

    public override void _Ready() {
        gameTimer = GetNode<Timer>("GameTimer");
        OnTick();
    }

    public void OnTick() {
        gameTimer.Start();
        this.EventBus().EmitSignal(EventBus.SignalName.GameTimerTick, gameState.GameTime);
        gameState.GameTime -= 1;
    }
}