using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Shipment : MonoBehaviour
{
    public string country;
    public string car;
    public string train;
    public string boat;
    public string plane;
    [Space]
    public float maxScale = 2;
    public float duration = 1;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }

    public void onClick()
    {
        if (ShippingPanel.countrySelected == null)
        {
            gameObject.transform.DOScale(maxScale, duration);
            ShippingPanel.countrySelected = GetComponent<RectTransform>();
        }
        else if (ShippingPanel.countrySelected.name != gameObject.name)
        {
            RectTransform temp = ShippingPanel.countrySelected;
            temp.DOScale(1, duration);
            gameObject.transform.DOScale(maxScale, duration);
            ShippingPanel.countrySelected = GetComponent<RectTransform>();
        }
        transform.SetSiblingIndex(transform.parent.childCount -1);
    }

    public void Next()
    {

    }
}
