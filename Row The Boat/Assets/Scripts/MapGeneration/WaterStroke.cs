using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    using Object = UnityEngine.Object;

    class WaterStroke
    {
        private List<GameObject>[] water;
        private GameObject dirtObject;
        private int zPos;
        private WaterStroke last;
        private List<GameObject> other;

        private Mesh underWaterMesh;
        private Mesh sideMesh;
        private Mesh topMesh;

        private GameObject underWater;
        private GameObject side;
        private GameObject top;

        private List<Vector3> lastData;
        private List<Vector3> lastSideData;
        private List<Vector3> lastTopData;

        public WaterStroke(int size, int zPos, GameObject dirtObject, WaterStroke last, Transform parent)
        {
            this.water = new List<GameObject>[size];
            this.other = new List<GameObject>();
            this.zPos = zPos;
            this.dirtObject = dirtObject;
            this.last = last;
            for (int i = 0; i < size; i++)
            {
                this.water[i] = new List<GameObject>();
            }

            this.underWaterMesh = new Mesh();
            this.sideMesh = new Mesh();
            this.topMesh = new Mesh();
            this.underWater = Object.Instantiate(dirtObject);
            this.side = Object.Instantiate(dirtObject);
            this.top = Object.Instantiate(dirtObject);
            this.underWater.transform.parent = parent;
            this.side.transform.parent = parent;
            this.top.transform.parent = parent;

            List<Vector3> vertices = new List<Vector3>();
            List<int> faces = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                vertices.Add(new Vector3());
                faces.Add(0);
            }
            this.underWaterMesh.SetVertices(vertices);
            this.underWaterMesh.SetTriangles(faces.GetRange(0, 21), 0);
            this.sideMesh.SetVertices(vertices);
            this.sideMesh.SetTriangles(faces.GetRange(0, 21), 0);
            this.topMesh.SetVertices(vertices);
            this.topMesh.SetTriangles(faces.GetRange(0, 21), 0);
        }

        public void AddWater(GameObject obj, int row)
        {
            this.water[row].Add(obj);
        }

        public void AddObstacle(GameObject obj)
        {
            this.other.Add(obj);
        }

        public float GetPivit(int row, bool findLeft)
        {
            int found = 0;
            for (int i = 1; i < this.water[row].Count; i++)
            {
                if (this.water[row][i].transform.position.x < this.water[row][found].transform.position.x && findLeft)
                {
                    found = i;
                }
                else if (this.water[row][i].transform.position.x > this.water[row][found].transform.position.x && !findLeft)
                {
                    found = i;
                }
            }

            return this.water[row][found].transform.position.x;
        }

        private void GenerateUnderWater()
        {

        }

        public void GenerateSides(Transform parent)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> faces = new List<int>();

            int amountToDo = 5;

            if (this.last != null)
            {
                vertices.AddRange(this.last.lastData);
                amountToDo = 6;
            }

            float low = 1000, high = -1000;

            for (int i = 0; i < 5; i++)
            {
                float left = this.GetPivit(i, true);
                float right = this.GetPivit(i, false);

                if (left < low) low = left;
                if (right > high) high = right;

                vertices.Add(new Vector3(left + 1, -1, this.zPos + i * 2));
                vertices.Add(new Vector3(right - 1, -1, this.zPos + i * 2));

                vertices.Add(new Vector3(left, 0, this.zPos + i * 2));
                vertices.Add(new Vector3(right, 0, this.zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(this.GetPivit(4, true) + 1, -1, this.zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false) - 1, -1, this.zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(this.GetPivit(4, true), 0, this.zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false), 0, this.zPos + 4 * 2 + 1));

            this.lastData = vertices.GetRange(vertices.Count - 4, 4);

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
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            MeshFilter mf = (MeshFilter)this.underWater.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)this.underWater.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh.Clear();
            mf.mesh = mesh;

            this.underWater.transform.parent = parent;
            this.other.Add(this.underWater);






            //dirtObject = ObjectPool.ObjectPool.GetInstance().GetObject(ObjectPool.GameObjectType.Dirt);
            //dirtObject = GameObject.Instantiate(dirtObject);
            vertices = new List<Vector3>();
            faces = new List<int>();

            if (this.last != null)
            {
                vertices.AddRange(this.last.lastSideData);
                amountToDo = 6;
            }

            for (int i = 0; i < 5; i++)
            {
                float left = this.GetPivit(i, true);
                float right = this.GetPivit(i, false);

                vertices.Add(new Vector3(left, 0, this.zPos + i * 2));
                vertices.Add(new Vector3(right, 0, this.zPos + i * 2));

                vertices.Add(new Vector3(left - 5, 1, this.zPos + i * 2));
                vertices.Add(new Vector3(right + 5, 1, this.zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(this.GetPivit(4, true), 0, this.zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false), 0, this.zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(this.GetPivit(4, true) - 5, 1, this.zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false) + 5, 1, this.zPos + 4 * 2 + 1));

            this.lastSideData = vertices.GetRange(vertices.Count - 4, 4);

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
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            mf = (MeshFilter)this.side.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)this.side.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh.Clear();
            mf.mesh = mesh;

            this.side.transform.parent = parent;
            this.other.Add(this.side);







            //dirtObject = ObjectPool.ObjectPool.GetInstance().GetObject(ObjectPool.GameObjectType.Dirt);
            //dirtObject = GameObject.Instantiate(dirtObject);
            vertices = new List<Vector3>();
            faces = new List<int>();
            List<Color> colors = new List<Color>();

            int sideSize = 50;

            if (this.last != null)
            {
                vertices.AddRange(this.last.lastTopData);

                amountToDo = 6;
            }

            for (int i = 0; i < 5; i++)
            {
                vertices.Add(new Vector3(this.GetPivit(i, true) - 5, 1, this.zPos + i * 2));
                vertices.Add(new Vector3(this.GetPivit(i, false) + 5, 1, this.zPos + i * 2));

                vertices.Add(new Vector3(low - 1 - sideSize, 5, this.zPos + i * 2));
                vertices.Add(new Vector3(high + 1 + sideSize, 5, this.zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(this.GetPivit(4, true) - 5, 1, this.zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false) + 5, 1, this.zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(low - 1 - sideSize, 5, this.zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(high + 1 + sideSize, 5, this.zPos + 4 * 2 + 1));

            this.lastTopData = vertices.GetRange(vertices.Count - 4, 4);

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
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetColors(colors);
            mesh.SetTriangles(faces, 0);
            mesh.RecalculateNormals();

            mf = (MeshFilter)this.top.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)this.top.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh.Clear();
            mf.mesh = mesh;

            this.top.transform.parent = parent;
            this.other.Add(this.top);
        }

        public void Destroy()
        {
            for (int i = 0; i < this.water.Length; i++)
            {
                for (int j = 0; j < this.water[i].Count; j++)
                {
                    ObjectPool.ObjectPool.GetInstance().SetBeschikbaar(this.water[i][j]);
                }
            }

            for (int i = 0; i < this.other.Count; i++)
            {
                ObjectPool.ObjectPool.GetInstance().SetBeschikbaar(this.other[i]);
            }

            Object.Destroy(this.underWater);
            Object.Destroy(this.side);
            Object.Destroy(this.top);
        }

        public float ZPosition { get { return this.zPos; } }
    }
}
