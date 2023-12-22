using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillAmoutGuage : MonoBehaviour
{
    [SerializeField] private Image variable;
    [SerializeField] private float ratio;
    void Start()
    {
        if (variable == null) { GetComponent<Image>(); }
    }

    void Update()
    {
        variable.fillAmount = ratio;
    }

    public void OnChangeValue(float ratio)
    {
        this.ratio = ratio;
    }
}
