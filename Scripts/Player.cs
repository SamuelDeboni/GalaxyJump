using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 up;
    Vector2 right;
    float G = 1000;
    public Vector2 vel;
    StaticBody2D planet;
    public bool launched;
    RayCast2D normalDetector;

    public Vector2 checkpoint;

    public override void _Ready()
    {
        planet = GetNode("../Planets/initialPlanet") as StaticBody2D;
        normalDetector =GetNode<RayCast2D>("NormalDetector");
        checkpoint = Position;
    }


    public override void _Process(float delta)
    {
        planet = FindClosestPlanet() as StaticBody2D;

        // Define up
        normalDetector.LookAt(planet.GetGlobalPosition());
        if(normalDetector.IsColliding() && (planet as Planet).nonCircular)
            up = normalDetector.GetCollisionNormal();
        else
            up = -Position.DirectionTo(planet.GetGlobalPosition());
        right = -up.Tangent();


        if(launched && normalDetector.IsColliding())
            launched = false;

        LookAtPlanet(delta * 4);

        float pDistance = planet.GetGlobalPosition().DistanceTo(Position);
        float zoom = Mathf.Pow(pDistance * 0.0015f,1.2f)  + 1;
        zoom = Mathf.Clamp(zoom,1,3);
        GetNode<Camera2D>("Camera2D").SetZoom(new Vector2(zoom,zoom));

        // Gravity
        if (!OnFloor())
            SetLocalVel(new Vector2(GetLocalVel().x, GetLocalVel().y + G * delta));
        else
            SetLocalVel(new Vector2(GetLocalVel().x, 10));

        // Jump   
        if(Input.IsActionJustPressed("jump") && OnFloor())
            vel += up * 700;

        float movVel = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        float runspeed =  Input.GetActionStrength("run") * 250;
        float targetVel = movVel * (300 + runspeed);

        if(!launched)
            SetLocalVel(new Vector2(Mathf.Lerp(GetLocalVel().x, targetVel, delta*10), GetLocalVel().y));
        else if (OnFloor())
            launched = false;


        // Animation
        AnimatedSprite sprite =GetNode<AnimatedSprite>("Sprite");
        if (!OnFloor())
            sprite.SetAnimation("jump");
        else if (movVel != 0 && runspeed > 0)
        {
            sprite.SetAnimation("walk");
            sprite.SpeedScale = 2;
        }
        else if (movVel != 0)
        {
            sprite.SetAnimation("walk");
            sprite.SpeedScale = 2;
        }
        else 
            sprite.SetAnimation("default");
        
        if (movVel < 0)
            sprite.FlipH = true;
        else if (movVel > 0)
            sprite.FlipH = false;

        MoveAndSlide(vel);
    }
    
    Node2D FindClosestPlanet()
    {   
        Node2D tmp = null;
        foreach (Node2D p in GetNode<Node2D>("../Planets").GetChildren())
        {
            if(p is Planet)
            {               
                float p1 = (p as Planet).mass / p.GlobalPosition.DistanceTo(Position);
                float p2 = tmp != null ? (tmp as Planet).mass / tmp.GlobalPosition.DistanceTo(Position) : -1.0f;
                    
                if(p1 > p2)
                    tmp = p;
            }     
        }
        G = (tmp as Planet).mass / (tmp.GlobalPosition.DistanceTo(Position));
        G = Mathf.Clamp(G,100,10000);
        return tmp;
    }

    void LookAtPlanet(float speed)
    {   
        if (OnFloor())
            speed = speed * 4;
        Vector2 globaRotation = Mathf.Polar2Cartesian(1,GetGlobalRotation());
        Vector2 lerpRotation = new Vector2(Mathf.Lerp(globaRotation.x, right.x, speed),
                                           Mathf.Lerp(globaRotation.y, right.y, speed));
        SetRotation(Mathf.Cartesian2Polar(lerpRotation.x,lerpRotation.y).y);
    }

    Vector2 GetLocalVel() => vel.Rotated(up.AngleTo(Vector2.Up));
    
    void SetLocalVel(Vector2 nVel) => vel = nVel.Rotated(Vector2.Up.AngleTo(up));

    bool OnFloor() => 
        GetNode<RayCast2D>("FloorDetector" ).IsColliding() || 
        GetNode<RayCast2D>("FloorDetector2").IsColliding() ||
        GetNode<RayCast2D>("FloorDetector3").IsColliding();

}
