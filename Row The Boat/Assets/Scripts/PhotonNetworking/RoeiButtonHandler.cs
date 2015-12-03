using UnityEngine;

namespace Assets.Scripts.PhotonNetworking
{
    public class RoeiButtonHandler : MonoBehaviour
	{
		public PhotonRoeier Roeier;

		public void ButtonClick()
		{
			if (this.Roeier == null)
				if (!this.SetRoeier())
					return;
			this.Roeier.Roei(20f);
		}


		public bool SetRoeier()
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("PhotonPlayer"))
				if (go.GetComponent<PhotonRoeier>().PaddleViewId != 0)
				{
				    this.Roeier = go.GetComponent<PhotonRoeier>();
					break;
				}
			if (this.Roeier == null)
				GameObject.Find("Push").SetActive(false);
			return this.Roeier != null;
		}

	}
}
