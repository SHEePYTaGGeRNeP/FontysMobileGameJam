using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class MapGenerator : MonoBehaviour
    {
        public GameObject WaterObject;
        public GameObject DirtObject;
        public int PathWidth = 7;
        private int zPosition = 0;

        public void Start()
        {
            CreateWaterMesh();
            CreateDirtMesh();
        }

        private void CreateDirtMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "DirtMesh";
            mesh.Clear();
            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(new Vector3(PathWidth, 0, 1));
            vertices.Add(new Vector3(PathWidth, 0, -1));
            vertices.Add(new Vector3(-PathWidth, 0, -1));
            vertices.Add(new Vector3(-PathWidth, 0, 1));
            vertices.Add(new Vector3(PathWidth - 1, -1, 1));
            vertices.Add(new Vector3(PathWidth - 1, -1, -1));
            vertices.Add(new Vector3(-PathWidth + 1, -1, -1));
            vertices.Add(new Vector3(-PathWidth + 1, -1, 1));

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
            GameObject dirt = Instantiate(DirtObject) as GameObject;
            dirt.transform.position = new Vector3(-1, 0, zPosition);
            dirt.transform.parent = this.transform;

            for (int i = 0; i < PathWidth; i++)
            {
                GameObject water = Instantiate(WaterObject) as GameObject;
                water.transform.position = new Vector3(-PathWidth + (i * 2), 0, zPosition);
                water.transform.parent = this.transform;
            }

            zPosition += 2;
        }
    }
}
