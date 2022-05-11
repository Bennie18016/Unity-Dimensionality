using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float maxHealthBar;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health, PhotonView PV)
    {
        if(PV.IsMine){
            slider.value = health;
        }
    }
}
