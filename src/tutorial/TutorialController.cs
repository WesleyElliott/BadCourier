using Godot;
using System.Threading.Tasks;

public partial class TutorialController : Node {

    [Export]
    public TutorialDialog TutorialDialog { get; private set; }

    [Export]
    public Player Player { get; private set; }

    private TutorialState state = TutorialState.Start;
    private Key? lastPressedKey;

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
        Player.Van.CanDrive = true;

        await WaitUntilWASDPressed();
        TutorialDialog.HideDialog();
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
}