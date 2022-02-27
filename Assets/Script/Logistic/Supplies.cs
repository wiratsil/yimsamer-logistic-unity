using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxTypes
{
    None,
    Normal,
    Fragile,
    Freeze
}

public enum ShippingType
{
    None,Car,Train,Boat,Plane
}
public class Supplies : MonoBehaviour
{
    public BoxTypes myType;
    [Space]
    public BoxTypes type_1;
    public BoxTypes type_2;
    public BoxTypes type_3;

    [Space]
    public Sprite boxNormal;
    public Sprite boxFragile;
    public Sprite boxFreeze;

    [Space]
    public List<Symbol> symbols;
    [Space]
    public bool correntType;
    public bool correntSymbol;
    public bool comfrimShipping;

    public string countryShip;
    public string costShip;
    public ShippingType shippingType;

    public SpriteRenderer label1;
    public SpriteRenderer label2;
    public SpriteRenderer label3;
    public SpriteRenderer label4;
    public SpriteRenderer label5;
    public SpriteRenderer label6;
    [Space]
    public Sprite itemInBox;
    public string productName;
    public Box box;

    private void Start()
    {
        SpriteRenderer[] sprites = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        label1 = sprites[0];
        label2 = sprites[1];
        label3 = sprites[2];
        label4 = sprites[3];
        label5 = sprites[4];
        label6 = sprites[5];

        itemInBox = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        box = collision.GetComponent<Box>();
        box.spriteRenderer.material = box.outline;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Box box = collision.GetComponent<Box>();
        correntType = true;
        if (box == null)
        {
            return;
        }
        else if (type_1 == box.boxType)
        {
            myType = box.boxType;
        }
        else if (type_2 == box.boxType)
        {
            myType = box.boxType;
        }
        else if (type_3 == box.boxType)
        {
            myType = box.boxType;
        }
        else
        {
            correntType = false;

            if (Input.GetMouseButtonUp(0))
            {
                box.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        correntType = false;
        box.spriteRenderer.material = box.material;
    }

    public void Pack()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (myType)
        {
            case BoxTypes.Normal:
                spriteRenderer.sprite = boxNormal;
                break;

            case BoxTypes.Fragile:
                spriteRenderer.sprite = boxFragile;
                break;

            case BoxTypes.Freeze:
                spriteRenderer.sprite = boxFreeze;
                break;
        }
    }
}
