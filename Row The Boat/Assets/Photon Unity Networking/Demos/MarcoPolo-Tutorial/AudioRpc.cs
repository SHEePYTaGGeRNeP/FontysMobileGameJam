using UnityEngine;

public class AudioRpc : Photon.MonoBehaviour
{

    public AudioClip marco;
    public AudioClip polo;

    AudioSource m_Source;

    void Awake()
    {
        this.m_Source = this.GetComponent<AudioSource>();
    }

    [PunRPC]
    void Marco()
    {
        if( !this.enabled )
        {
            return;
        }

        Debug.Log( "Marco" );

        this.m_Source.clip = this.marco;
        this.m_Source.Play();
    }

    [PunRPC]
    void Polo()
    {
        if( !this.enabled )
        {
            return;
        }

        Debug.Log( "Polo" );

        this.m_Source.clip = this.polo;
        this.m_Source.Play();
    }

    void OnApplicationFocus( bool focus )
    {
        this.enabled = focus;
    }
}
