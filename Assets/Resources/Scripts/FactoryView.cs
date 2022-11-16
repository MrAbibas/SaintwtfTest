using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryView : MonoBehaviour
{
    [SerializeField]
    private Image image;
    internal void UpdateProgres(float progres)
    {
        image.fillAmount = progres;
    }
}
