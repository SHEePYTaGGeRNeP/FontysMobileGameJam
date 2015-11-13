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
        public GameObject DirtSideObject = null;

        public GameObject Finish = null;

        public List<GameObject> stones = null;
        public List<GameObject> trees = null;
        public List<GameObject> decoration = null;

        public GameObject Player = null;

        public int WaterWidth = 7;
        private int zPosition = 0;

        private float lastDisplacement = 0;
        private float displacementValue = 0;
        private float displacementInterpolation = 1;

        private WaterStroke lastWaterBlock;

        private int amount = 20;

        private List<WaterStroke> strokes;
        private List<GameObject> strokeObjects;
        private List<GameObject> dirt;

        private bool generateObstacle = false;

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

            strokes = new List<WaterStroke>();
            strokeObjects = new List<GameObject>();
            dirt = new List<GameObject>();
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
            //Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z + 0.25f);

            if (strokes.Count != 30)
            {
                GameObject parent = new GameObject();
                parent.name = "WaterStroke";
                parent.transform.parent = this.transform;
                strokeObjects.Add(parent);

                //WaterStroke ws = new WaterStroke(5, zPosition, ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Dirt), lastWaterBlock);
                WaterStroke ws = new WaterStroke(5, zPosition, DirtSideObject, lastWaterBlock, parent.transform);
                for (int row = 0; row < 5; row++)
                {
                    GameObject dirt = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Dirt);
                    dirt.transform.position = new Vector3(-1 + lastDisplacement, 0, zPosition);
                    dirt.transform.parent = parent.transform;
                    this.dirt.Add(dirt);

                    for (int i = 0; i < WaterWidth; i++)
                    {
                        GameObject water = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Water);
                        water.transform.position = new Vector3(-WaterWidth + (i * 2) + lastDisplacement, 0, zPosition);
                        water.transform.parent = parent.transform;

                        ws.AddWater(water, row);
                    }

                    CapsuleCollider bc = dirt.AddComponent<CapsuleCollider>();
                    bc.center = new Vector3(-(WaterWidth - 1.25f), 0, 0);
                    //bc.size = new Vector3(2, 1, 2);
                    bc.height = 1;
                    bc.radius = 1.5f;

                    bc = dirt.AddComponent<CapsuleCollider>();
                    bc.center = new Vector3(WaterWidth - 1.25f, 0, 0);
                    //bc.size = new Vector3(2, 1, 2);
                    bc.height = 1;
                    bc.radius = 1.5f;


                    zPosition += 2;

                    if (displacementInterpolation >= 1)
                    {
                        displacementValue += UnityEngine.Random.Range(-100, 100) / 10;
                        displacementInterpolation = 0;
                    }

                    lastDisplacement = Lerp(lastDisplacement, displacementValue, displacementInterpolation);
                    displacementInterpolation += 0.2f;
                }

                // Generate an obstacle
                if (generateObstacle)
                {
                    List<GameObjectType> obsta = new List<GameObjectType>();
                    obsta.Add(GameObjectType.Stone);
                    obsta.Add(GameObjectType.Tree);

                    GameObjectType type = obsta[UnityEngine.Random.Range(0, obsta.Count)];

                    GameObject go = ObjectPool.ObjectPool.GetInstance().GetObject(type);
                    if (type == GameObjectType.Stone)
                    {
                        go.transform.position = new Vector3(lastDisplacement + UnityEngine.Random.Range(-WaterWidth, WaterWidth), -1, zPosition);
                    }
                    else if (type == GameObjectType.Tree)
                    {
                        int dist = UnityEngine.Random.Range(-25, 25);
                        go.transform.position = new Vector3(lastDisplacement + dist, -1, zPosition);

                        go.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-45, 45));
                    }
                    go.transform.parent = parent.transform;
                    ws.AddObstacle(go);
                }

                generateObstacle = !generateObstacle;

                // Generate decoration

                for (int i = 0; i < UnityEngine.Random.Range(2, 4); i++)
                {
                    GameObject go = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                    go.transform.position = new Vector3(lastDisplacement + UnityEngine.Random.Range(-WaterWidth * 3, -WaterWidth), go.transform.position.y, zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
                    go.transform.parent = parent.transform;
                    ws.AddObstacle(go);

                    //go = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Decoration);
                    //go.transform.position = new Vector3(lastDisplacement + UnityEngine.Random.Range(-WaterWidth * 3, -WaterWidth), 1, zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
                    //go.transform.parent = parent.transform;
                    //ws.AddObstacle(go);
                }

                for (int i = 0; i < UnityEngine.Random.Range(2, 4); i++)
                {
                    GameObject go = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                    go.transform.position = new Vector3(lastDisplacement + UnityEngine.Random.Range(WaterWidth, WaterWidth * 3), go.transform.position.y, zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
                    go.transform.parent = parent.transform;
                    ws.AddObstacle(go);

                    //go = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Decoration);
                    //go.transform.position = new Vector3(lastDisplacement + UnityEngine.Random.Range(WaterWidth, WaterWidth * 3), 1, zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
                    //go.transform.parent = parent.transform;
                    //ws.AddObstacle(go);
                }



                ws.GenerateSides(parent.transform);

                strokes.Add(ws);

                lastWaterBlock = ws;         
            }

            if (strokes.Count > 0)
            {
                if (strokes[0].ZPosition + 20 * 2 < Player.transform.position.z)
                {
                    strokes[0].Destroy();
                    strokes.RemoveAt(0);
                    strokeObjects.RemoveAt(0);
                    for (int i = 0; i < 5; i++)
                    {
                        ObjectPool.ObjectPool.GetInstance().SetBeschikbaar(dirt[0]);
                        dirt.RemoveAt(0);
                    }
                }
            }

            if (amount == 0)
            {
                GameObject fi = Instantiate(Finish);
                fi.transform.position = new Vector3(lastDisplacement, 0.2f, zPosition);
            }

            amount--;
        }

        private float Lerp(float start, float end, float i)
        {
            return start + (end - start) * i;
        }
    }
}
