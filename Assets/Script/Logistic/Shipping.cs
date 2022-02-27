using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shipping : MonoBehaviour
{
    public ShippingPanel shippingPanel;

    Shipment shipment;

    private void Start()
    {
        if (shippingPanel.gameObject.activeInHierarchy)
        {

        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && gameObject.activeInHierarchy)
        {
            if (!shippingPanel.gameObject.activeInHierarchy)
                shippingPanel.gameObject.SetActive(true);

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000);
            if (hit.collider != null && hit.collider.GetComponent<Shipment>())
            {
                shipment = hit.collider.gameObject.GetComponent<Shipment>();
                shippingPanel.Shipment = shipment;
            }

        }
    }

}
