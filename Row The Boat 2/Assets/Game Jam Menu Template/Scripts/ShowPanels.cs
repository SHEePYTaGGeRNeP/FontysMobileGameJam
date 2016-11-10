using UnityEngine;

namespace Assets.Game_Jam_Menu_Template.Scripts
{
    public class ShowPanels : MonoBehaviour {

        public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
        public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
        public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
        public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 


        //Call this function to activate and display the Options panel during the main menu
        public void ShowOptionsPanel()
        {
            this.optionsPanel.SetActive(true);
            this.optionsTint.SetActive(true);
        }

        //Call this function to deactivate and hide the Options panel during the main menu
        public void HideOptionsPanel()
        {
            this.optionsPanel.SetActive(false);
            this.optionsTint.SetActive(false);
        }

        //Call this function to activate and display the main menu panel during the main menu
        public void ShowMenu()
        {
            this.menuPanel.SetActive (true);
        }

        //Call this function to deactivate and hide the main menu panel during the main menu
        public void HideMenu()
        {
            this.menuPanel.SetActive (false);
        }
	
        //Call this function to activate and display the Pause panel during game play
        public void ShowPausePanel()
        {
            this.pausePanel.SetActive (true);
            this.optionsTint.SetActive(true);
        }

        //Call this function to deactivate and hide the Pause panel during game play
        public void HidePausePanel()
        {
            this.pausePanel.SetActive (false);
            this.optionsTint.SetActive(false);

        }
    }
}
