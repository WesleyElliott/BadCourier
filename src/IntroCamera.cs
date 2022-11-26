using Godot;

public partial class IntroCamera : Camera3D {


    private float screenWidth;
    private float screenHeight;
    private Vector3 initialPosition;
    private float deltaAmount = 0.01f;


    public override void _Ready() {
        screenWidth = GetViewport().GetVisibleRect().Size.x;
        screenHeight = GetViewport().GetVisibleRect().Size.y;
        initialPosition = Position;
    }

    public override void _Process(double delta) {
        var mousePos = GetViewport().GetMousePosition();
        var xPos = (-deltaAmount - deltaAmount) * ((mousePos.x) / (screenWidth)) + deltaAmount;
        var yPos = (-deltaAmount - deltaAmount) * ((mousePos.y) / (screenHeight)) + deltaAmount;
        Position = new Vector3(
            initialPosition.x + Mathf.Clamp(xPos, -deltaAmount, deltaAmount), 
            initialPosition.y + Mathf.Clamp(yPos, -deltaAmount, deltaAmount), 
            initialPosition.z
        );
    }
}