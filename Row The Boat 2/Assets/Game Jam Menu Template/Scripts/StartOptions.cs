using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Game_Jam_Menu_Template.Scripts
{
    public class StartOptions : MonoBehaviour {



        public int sceneToStart = 1;										//Index number in build settings of scene to load if changeScenes is true
        public bool changeScenes;											//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
        public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip


        [HideInInspector] public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
        public Animator animColorFade; 					//Reference to animator which will fade to and from black when starting game.
        public Animator animMenuAlpha;					//Reference to animator that will fade out alpha of MenuPanel canvas group
        public AnimationClip fadeColorAnimationClip;		//Animation clip fading to color (black default) when changing scenes
        [HideInInspector] public AnimationClip fadeAlphaAnimationClip;		//Animation clip fading out UI elements alpha


        private PlayMusic playMusic;										//Reference to PlayMusic script
        private float fastFadeIn = .01f;									//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
        private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels

	
        void Awake()
        {
            //Get a reference to ShowPanels attached to UI object
            this.showPanels = this.GetComponent<ShowPanels> ();

            //Get a reference to PlayMusic attached to UI object
            this.playMusic = this.GetComponent<PlayMusic> ();
        }


        public void StartButtonClicked()
        {
            //If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
            //To change fade time, change length of animation "FadeToColor"
            if (this.changeMusicOnStart) 
            {
                this.playMusic.FadeDown(this.fadeColorAnimationClip.length);
            }

            //If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
            if (this.changeScenes) 
            {
                //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
                this.Invoke ("LoadDelayed", this.fadeColorAnimationClip.length * .5f);

                //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
                this.animColorFade.SetTrigger ("fade");
            } 

            //If changeScenes is false, call StartGameInScene
            else 
            {
                //Call the StartGameInScene function to start game without loading a new scene.
                this.StartGameInScene();
            }

        }

        //Once the level has loaded, check if we want to call PlayLevelMusic
        void OnLevelWasLoaded()
        {
            //if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
            if (this.changeMusicOnStart)
            {
                this.playMusic.PlayLevelMusic ();
            }	
        }


        public void LoadDelayed()
        {
            //Pause button now works if escape is pressed since we are no longer in Main menu.
            this.inMainMenu = false;

            //Hide the main menu UI element
            this.showPanels.HideMenu ();

            //Load the selected scene, by scene index number in build settings
            SceneManager.LoadScene (this.sceneToStart);
        }

        public void HideDelayed()
        {
            //Hide the main menu UI element after fading out menu for start game in scene
            this.showPanels.HideMenu();
        }

        public void StartGameInScene()
        {
            //Pause button now works if escape is pressed since we are no longer in Main menu.
            this.inMainMenu = false;

            //If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
            //To change fade time, change length of animation "FadeToColor"
            if (this.changeMusicOnStart) 
            {
                //Wait until game has started, then play new music
                this.Invoke ("PlayNewMusic", this.fadeAlphaAnimationClip.length);
            }
            //Set trigger for animator to start animation fading out Menu UI
            this.animMenuAlpha.SetTrigger ("fade");
            this.Invoke("HideDelayed", this.fadeAlphaAnimationClip.length);
            Debug.Log ("Game started in same scene! Put your game starting stuff here.");
        }


        public void PlayNewMusic()
        {
            //Fade up music nearly instantly without a click 
            this.playMusic.FadeUp (this.fastFadeIn);
            //Play music clip assigned to mainMusic in PlayMusic script
            this.playMusic.PlaySelectedMusic (1);
        }
    }
}
