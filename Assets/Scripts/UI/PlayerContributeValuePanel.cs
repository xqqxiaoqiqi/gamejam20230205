using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContributeValuePanel : MonoBehaviour
{
    public Slider slider;
    public Text total;
    public Text current;

    public void UpdateValue()
    {
        slider.value = (float) PlayerManager.instance.currContributeValue / PlayerManager.instance.maxContributeValue;
        total.text = PlayerManager.instance.maxContributeValue.ToString();
        current.text = PlayerManager.instance.currContributeValue.ToString();
    }
}
