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

        private List<Vector3> lastData;
        private List<Vector3> lastSideData;
        private List<Vector3> lastTopData;

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

        public void GenerateSides(Transform parent)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> faces = new List<int>();

            int amountToDo = 5;

            if (last != null)
            {
                vertices.AddRange(last.lastData);
                amountToDo = 6;
            }

            float low = 1000, high = -1000;

            for (int i = 0; i < 5; i++)
            {
                float left = GetPivit(i, true);
                float right = GetPivit(i, false);

                if (left < low) low = left;
                if (right > high) high = right;

                vertices.Add(new Vector3(left + 1, -1, zPos + i * 2));
                vertices.Add(new Vector3(right - 1, -1, zPos + i * 2));

                vertices.Add(new Vector3(left, 0, zPos + i * 2));
                vertices.Add(new Vector3(right, 0, zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(GetPivit(4, true) + 1, -1, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false) - 1, -1, zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(GetPivit(4, true), 0, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false), 0, zPos + 4 * 2 + 1));

            lastData = vertices.GetRange(vertices.Count - 4, 4);

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

            // Generate a bit of floor

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            MeshFilter mf = (MeshFilter)dirtObject.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)dirtObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            dirtObject.transform.parent = parent;







            dirtObject = GameObject.Instantiate(dirtObject);
            vertices = new List<Vector3>();
            faces = new List<int>();

            if (last != null)
            {
                vertices.AddRange(last.lastSideData);
                amountToDo = 6;
            }

            for (int i = 0; i < 5; i++)
            {
                float left = GetPivit(i, true);
                float right = GetPivit(i, false);

                vertices.Add(new Vector3(left, 0, zPos + i * 2));
                vertices.Add(new Vector3(right, 0, zPos + i * 2));

                vertices.Add(new Vector3(left - 5, 1, zPos + i * 2));
                vertices.Add(new Vector3(right + 5, 1, zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(GetPivit(4, true), 0, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false), 0, zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(GetPivit(4, true) - 5, 1, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false) + 5, 1, zPos + 4 * 2 + 1));

            lastSideData = vertices.GetRange(vertices.Count - 4, 4);

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

            // Generate a bit of floor

            mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            mf = (MeshFilter)dirtObject.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)dirtObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            dirtObject.transform.parent = parent;







            dirtObject = GameObject.Instantiate(dirtObject);
            vertices = new List<Vector3>();
            faces = new List<int>();

            if (last != null)
            {
                vertices.AddRange(last.lastTopData);
                amountToDo = 6;
            }

            for (int i = 0; i < 5; i++)
            {
                vertices.Add(new Vector3(GetPivit(i, true) - 5, 1, zPos + i * 2));
                vertices.Add(new Vector3(GetPivit(i, false) + 5, 1, zPos + i * 2));

                vertices.Add(new Vector3(low - 1 - 5, 1, zPos + i * 2));
                vertices.Add(new Vector3(high + 1 + 5, 1, zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(GetPivit(4, true) - 5, 1, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(GetPivit(4, false) + 5, 1, zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(low - 1 - 5, 1, zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(high + 1 + 5, 1, zPos + 4 * 2 + 1));

            lastTopData = vertices.GetRange(vertices.Count - 4, 4);

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

            // Generate a bit of floor

            mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            mf = (MeshFilter)dirtObject.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)dirtObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            dirtObject.transform.parent = parent;
        }
    }
}
