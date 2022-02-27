using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShippingPanel : MonoBehaviour
{
    private ShippingType shippingType;
    private Shipment shipment;
    public Shipment Shipment
    {
        get { return shipment; }
        set { shipment = value; SetButton();shippingType = ShippingType.None; }
    }

    //ref
    public GameObject butGroup;
    public Button butCar;
    public Button butTrain;
    public Button butBoat;
    public Button butPlane;
    [Space]
    public TextMeshProUGUI shipType;
    public TextMeshProUGUI destination;
    public TextMeshProUGUI time;
    public TextMeshProUGUI cost;
    [Space]
    public GameObject transportPanel;
    public GameObject transportCar;
    public GameObject transportTrain;
    public GameObject transportBoat;
    public GameObject transportPlane;
    [Space]
    public GameObject result;
    public Button confirmBut;
    public static RectTransform countrySelected;
    [Space]
    public Transform lineParent;
    public GameObject headLine;
    public GameObject line;
    public float speedLine = 0.07f;

    void SetButton()
    {
        butGroup.SetActive(true);
        butCar.gameObject.SetActive(true);
        butTrain.gameObject.SetActive(true);
        butBoat.gameObject.SetActive(true);
        butPlane.gameObject.SetActive(true);
        confirmBut.interactable = false;

        if (shipment.car == "-")
        {
            butCar.gameObject.SetActive(false);
        }
        if (shipment.train == "-")
        {
            butTrain.gameObject.SetActive(false);
        }
        if (shipment.boat == "-")
        {
            butBoat.gameObject.SetActive(false);
        }
        if (shipment.plane == "-")
        {
            butPlane.gameObject.SetActive(false);
        }
    }

    public void ShowCost(int index)
    {
        shippingType = (ShippingType)index;
        transportCar.SetActive(false);
        transportTrain.SetActive(false);
        transportBoat.SetActive(false);
        transportPlane.SetActive(false);
        confirmBut.interactable = true;

        switch (index)
        {
            case (int)ShippingType.Car:
                time.text = shipment.car.Split('d')[0];
                cost.text = shipment.car.Split(new char[] { '(', ')' })[1];
                shipType.text = "รถยนต์";
                transportCar.SetActive(true);
                break;

            case (int)ShippingType.Train:
                time.text = shipment.train.Split('d')[0];
                cost.text = shipment.train.Split(new char[] { '(', ')' })[1];
                shipType.text = "รถไฟ";
                transportTrain.SetActive(true);
                break;

            case (int)ShippingType.Boat:
                time.text = shipment.boat.Split('d')[0];
                cost.text = shipment.boat.Split(new char[] { '(', ')' })[1];
                shipType.text = "เรือ";
                transportBoat.SetActive(true);
                break;

            case (int)ShippingType.Plane:
                time.text = shipment.plane.Split('d')[0];
                cost.text = shipment.plane.Split(new char[] { '(', ')' })[1];
                shipType.text = "เครื่องบิน";
                transportPlane.SetActive(true);
                break;

            case (int)ShippingType.None:
                confirmBut.interactable = false;
                break;
        }

    }

    public void ConfrimShip()
    {
        if (shippingType == ShippingType.None)
            return;

        Supplies supplies = FindObjectOfType<Supplies>();
        supplies.comfrimShipping = true;
        transportPanel.SetActive(true);
        confirmBut.interactable = false;
    }

    public void Cancel()
    {
        result.SetActive(false);
    }

    public void SelectCountry(Shipment s)
    {
        Shipment = s;
        result.SetActive(true);
        destination.text = shipment.country;
        time.text = "-";
        cost.text = "-";
        shipType.text = "-";
       
        DrawLineNavigate(s.GetComponent<RectTransform>());
    }

    public void DrawLineNavigate(RectTransform end)
    {
        StopCoroutine(HeadLine());
        if (lineParent.childCount > 0)
        {
            for (int c = lineParent.childCount-1; c > 0; c--)
            {
                Destroy(lineParent.GetChild(c).gameObject);
            }
        }
        //thai -271, -211
        Vector3 start = new Vector3(-271, -211, 0);
        Vector3 middle = end.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D + end.anchoredPosition3D;
        Vector3[] path = Bezier_Curve.GetCurve(start, middle, end.anchoredPosition3D);
        for (int i = 0; i < path.Length; i++)
        {
            if (i > path.Length-2)
            {
                break ;
            }
            else
            {
                RectTransform clone = Instantiate(line, lineParent).GetComponent<RectTransform>();
                clone.localPosition = path[i];
                var m_Angle = Bezier_Curve.AngleBetweenTwoPoints(clone.localPosition, path[i + 1]);
                clone.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_Angle - 90));
            }
        }
        StartCoroutine(HeadLine());
    }

    IEnumerator HeadLine()
    {
        yield return null;
        RectTransform clone = Instantiate(headLine, lineParent).GetComponent<RectTransform>();
        int count = 0;
        while (lineParent.childCount > 0)
        {
            if (count > lineParent.childCount - 1)
            {
                count = 0;
            }
            if (lineParent.childCount > 0)
            {
                RectTransform rect = lineParent.GetChild(count).GetComponent<RectTransform>();
                if (rect != null && clone != null)
                {
                    clone.anchoredPosition = rect.anchoredPosition;
                    clone.rotation = rect.localRotation;
                }
            }
           
            count++;
            
            yield return new WaitForSeconds(speedLine);
        }
    }

}
