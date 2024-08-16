using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    [SerializeField]
    private GameObject restArea;

    [SerializeField]
    private Slider hpSlider;

    public void Show(bool display)
    {
        restArea.SetActive(display);
    }

    public void UpdateHealth(float value)
    {
        hpSlider.value = value;
    }
}
