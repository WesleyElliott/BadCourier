using Godot;

public partial class Box : Node3D {

    public DropOff DropOff;

    public Player Player;

    public override void _Process(double delta) {
        if (Visible && Player != null) {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer() {
        // Move the object towards the player, accelerating as it gets closer
        var rawDist = GlobalPosition.DistanceTo(Player.Container.GlobalPosition);
        if (rawDist < 0.5f) {
            Hide();
        } else {
            var dist = Mathf.Clamp(rawDist, 0, 10);
            var speed = (1 - (dist / 10)) * 0.15f + 0.02f;
            GlobalPosition = GlobalPosition.MoveToward(Player.Container.GlobalPosition, speed);
        }
    }
}
