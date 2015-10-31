using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_originalPlayer;

    void Awake()
    {
        GameObject player = Instantiate(m_originalPlayer);
        player.GetComponent<CharacterMovement>().init(this);
    }
}
