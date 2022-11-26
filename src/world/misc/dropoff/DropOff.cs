using Godot;

public partial class DropOff : Node3D {

    public MeshInstance3D Model { get; private set; }
    public Timer Timer { get; private set; }

    public bool CanDropOff { get; private set; } = true;

    private RandomNumberGenerator rng;

    public override void _Ready() {
        Model = GetNode<MeshInstance3D>("Model");
        Timer = GetNode<Timer>("Timer");

        rng = new RandomNumberGenerator();
        rng.Randomize();
        OnTimeout();
    }

    // TODO: This is temporary. At the moment, we just show green for can deliver, and red for cannot deliver
    // and the time between states is random between 2 and 8 seconds.
    public void OnTimeout() {
        Timer.WaitTime = rng.RandiRange(2, 8);
        Timer.Start();
        CanDropOff = !CanDropOff;

        StandardMaterial3D material = (StandardMaterial3D) Model.GetActiveMaterial(0);
        if (CanDropOff) {
            material.AlbedoColor = new Color(0, 1, 0);
        } else {
            material.AlbedoColor = new Color(1, 0, 0);
        }
    }
}