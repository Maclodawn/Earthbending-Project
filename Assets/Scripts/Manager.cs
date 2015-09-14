using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public List<BasicRockBullet> m_bulletList { get; set; }

    void Start()
    {
        m_bulletList = new List<BasicRockBullet>();
    }

    void Update()
    {

    }
}
