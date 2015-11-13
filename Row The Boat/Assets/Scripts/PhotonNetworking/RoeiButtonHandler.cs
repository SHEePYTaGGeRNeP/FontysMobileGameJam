using UnityEngine;

namespace Assets.Scripts.PhotonNetworking
{
	class RoeiButtonHandler : MonoBehaviour
	{
		public PhotonRoeier Roeier;

		public void ButtonClick()
		{
			if (this.Roeier == null)
				if (!this.SetRoeier())
					return;
			this.Roeier.Roei(20f);
		}


		private bool SetRoeier()
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("PhotonPlayer"))
				if (go.GetComponent<PhotonRoeier>().PaddleViewId != 0)
				{
					Roeier = go.GetComponent<PhotonRoeier>();
					break;
				}
			if (Roeier == null)
				GameObject.Find("Push").SetActive(false);
			return Roeier != null;
		}

	}
}
