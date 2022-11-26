using Godot;

public partial class DropOff : Node3D {

    public MeshInstance3D Model { get; private set; }
    public Timer Timer { get; private set; }
    public CollisionShape3D CollisionShape { get; private set; }
    public WayPoint WayPoint { get; private set; }

    public bool SomeoneHome { get; private set; } = true;
    public Color DropOffColor { 
        get {
            return dropOffColor;
        } 
        set {
            dropOffColor = value;
            WayPoint.Modulate = dropOffColor;
        }
    }

    private Color dropOffColor;
    private RandomNumberGenerator rng;

    public override void _Ready() {
        Model = GetNode<MeshInstance3D>("Model");
        Timer = GetNode<Timer>("Timer");
        CollisionShape = GetNode<CollisionShape3D>("Collider/CollisionShape");
        WayPoint = GetNode<WayPoint>("WayPoint");

        rng = new RandomNumberGenerator();
        rng.Randomize();
        OnTimeout();
    }

    // TODO: This is temporary. At the moment, we just show green for can deliver, and red for cannot deliver
    // and the time between states is random between 2 and 8 seconds.
    public void OnTimeout() {
        if (!Visible) {
            return;
        }
        Timer.WaitTime = rng.RandiRange(5, 8);
        Timer.Start();
        SomeoneHome = !SomeoneHome;

        StandardMaterial3D material = (StandardMaterial3D) Model.GetActiveMaterial(0);
        if (SomeoneHome) {
            material.AlbedoColor = new Color(0, 1, 0);
        } else {
            material.AlbedoColor = new Color(1, 0, 0);
        }
    }

    public void Enable() {
        Show();
        WayPoint.Show();
        CollisionShape.Disabled = false;
    }

    public void Disable() {
        Hide();
        WayPoint.Hide();
        CollisionShape.Disabled = true;
    }
}