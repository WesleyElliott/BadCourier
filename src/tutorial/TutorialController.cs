using Godot;
using System.Threading.Tasks;

public partial class TutorialController : Node {

    [Export]
    public TutorialDialog TutorialDialog { get; private set; }

    [Export]
    public Player Player { get; private set; }

    [Export]
    public Area3D CheckPoint1 { get; private set; }

    [Export]
    public Control PackagesUI { get; private set; }

    private TutorialState state = TutorialState.Start;
    private Key? lastPressedKey;

    public override void _EnterTree() {
        CheckPoint1.AreaEntered += OnCheckpoint1;
    }

    public override void _ExitTree() {
        CheckPoint1.AreaExited -= OnCheckpoint1;
    }

    public override void _UnhandledKeyInput(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed && eventKey.Keycode == Key.Key3) {
                if (state == TutorialState.Start) {
                    HandleIntro();
                }
            }

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
        
        TutorialDialog.SetMessage("As you know, we pride ourselves on always delivering packages at the most inconvenient of times", true);
        
        await WaitUntilSpacePressed();

        TutorialDialog.SetMessage("So lets get started! Using the Arrow Keys, you drive your truck around", true);
        TutorialDialog.SetHelpText("[WASD]");
        Player.Van.CanDrive = true;

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

        var tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(PackagesUI, "position:x", 0, 0.7f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);

        TutorialDialog.SetMessage("Check the top left of your screen. That will show you how many packages are at the warehouse", true);
        await WaitUntilSpacePressed();
        TutorialDialog.SetMessage("There are already 2 packages to pick up. Don't let the warehouse get full, else you're fired.", true);

        await WaitUntilSpacePressed();

        TutorialDialog.HideDialog();
        Player.Van.CanDrive = true;
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
}