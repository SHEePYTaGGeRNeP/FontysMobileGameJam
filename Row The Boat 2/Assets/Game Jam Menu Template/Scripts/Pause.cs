using UnityEngine;

namespace Assets.Game_Jam_Menu_Template.Scripts
{
    public class Pause : MonoBehaviour {


        private ShowPanels showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
        private bool isPaused;								//Boolean to check if the game is paused or not
        private StartOptions startScript;					//Reference to the StartButton script
	
        //Awake is called before Start()
        void Awake()
        {
            //Get a component reference to ShowPanels attached to this object, store in showPanels variable
            this.showPanels = this.GetComponent<ShowPanels> ();
            //Get a component reference to StartButton attached to this object, store in startScript variable
            this.startScript = this.GetComponent<StartOptions> ();
        }

        // Update is called once per frame
        void Update () {

            //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
            if (Input.GetButtonDown ("Cancel") && !this.isPaused && !this.startScript.inMainMenu) 
            {
                //Call the DoPause function to pause the game
                this.DoPause();
            } 
            //If the button is pressed and the game is paused and not in main menu
            else if (Input.GetButtonDown ("Cancel") && this.isPaused && !this.startScript.inMainMenu) 
            {
                //Call the UnPause function to unpause the game
                this.UnPause ();
            }
	
        }


        public void DoPause()
        {
            //Set isPaused to true
            this.isPaused = true;
            //Set time.timescale to 0, this will cause animations and physics to stop updating
            Time.timeScale = 0;
            //call the ShowPausePanel function of the ShowPanels script
            this.showPanels.ShowPausePanel ();
        }


        public void UnPause()
        {
            //Set isPaused to false
            this.isPaused = false;
            //Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
            Time.timeScale = 1;
            //call the HidePausePanel function of the ShowPanels script
            this.showPanels.HidePausePanel ();
        }


    }
}
