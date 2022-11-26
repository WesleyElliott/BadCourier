using Godot;
using MonoCustomResourceRegistry;

[RegisteredType("OrderColorData")]
public partial class OrderColorData : Resource {

    [Export]
    public Color[] OrderColorOptions;

}