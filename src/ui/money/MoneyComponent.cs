using Godot;

public partial class MoneyComponent : Control {

    [Export]
    public Label MoneyLabel;

    public override void _EnterTree() {
        MoneyLabel.Text = "$0";
        this.EventBus().MoneyChanged += UpdateMoneyLabel;
    }

    private void UpdateMoneyLabel(int newMoney) {
        var text = string.Format("{0:N0}", newMoney);
        MoneyLabel.Text = text;
    }
}