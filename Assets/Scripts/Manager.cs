using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_originalPlayer;

    [SerializeField]
    HealthBarController m_healthBar;

    [SerializeField]
    PowerBarController m_powerBar;

    [SerializeField]
    ChargeBarController m_chargeBar;

    void Awake()
    {
        GameObject player = Instantiate(m_originalPlayer);
        player.GetComponent<CharacterMovement>().init(this);

        if(m_healthBar != null)
        {
            m_healthBar.Setup(player.GetComponent<HealthComponent>());
        }

        if(m_powerBar != null)
        {
            m_powerBar.Setup(player.GetComponent<PowerComponent>());
        }

        if(m_chargeBar != null)
        {
            m_chargeBar.Setup(player.GetComponent<SpellChargingComponent>());
        }
    }
}
