using Photon;
using UnityEngine;
using System.Collections;

public class DemoMecanimGUI : PunBehaviour
{
    #region Properties

    public GUISkin Skin;
    
    #endregion


    #region Members
    
    private PhotonAnimatorView m_AnimatorView;  // local animatorView. set when we create our character in CreatePlayerObject()
    private Animator m_RemoteAnimator;          // to display the synchronized values on the right side in the GUI. A third player will simply be ignored (until the second player leaves)

    private float m_SlideIn = 0f;
    private float m_FoundPlayerSlideIn = 0f;
    private bool m_IsOpen = false;

    #endregion


    #region Unity

    public void Awake()
    {

    }

    public void Update()
    {
        this.FindRemoteAnimator();

        this.m_SlideIn = Mathf.Lerp(this.m_SlideIn, this.m_IsOpen ? 1f : 0f, Time.deltaTime * 9f );
        this.m_FoundPlayerSlideIn = Mathf.Lerp(this.m_FoundPlayerSlideIn, this.m_AnimatorView == null ? 0f : 1f, Time.deltaTime * 5f );
    }

    /// <summary>Finds the Animator component of a remote client on a GameObject tagged as Player and sets m_RemoteAnimator.</summary>
    public void FindRemoteAnimator()
    {
        if(this.m_RemoteAnimator != null )
        {
            return;
        }

        // the prefab has to be tagged as Player
        GameObject[] gos = GameObject.FindGameObjectsWithTag( "Player" );
        for( int i = 0; i < gos.Length; ++i )
        {
            PhotonView view = gos[ i ].GetComponent<PhotonView>();
            if( view != null && view.isMine == false )
            {
                this.m_RemoteAnimator = gos[ i ].GetComponent<Animator>();
            }
        }
    }

    public void OnGUI()
    {
        GUI.skin = this.Skin;

        string[] synchronizeTypeContent = new string[] { "Disabled", "Discrete", "Continuous" };

        GUILayout.BeginArea( new Rect( Screen.width - 200 * this.m_FoundPlayerSlideIn - 400 * this.m_SlideIn, 0, 600, Screen.height ), GUI.skin.box );
        {
            GUILayout.Label( "Mecanim Demo", GUI.skin.customStyles[ 0 ] );

            GUI.color = Color.white;
            string label = "Settings";

            if(this.m_IsOpen == true )
            {
                label = "Close";
            }

            if( GUILayout.Button( label, GUILayout.Width( 110 ) ) )
            {
                this.m_IsOpen = !this.m_IsOpen;
            }

            string parameters = "";

            if(this.m_AnimatorView != null )
            {
                parameters += "Send Values:\n";

                for( int i = 0; i < this.m_AnimatorView.GetSynchronizedParameters().Count; ++i )
                {
                    PhotonAnimatorView.SynchronizedParameter parameter = this.m_AnimatorView.GetSynchronizedParameters()[ i ];
                    
                    try
                    {
                        switch( parameter.Type )
                        {
                        case PhotonAnimatorView.ParameterType.Bool:
                            parameters += parameter.Name + " (" + (this.m_AnimatorView.GetComponent<Animator>().GetBool( parameter.Name ) ? "True" : "False" ) + ")\n";
                            break;
                        case PhotonAnimatorView.ParameterType.Int:
                            parameters += parameter.Name + " (" + this.m_AnimatorView.GetComponent<Animator>().GetInteger( parameter.Name ) + ")\n";
                            break;
                        case PhotonAnimatorView.ParameterType.Float:
                            parameters += parameter.Name + " (" + this.m_AnimatorView.GetComponent<Animator>().GetFloat( parameter.Name ).ToString( "0.00" ) + ")\n";
                            break;
                        }
                    }
                    catch
                    {
                        Debug.Log( "derrrr for " + parameter.Name );
                    }
                }
            }

            if(this.m_RemoteAnimator != null )
            {
                parameters += "\nReceived Values:\n";

                for( int i = 0; i < this.m_AnimatorView.GetSynchronizedParameters().Count; ++i )
                {
                    PhotonAnimatorView.SynchronizedParameter parameter = this.m_AnimatorView.GetSynchronizedParameters()[ i ];

                    try
                    {
                        switch( parameter.Type )
                        {
                        case PhotonAnimatorView.ParameterType.Bool:
                            parameters += parameter.Name + " (" + (this.m_RemoteAnimator.GetBool( parameter.Name ) ? "True" : "False" ) + ")\n";
                            break;
                        case PhotonAnimatorView.ParameterType.Int:
                            parameters += parameter.Name + " (" + this.m_RemoteAnimator.GetInteger( parameter.Name ) + ")\n";
                            break;
                        case PhotonAnimatorView.ParameterType.Float:
                            parameters += parameter.Name + " (" + this.m_RemoteAnimator.GetFloat( parameter.Name ).ToString( "0.00" ) + ")\n";
                            break;
                        }
                    }
                    catch
                    {
                        Debug.Log( "derrrr for " + parameter.Name );
                    }
                }
            }

            GUIStyle style = new GUIStyle( GUI.skin.label );
            style.alignment = TextAnchor.UpperLeft;

            GUI.color = new Color( 1f, 1f, 1f, 1 - this.m_SlideIn );
            GUI.Label( new Rect( 10, 100, 600, Screen.height ), parameters, style );

            if(this.m_AnimatorView != null )
            {
                GUI.color = new Color( 1f, 1f, 1f, this.m_SlideIn );

                GUILayout.Space( 20 );
                GUILayout.Label( "Synchronize Parameters" );

                for( int i = 0; i < this.m_AnimatorView.GetSynchronizedParameters().Count; ++i )
                {
                    GUILayout.BeginHorizontal();
                    {
                        PhotonAnimatorView.SynchronizedParameter parameter = this.m_AnimatorView.GetSynchronizedParameters()[ i ];

                        GUILayout.Label( parameter.Name, GUILayout.Width( 100 ), GUILayout.Height( 36 ) );

                        int selectedValue = (int)parameter.SynchronizeType;
                        int newValue = GUILayout.Toolbar( selectedValue, synchronizeTypeContent );

                        if( newValue != selectedValue )
                        {
                            this.m_AnimatorView.SetParameterSynchronized( parameter.Name, parameter.Type, (PhotonAnimatorView.SynchronizeType)newValue );
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
        GUILayout.EndArea();
    }

    #endregion


    #region Photon

    public override void OnJoinedRoom()
    {
        this.CreatePlayerObject();
    }

    private void CreatePlayerObject()
    {
        Vector3 position = new Vector3( -2, 0, 0 );
        position.x += Random.Range( -3f, 3f );
        position.z += Random.Range( -4f, 4f );

        GameObject newPlayerObject = PhotonNetwork.Instantiate( "Robot Kyle Mecanim", position, Quaternion.identity, 0 );
        this.m_AnimatorView = newPlayerObject.GetComponent<PhotonAnimatorView>();
    }

    #endregion
}
