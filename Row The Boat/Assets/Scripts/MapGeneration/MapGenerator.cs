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

        public GameObject EmptyMeshObject = null;
        public Material Dirt = null;
        public Material Water = null;
        public int SideWidth = 10;
        public int WaterWidth = 10;
        public int UncalculatedNormalSize = 10;

        public int PondWidth = 15;
        public int PondLength = 20;
        public int PondDirtLength = 5;
        public int TotalLength = 40;

        private int _zPosition = 0;
        private int _normalCounter = 10;
        private int _bushCounter = 5;

        private Mesh _mDirt;
        private Mesh _mWater;
        private GameObject _goDirt;
        private GameObject _goWater;

        private float _nextDisplacement;
        private float _lerpDisplacement = 1.0f;
        private float _startDisplacement = 0.0f;


        private bool _hasGenerated = false;

        private static MapGenerator instance;
        public static MapGenerator GetInstance()
        {
            return instance;
        }

        public void Start()
        {
            instance = this;
            this._normalCounter = UncalculatedNormalSize;
            
            Debug.Log("Generating map");
            //this.CreateWaterMesh();
            //this.CreateDirtMesh();

            //this.strokes = new List<WaterStroke>();
            //this.strokeObjects = new List<GameObject>();
            //this.dirt = new List<GameObject>();
            this._hasGenerated = true;

            // Setup the dirtMesh
            List<Vector3> v = new List<Vector3>();

            this._mDirt = new Mesh();
            this._mDirt.name = "Dirt";
            this._mDirt.Clear();
            this._mDirt.SetVertices(v);
            this._goDirt = Instantiate(EmptyMeshObject);
            MeshFilter mf = (MeshFilter)this._goDirt.gameObject.GetComponent(typeof(MeshFilter));
            MeshRenderer mr = (MeshRenderer)this._goDirt.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = this._mDirt;
            mr.material = Dirt;
            this._goDirt.transform.parent = this.transform;
            this._goDirt.name = "Dirt";



            // Setup the waterMesh
            v = new List<Vector3>();

            this._mWater = new Mesh();
            this._mWater.name = "Water";
            this._mWater.Clear();
            this._mWater.SetVertices(v);
            this._goWater = Instantiate(EmptyMeshObject);
            mf = (MeshFilter)this._goWater.gameObject.GetComponent(typeof(MeshFilter));
            mr = (MeshRenderer)this._goWater.gameObject.GetComponent(typeof(MeshRenderer));
            mf.mesh = this._mWater;
            mr.material = Water;
            this._goWater.transform.parent = this.transform;
            this._goWater.name = "Water";

            GeneratePond();
        }


        public void Update()
        {
            if (this.TotalLength > 0)
            {
                float displacement = GenerateMoat();

                this.TotalLength--;

                if (this.TotalLength == 0)
                {
                    GenerateReversePond(displacement);
                    this._mDirt.RecalculateNormals();
                    this._mWater.RecalculateNormals();
                }
            }
        }

        private float GetNextRandomValue(float min, float max)
        {
            //float value = UnityEngine.Random.Range(min, max - 1) + UnityEngine.Random.value;
            float value = SRandom.Get(1560).Random((float)min, (float)max);
            //while (value > max)
            //    value /= 10;
            return value;
        }

        private float Lerp(float start, float end, float i)
        {
            return start + (end - start) * i;
        }

        private float GenerateMoat()
        {
            float displacement = Lerp(this._startDisplacement, this._nextDisplacement, this._lerpDisplacement) + (GetNextRandomValue(0, 1) == 1 ? GetNextRandomValue(-0.25f, 0.25f) : 0);
            this._lerpDisplacement += 0.1f;
            if (this._lerpDisplacement >= 1.0f)
            {
                this._lerpDisplacement = 0;
                this._startDisplacement = this._nextDisplacement;
                this._nextDisplacement += GetNextRandomValue(-7.5f, 7.5f);
            }

            GenerateDirtAndWater(displacement, WaterWidth, SideWidth);

            return displacement;
        }

        private void GeneratePond()
        {
            for (int i = 0; i < PondDirtLength; i++)
            {
                GenerateDirtAndWater(0, 0, SideWidth);
            }

            int lerpMovement = (PondLength - 5) / 4;
            float lerp = 0;

            for (int i = 0; i < lerpMovement; i++)
            {
                GenerateDirtAndWater(GetNextRandomValue(-0.25f, 0.25f), (int)Lerp(3, PondWidth / 4 * 3, lerp), SideWidth);
                lerp += (1.0f / ((float)lerpMovement));
            }

            lerp = 0;
            for (int i = 0; i < lerpMovement; i++)
            {
                GenerateDirtAndWater(GetNextRandomValue(-0.25f, 0.25f), (int)Lerp(PondWidth / 4 * 3, PondWidth, lerp), SideWidth);
                lerp += (1.0f / ((float)lerpMovement));
            }

            for (int i = 0; i < 10; i++)
            {
                GenerateDirtAndWater(GetNextRandomValue(-0.005f, 0.005f), PondWidth, SideWidth);
            }

            lerp = 0;
            for (int i = 0; i < PondLength / 2; i++)
            {
                GenerateDirtAndWater(GetNextRandomValue(-0.25f, 0.25f), (int)Lerp(PondWidth, WaterWidth, lerp), SideWidth);
                lerp += (1.0f / ((float)PondLength / 2.0f));
            }
        }

        private void GenerateReversePond(float displacement)
        {
            int lerpMovement = (PondLength - 5) / 4;
            float lerp = 1;

            for (int i = 0; i < PondLength / 2; i++)
            {
                GenerateDirtAndWater(displacement + GetNextRandomValue(-0.25f, 0.25f), (int)Lerp(PondWidth, WaterWidth, lerp), SideWidth);
                lerp -= (1.0f / ((float)PondLength / 2.0f));
            }

            for (int i = 0; i < 10; i++)
            {
                GenerateDirtAndWater(displacement + GetNextRandomValue(-0.005f, 0.005f), PondWidth, SideWidth);
            }

            lerp = 1;


            for (int i = 0; i < lerpMovement; i++)
            {
                GenerateDirtAndWater(displacement + GetNextRandomValue(-0.25f, 0.25f), (int)Lerp(PondWidth / 4 * 3, PondWidth, lerp), SideWidth);
                lerp -= (1.0f / ((float)lerpMovement));
            }

            lerp = 1;
            for (int i = 0; i < lerpMovement; i++)
            {
                GenerateDirtAndWater(displacement + GetNextRandomValue(-0.25f, 0.25f), (int)Lerp(3, PondWidth / 4 * 3, lerp), SideWidth);
                lerp -= (1.0f / ((float)lerpMovement));
            }

            for (int i = 0; i < PondDirtLength; i++)
            {
                GenerateDirtAndWater(displacement, 0, SideWidth);
            }
        }

        private void GenerateDirtAndWater(float displacement, int water, int dirt)
        {
            List<Vector3> v = new List<Vector3>(this._mDirt.vertices);
            List<int> f = new List<int>(this._mDirt.triangles);

            this._zPosition++;

            //  0        1                6         7
            //  ---------\ 2           5 /----------
            //            \______|______/
            //            3            4

            v.Add(new Vector3(-dirt - water / 2 + displacement, 3.0f, this._zPosition));          // 0
            v.Add(new Vector3(-water / 2 - 2 + displacement, 0.75f, this._zPosition));          // 1
            if (water > 0)
            {
                v.Add(new Vector3(-water / 2 - 1 + displacement, 0.25f, this._zPosition));          // 2
                v.Add(new Vector3(-water / 2 + displacement, -1.0f, this._zPosition));          // 3
                v.Add(new Vector3(water / 2 + displacement, -1.0f, this._zPosition));          // 4
                v.Add(new Vector3(water / 2 + 1 + displacement, 0.25f, this._zPosition));          // 5
            }
            else
            {
                v.Add(new Vector3(-water / 2 - 1 + displacement, 0.75f, this._zPosition));          // 2
                v.Add(new Vector3(-water / 2 + displacement, 0.75f, this._zPosition));          // 3
                v.Add(new Vector3(water / 2 + displacement, 0.75f, this._zPosition));          // 4
                v.Add(new Vector3(water / 2 + 1 + displacement, 0.75f, this._zPosition));          // 5
            }
            v.Add(new Vector3(water / 2 + 1.5f + displacement, 0.75f, this._zPosition));          // 6
            v.Add(new Vector3(dirt + water / 2 + displacement, 3.0f, this._zPosition));          // 7

            if (v.Count > 16)
            {

                // 0 (0, 8, 1)
                f.Add(v.Count - 16);
                f.Add(v.Count - 8);
                f.Add(v.Count - 15);

                // 1 (1, 8, 9)
                f.Add(v.Count - 15);
                f.Add(v.Count - 8);
                f.Add(v.Count - 7);

                // 2 (1, 9, 2)
                f.Add(v.Count - 15);
                f.Add(v.Count - 7);
                f.Add(v.Count - 14);
                // 3 (2, 9, 10)
                f.Add(v.Count - 14);
                f.Add(v.Count - 7);
                f.Add(v.Count - 6);

                // 4 (2, 10, 3)
                f.Add(v.Count - 14);
                f.Add(v.Count - 6);
                f.Add(v.Count - 13);

                // 5 (3, 10, 11)
                f.Add(v.Count - 13);
                f.Add(v.Count - 6);
                f.Add(v.Count - 5);

                // 6 (3, 11, 4)
                f.Add(v.Count - 13);
                f.Add(v.Count - 5);
                f.Add(v.Count - 12);

                // 7 (4, 11, 12)
                f.Add(v.Count - 12);
                f.Add(v.Count - 5);
                f.Add(v.Count - 4);

                // 8 (4, 12, 5)
                f.Add(v.Count - 12);
                f.Add(v.Count - 4);
                f.Add(v.Count - 11);

                // 9 (5, 12, 13)
                f.Add(v.Count - 11);
                f.Add(v.Count - 4);
                f.Add(v.Count - 3);

                // 10 (5, 13, 6)
                f.Add(v.Count - 11);
                f.Add(v.Count - 3);
                f.Add(v.Count - 10);

                // 11 (6, 13, 14)
                f.Add(v.Count - 10);
                f.Add(v.Count - 3);
                f.Add(v.Count - 2);

                // 12 (6, 14, 7)
                f.Add(v.Count - 10);
                f.Add(v.Count - 2);
                f.Add(v.Count - 9);

                // 13 (7, 14, 15)
                f.Add(v.Count - 9);
                f.Add(v.Count - 2);
                f.Add(v.Count - 1);

            }

            this._mDirt.SetVertices(v);
            this._mDirt.SetTriangles(f, 0);
            if (this._normalCounter == 0)
                this._mDirt.RecalculateNormals();



            v = new List<Vector3>(this._mWater.vertices);
            f = new List<int>(this._mWater.triangles);

            v.Add(new Vector3(-water / 2 - 2.5f + displacement, 0.0f, this._zPosition));              // 0
            v.Add(new Vector3(UnityEngine.Random.Range(-water / 4, water / 4 - 1) + UnityEngine.Random.value + displacement, 0.25f, this._zPosition + 0.5f));       // 1
            v.Add(new Vector3(water / 2 + 2.5f + displacement, 0.0f, this._zPosition));              // 2


            if (v.Count > 3)
            {
                // 0 (0, 1, 2)
                f.Add(v.Count - 6);
                f.Add(v.Count - 5);
                f.Add(v.Count - 4);

                // 1 (0, 3, 1)
                f.Add(v.Count - 6);
                f.Add(v.Count - 3);
                f.Add(v.Count - 5);

                // 2 (3, 5, 1)
                f.Add(v.Count - 3);
                f.Add(v.Count - 1);
                f.Add(v.Count - 5);

                // 3 (5, 2, 1)
                f.Add(v.Count - 1);
                f.Add(v.Count - 4);
                f.Add(v.Count - 5);
            }


            this._mWater.SetVertices(v);
            this._mWater.SetTriangles(f, 0);
            if (this._normalCounter == 0)
            {
                this._mWater.RecalculateNormals();
                this._normalCounter = UncalculatedNormalSize;
            }
            else this._normalCounter--;


            if (water > 0)
            {
                // Add 2 capsule colliders
                CapsuleCollider cc = this._goDirt.AddComponent<CapsuleCollider>();
                cc.center = new Vector3(-water / 2 - 1 + displacement, 0, this._zPosition);
                cc.radius = 1.0f;
                cc.height = 1.0f;

                cc = this._goDirt.AddComponent<CapsuleCollider>();
                cc.center = new Vector3(water / 2 + 1 + displacement, 0, this._zPosition);
                cc.radius = 1.0f;
                cc.height = 1.0f;


                // Place a rock sometimes
                if (GetNextRandomValue(0, 5) == 1)
                {
                    GameObject rock = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Stone);
                    rock.transform.position = new Vector3(-water / 2 - 1 + displacement, 0, this._zPosition);
                    rock.transform.parent = this.transform;
                    rock.transform.localScale = new Vector3(1, 0.5f, 1);
                    rock.transform.localEulerAngles = new Vector3(0, GetNextRandomValue(0.0f, 360.0f), 0);
                }

                // Place a rock sometimes
                if (GetNextRandomValue(0, 5) == 1)
                {
                    GameObject rock = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Stone);
                    rock.transform.position = new Vector3(water / 2 + 1 + displacement, 0, this._zPosition);
                    rock.transform.parent = this.transform;
                    rock.transform.localScale = new Vector3(1, 0.5f, 1);
                    rock.transform.localEulerAngles = new Vector3(0, GetNextRandomValue(0.0f, 360.0f), 0);
                }
            }

            GameObject tree = null;
            if (GetNextRandomValue(0, 4) == 1)
            {
                tree = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                GameObject.Destroy(tree.GetComponent<CapsuleCollider>());
                tree.transform.position = new Vector3(-water / 2 - 1 + displacement - GetNextRandomValue(0.0f, SideWidth), 0, this._zPosition);
                tree.transform.parent = this.transform;
                tree.transform.localScale = new Vector3(3, 3, 3);
                tree.transform.localEulerAngles = new Vector3(0, GetNextRandomValue(0.0f, 360.0f), 0);
            }

            if (GetNextRandomValue(0, 3) == 1)
            {
                tree = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                GameObject.Destroy(tree.GetComponent<CapsuleCollider>());
                tree.transform.position = new Vector3(water / 2 + 1 + displacement + GetNextRandomValue(0.0f, SideWidth), 0, this._zPosition);
                tree.transform.parent = this.transform;
                tree.transform.localScale = new Vector3(3, 3, 3);
                tree.transform.localEulerAngles = new Vector3(0, GetNextRandomValue(0.0f, 360.0f), 0);
            }

            // Bush on the sides
            if (this._bushCounter == 0)
            {
                tree = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                GameObject.Destroy(tree.GetComponent<CapsuleCollider>());
                tree.transform.position = new Vector3(-water / 2 - 1 + displacement - SideWidth - GetNextRandomValue(0.0f, 5.0f), -4, this._zPosition);
                tree.transform.parent = this.transform;
                tree.transform.localScale = new Vector3(3, 3, 3);
                tree.transform.localEulerAngles = new Vector3(0, GetNextRandomValue(0.0f, 360.0f), 0);

                tree = ObjectPool.ObjectPool.GetInstance().GetObject(GameObjectType.Tree);
                GameObject.Destroy(tree.GetComponent<CapsuleCollider>());
                tree.transform.position = new Vector3(water / 2 + 1 + displacement + SideWidth + GetNextRandomValue(0.0f, 5.0f), -4, this._zPosition);
                tree.transform.parent = this.transform;
                tree.transform.localScale = new Vector3(3, 3, 3);
                tree.transform.localEulerAngles = new Vector3(0, GetNextRandomValue(0.0f, 360.0f), 0);

                this._bushCounter = 5;
            }
            else
                this._bushCounter--;
        }
    }
}
