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
        private List<GameObject>[] _water;
        private GameObject _dirtObject;
        private int _zPos;
        private WaterStroke _last;
        private List<GameObject> _other;

        private Mesh _underWaterMesh;
        private Mesh _sideMesh;
        private Mesh _topMesh;

        private GameObject _underWater;
        private GameObject _side;
        private GameObject _top;

        private List<Vector3> _lastData;
        private List<Vector3> _lastSideData;
        private List<Vector3> _lastTopData;

        public WaterStroke(int size, int zPos, GameObject dirtObject, WaterStroke last, Transform parent)
        {
            this._water = new List<GameObject>[size];
            this._other = new List<GameObject>();
            this._zPos = zPos;
            this._dirtObject = dirtObject;
            this._last = last;
            for (int i = 0; i < size; i++)
            {
                this._water[i] = new List<GameObject>();
            }

            this._underWaterMesh = new Mesh();
            this._sideMesh = new Mesh();
            this._topMesh = new Mesh();
            this._underWater = Object.Instantiate(dirtObject);
            this._side = Object.Instantiate(dirtObject);
            this._top = Object.Instantiate(dirtObject);
            this._underWater.transform.parent = parent;
            this._side.transform.parent = parent;
            this._top.transform.parent = parent;

            List<Vector3> vertices = new List<Vector3>();
            List<int> faces = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                vertices.Add(new Vector3());
                faces.Add(0);
            }
            this._underWaterMesh.SetVertices(vertices);
            this._underWaterMesh.SetTriangles(faces.GetRange(0, 21), 0);
            this._sideMesh.SetVertices(vertices);
            this._sideMesh.SetTriangles(faces.GetRange(0, 21), 0);
            this._topMesh.SetVertices(vertices);
            this._topMesh.SetTriangles(faces.GetRange(0, 21), 0);
        }

        public void AddWater(GameObject obj, int row)
        {
            this._water[row].Add(obj);
        }

        public void AddObstacle(GameObject obj)
        {
            this._other.Add(obj);
        }

        public float GetPivit(int row, bool findLeft)
        {
            int found = 0;
            for (int i = 1; i < this._water[row].Count; i++)
            {
                if (this._water[row][i].transform.position.x < this._water[row][found].transform.position.x && findLeft)
                {
                    found = i;
                }
                else if (this._water[row][i].transform.position.x > this._water[row][found].transform.position.x && !findLeft)
                {
                    found = i;
                }
            }

            return this._water[row][found].transform.position.x;
        }

        private void GenerateUnderWater()
        {

        }

        public void GenerateSides(Transform parent)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> faces = new List<int>();

            int amountToDo = 5;

            if (this._last != null)
            {
                vertices.AddRange(this._last._lastData);
                amountToDo = 6;
            }

            float low = 1000, high = -1000;

            for (int i = 0; i < 5; i++)
            {
                float left = this.GetPivit(i, true);
                float right = this.GetPivit(i, false);

                if (left < low) low = left;
                if (right > high) high = right;

                vertices.Add(new Vector3(left + 1, -1, this._zPos + i * 2));
                vertices.Add(new Vector3(right - 1, -1, this._zPos + i * 2));

                vertices.Add(new Vector3(left, 0, this._zPos + i * 2));
                vertices.Add(new Vector3(right, 0, this._zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(this.GetPivit(4, true) + 1, -1, this._zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false) - 1, -1, this._zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(this.GetPivit(4, true), 0, this._zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false), 0, this._zPos + 4 * 2 + 1));

            this._lastData = vertices.GetRange(vertices.Count - 4, 4);

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

            MeshFilter mf = (MeshFilter)this._underWater.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)this._underWater.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh.Clear();
            mf.mesh = mesh;

            this._underWater.transform.parent = parent;
            this._other.Add(this._underWater);






            //dirtObject = ObjectPool.ObjectPool.GetInstance().GetObject(ObjectPool.GameObjectType.Dirt);
            //dirtObject = GameObject.Instantiate(dirtObject);
            vertices = new List<Vector3>();
            faces = new List<int>();

            if (this._last != null)
            {
                vertices.AddRange(this._last._lastSideData);
                amountToDo = 6;
            }

            for (int i = 0; i < 5; i++)
            {
                float left = this.GetPivit(i, true);
                float right = this.GetPivit(i, false);

                vertices.Add(new Vector3(left, 0, this._zPos + i * 2));
                vertices.Add(new Vector3(right, 0, this._zPos + i * 2));

                vertices.Add(new Vector3(left - 5, 1, this._zPos + i * 2));
                vertices.Add(new Vector3(right + 5, 1, this._zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(this.GetPivit(4, true), 0, this._zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false), 0, this._zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(this.GetPivit(4, true) - 5, 1, this._zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false) + 5, 1, this._zPos + 4 * 2 + 1));

            this._lastSideData = vertices.GetRange(vertices.Count - 4, 4);

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

            mf = (MeshFilter)this._side.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)this._side.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh.Clear();
            mf.mesh = mesh;

            this._side.transform.parent = parent;
            this._other.Add(this._side);







            //dirtObject = ObjectPool.ObjectPool.GetInstance().GetObject(ObjectPool.GameObjectType.Dirt);
            //dirtObject = GameObject.Instantiate(dirtObject);
            vertices = new List<Vector3>();
            faces = new List<int>();
            List<Color> colors = new List<Color>();

            int sideSize = 50;

            if (this._last != null)
            {
                vertices.AddRange(this._last._lastTopData);

                amountToDo = 6;
            }

            for (int i = 0; i < 5; i++)
            {
                vertices.Add(new Vector3(this.GetPivit(i, true) - 5, 1, this._zPos + i * 2));
                vertices.Add(new Vector3(this.GetPivit(i, false) + 5, 1, this._zPos + i * 2));

                vertices.Add(new Vector3(low - 1 - sideSize, 5, this._zPos + i * 2));
                vertices.Add(new Vector3(high + 1 + sideSize, 5, this._zPos + i * 2));
            }

            // Add vertices at the end, so the next stroke can connect to it
            vertices.Add(new Vector3(this.GetPivit(4, true) - 5, 1, this._zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(this.GetPivit(4, false) + 5, 1, this._zPos + 4 * 2 + 1));

            vertices.Add(new Vector3(low - 1 - sideSize, 5, this._zPos + 4 * 2 + 1));
            vertices.Add(new Vector3(high + 1 + sideSize, 5, this._zPos + 4 * 2 + 1));

            this._lastTopData = vertices.GetRange(vertices.Count - 4, 4);

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

            mf = (MeshFilter)this._top.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)this._top.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh.Clear();
            mf.mesh = mesh;

            this._top.transform.parent = parent;
            this._other.Add(this._top);
        }

        public void Destroy()
        {
            for (int i = 0; i < this._water.Length; i++)
            {
                for (int j = 0; j < this._water[i].Count; j++)
                {
                    ObjectPool.ObjectPool.GetInstance().SetBeschikbaar(this._water[i][j]);
                }
            }

            for (int i = 0; i < this._other.Count; i++)
            {
                ObjectPool.ObjectPool.GetInstance().SetBeschikbaar(this._other[i]);
            }

            Object.Destroy(this._underWater);
            Object.Destroy(this._side);
            Object.Destroy(this._top);
        }

        public float ZPosition { get { return this._zPos; } }
    }
}
