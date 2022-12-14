using Godot;
using System.Threading.Tasks;

public partial class TutorialController : Node {

    [Export]
    public GameController GameController { get; private set; }

    [Export]
    public TutorialDialog TutorialDialog { get; private set; }

    [Export]
    public Player Player { get; private set; }

    [Export]
    public Warehouse Warehouse { get; private set; }

    [Export]
    public Area3D CheckPoint1 { get; private set; }

    [Export]
    public Control PackagesUI { get; private set; }

    [Export]
    public Control TimerUI { get; private set; }

    [Export]
    public Control MoneyUI { get; private set; }

    [Export]
    public ColorRect FadePanel { get; private set; }

    private LevelData LevelData {
        get {
             return this.Level().LevelData;
        }
    }

    private TutorialState state = TutorialState.Start;
    private Key? lastPressedKey;
    private int packageExpiryTime = 0;

    public override void _EnterTree() {
        CheckPoint1.AreaEntered += OnCheckpoint1;
        this.EventBus().BoxCollected += OnBoxCollected;
        this.EventBus().GameTimerTick += OnGameTick;
        this.EventBus().PackageDelivered += OnPackageDelivered;
    }

    public override void _ExitTree() {
        this.EventBus().BoxCollected -= OnBoxCollected;
        this.EventBus().GameTimerTick -= OnGameTick;
        this.EventBus().PackageDelivered -= OnPackageDelivered;
    }

    public override void _Ready() {
        var startTimer = GetTree().CreateTimer(1);
        startTimer.Timeout += () => {
            HandleIntro();
        };
    }

    public override void _UnhandledKeyInput(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed) {
                lastPressedKey = eventKey.Keycode;
            }
        }
    }

    private async void HandleIntro() {
        state = TutorialState.Controls;
        TutorialDialog.SetMessage("Hey, you must be the new recruit! Welcome to Bad Courier Inc - I'm here to show you around.");
        TutorialDialog.ShowDialog();

        await WaitUntilSpacePressed();
        if (TutorialDialog.SkipPositionTween(true)) {
            await WaitUntilSpacePressed();
        }
        
        TutorialDialog.SetMessage("As you know, we pride ourselves on always delivering packages at the most inconvenient of times.", true);
        await WaitUntilSpacePressed();

        var tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(TimerUI, "position:x", 0, 0.7f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);
        TutorialDialog.SetMessage("Bare in mind, your shift has a fixed time. Check the top left for how much time you have left!", true);
        
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("So lets get started! Using the Arrow/WASD Keys, drive your truck around.", true);
        TutorialDialog.SetHelpText("[WASD]");
        Player.Van.CanDrive = true;
        this.EventBus().EmitSignal(EventBus.SignalName.GameStart);

        await WaitUntilWASDPressed();
        TutorialDialog.HideDialog();
    }

    private async void HandleWarehouse() {
        Player.Van.CanDrive = false;
        state = TutorialState.Packages;

        TutorialDialog.SetMessage("Thats the warehouse up ahead. Driving through the loading zone will collect any packages ready for drop off.");
        TutorialDialog.SetHelpText("[Space]");
        TutorialDialog.ShowDialog();

        await WaitUntilSpacePressed();
        if (TutorialDialog.SkipPositionTween(true)) {
            await WaitUntilSpacePressed();
        }

        Warehouse.OrderManager.GenerateOrder();
        Warehouse.OrderManager.GenerateOrder();
        var tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(PackagesUI, "position:x", 0, 0.7f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);

        TutorialDialog.SetMessage("Check the top left of your screen. That will show you how many packages are at the warehouse.", true);
        await WaitUntilSpacePressed();
        TutorialDialog.SetMessage("There are already 2 packages to pick up. Don't let the warehouse get full, else you're fired.", true);

        await WaitUntilSpacePressed();

        TutorialDialog.HideDialog();
        Player.Van.CanDrive = true;
        packageExpiryTime = LevelData.PackageExpiryTime;
    }

    private async void HandleDropOffLocations() {
        Player.Van.CanDrive = false;
        state = TutorialState.DropOffLocations;

        TutorialDialog.SetMessage("Once you have the packages, you should see some markers to show you where to drop them off.");
        TutorialDialog.SetHelpText("[Space]");
        TutorialDialog.ShowDialog();

        await WaitUntilSpacePressed();
        if (TutorialDialog.SkipPositionTween(true)) {
            await WaitUntilSpacePressed();
        }

        TutorialDialog.SetMessage("On the right, you'll see how much time you have to get the package delivered. Make sure its delivered on time, or you'll lose time!", true);
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("If the icon has an [x] on it, the person isn't at home. This is the ideal time to make the delivery.", true);
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("You'll get bonus money and a TIME BOOST if you deliver to them when they're not at home.", true);
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("If they're at home, you'll still get some money - but none of the bonuses!", true);
        await WaitUntilSpacePressed();
        TutorialDialog.HideDialog();
        Player.Van.CanDrive = true;
    }

    private async void HandleDelivery(bool anyoneHome) {
        Player.Van.CanDrive = false;
        state = TutorialState.Money;

        var moneyEarned = LevelData.PackageDeliveredMoneyAward;
        if (!anyoneHome) {
            moneyEarned += LevelData.PackageDeliveredMoneyBonus;
        }

        TutorialDialog.SetMessage($"You're a natural! A sweet ${moneyEarned} in your pocket. Well, our pocket...");
        TutorialDialog.SetHelpText("[Space]");
        TutorialDialog.ShowDialog();

        var tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(MoneyUI, "position:x", 0, 0.7f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);

        this.EventBus().EmitSignal(EventBus.SignalName.ExpireDeliveries);

        await WaitUntilSpacePressed();
        if (TutorialDialog.SkipPositionTween(true)) {
            await WaitUntilSpacePressed();
        }

        TutorialDialog.SetMessage("You can see how much money you've earned at the top left. Your final score at the end of your shift will be this!", true);
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("You'll also notice the other delivery expired, so you lost some time on that order. Make sure to get those deliveries done.", true);
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("Well. It looks like you know what you're doing here. Time to send you off on some real deliveries", true);
        await WaitUntilSpacePressed();

        TutorialDialog.HideDialog();

        var exitTween = GetTree().CreateTween();
        exitTween.SetPauseMode(Tween.TweenPauseMode.Process);
        exitTween.TweenProperty(FadePanel, "color:a", 1f, 0.7f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);
        exitTween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToFile("res://src/ui/main_menu/MainMenu.tscn")));
    }

    private async Task WaitUntilSpacePressed() {
        while (lastPressedKey != Key.Space) {
            await Task.Delay(100);
        }
        lastPressedKey = null;
    }

     private async Task WaitUntilWASDPressed() {
        var playerPos = Player.Container.Position;
        while (playerPos.DistanceTo(Player.Container.Position) < 0.3) {
            await Task.Delay(100);
        }
        lastPressedKey = null;
    }

    private void OnCheckpoint1(Area3D body) {
        HandleWarehouse();
    }

    private void OnBoxCollected() {
        if (state == TutorialState.Packages)
            HandleDropOffLocations();
    }

    private void OnGameTick(int newTime) {
        if (state == TutorialState.DropOffLocations) {
            packageExpiryTime -= 1;
            if (packageExpiryTime <= 2) {
                packageExpiryTime = LevelData.PackageExpiryTime;
                this.EventBus().EmitSignal(EventBus.SignalName.ResetDropOffTimers);
            }
        }

        if (newTime <= 2) {
            GameController.ResetGameTime();
        }
    }

    private void OnPackageDelivered(DropOff dropOff, bool anyoneHome) {
        HandleDelivery(anyoneHome);
    }
}