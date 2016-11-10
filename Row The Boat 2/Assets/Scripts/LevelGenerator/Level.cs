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

        #endregion

        #region "Constructors"

        public Level()
        {
            mesh = new FacesMesh(6);
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public void Generate()
        {
            float displacement = 0f;
            for (int i = 0; i < 50; i++)
            {
                int width = PRNG.GetNumber(3, 7);
                float hwidth = width / 2f;

                mesh.AddLine(new Vector3[] {
                    new Vector3(displacement + -hwidth - 2, 0.5f, i),
                    new Vector3(displacement + -hwidth - 1.5f, 0.5f, i),
                    new Vector3(displacement + -hwidth, -0.2f, i),
                    new Vector3(displacement + hwidth, -0.2f, i),
                    new Vector3(displacement + hwidth  + 1.5f, 0.5f, i),
                    new Vector3(displacement + hwidth + 2, 0.5f, i)
                });

                displacement += (PRNG.GetNumber(0, 10) % 2 == 0 ? PRNG.GetFloatNumber(0, 1f) : PRNG.GetFloatNumber(-1f, 0));
            }
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
