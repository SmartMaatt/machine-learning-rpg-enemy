using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementBar : MonoBehaviour
{
	public Slider slider;
	public Image fill;
    public Image background;

    private Dictionary<CastSpell, Sprite> spriteByCastDict;
    private CastSpell currentSpell;

    public void SetupBarValues(float maxValue, float value, CastSpell spell)
	{
		slider.maxValue = maxValue;
		slider.value = value;

        currentSpell = spell;
        ChangeElement(spell);
	}

    public void SetupBarSprites(Sprite fire, Sprite water, Sprite snow)
    {
        spriteByCastDict = new Dictionary<CastSpell, Sprite>();
        spriteByCastDict.Add(CastSpell.FIRE, fire);
        spriteByCastDict.Add(CastSpell.WATER, water);
        spriteByCastDict.Add(CastSpell.SNOW, snow);
    }

    public void ChangeValue(float newValue)
	{
		slider.value = 1 - newValue;
	}

    public void ChangeElement(CastSpell spell)
    {
        fill.sprite = spriteByCastDict[spell];
        background.sprite = spriteByCastDict[spell];
    }
}
