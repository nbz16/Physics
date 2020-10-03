using Altseed2;
using System;

namespace Physics
{
    class Program
    {
        protected static float Dot(Vector2F v1, Vector2F v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        static void Main(string[] args)
        {
            Engine.Initialize("Phys2D", 960, 720);


            float gravity = 9.8f;
            Vector2F speed = new Vector2F(0f,0f);
            Color intersect = new Color(200, 0, 0);
            Color normalColor = new Color(100, 130, 180);

            //プレイヤー
            var player = new CircleNode();
            player.Radius = 70;
            player.Position = new Vector2F(100, 100);
            player.Color = normalColor;
            player.VertNum = 100;
            //circle.ZOrder = 1;
            Engine.AddNode(player);

            //円の相手
            var other = new CircleNode();
            other.Radius = 50;
            other.Position = new Vector2F(150, 300);
            other.Color = normalColor;
            other.VertNum = 100;
            Engine.AddNode(other);

            //三角形の相手
            var triangle = new TriangleNode();
            triangle.Point1 = new Vector2F(400, 100);
            triangle.Point2 = new Vector2F(450, 200);
            triangle.Point3 = new Vector2F(500, 100);
            triangle.Color = normalColor;
            Engine.AddNode(triangle);

            //cup
            var wallLeft = new RectangleNode();
            wallLeft.RectangleSize = new Vector2F(10, 400);
            wallLeft.Position = new Vector2F(Engine.WindowSize.X / 2 - 150, Engine.WindowSize.Y / 2 - wallLeft.RectangleSize.Y / 2);
            wallLeft.Color = new Color(255, 255, 255);
            Engine.AddNode(wallLeft);

            var wallRight = new RectangleNode();
            wallRight.RectangleSize = new Vector2F(10, 400);
            wallRight.Position = new Vector2F(Engine.WindowSize.X / 2 + 150, Engine.WindowSize.Y / 2 - wallLeft.RectangleSize.Y / 2);
            wallRight.Color = new Color(255, 255, 255);
            Engine.AddNode(wallRight);

            var wallBottom = new RectangleNode();
            wallBottom.RectangleSize = new Vector2F(300, 10);
            wallBottom.Position = new Vector2F(Engine.WindowSize.X / 2 - 150, Engine.WindowSize.Y / 2 + wallLeft.RectangleSize.Y / 2);
            wallBottom.Color = new Color(255, 255, 255);
            Engine.AddNode(wallBottom);

            


            while (Engine.DoEvents())
            {
                Engine.Update();

                //speed += new Vector2F(0f, gravity/100);
                player.Position += new Vector2F(0f,1f);
                if (player.Position.Y > 400f) player.Position = new Vector2F(player.Position.X, 400f);

                //円の衝突判定
                float len = (player.Position - other.Position).Length;
                float dist = player.Radius + other.Radius;
                if(len <= dist)
                {
                    player.Color = intersect;

                    //簡単な衝突応答
                    var dir = (other.Position - player.Position);
                    //中心間を結んだベクトルの方向に、めり込んだ分だけ移動
                    other.Position += dir.Normal * (player.Radius + other.Radius - dir.Length);
                }
                else
                {
                    player.Color = normalColor;
                }

                //円と三角形の衝突判定
                if ((triangle.Point1 - player.Position).Length < player.Radius
                    || (triangle.Point2 - player.Position).Length < player.Radius
                    || (triangle.Point3 - player.Position).Length < player.Radius) player.Color = intersect;

                var AO = player.Position - triangle.Point1;
                var BO = player.Position - triangle.Point3;
                var CO = player.Position - triangle.Point2;
                var AB = triangle.Point3 - triangle.Point1;
                var BC = triangle.Point2 - triangle.Point3;
                var CA = triangle.Point1 - triangle.Point2;
                var dist1 = Math.Abs(Vector2F.Cross(AO, AB)) / AB.Length;
                var dist2 = Math.Abs(Vector2F.Cross(BO, BC)) / BC.Length;
                var dist3 = Math.Abs(Vector2F.Cross(CO, CA)) / CA.Length;
                if (dist1 <= player.Radius && Dot(AO, AB) * Dot(BO, AB) <= 0) player.Color = intersect;
                if (dist2 <= player.Radius && Dot(BO, BC) * Dot(CO, BC) <= 0) player.Color = intersect;
                if (dist3 <= player.Radius && Dot(CO, CA) * Dot(AO, CA) <= 0) player.Color = intersect;
                if (Vector2F.Cross(AO, AB) <= 0 && Vector2F.Cross(BO, BC) <= 0 && Vector2F.Cross(CO, CA) <= 0) player.Color = intersect;

                // 移動
                if (Engine.Keyboard.GetKeyState(Key.Right) == ButtonState.Hold)
                {
                    player.Position += new Vector2F(5f, 0);
                }
                if (Engine.Keyboard.GetKeyState(Key.Left) == ButtonState.Hold)
                {
                    player.Position += new Vector2F(-5f, 0);
                }
                if (Engine.Keyboard.GetKeyState(Key.Up) == ButtonState.Hold)
                {
                    player.Position += new Vector2F(0, -5f);
                }
                if (Engine.Keyboard.GetKeyState(Key.Down) == ButtonState.Hold)
                {
                    player.Position += new Vector2F(0, 5f);
                }

                //終了
                if (Engine.Keyboard.GetKeyState(Key.Escape) == ButtonState.Push)
                {
                    break;
                }
            }

            Engine.Terminate();
        }
    }
}
