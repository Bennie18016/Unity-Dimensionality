using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController{
    public class Water : MonoBehaviour
{
       public GameObject blueFilter;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<Movement>() != null){
            Movement movement = other.GetComponent<Movement>();
            movement.isSwiming = true;
                blueFilter.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<Movement>() != null){
            Movement movement = other.GetComponent<Movement>();
            movement.isSwiming = false;
            blueFilter.SetActive(false);
        }
    }
}
}
