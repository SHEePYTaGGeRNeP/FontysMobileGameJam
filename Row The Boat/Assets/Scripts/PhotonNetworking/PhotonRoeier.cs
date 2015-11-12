using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.PhotonNetworking
{

	public class PhotonRoeier : MonoBehaviour
	{
		public Paddle Paddle;
	

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
				this.Roei(args.Strength * args.Efficiency);
			};
			GameObject.Find("Side").GetComponent<Text>().text = Paddle.RowSide.ToString();
			//this.Paddle = this._targetRPC.RPC("RequestPaddle", PhotonTargets.MasterClient, );
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				this.Roei(1f);
		}

		public void Roei(float force)
		{
			this._targetRPC.RPC("AddForce", PhotonTargets.MasterClient, this.Paddle.transform.position, force);
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
