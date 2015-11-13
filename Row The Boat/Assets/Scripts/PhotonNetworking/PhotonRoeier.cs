using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.PhotonNetworking
{

	public class PhotonRoeier : MonoBehaviour
	{
		public int PaddleViewId;
	    public RowTiltController.RowSide Side { get; set; }

		private RowTiltController _rowController;
		private PhotonView _photonView;
		private PhotonView _targetRPC;

		void Start()
		{
			this._targetRPC = PhotonManager.Instance.GetComponent<PhotonView>();
			this._photonView = this.GetComponent<PhotonView>();
			this._rowController = this.GetComponent<RowTiltController>();
			this._rowController.Row += (sender, args) =>
			{
                if (args.Side == this.Side)
				    this.Roei(args.Strength * args.Efficiency);
			};
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				this.Roei(10f);
		}

		public void Roei(float force)
		{
			if (PaddleViewId == 0) return;
			this._targetRPC.RPC("AddForce", PhotonTargets.MasterClient, this.PaddleViewId, force);
		}

		// in an "observed" script:
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				Vector3 pos = transform.localPosition;
				stream.Serialize(ref pos);
			}
			else
			{
				Vector3 pos = Vector3.zero;
				stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
			}
		}
	}
}
