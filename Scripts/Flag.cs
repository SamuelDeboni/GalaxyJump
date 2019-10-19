using Godot;
using System;

public class Flag : Area2D
{
    public override void _Ready()
    {
        GetNode<Node2D>("Flag").Hide();
    }
    public void _on_Flag_body_entered(PhysicsBody2D body)
    {
        if(body.IsInGroup("Player"))
        {
            GD.Print(body);
            GetNode<Node2D>("Flag").Show();
            (body as Player).checkpoint = GlobalPosition;
        }
    }
}
