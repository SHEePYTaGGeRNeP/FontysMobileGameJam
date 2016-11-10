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
	    public bool LocalRoeier { get; set; }

		private PaddleSoundController _paddleSoundController;

	    public Camera RoeierCamera;

	    private void Start()
		{
			this._targetRPC = PhotonManager.Instance.GetComponent<PhotonView>();
			this._photonView = this.GetComponent<PhotonView>();
			this._rowController = this.GetComponent<RowTiltController>();
			this._paddleSoundController = this.GetComponent<PaddleSoundController> ();            
            this.transform.GetChild(0).gameObject.SetActive(!PhotonNetwork.isMasterClient);
	        this.transform.SetParent(GameObject.Find("Boat_Mobile_Roeien(Clone)").transform);
            this._rowController.Row += (sender, args) =>
			{
                if (args.Side == this.Side)
				    this.Roei(args.Strength * args.Efficiency);
			};
		}

	    private void Update()
	    {
	        if (this.RoeierCamera.gameObject.activeSelf && !this.LocalRoeier)
	        {
                Helpers.LogHelper.WriteErrorMessage(typeof(PhotonRoeier),"Update", "Disabled Paddle in update");
	            this.RoeierCamera.gameObject.SetActive(false);
	        }
	        if (Input.GetKeyDown(KeyCode.Space))
				this.Roei(20f);
		}

		public void Roei(float force)
		{
			if (this.PaddleViewId == 0) return;
		    this._paddleSoundController.PlayRandomPaddleSound ();
			this._targetRPC.RPC("AddForce", PhotonTargets.MasterClient, this.PaddleViewId, force);
		}

		// in an "observed" script:
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				Vector3 pos = this.transform.localPosition;
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
