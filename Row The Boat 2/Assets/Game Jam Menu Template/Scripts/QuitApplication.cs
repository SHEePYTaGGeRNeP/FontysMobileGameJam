using UnityEngine;

namespace Assets.Game_Jam_Menu_Template.Scripts
{
    public class QuitApplication : MonoBehaviour
    {

        public void Quit()
        {
            //If we are running in the editor
#if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        }
    }
}
