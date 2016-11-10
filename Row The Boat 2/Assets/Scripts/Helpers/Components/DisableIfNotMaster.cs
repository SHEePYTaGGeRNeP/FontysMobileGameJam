namespace Assets.Scripts.Helpers.Components
{
    class DisableIfNotMaster : UnityEngine.MonoBehaviour
    {
        private void Awake()
        {
            if (!PhotonNetwork.isMasterClient)
                this.enabled = false;
        }

    }
}
