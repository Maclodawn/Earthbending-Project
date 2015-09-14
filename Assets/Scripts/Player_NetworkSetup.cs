using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

    [SerializeField]
    GameObject m_TPSCharacterCam;

	// Use this for initialization
	void Start ()
    {
	    if (isLocalPlayer)
        {
            //GetComponent<CharacterController>().enabled = true;
            GetComponent<Player_Movement>().enabled = true;

            m_TPSCharacterCam.GetComponent<Camera>().enabled = true;
            m_TPSCharacterCam.GetComponent<AudioListener>().enabled = true;
            m_TPSCharacterCam.GetComponent<Player_AimPoint>().enabled = true;
            m_TPSCharacterCam.GetComponent<Player_Look>().enabled = true;
        }
	}
}
