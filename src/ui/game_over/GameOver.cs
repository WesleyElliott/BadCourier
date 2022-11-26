using Godot;

public partial class GameOver : Control {

    [Export]
    public Label Money;

    [Export]
    public Label AngryCustomers;

    [Export]
    public Label MissedDeliveries;

    [Export]
    public Label FinalScore;

    [Export]
    public TextureButton RestartButton;

    [Export]
    public TextureButton QuitButton;

    public override void _EnterTree() {
        RestartButton.Pressed += OnRestartPressed;
        QuitButton.Pressed += OnQuitPressed;
    }

    public override void _ExitTree() {
        RestartButton.Pressed -= OnRestartPressed;
        QuitButton.Pressed -= OnQuitPressed;
    }

    public void Render(int money, int angryCustomers, int missedDeliveries) {
        var finalScore = CalculateFinalScore(money, angryCustomers, missedDeliveries);

        Money.Text = $"+ {string.Format("{0:N0}", money)}";
        AngryCustomers.Text = $"x {angryCustomers}";
        MissedDeliveries.Text = $"- {string.Format("{0:N0}", missedDeliveries)}";
        FinalScore.Text = $"{string.Format("{0:N0}", finalScore)}";
    }

    private int CalculateFinalScore(int money, int angryCustomers, int missedDeliveries) {
        return (money * angryCustomers) - missedDeliveries;
    }

    private void OnRestartPressed() {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnQuitPressed() {
        GetTree().Quit();
    }
}