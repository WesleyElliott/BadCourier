using Godot;

public partial class Car : Node3D {

    [Export]
    public Node3D Model { get; private set; }

    [Export]
    public RigidBody3D Sphere { get; private set; }

    [Export]
    public Node3D Container { get; private set; }

    [Export]
    public RayCast3D RayCast { get; private set; }

    [Export]
    public AudioStreamPlayer3D EngineAudio { get; private set; }

    [Export]
    public AudioStreamPlayer3D SkidAudio { get; private set; }

    public bool CanDrive = false;
    private Node3D Body { get; set; }
    private Node3D RightWheel { get; set; }
    private Node3D LeftWheel { get; set; }

    private Vector3 modelRotation = new Vector3();
    private float maxSpeed = 200;
    private float speedTarget = 0;
    private float speed = 0;
    private float torqueVelocity = 0;
    private Vector3 normal = new Vector3(0, 1, 0);
    private Vector3 raycastNormal = new Vector3(0, 1, 0);
    private Vector3 bodyPosition = new Vector3(0, 0, 0);

    public override void _Ready() {
        Body = Model.GetNode<Node3D>("model/tmpParent/van2/body");
        RightWheel = Model.GetNode<Node3D>("model/tmpParent/van2/wheel_frontRight");
        LeftWheel = Model.GetNode<Node3D>("model/tmpParent/van2/wheel_frontLeft");
        EngineAudio.Play();
    }

    public override void _PhysicsProcess(double delta) {

        Container.Position = Sphere.Position - new Vector3(0, 1, 0);
        Model.Rotation = Model.Rotation.Lerp(modelRotation, (float) delta * 5);
        
        var turnRotation = Mathf.Clamp(14 * (float) delta * Sphere.LinearVelocity.Length(), 0f, 3.33f);
        if (Input.IsActionPressed("left") && CanDrive) {
            modelRotation.y += Mathf.DegToRad(turnRotation);
        } else if (Input.IsActionPressed("right") && CanDrive) {
            modelRotation.y -=  Mathf.DegToRad(turnRotation);
        }

        speedTarget = Mathf.Lerp(speedTarget, speed, 4 * (float) delta);

        var forward = -Model.GlobalTransform.basis.z;
        Sphere.ApplyForce(forward * speedTarget * 1.25f, Model.Position);

        if (Input.IsActionPressed("forward") && CanDrive) {
            speed = maxSpeed;
        } else if (Input.IsActionPressed("back") && CanDrive) {
            speed = -maxSpeed / 2;
        } else {
            speed = 0;
        }

        var rotationVelocity = (modelRotation - Model.Rotation).y;

        LeftWheel.Rotation = new Vector3(LeftWheel.Rotation.x, rotationVelocity, LeftWheel.Rotation.z);
        RightWheel.Rotation = new Vector3(RightWheel.Rotation.x, rotationVelocity, RightWheel.Rotation.z);

        bodyPosition = new Vector3(0, 0.325f, -0.074f);
        if (!RayCast.IsColliding()) {
            bodyPosition += new Vector3(0, 0.15f, 0);
        }

        Body.Position = Body.Position.Lerp(bodyPosition, (float) delta * 15f);

        var torque = (speed / 20) - Sphere.LinearVelocity.Length();
        torqueVelocity = Mathf.Clamp(Mathf.Lerp(torqueVelocity, torque, 2 * (float) delta), -5, 5);

        Body.Rotation = new Vector3(Mathf.DegToRad((torqueVelocity * 1.2f)), 0, 0);
        Body.RotateObjectLocal(Vector3.Forward, rotationVelocity / 5f);

        var skidding = (Sphere.LinearVelocity.Cross(forward).Length() > 5);
        if (skidding) {
            if (!SkidAudio.Playing)
                SkidAudio.Play();
        } else {
            SkidAudio.Stop();
        }

        HandleEngineAudio();
    }

    public void OnGameStart() {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(EngineAudio, "volume_db", -3, 0.8f);
    }

    private void HandleEngineAudio() {
        var pitchOffset = Sphere.LinearVelocity.Length() / 30;
        EngineAudio.PitchScale = 1 + pitchOffset;
    }

}