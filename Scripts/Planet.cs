using Godot;
using System;

public class Planet : StaticBody2D
{
    [Export]
    public float G = 1000;
    [Export]
    public bool nonCircular = false;

    public float mass = 100;
    public override void _Ready()
    {
        mass = G * (GlobalScale.x * 512);
    }

    // Only used on black holes
    public void _on_EventHotizon_body_entered(PhysicsBody2D body)
    {
        if(body.IsInGroup("Player"))
        {
            body.Position = (body as Player).checkpoint;
            (body as Player).vel = Vector2.Zero;
        }
    }
}
