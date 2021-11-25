using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueBar : MonoBehaviour
{
	public Slider slider;
	public Image fill;
    public TMP_Text barInfoText;

    public void SetupBar(float maxValue, float value)
	{
		slider.maxValue = maxValue;
		slider.value = value;
        barInfoText.text = GenerateBarText(maxValue, value);
	}

    public void ChangeValue(float newValue)
	{
		slider.value = newValue;
        barInfoText.text = GenerateBarText(slider.maxValue, slider.value);
	}

    private string GenerateBarText(float maxValue, float value)
    {
        return (int)value + " / " + (int)maxValue;
    }
}
