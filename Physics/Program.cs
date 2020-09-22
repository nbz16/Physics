using Altseed2;
using System;

namespace Physics
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Initialize("Phys2D", 960, 720);


            float gravity = 9.8f;
            float speed = 0f;
            Color intersect = new Color(200, 0, 0);
            Color normalColor = new Color(100, 130, 180);

            var circle = new CircleNode();
            circle.Radius = 50;
            circle.Position = new Vector2F(100, 100);
            circle.Color = normalColor;
            circle.VertNum = 100;
            Engine.AddNode(circle);

            var other = new CircleNode();
            other.Radius = 50;
            other.Position = new Vector2F(300, 300);
            other.Color = normalColor;
            other.VertNum = 100;
            Engine.AddNode(other);


            while (Engine.DoEvents())
            {
                Engine.Update();

                float len = (circle.Position - other.Position).Length;
                float dist = circle.Radius + other.Radius;
                if(len <= dist)
                {
                    circle.Color = intersect;
                }
                else
                {
                    circle.Color = normalColor;
                }

                // 移動
                if(Engine.Keyboard.GetKeyState(Key.Right) == ButtonState.Hold)
                {
                    circle.Position += new Vector2F(5f, 0);
                }
                if (Engine.Keyboard.GetKeyState(Key.Left) == ButtonState.Hold)
                {
                    circle.Position += new Vector2F(-5f, 0);
                }
                if (Engine.Keyboard.GetKeyState(Key.Up) == ButtonState.Hold)
                {
                    circle.Position += new Vector2F(0, -5f);
                }
                if (Engine.Keyboard.GetKeyState(Key.Down) == ButtonState.Hold)
                {
                    circle.Position += new Vector2F(0, 5f);
                }

                if (Engine.Keyboard.GetKeyState(Key.Escape) == ButtonState.Push)
                {
                    break;
                }
            }

            Engine.Terminate();
        }
    }
}
