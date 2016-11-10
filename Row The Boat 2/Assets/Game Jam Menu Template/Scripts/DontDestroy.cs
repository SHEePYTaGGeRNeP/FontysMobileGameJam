using UnityEngine;

namespace Assets.Game_Jam_Menu_Template.Scripts
{
    public class DontDestroy : MonoBehaviour {

        void Start()
        {
            //Causes UI object not to be destroyed when loading a new scene. If you want it to be destroyed, destroy it manually via script.
            DontDestroyOnLoad(this.gameObject);
        }

	

    }
}
