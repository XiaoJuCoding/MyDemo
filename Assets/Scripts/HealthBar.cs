using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Sli1,Sli2;
    public GameObject EnemyHealthFrame;

    public void Start()
    {
        EnemyHealthFrame.SetActive(false);
    }
    public void SetPlayerHealthBar(int health)
    {
        Sli1.value = health;
    }

    public void SetMaxEnemyHealthBar(int health)
    {
        Sli2.maxValue = health;
    }

    public void SetEnemyHealthBar(int health)
    {
        Sli2.value = health;
    }
}
