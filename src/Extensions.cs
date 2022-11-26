using Godot;

public static class Extensions {

    public static EventBus EventBus(this Node node) {
        return node.GetNode<EventBus>("/root/EventBus");
    }
}