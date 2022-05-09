using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DayNight
{
    public class Day : MonoBehaviour
    {
        public float day;
        int maxDay;

        void Start()
        {
            maxDay = 5;
        }

        // void Update()
        // {
        //     if(day >= maxDay){
        //         Debug.Log("End Of Game");
        //     }
        // }
    }
}