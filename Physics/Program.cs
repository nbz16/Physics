using Altseed2;
using System;
using System.Collections.Generic;

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
            List<CircleNode> collision = new List<CircleNode>();

            //プレイヤー
            var player = new CircleNode();
            player.Radius = 70;
            player.Position = new Vector2F(300, 100);
            player.Color = normalColor;
            player.VertNum = 100;
            //circle.ZOrder = 1;
            Engine.AddNode(player);

            //円の相手
            var other = new CircleNode();
            other.Radius = 50;
            other.Position = new Vector2F(350, 300);
            other.Color = normalColor;
            other.VertNum = 100;
            Engine.AddNode(other);

            var other2 = new CircleNode();
            other2.Radius = 50;
            other2.Position = new Vector2F(300, 50);
            other2.Color = normalColor;
            other2.VertNum = 100;
            Engine.AddNode(other2);

            //三角形の相手
            var triangle = new TriangleNode();
            triangle.Point1 = new Vector2F(400, 100);
            triangle.Point2 = new Vector2F(450, 200);
            triangle.Point3 = new Vector2F(500, 100);
            //triangle.Position = new Vector2F((triangle.Point1.X + triangle.Point2.X + triangle.Point3.X) / 3, (triangle.Point1.Y + triangle.Point2.Y + triangle.Point3.Y) / 3);
            //triangle.Position = new Vector2F((400 + 450 + 500) / 3, (100 + 200 + 100) / 3);
            triangle.Color = normalColor;
            //Engine.AddNode(triangle);

            var line = new RectangleNode();
            line.RectangleSize = new Vector2F(960, 2);
            line.Position = new Vector2F(0, 400);
            line.Color = new Color(255, 30, 30);
            Engine.AddNode(line);

            var lineL = new RectangleNode();
            lineL.RectangleSize = new Vector2F(2, 720-320);
            lineL.Position = new Vector2F(200, 0);
            lineL.Color = new Color(255, 30, 30);
            Engine.AddNode(lineL);

            var lineR = new RectangleNode();
            lineR.RectangleSize = new Vector2F(2, 720 - 320);
            lineR.Position = new Vector2F(500, 0);
            lineR.Color = new Color(255, 30, 30);
            Engine.AddNode(lineR);


            collision.Add(player);
            collision.Add(other);
            collision.Add(other2);

            while (Engine.DoEvents())
            {
                Engine.Update();

                //speed += new Vector2F(0f, gravity/100);
                //player.Position += new Vector2F(0f, 1f);
                for(int i = 0; i < collision.Count; i++)
                {
                    for(int j = 0; j < collision.Count; j++)
                    {
                        if (collision[i] == collision[j]) continue;
                        float length = (collision[i].Position - collision[j].Position).Length;
                        float distance = collision[i].Radius + collision[j].Radius;
                        if(length <= distance)
                        {
                            //collision[i].Color = intersect;
                            //collision[j].Color = intersect;

                            //簡単な衝突応答
                            var dir = (collision[j].Position - collision[i].Position);
                            //中心間を結んだベクトルの方向に、めり込んだ分だけ移動
                            collision[i].Position -= dir.Normal * (distance - dir.Length);
                            collision[j].Position += dir.Normal * (distance - dir.Length);
                        }
                        else
                        {
                            collision[i].Color = normalColor;
                            collision[j].Color = normalColor;
                        }
                    }

                    CircleNode obj = collision[i];
                    obj.Position += new Vector2F(0f, 2f);
                    if (obj.Position.Y + obj.Radius > 400f) obj.Position = new Vector2F(obj.Position.X, 400f - obj.Radius);
                    if (obj.Position.X - obj.Radius < 200f) obj.Position = new Vector2F(200f + obj.Radius, obj.Position.Y);
                    if (obj.Position.X + obj.Radius > 500f) obj.Position = new Vector2F(500f - obj.Radius, obj.Position.Y);
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
                var center = new Vector2F((triangle.Point1.X + triangle.Point2.X + triangle.Point3.X) / 3, (triangle.Point1.Y + triangle.Point2.Y + triangle.Point3.Y) / 3);
                var direction = center - player.Position;
                var dist1 = Math.Abs(Vector2F.Cross(AO, AB)) / AB.Length;
                var dist2 = Math.Abs(Vector2F.Cross(BO, BC)) / BC.Length;
                var dist3 = Math.Abs(Vector2F.Cross(CO, CA)) / CA.Length;
                if (dist1 <= player.Radius && Dot(AO, AB) * Dot(BO, AB) <= 0)
                {
                    player.Color = intersect;
                    triangle.Point1 += direction.Normal * (player.Radius - dist1);
                    triangle.Point2 += direction.Normal * (player.Radius - dist1);
                    triangle.Point3 += direction.Normal * (player.Radius - dist1);
                }
                if(dist2 <= player.Radius && Dot(BO, BC) * Dot(CO, BC) <= 0)
                {
                    player.Color = intersect;
                    triangle.Point1 += direction.Normal * (player.Radius - dist2);
                    triangle.Point2 += direction.Normal * (player.Radius - dist2);
                    triangle.Point3 += direction.Normal * (player.Radius - dist2);
                }
                if (dist3 <= player.Radius && Dot(CO, CA) * Dot(AO, CA) <= 0)
                {
                    player.Color = intersect;
                    triangle.Point1 += direction.Normal * (player.Radius - dist3);
                    triangle.Point2 += direction.Normal * (player.Radius - dist3);
                    triangle.Point3 += direction.Normal * (player.Radius - dist3);
                }
                if (triangle.Point1.Y < 400 || triangle.Point2.Y < 400 || triangle.Point2.Y < 400)
                {
                    triangle.Point1 += new Vector2F(0f, 1f);
                    triangle.Point2 += new Vector2F(0f, 1f);
                    triangle.Point3 += new Vector2F(0f, 1f);
                }

                //if (Vector2F.Cross(AO, AB) <= 0 && Vector2F.Cross(BO, BC) <= 0 && Vector2F.Cross(CO, CA) <= 0) player.Color = intersect;

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
