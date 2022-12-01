using Godot;

public partial class Van : VehicleBody3D {

    [Export]
    public VehicleWheel3D BackLeftWheel;

    [Export]
    public VehicleWheel3D BackRightWheel;

    [Export]
    public RayCast3D RayCast3D;

    [Export]
    public AudioStreamPlayer3D EngineAudio { get; private set; }

    [Export]
    public AudioStreamPlayer3D SkidAudio { get; private set; }

    [Export,]
    public bool CanDrive = false;

    float maxRPM = 700;
    float maxTorque = 160;

    bool resetFlipped = false;

    public override void _Ready() {
        EngineAudio.Play();
    }

    public override void _PhysicsProcess(double delta) {
        var currSpeed = LinearVelocity.Length();
        var turnFactor = 0.2f;
        if (currSpeed < 8) {
            turnFactor = 0.4f;
        }
        var steer = Input.GetAxis("right", "left") * turnFactor;
        Steering = Mathf.Lerp(Steering, steer, 5 * (float) delta);

        if (!CanDrive) {
            Brake = 1.6f;
            BackLeftWheel.EngineForce = 0;
            BackRightWheel.EngineForce = 0;
            SkidAudio.Stop();
            HandleEngineAudio((float) delta);
            return;
        }

        var acceleration = Input.GetAxis("back", "forward");
        var rpm = BackLeftWheel.GetRpm();
        BackLeftWheel.EngineForce = acceleration * maxTorque * (1 - Mathf.Abs(rpm)  / maxRPM);

        rpm = BackRightWheel.GetRpm();
        BackRightWheel.EngineForce = acceleration * maxTorque * (1 - Mathf.Abs(rpm)  / maxRPM);
        
        if (Input.IsActionPressed("brake")) {
            Brake = 1.5f;
        } else {
            Brake = 0f;
        }

        if (!RayCast3D.IsColliding()) {
            if (!resetFlipped) {
                resetFlipped = true;
                var timer = GetTree().CreateTimer(3);
                timer.Timeout += OnFlippedResetTimeout;
            }
        } else {
            if (resetFlipped) {
                resetFlipped = false;
            }
        }

        var forward = -GlobalTransform.basis.z;
        var skidding = (LinearVelocity.Cross(forward).Length() > 1.53f) || (Brake > 0.1f && LinearVelocity.Length() > 1.4f);
        if (skidding) {
            if (!SkidAudio.Playing)
                SkidAudio.Play();
        } else {
            SkidAudio.Stop();
        }

        HandleEngineAudio((float) delta);
    }

    private void OnFlippedResetTimeout() {
        if (resetFlipped) {
           resetFlipped = false;
           Rotation = new Vector3(Rotation.x, Rotation.y, 0);
        }
    }

    public void OnGameStart() {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(EngineAudio, "volume_db", 0, 0.8f);
    }

    private void HandleEngineAudio(float delta) {
        var pitchOffset = LinearVelocity.Length() / 30;
        EngineAudio.PitchScale = Mathf.Lerp(EngineAudio.PitchScale, 1 + pitchOffset, delta * 4f);
    }
}