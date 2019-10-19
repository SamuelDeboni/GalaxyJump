using Godot;
using System;

public class Star : Area2D
{
    public void _on_Star_body_entered(PhysicsBody2D body)
    {
        if(body.IsInGroup("Player"))
            this.QueueFree();
    }
}
