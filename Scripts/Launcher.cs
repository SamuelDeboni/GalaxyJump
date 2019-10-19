using Godot;
using System;

public class Launcher : Area2D
{
    [Export]
    float force = 2000;

    [Export]
    float quant = 0;

    public override void _Ready()
    {
        if (quant > 0)
            GetNode<Sprite>("Sprite").Hide();
    }

    public void _OnColectables(PhysicsBody2D body)
    {
        if(body.IsInGroup("Player") && quant > 0)
            quant--;
        
        if (quant > 0)
            GetNode<Sprite>("Sprite").Hide();
        else
            GetNode<Sprite>("Sprite").Show();
    }

    public void _OnLauncherBodyEntered(PhysicsBody2D body)
    {
        if(body.IsInGroup("Player") && quant == 0)
        {
            GD.Print(body);
            Player player = body as Player;
            player.launched = true;
            player.vel = Mathf.Polar2Cartesian(1,GetGlobalRotation()) * force;
        }
    }
}
