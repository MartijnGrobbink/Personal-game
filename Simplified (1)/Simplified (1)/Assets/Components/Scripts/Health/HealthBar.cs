using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //source
    //https://www.youtube.com/watch?v=BLfNP4Sc_iA

    //get the slider
    public Slider slider;
    //set the max value of the slider
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    //set the currenthealth
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
