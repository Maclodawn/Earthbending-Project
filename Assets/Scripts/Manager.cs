﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_originalPlayer;

    public Terrain m_terrain;

    private static Manager m_managerInstance = null;

    public static Manager getManager()
    {
        if (m_managerInstance)
            return m_managerInstance;

        m_managerInstance = FindObjectOfType<Manager>();
        return m_managerInstance;
    }

    void Awake()
    {
        GameObject player = Instantiate(m_originalPlayer);
        player.GetComponent<CharacterMovement>().init(this);
    }

    void Update()
    {
        //ShowSize.PrintShowSize();
    }
}
