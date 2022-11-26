using Godot;

public partial class WayPoint : Control {

    public TextureRect Marker { get; private set; }

    private Camera3D camera;
    private static int MARGIN = 48;

    public override void _Ready() {
        Marker = GetNode<TextureRect>("Marker");
        camera = GetViewport().GetCamera3d();
    }

    public override void _Process(double delta) {
        var parentOrigin = GetParent<Node3D>().GlobalTransform.origin;
        var cameraTransform = camera.GlobalTransform;
        var cameraOrigin = cameraTransform.origin;

        var isBehind = cameraTransform.basis.z.Dot(parentOrigin - cameraOrigin) > 0;
        
        var distance = cameraOrigin.DistanceTo(parentOrigin);

        var unprojectPosition = camera.UnprojectPosition(parentOrigin);
        var viewportBaseSize = GetViewport().GetVisibleRect().Size;

        if (isBehind) {
            if (unprojectPosition.x < viewportBaseSize.x / 2) {
                unprojectPosition.x = viewportBaseSize.x - MARGIN;
            } else {
                unprojectPosition.x = MARGIN;
            }
        }

        if (isBehind || unprojectPosition.x < MARGIN || unprojectPosition.x > viewportBaseSize.x - MARGIN) {
            var look = cameraTransform.LookingAt(parentOrigin, Vector3.Up);
            var diff = AngleDiff(look.basis.GetEuler().x, cameraTransform.basis.GetEuler().x);
            unprojectPosition.y = viewportBaseSize.y * (0.5f + (diff / Mathf.DegToRad(camera.Fov)));
        }

        Position = new Vector2(
            Mathf.Clamp(unprojectPosition.x, MARGIN, viewportBaseSize.x - MARGIN),
            Mathf.Clamp(unprojectPosition.y, MARGIN, viewportBaseSize.y - MARGIN)
        );

        Rotation = 0;
    }

    private float AngleDiff(float from, float to) {
        var diff = (to - from) % Mathf.Tau;
        return (2f * diff % Mathf.Tau) - diff;
    }
}