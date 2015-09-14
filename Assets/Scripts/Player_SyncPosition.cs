using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour
{
    [SyncVar]
    private Vector3 m_syncPos;

    [SerializeField]
    Transform m_myTransform;

    [SerializeField]
    float m_lerpRate = 15;

    Vector3 m_lastPos;
    
    [SerializeField]
    float m_threashold = 0.0f;
	
	// Update is called once per frame
    void Update()
    {
        LerpPosition();
    }

	void FixedUpdate ()
    {
        TransmitPosition();
	}

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            m_myTransform.position = Vector3.Lerp(m_myTransform.position, m_syncPos, Time.deltaTime * m_lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        m_syncPos = pos;
        Debug.Log("cmd called");
    }

    [ClientCallback]
    void TransmitPosition ()
    {
        if (isLocalPlayer && Vector3.Distance(m_lastPos, m_myTransform.position) > m_threashold)
        {
            CmdProvidePositionToServer(m_myTransform.position);
            m_lastPos = m_myTransform.position;
        }
    }
}
