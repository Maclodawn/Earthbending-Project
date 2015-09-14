using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncRotation : NetworkBehaviour
{

    [SyncVar]
    Quaternion m_syncPlayerRotation;

    [SyncVar]
    Quaternion m_syncCamRotation;

    [SerializeField]
    Transform m_playerTransform;

    [SerializeField]
    Transform m_camTransform;

    [SerializeField]
    float lerpRate = 15;

    Quaternion m_lastPlayerRot;
    Quaternion m_lastCamRot;
    
    [SerializeField]
    float m_threashold = 5;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
    void Update()
    {
        LerpRotations();
    }

	void FixedUpdate ()
    {
        TransmitRotations();
	}

    void LerpRotations()
    {
        if (isLocalPlayer)
        {
            m_playerTransform.rotation = Quaternion.Lerp(m_playerTransform.rotation, m_syncPlayerRotation, Time.deltaTime);
            m_camTransform.rotation = Quaternion.Lerp(m_camTransform.rotation, m_syncCamRotation, Time.deltaTime);
        }
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRot, Quaternion camRot)
    {
        m_syncPlayerRotation = playerRot;
        m_syncCamRotation = camRot;
        Debug.Log("cmd called");
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer && (Quaternion.Angle(m_lastCamRot, m_camTransform.rotation) > m_threashold
                             || Quaternion.Angle(m_lastPlayerRot, m_playerTransform.rotation) > m_threashold))
        {
            CmdProvideRotationsToServer(m_playerTransform.rotation, m_camTransform.rotation);
            m_lastPlayerRot = m_playerTransform.rotation;
            m_lastCamRot = m_camTransform.rotation;
        }
    }
}
