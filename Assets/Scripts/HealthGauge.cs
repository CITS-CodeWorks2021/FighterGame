using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGauge : MonoBehaviour
{
    public int maxHealth, curHealth;
    public Image imageGauge;

    public void SetStartHealth(int howMuch)
    {
        maxHealth = howMuch;
        curHealth = howMuch;
        UpdateGauge(curHealth);
    }

    public void UpdateGauge(int newHealth)
    {
        curHealth = newHealth;
        float perc = (float)curHealth / maxHealth;
        imageGauge.fillAmount = perc;
    }
}
