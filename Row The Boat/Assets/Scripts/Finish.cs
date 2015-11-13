using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Finish : MonoBehaviour
    {
        public void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.name.ToLower().Contains("ai"))
            {
                // AI heeft gewonnen
                Debug.Log("AI");
            }
            else
            {
                // Spelers hebben gewonnen
                Debug.Log("Spelers");
            }
        }
    }
}
