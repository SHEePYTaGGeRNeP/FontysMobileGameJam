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

        public int MapLengte = 20;

        private List<WaterStroke> strokes;
        private List<GameObject> strokeObjects;
        private List<GameObject> dirt;

        private bool generateObstacle = false;
        private bool hasGenerated = false;

        private static MapGenerator instance;
        public static MapGenerator GetInstance()
        {
            return instance;
        }

        public void Start()
        {
            instance = this;            
        }

        public void Generate()
        {
            // alleen de master client moet generen
            if (!PhotonNetwork.isMasterClient) return;
            Debug.Log("Generating map");
            this.CreateWaterMesh();
            this.CreateDirtMesh();

            this.strokes = new List<WaterStroke>();
            this.strokeObjects = new List<GameObject>();
            this.dirt = new List<GameObject>();
            this.hasGenerated = true;
        }


        private void CreateDirtMesh()
        {
            // THIS NEEDS TO CHANGE
            Mesh mesh = new Mesh();
            mesh.name = "DirtMesh";
            mesh.Clear();
            List<Vector3> vertices = new List<Vector3>();

            //vertices.Add(new Vector3(WaterWidth, 0, 1));
            //vertices.Add(new Vector3(WaterWidth, 0, -1));
            //vertices.Add(new Vector3(-WaterWidth, 0, -1));
            //vertices.Add(new Vector3(-WaterWidth, 0, 1));

            vertices.Add(new Vector3(this.WaterWidth, -1, 1));
            vertices.Add(new Vector3(this.WaterWidth, -1, -1));
            vertices.Add(new Vector3(-this.WaterWidth, -1, -1));
            vertices.Add(new Vector3(-this.WaterWidth, -1, 1));

            vertices.Add(new Vector3(this.WaterWidth - 1, -1, 1));
            vertices.Add(new Vector3(this.WaterWidth - 1, -1, -1));
            vertices.Add(new Vector3(-this.WaterWidth + 1, -1, -1));
            vertices.Add(new Vector3(-this.WaterWidth + 1, -1, 1));

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

            MeshFilter mf = (MeshFilter)this.DirtObject.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)this.DirtObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            this.DirtObject.name = "Dirt";
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

            MeshFilter mf = (MeshFilter)this.WaterObject.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)this.WaterObject.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = mesh;

            this.WaterObject.name = "Water";
            Water w = this.WaterObject.GetComponent<Water>();
            w.mesh = mesh;
        }

        public void Update()
        {
            //Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z + 0.25f);

            if (!PhotonNetwork.isMasterClient || !this.hasGenerated) return;

            if (this.strokes.Count != 30)
            {
                GameObject parent = new GameObject();
                parent.name = "WaterStroke";
                parent.transform.parent = this.transform;
                this.strokeObjects.Add(parent);

                //WaterStroke ws = new WaterStroke(5, zPosition, ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Dirt), lastWaterBlock);
                WaterStroke ws = new WaterStroke(5, this.zPosition, this.DirtSideObject, this.lastWaterBlock, parent.transform);
                for (int row = 0; row < 5; row++)
                {
                    GameObject dirt = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Dirt);
                    dirt.transform.position = new Vector3(-1 + this.lastDisplacement, 0, this.zPosition);
                    dirt.transform.parent = parent.transform;
                    this.dirt.Add(dirt);

                    for (int i = 0; i < this.WaterWidth; i++)
                    {
                        GameObject water = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Water);
                        water.transform.position = new Vector3(-this.WaterWidth + (i * 2) + this.lastDisplacement, 0, this.zPosition);
                        water.transform.parent = parent.transform;

                        ws.AddWater(water, row);
                    }

                    CapsuleCollider bc = dirt.AddComponent<CapsuleCollider>();
                    bc.center = new Vector3(-(this.WaterWidth - 1.25f), 0, 0);
                    //bc.size = new Vector3(2, 1, 2);
                    bc.height = 1;
                    bc.radius = 1.5f;

                    bc = dirt.AddComponent<CapsuleCollider>();
                    bc.center = new Vector3(this.WaterWidth - 1.25f, 0, 0);
                    //bc.size = new Vector3(2, 1, 2);
                    bc.height = 1;
                    bc.radius = 1.5f;

                    this.zPosition += 2;

                    if (this.displacementInterpolation >= 1)
                    {
                        this.displacementValue += UnityEngine.Random.Range(-100, 100) / 10;
                        this.displacementInterpolation = 0;
                    }

                    this.lastDisplacement = this.Lerp(this.lastDisplacement, this.displacementValue, this.displacementInterpolation);
                    this.displacementInterpolation += 0.2f;
                }

                // Generate an obstacle
                if (this.generateObstacle)
                {
                    List<GameObjectType> obsta = new List<GameObjectType>();
                    obsta.Add(GameObjectType.Stone);
                    obsta.Add(GameObjectType.Tree);

                    GameObjectType type = obsta[UnityEngine.Random.Range(0, obsta.Count)];

                    GameObject go = ObjectPool.ObjectPool.GetInstance().GetObject(type);
                    if (type == GameObjectType.Stone)
                    {
                        go.transform.position = new Vector3(this.lastDisplacement + UnityEngine.Random.Range(-this.WaterWidth, this.WaterWidth), -1, this.zPosition);
                    }
                    else if (type == GameObjectType.Tree)
                    {
                        int dist = UnityEngine.Random.Range(-25, 25);
                        go.transform.position = new Vector3(this.lastDisplacement + dist, -1, this.zPosition);

                        go.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-45, 45));
                    }
                    go.transform.parent = parent.transform;
                    ws.AddObstacle(go);
                }

                this.generateObstacle = !this.generateObstacle;

                // Generate decoration

                for (int i = 0; i < UnityEngine.Random.Range(2, 4); i++)
                {
                    GameObject go = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                    go.transform.position = new Vector3(this.lastDisplacement + UnityEngine.Random.Range(-this.WaterWidth * 3, -this.WaterWidth), go.transform.position.y, this.zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
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
                    go.transform.position = new Vector3(this.lastDisplacement + UnityEngine.Random.Range(this.WaterWidth, this.WaterWidth * 3), go.transform.position.y, this.zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
                    go.transform.parent = parent.transform;
                    ws.AddObstacle(go);

                    //go = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Decoration);
                    //go.transform.position = new Vector3(lastDisplacement + UnityEngine.Random.Range(WaterWidth, WaterWidth * 3), 1, zPosition + UnityEngine.Random.Range(-2, 1) + UnityEngine.Random.value);
                    //go.transform.parent = parent.transform;
                    //ws.AddObstacle(go);
                }



                ws.GenerateSides(parent.transform);

                this.strokes.Add(ws);

                this.lastWaterBlock = ws;

                this.MapLengte--;
            }

            if (this.strokes.Count > 0)
            {
                if (this.strokes[0].ZPosition + 20 * 2 < this.Player.transform.position.z)
                {
                    this.strokes[0].Destroy();
                    this.strokes.RemoveAt(0);
                    this.strokeObjects.RemoveAt(0);
                    for (int i = 0; i < 5; i++)
                    {
                        ObjectPool.ObjectPool.GetInstance().SetBeschikbaar(this.dirt[0]);
                        this.dirt.RemoveAt(0);
                    }
                }
            }

            if (this.MapLengte == 0)
            {
                GameObject fi = Instantiate(this.Finish);
                fi.transform.position = new Vector3(this.lastDisplacement, 0.2f, this.zPosition);
                this.MapLengte = -1;
            }

            
        }

        private float Lerp(float start, float end, float i)
        {
            return start + (end - start) * i;
        }
    }
}
