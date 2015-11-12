using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class WaterStroke
    {
        private List<GameObject>[] water;
        private GameObject dirtObject;
        private int zPos;
        private WaterStroke last;

        public WaterStroke(int size, int zPos, GameObject dirtObject, WaterStroke last)
        {
            water = new List<GameObject>[size];
            this.zPos = zPos;
            this.dirtObject = dirtObject;
            this.last = last;
            for (int i = 0; i < size; i++)
            {
                water[i] = new List<GameObject>();
            }
        }

        public void AddWater(GameObject obj, int row)
        {
            water[row].Add(obj);
        }

        public float GetPivit(int row, bool findLeft)
        {
            int found = 0;
            for (int i = 1; i < water[row].Count; i++)
            {
                if (water[row][i].transform.position.x < water[row][found].transform.position.x && findLeft)
                {
                    found = i;
                }
                else if (water[row][i].transform.position.x > water[row][found].transform.position.x && !findLeft)
                {
                    found = i;
                }
            }

            return water[row][found].transform.position.x;
        }

        public void GenerateSides()
        {
            GameObject obj;
            List<Vector3> vertices = new List<Vector3>();
            List<int> faces = new List<int>();

            int amountToDo = 5;

            if (last != null)
            {
            }

            for (int i = 0; i < 5; i++)
            {
                vertices.Add(new Vector3(GetPivit(i, true), -1, zPos + i * 2));
                vertices.Add(new Vector3(GetPivit(i, false), -1, zPos + i * 2));

                vertices.Add(new Vector3(GetPivit(i, true), 1, zPos + i * 2));
                vertices.Add(new Vector3(GetPivit(i, false), 1, zPos + i * 2));

                //obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //obj.transform.position = new Vector3(GetPivit(i, true), 0, water[i][0].transform.position.z);

                //obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //obj.transform.position = new Vector3(GetPivit(i, false), 0, water[i][0].transform.position.z);
            }

            vertices.Add(new Vector3(GetPivit(4, true), -1, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false), -1, zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(GetPivit(4, true), 1, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false), 1, zPos + 4 * 2 + 1));

            for (int i = 0; i < amountToDo; i++)
            {
                faces.Add(i * 4);
                faces.Add(i * 4 + 2);
                faces.Add(i * 4 + 6);

                faces.Add(i * 4);
                faces.Add(i * 4 + 6);
                faces.Add(i * 4 + 4);

                faces.Add(i * 4 + 1);
                faces.Add(i * 4 + 5);
                faces.Add(i * 4 + 3);

                faces.Add(i * 4 + 3);
                faces.Add(i * 4 + 5);
                faces.Add(i * 4 + 7);
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            MeshFilter mf = (MeshFilter)dirtObject.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)dirtObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;


        }
    }
}
