using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_originalPlayer;

    public List<BasicRockBullet> m_bulletList { get; set; }

    void Awake()
    {
        m_bulletList = new List<BasicRockBullet>();

        GameObject player = Instantiate(m_originalPlayer);
        player.GetComponent<Player_Movement>().init(this);
    }
}
