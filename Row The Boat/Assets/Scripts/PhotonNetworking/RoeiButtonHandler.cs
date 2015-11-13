using UnityEngine;

namespace Assets.Scripts.PhotonNetworking
{
	class RoeiButtonHandler : MonoBehaviour
	{
		public PhotonRoeier Roeier;

		public void ButtonClick()
		{
			if (this.Roeier == null) this.SetRoeier();
			this.Roeier.Roei(20f);
		}


		private void SetRoeier()
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("PhotonPlayer"))
				if (go.GetComponent<PhotonRoeier>().PaddleViewId != 0)
				{
					Roeier = go.GetComponent<PhotonRoeier>();
					break;
				}
		}

	}
}
