using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkScript : MonoBehaviour
{
    public bool parked;
    public MoveAgent moveAgentScript;
    void start()
    {
        Collider m_Collider = GetComponent<Collider>(); ;

    }


    void OnTriggerStay(Collider other)
    {
        onTriggerStayAndEnter(other);

    }


    void onTriggerStayAndEnter(Collider other)
    {

        Collider m_Collider = GetComponent<Collider>();
        if (other.gameObject.name == "Bus_1")
        {

            if (moveAgentScript.parkingReached == false && m_Collider.bounds.Contains(other.gameObject.GetComponent<Collider>().bounds.min) && m_Collider.bounds.Contains(other.gameObject.GetComponent<Collider>().bounds.max))
            {
                moveAgentScript.parkingReached = true;
                moveAgentScript.Parked();

            }
        }
    }
}
