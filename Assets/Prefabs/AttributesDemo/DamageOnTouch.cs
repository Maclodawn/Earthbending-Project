using UnityEngine;
using System.Collections;

public class DamageOnTouch : MonoBehaviour {

    public float Damage = 25;

    void OnTriggerEnter(Collider collider)
    {
        HealthComponent health = collider.gameObject.GetComponent<HealthComponent>();
        if(health != null)
        {
            health.Health -= Damage;
        }
    }
}
