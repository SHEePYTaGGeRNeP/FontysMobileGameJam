using UnityEngine;

namespace Assets.Scripts.PhotonNetworking
{

	public class PhotonRoeier : MonoBehaviour
	{

		private PhotonManager _manager;

		public Transform Paddle;


		void Start()
		{
			this._manager = GameObject.Find("_PhotonManager").GetComponent<PhotonManager>();

		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				this.Roei();
		}

		public void Roei()
		{
			this._manager.Boot.AddForce(Paddle);
		}

	}
}
