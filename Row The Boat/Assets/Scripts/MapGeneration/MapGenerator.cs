using Assets.Scripts.MapGeneration.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class MapGenerator : MonoBehaviour
    {
        public GameObject ObjectPoolHolder = null;

        public GameObject WaterObject = null;
        public GameObject DirtObject = null;
        public GameObject Oever = null;

        public int WaterWidth = 7;
        private int zPosition = 0;

        private float lastDisplacement = 0;
        private float displacementValue = 0;
        private float displacementInterpolation = 1;

        private WaterStroke lastWaterBlock;

        private int amount = 30;

        private static MapGenerator instance;
        public static MapGenerator GetInstance()
        {
            return instance;
        }

        public void Start()
        {
            instance = this;
            CreateWaterMesh();
            CreateDirtMesh();
        }

        private void CreateDirtMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "DirtMesh";
            mesh.Clear();
            List<Vector3> vertices = new List<Vector3>();

            //vertices.Add(new Vector3(WaterWidth, 0, 1));
            //vertices.Add(new Vector3(WaterWidth, 0, -1));
            //vertices.Add(new Vector3(-WaterWidth, 0, -1));
            //vertices.Add(new Vector3(-WaterWidth, 0, 1));

            vertices.Add(new Vector3(WaterWidth, -1, 1));
            vertices.Add(new Vector3(WaterWidth, -1, -1));
            vertices.Add(new Vector3(-WaterWidth, -1, -1));
            vertices.Add(new Vector3(-WaterWidth, -1, 1));

            vertices.Add(new Vector3(WaterWidth - 1, -1, 1));
            vertices.Add(new Vector3(WaterWidth - 1, -1, -1));
            vertices.Add(new Vector3(-WaterWidth + 1, -1, -1));
            vertices.Add(new Vector3(-WaterWidth + 1, -1, 1));

            mesh.SetVertices(vertices);

            List<int> triangles = new List<int>();

            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(5);

            triangles.Add(0);
            triangles.Add(5);
            triangles.Add(4);

            triangles.Add(4);
            triangles.Add(5);
            triangles.Add(6);

            triangles.Add(4);
            triangles.Add(6);
            triangles.Add(7);

            triangles.Add(7);
            triangles.Add(6);
            triangles.Add(2);

            triangles.Add(7);
            triangles.Add(2);
            triangles.Add(3);

            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateNormals();

            MeshFilter mf = (MeshFilter)DirtObject.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)DirtObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            DirtObject.name = "Dirt";
        }

        private void CreateWaterMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "WaterMesh";
            mesh.Clear();
            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(1, 0, -1));
            vertices.Add(new Vector3(-1, 0, -1));
            vertices.Add(new Vector3(-1, 0, 1));
            vertices.Add(new Vector3(0, 0.2f, 0));

            mesh.SetVertices(vertices);

            List<int> triangles = new List<int>();

            triangles.Add(4);
            triangles.Add(0);
            triangles.Add(1);

            triangles.Add(4);
            triangles.Add(1);
            triangles.Add(2);

            triangles.Add(4);
            triangles.Add(2);
            triangles.Add(3);

            triangles.Add(4);
            triangles.Add(3);
            triangles.Add(0);

            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateNormals();

            MeshFilter mf = (MeshFilter)WaterObject.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)WaterObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            WaterObject.name = "Water";
            Water w = WaterObject.GetComponent<Water>();
            w.mesh = mesh;
        }

        public void Update()
        {
            if (amount > 0)
            {
                WaterStroke ws = new WaterStroke(5, zPosition, Instantiate(DirtObject), lastWaterBlock);
                for (int row = 0; row < 5; row++)
                {
                    GameObject dirt = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Dirt);
                    dirt.transform.position = new Vector3(-1 + lastDisplacement, 0, zPosition);
                    dirt.transform.parent = this.transform;

                    for (int i = 0; i < WaterWidth; i++)
                    {
                        GameObject water = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Water);
                        water.transform.position = new Vector3(-WaterWidth + (i * 2) + lastDisplacement, 0, zPosition);
                        water.transform.parent = this.transform;
                        ws.AddWater(water, row);
                    }
                    zPosition += 2;

                    if (displacementInterpolation >= 1)
                    {
                        displacementValue += UnityEngine.Random.Range(-100, 100) / 10;
                        displacementInterpolation = 0;
                    }

                    lastDisplacement = Lerp(lastDisplacement, displacementValue, displacementInterpolation);
                    displacementInterpolation += 0.2f;
                }

                ws.GenerateSides(this.transform);

                lastWaterBlock = ws;
            }
            amount--;
        }

        private float Lerp(float start, float end, float i)
        {
            return start + (end - start) * i;
        }
    }
}
