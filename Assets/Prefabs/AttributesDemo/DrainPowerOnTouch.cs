using UnityEngine;
using System.Collections;

public class DrainPowerOnTouch : MonoBehaviour {

    public float Power = 25;

    void OnTriggerEnter(Collider collider)
    {
        PowerComponent power = collider.gameObject.GetComponent<PowerComponent>();
        if(power != null)
        {
            power.Power -= Power;
        }
    }
}
