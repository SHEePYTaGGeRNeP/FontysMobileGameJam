using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Game_Jam_Menu_Template.Scripts
{
    public class SetAudioLevels : MonoBehaviour {

        public AudioMixer mainMixer;					//Used to hold a reference to the AudioMixer mainMixer


        //Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
        public void SetMusicLevel(float musicLvl)
        {
            this.mainMixer.SetFloat("musicVol", musicLvl);
        }

        //Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
        public void SetSfxLevel(float sfxLevel)
        {
            this.mainMixer.SetFloat("sfxVol", sfxLevel);
        }
    }
}
