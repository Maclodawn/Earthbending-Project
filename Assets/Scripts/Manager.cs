using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_originalPlayer;

	public GameObject AIBody;
	public int nAI;

    void Awake()
    {
        GameObject player = Instantiate(m_originalPlayer);
        player.GetComponent<CharacterMovement>().init(this);

		for (int i = 0; i < nAI; ++i)
			Instantiate(AIBody);
    }
}
