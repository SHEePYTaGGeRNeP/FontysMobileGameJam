using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LevelGenerator
{
    class Level
    {
        #region "Fields"

        private FacesMesh mesh;
        private int distance;

        #endregion

        #region "Constructors"

        public Level()
        {
            mesh = new FacesMesh(8);
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public void Generate()
        {
            GenerateWater();

            GenerateDecorations();
        }

        private void GenerateDecorations()
        {
            // Generate some trees
            for (int i = 0; i < distance; i++)
            {
                GameObject leftTree = ObjectPool.Instantiate("tree" + PRNG.GetNumber(1, 7));
                leftTree.transform.parent = LevelGenerator.INSTANCE.transform;
                leftTree.transform.position = new Vector3(PRNG.GetFloatNumber(-20, -5), -1f, i);

                GameObject rightTree = ObjectPool.Instantiate("tree" + PRNG.GetNumber(1, 7));
                rightTree.transform.parent = LevelGenerator.INSTANCE.transform;
                rightTree.transform.position = new Vector3(PRNG.GetFloatNumber(10, 20), -1f, i);
            }

            // Generate some stones
            for (int i = 0; i < distance / 3; i++)
            {
                GameObject stone = ObjectPool.Instantiate("stone" + PRNG.GetNumber(0, 4));
                stone.transform.parent = LevelGenerator.INSTANCE.transform;
                stone.transform.position = new Vector3(PRNG.GetFloatNumber(-10, 10), -1.5f, i * 3);
            }
        }

        private void GenerateWater()
        {
            distance = 0;

            float displacement = 0f;
            float width = 3;

            for (int i = 0; i < 2; i++)
            {
                float hwidth = width / 2f;
                mesh.AddLine(new Vector3[] {
                    new Vector3(-20,                            0,   distance),
                    new Vector3(displacement + -hwidth - 3f,    0,   distance),
                    new Vector3(displacement + -hwidth - 1.5f,  0,   distance),
                    new Vector3(displacement + -hwidth,         0,   distance),
                    new Vector3(displacement + hwidth,          0,   distance),
                    new Vector3(displacement + hwidth  + 1.5f,  0,   distance),
                    new Vector3(displacement + hwidth  + 3f,    0,   distance),
                    new Vector3(20,                             0,   distance)
                });

                displacement += PRNG.GetFloatNumber(-1f, 1f);

                distance += 15;
            }
            distance -= 14;

            distance += GeneratePuddle(ref displacement, distance, 3f, 4.5f);
            distance += GenerateMoat(ref displacement, distance, 50);
            distance += GeneratePuddle(ref displacement, distance, 4.5f, 3f);

            for (int i = 0; i < 2; i++)
            {
                float hwidth = width / 2f;
                mesh.AddLine(new Vector3[] {
                    new Vector3(-20,                            0,   distance),
                    new Vector3(displacement + -hwidth - 3f,    0,   distance),
                    new Vector3(displacement + -hwidth - 1.5f,  0,   distance),
                    new Vector3(displacement + -hwidth,         0,   distance),
                    new Vector3(displacement + hwidth,          0,   distance),
                    new Vector3(displacement + hwidth  + 1.5f,  0,   distance),
                    new Vector3(displacement + hwidth  + 3f,    0,   distance),
                    new Vector3(20,                             0,   distance)
                });

                displacement += PRNG.GetFloatNumber(-1f, 1f);

                distance += 15;
            }

            distance -= 15;
        }

        public int GenerateMoat(ref float displacement, int distance, int lenght)
        {
            float width = PRNG.GetFloatNumber(2, 6);

            for (int i = 0; i < 50; i++)
            {
                float hwidth = width / 2f;

                AddPoints(displacement, width, distance + i);

                displacement += PRNG.GetFloatNumber(-1f, 1f);
                width += PRNG.GetFloatNumber(-1f, 1f);
                if (width < 2 || width > 8f)
                    width = PRNG.GetFloatNumber(2, 6);
            }

            return lenght;
        }

        private int GeneratePuddle(ref float displacement, int distance, float startWidth, float endWidth)
        {
            int width = PRNG.GetNumber(10, 20);
            int depth = PRNG.GetNumber(15, 20);

            for (int i = 0; i < depth; i++)
            {
                float interpolation = 1f / (depth - 1) * i;

                float currentWidth = width;
                if (interpolation < 0.5f)
                {
                    currentWidth = Interpolate(startWidth, width, interpolation * 2f);
                }
                else if (interpolation == 0.5f)
                    currentWidth = width;
                else
                    currentWidth = Interpolate(width, endWidth, (interpolation - 0.5f) * 2f);

                AddPoints(displacement, currentWidth, distance + i);
                displacement += PRNG.GetFloatNumber(-1f, 1f);
            }

            return depth;
        }

        private float Interpolate(float start, float end, float interpolation)
        {
            return start + (end - start) * interpolation;
        }

        private void AddPoints(float displacement, float width, int distance)
        {
            float hwidth = width / 2f;
            mesh.AddLine(new Vector3[] {
                new Vector3(-20,                             0f,     distance),
                new Vector3(displacement + -hwidth - 3f,     0f,     distance),
                new Vector3(displacement + -hwidth - 1.5f,   -0.3f,  distance),
                new Vector3(displacement + -hwidth,          -1.3f,  distance),
                new Vector3(displacement + hwidth,           -1.3f,  distance),
                new Vector3(displacement + hwidth  + 1.5f,   -0.3f,  distance),
                new Vector3(displacement + hwidth  + 3f,     0f,     distance),
                new Vector3(20,                              0f,     distance)
            });

            CapsuleCollider leftCollider = LevelGenerator.INSTANCE.RiverCollider.AddComponent<CapsuleCollider>();
            leftCollider.center = new Vector3(displacement + -hwidth - 1.2f, -0.8f, distance);
            leftCollider.radius = 0.8f;

            CapsuleCollider rightCollider = LevelGenerator.INSTANCE.RiverCollider.AddComponent<CapsuleCollider>();
            rightCollider.center = new Vector3(displacement + hwidth + 1.2f, -0.8f, distance);
            rightCollider.radius = 0.8f;
        }

        public void Draw()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = this.mesh.Vertices;
            mesh.triangles = this.mesh.Faces;

            mesh.RecalculateNormals();


            GameObject obj = ObjectPool.Instantiate("emptymesh");
            obj.GetComponent<MeshFilter>().mesh = mesh;
            obj.transform.parent = LevelGenerator.INSTANCE.transform;
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
