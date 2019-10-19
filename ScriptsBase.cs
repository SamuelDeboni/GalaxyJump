using Godot;
using System;

public class ScriptsBase : Sprite
{
    public void _on_Button_button_up()
    {
        GetTree().ChangeScene("res://Menu.tscn");
    }
}
