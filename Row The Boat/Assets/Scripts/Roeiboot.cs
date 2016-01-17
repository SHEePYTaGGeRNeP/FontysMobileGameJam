using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.PhotonNetworking;

using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Roeiboot : MonoBehaviour
    {
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private List<Paddle> _paddles = new List<Paddle>();
        public IList<Paddle> FreePaddles { get { return (from p in this._paddles where !p.Taken select p).ToList(); } }

        public bool Photon;

        [SerializeField]
        private Transform _achter;
        private RowTiltController _rowController;

        private Rigidbody _rb;

        public float ForceMultiplier = 2;

        private Paddle _nextPaddle;
        public Paddle NextPaddle
        {
            get
            {
                if (this._nextPaddle == null)
                    this.SetNextPaddle();
                return this._nextPaddle;
            }
        }

        // Use this for initialization
        private void Start()
        {
            this._rb = this.GetComponent<Rigidbody>();

            this._rowController = this.GetComponent<RowTiltController>();
            if (this._rowController != null)
            {
                Debug.Log("Added row controls!");
                this._rowController.Row += (sender, args) =>
                    {
                        //if (args.Side != RowTiltController.RowSide.Left) return;
                        this.AddForce(this._achter.position, args.Strength * args.Efficiency);
                    };
            }
            if (!PhotonNetwork.isMasterClient)
            {
                PhotonManager.Instance.Boot = this;
            }
        }

        private void FixedUpdate()
        {
            if (this.Photon) return;
            if (Input.GetKeyDown(KeyCode.A))
            {
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._paddles[3].transform.position);
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._paddles[2].transform.position);
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._paddles[1].transform.position);
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._paddles[0].transform.position);
                this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
            }

        }

        public void AddForce(Vector3 paddle, float force = 1f)
        {
            this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier, paddle);
            this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier, this._achter.position);
        }


        private void SetNextPaddle()
        {
            if (this.FreePaddles.Count == 0 && this._nextPaddle != null)
                this._nextPaddle = null;
            else
            {
                IList<Paddle> paddlesLeft = (from p in this.FreePaddles where p.RowSide == RowTiltController.RowSide.Left select p).ToList();
                IList <Paddle> paddlesRight = (from p in this.FreePaddles where p.RowSide == RowTiltController.RowSide.Right select p).ToList();
                if (paddlesLeft.Count < paddlesRight.Count)
                    this._nextPaddle = paddlesRight[Random.Range(0, paddlesRight.Count)];
                else if (paddlesLeft.Count > paddlesRight.Count)
                    this._nextPaddle = paddlesLeft[Random.Range(0, paddlesLeft.Count)];
                else
                    this._nextPaddle = this.FreePaddles[Random.Range(0, this.FreePaddles.Count)];
            }
        }

        public void RemovePlayer(PhotonRoeier player)
        {
            Paddle freePaddle = PhotonView.Find(player.PaddleViewId).gameObject.GetComponent<Paddle>();
            this._paddles.Add(freePaddle);
        }

        public void AssignPlayer(PhotonRoeier player)
        {
            Paddle nextPaddle = this.NextPaddle;
            player.PaddleViewId = nextPaddle.gameObject.GetPhotonView().viewID;
            this._paddles.Remove(nextPaddle);
            this._nextPaddle = null;
        }

    }
}
