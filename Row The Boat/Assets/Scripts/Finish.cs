using UnityEngine;

namespace Assets.Scripts
{
    class Finish : MonoBehaviour
    {
        public void OnTriggerEnter(Collider collision)
        {
            if (this.name.ToUpper().Contains("AI"))
            {
                // AI heeft gewonnen
                Debug.Log("AI heeft gewonnen");
            }
            else
            {
                // Spelers hebben gewonnen
                Debug.Log("Spelers hebben gewonnen");
            }
        }
    }
}
