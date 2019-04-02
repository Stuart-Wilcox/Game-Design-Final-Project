using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Character character;
    public Transform healthBarValue;
    public float maxValue = 0.035f;

    public void Update()
    {
        // determine the percentage of health
        float factor = (float)this.character.hp / (float)this.character.maxHp;
        // set the health bar value by percentage
        this.SetValue(factor);
    }

    public void SetValue(float factor)
    {
        // scale the factor to work for the health bar size
        float scale;
        if (factor != 0)
        {
            scale = Mathf.Max(0.001f, factor * this.maxValue);
        }
        else {
            scale = 0.0f;
        }
        
        // set the scale (shrinks health bar)
        this.healthBarValue.localScale = new Vector3(scale, 0.005f, 1);
    }
}
