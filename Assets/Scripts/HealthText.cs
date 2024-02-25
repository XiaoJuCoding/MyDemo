using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = Vector3.up;
    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;
    Color startColor;
    float elapsedTime;
    public float fadingTime;

    void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime;
        float newAlpha = startColor.a * (1 - (elapsedTime/fadingTime));
        if (elapsedTime < fadingTime)
        {
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
