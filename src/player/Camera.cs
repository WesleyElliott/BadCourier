using Godot;

public partial class Camera : Camera3D {

    [Export]
    public Node3D FollowTarget { get; private set; }

    [Export]
    public float TargetDistance { get; private set; } = 6f;

    [Export]
    public float TargetHeight { get; private set; } = 18f/4f;

    private Vector3 lastLookAt;

    public override void _Ready() {
        lastLookAt = FollowTarget.GlobalTransform.origin;
    }

    public override void _PhysicsProcess(double delta) {
        var deltaV = GlobalTransform.origin - FollowTarget.GlobalTransform.origin;
        var targetPosition = GlobalTransform.origin;

        deltaV.y = 0f;
        
        if (deltaV.Length() > TargetDistance) {
            deltaV = deltaV.Normalized() * TargetDistance;
            deltaV.y = TargetHeight;
            targetPosition = FollowTarget.GlobalTransform.origin + deltaV;
        } else {
            targetPosition.y = FollowTarget.GlobalTransform.origin.y + TargetHeight;
        }
        
        var newOrigin = GlobalTransform.origin.Lerp(targetPosition, (float) delta * 20);
        var newTransform = new Transform3D(GlobalTransform.basis, newOrigin);
        GlobalTransform = newTransform;
        
        lastLookAt = lastLookAt.Lerp(FollowTarget.GlobalTransform.origin, (float) delta * 20);        
        LookAt(lastLookAt, Vector3.Up);
    }

}