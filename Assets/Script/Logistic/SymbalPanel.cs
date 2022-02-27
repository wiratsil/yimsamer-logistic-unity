using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Symbol
{
    None,
    Fragile,
    Frameable,
    StackNumber,
    UpwardArrows,
    BoxBeneathSun,
    Thermometer,
    Explosion,
    LithiumBattery,
    HandsWithBox,
    Umbrella,
    TwoPeopleLiftingBox,
    BoxOnTrolley
}

public class SymbalPanel : MonoBehaviour
{
    //ref
    public Sprite emptySlotSprite;
    [Space]
    public Image itemInBox;
    public Button confrimBut;

    [System.Serializable]
    public class DataSlot
    {
        //ref
        public Image symbolImage;
        [Space]
        public Symbol symbol;
        public GameObject data;

        public void CleaData()
        {
            if (data == null)
                return;

            data.GetComponent<Button>().interactable = true;
            symbol = Symbol.None;
            data = null;
        }
        public void NotCorrect()
        {
            if (data == null)
                return;

            symbol = Symbol.None;
            data = null;
        }
    }

    public DataSlot dataSlot1;
    public DataSlot dataSlot2;
    public DataSlot dataSlot3;
    public DataSlot dataSlot4;
    public DataSlot dataSlot5;
    public DataSlot dataSlot6;
    [Space]
    public float speedStamp = 0.5f;
    [Space]
    private Supplies supplies;

    private void OnEnable()
    {
        supplies = FindObjectOfType<Supplies>();
        itemInBox.sprite = supplies.itemInBox;
        confrimBut.interactable = false;

        dataSlot1.symbolImage.gameObject.SetActive(true);
        dataSlot2.symbolImage.gameObject.SetActive(true);
        dataSlot3.symbolImage.gameObject.SetActive(true);
        dataSlot4.symbolImage.gameObject.SetActive(true);
        dataSlot5.symbolImage.gameObject.SetActive(true);
        dataSlot6.symbolImage.gameObject.SetActive(true);

        if (supplies.symbols.Count == 1)
        {
            dataSlot2.symbolImage.gameObject.SetActive(false);
            dataSlot3.symbolImage.gameObject.SetActive(false);
            dataSlot4.symbolImage.gameObject.SetActive(false);
            dataSlot5.symbolImage.gameObject.SetActive(false);
            dataSlot6.symbolImage.gameObject.SetActive(false);
        }
        else if (supplies.symbols.Count == 2)
        {
            dataSlot3.symbolImage.gameObject.SetActive(false);
            dataSlot4.symbolImage.gameObject.SetActive(false);
            dataSlot5.symbolImage.gameObject.SetActive(false);
            dataSlot6.symbolImage.gameObject.SetActive(false);
        }
        else if (supplies.symbols.Count == 3)
        {
            dataSlot4.symbolImage.gameObject.SetActive(false);
            dataSlot5.symbolImage.gameObject.SetActive(false);
            dataSlot6.symbolImage.gameObject.SetActive(false);
        }
        else if (supplies.symbols.Count == 4)
        {
            dataSlot5.symbolImage.gameObject.SetActive(false);
            dataSlot6.symbolImage.gameObject.SetActive(false);
        }
        else if (supplies.symbols.Count == 5)
        {
            dataSlot6.symbolImage.gameObject.SetActive(false);
        }
    }

    public void SelectSymbol(GameObject data)
    {
        if (dataSlot1.data == null && supplies.symbols.Count >= 1)
        {
            dataSlot1.data = data;
            dataSlot1.symbolImage.sprite = data.GetComponent<Image>().sprite;
            dataSlot1.symbol = data.GetComponent<SymbolData>().symbol;
        }
        else if (dataSlot2.data == null && supplies.symbols.Count >= 2)
        {
            dataSlot2.data = data;
            dataSlot2.symbolImage.sprite = data.GetComponent<Image>().sprite;
            dataSlot2.symbol = data.GetComponent<SymbolData>().symbol;
        }
        else if (dataSlot3.data == null && supplies.symbols.Count >= 3)
        {
            dataSlot3.data = data;
            dataSlot3.symbolImage.sprite = data.GetComponent<Image>().sprite;
            dataSlot3.symbol = data.GetComponent<SymbolData>().symbol;
        }
        else if (dataSlot4.data == null && supplies.symbols.Count >= 4)
        {
            dataSlot4.data = data;
            dataSlot4.symbolImage.sprite = data.GetComponent<Image>().sprite;
            dataSlot4.symbol = data.GetComponent<SymbolData>().symbol;
        }
        else if (dataSlot5.data == null && supplies.symbols.Count >= 5)
        {
            dataSlot5.data = data;
            dataSlot5.symbolImage.sprite = data.GetComponent<Image>().sprite;
            dataSlot5.symbol = data.GetComponent<SymbolData>().symbol;
        }
        else if (dataSlot6.data == null && supplies.symbols.Count >= 6)
        {
            dataSlot6.data = data;
            dataSlot6.symbolImage.sprite = data.GetComponent<Image>().sprite;
            dataSlot6.symbol = data.GetComponent<SymbolData>().symbol;
        }
        else
            return;

        confrimBut.interactable = true;
        data.GetComponent<Button>().interactable = false;
    }

    public void UnSelectSymbol(int index)
    {
        switch (index)
        {
            case 1:
                dataSlot1.CleaData();
                dataSlot1.symbolImage.sprite = emptySlotSprite;
                break;

            case 2:
                dataSlot2.CleaData();
                dataSlot2.symbolImage.sprite = emptySlotSprite;
                break;

            case 3:
                dataSlot3.CleaData();
                dataSlot3.symbolImage.sprite = emptySlotSprite;
                break;

            case 4:
                dataSlot4.CleaData();
                dataSlot4.symbolImage.sprite = emptySlotSprite;
                break;

            case 5:
                dataSlot5.CleaData();
                dataSlot5.symbolImage.sprite = emptySlotSprite;
                break;

            case 6:
                dataSlot6.CleaData();
                dataSlot6.symbolImage.sprite = emptySlotSprite;
                break;
        }
    }

    public void ConfrimSymbol()
    {
        confrimBut.interactable = false;
        int symbolCount = supplies.symbols.Count;//> 3 ? 3 : supplies.symbols.Count;
        int correctCount = 0;
        //เช็คเครื่องหมาย
        if (supplies.symbols.Contains(dataSlot1.symbol))
        {
            correctCount++;

            if (dataSlot1.symbolImage.GetComponent<Button>().interactable)
            {
                dataSlot1.symbolImage.GetComponent<Button>().interactable = false;
                dataSlot1.data.transform.Find("Correct").gameObject.SetActive(true);
                RectTransform clone = Instantiate(
                     dataSlot1.symbolImage.gameObject,
                     dataSlot1.symbolImage.transform.position,
                     dataSlot1.symbolImage.transform.rotation,
                     transform
                     ).GetComponent<RectTransform>();

                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(supplies.label1.transform.position);
                clone.sizeDelta = new Vector2(360, 300);
                clone.DOAnchorMax(viewportPoint, speedStamp);
                clone.DOAnchorMin(viewportPoint, speedStamp);
                clone.DOAnchorPos(new Vector2(0, 0), speedStamp).OnStepComplete(() => supplies.label1.sprite = dataSlot1.symbolImage.sprite);
                clone.DOScale(new Vector3(0.25f, 0.25f, 1), speedStamp).OnStepComplete(() => DestroyImmediate(clone.gameObject));
            }
        }
        else if (supplies.symbols.Count >= 1 && dataSlot1.data != null)
        {
            dataSlot1.data.transform.Find("Wrong").gameObject.SetActive(true);
            dataSlot1.NotCorrect();
            dataSlot1.symbolImage.sprite = emptySlotSprite;
        }

        if (supplies.symbols.Contains(dataSlot2.symbol))
        {
            correctCount++;

            if (dataSlot2.symbolImage.GetComponent<Button>().interactable)
            {
                dataSlot2.symbolImage.GetComponent<Button>().interactable = false;
                dataSlot2.data.transform.Find("Correct").gameObject.SetActive(true);
                RectTransform clone = Instantiate(
                     dataSlot2.symbolImage.gameObject,
                     dataSlot2.symbolImage.transform.position,
                     dataSlot2.symbolImage.transform.rotation,
                     transform
                     ).GetComponent<RectTransform>();

                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(supplies.label2.transform.position);
                clone.sizeDelta = new Vector2(360, 300);
                clone.DOAnchorMax(viewportPoint, speedStamp);
                clone.DOAnchorMin(viewportPoint, speedStamp);
                clone.DOAnchorPos(new Vector2(0, 0), speedStamp).OnStepComplete(() => supplies.label2.sprite = dataSlot2.symbolImage.sprite);
                clone.DOScale(new Vector3(0.25f, 0.25f, 1), speedStamp).OnStepComplete(() => DestroyImmediate(clone.gameObject));
            }
        }
        else if (supplies.symbols.Count >= 2 && dataSlot2.data != null)
        {
            dataSlot2.data.transform.Find("Wrong").gameObject.SetActive(true);
            dataSlot2.NotCorrect();
            dataSlot2.symbolImage.sprite = emptySlotSprite;
        }

        if (supplies.symbols.Count >= 3 && supplies.symbols.Contains(dataSlot3.symbol))
        {
            correctCount++;

            if (dataSlot3.symbolImage.GetComponent<Button>().interactable)
            {
                dataSlot3.symbolImage.GetComponent<Button>().interactable = false;
                dataSlot3.data.transform.Find("Correct").gameObject.SetActive(true);
                RectTransform clone = Instantiate(
                     dataSlot3.symbolImage.gameObject,
                     dataSlot3.symbolImage.transform.position,
                     dataSlot3.symbolImage.transform.rotation,
                     transform
                     ).GetComponent<RectTransform>();

                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(supplies.label3.transform.position);
                clone.sizeDelta = new Vector2(360, 300);
                clone.DOAnchorMax(viewportPoint, speedStamp);
                clone.DOAnchorMin(viewportPoint, speedStamp);
                clone.DOAnchorPos(new Vector2(0, 0), speedStamp).OnStepComplete(() => supplies.label3.sprite = dataSlot3.symbolImage.sprite);
                clone.DOScale(new Vector3(0.25f, 0.25f, 1), speedStamp).OnStepComplete(() => DestroyImmediate(clone.gameObject));
            }
        }
        else if(supplies.symbols.Count >= 3 && dataSlot3.data != null)
        {
            dataSlot3.data.transform.Find("Wrong").gameObject.SetActive(true);
            dataSlot3.NotCorrect();
            dataSlot3.symbolImage.sprite = emptySlotSprite;
        }



        if (supplies.symbols.Count >= 4 && supplies.symbols.Contains(dataSlot4.symbol))
        {
            correctCount++;

            if (dataSlot4.symbolImage.GetComponent<Button>().interactable)
            {
                dataSlot4.symbolImage.GetComponent<Button>().interactable = false;
                dataSlot4.data.transform.Find("Correct").gameObject.SetActive(true);
                RectTransform clone = Instantiate(
                     dataSlot4.symbolImage.gameObject,
                     dataSlot4.symbolImage.transform.position,
                     dataSlot4.symbolImage.transform.rotation,
                     transform
                     ).GetComponent<RectTransform>();

                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(supplies.label4.transform.position);
                clone.sizeDelta = new Vector2(360, 300);
                clone.DOAnchorMax(viewportPoint, speedStamp);
                clone.DOAnchorMin(viewportPoint, speedStamp);
                clone.DOAnchorPos(new Vector2(0, 0), speedStamp).OnStepComplete(() => supplies.label4.sprite = dataSlot4.symbolImage.sprite);
                clone.DOScale(new Vector3(0.25f, 0.25f, 1), speedStamp).OnStepComplete(() => DestroyImmediate(clone.gameObject));
            }
        }
        else if (supplies.symbols.Count >= 4 && dataSlot4.data != null)
        {
            dataSlot4.data.transform.Find("Wrong").gameObject.SetActive(true);
            dataSlot4.NotCorrect();
            dataSlot4.symbolImage.sprite = emptySlotSprite;
        }


        if (supplies.symbols.Count >= 5 && supplies.symbols.Contains(dataSlot5.symbol))
        {
            correctCount++;

            if (dataSlot5.symbolImage.GetComponent<Button>().interactable)
            {
                dataSlot5.symbolImage.GetComponent<Button>().interactable = false;
                dataSlot5.data.transform.Find("Correct").gameObject.SetActive(true);
                RectTransform clone = Instantiate(
                     dataSlot5.symbolImage.gameObject,
                     dataSlot5.symbolImage.transform.position,
                     dataSlot5.symbolImage.transform.rotation,
                     transform
                     ).GetComponent<RectTransform>();

                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(supplies.label5.transform.position);
                clone.sizeDelta = new Vector2(360, 300);
                clone.DOAnchorMax(viewportPoint, speedStamp);
                clone.DOAnchorMin(viewportPoint, speedStamp);
                clone.DOAnchorPos(new Vector2(0, 0), speedStamp).OnStepComplete(() => supplies.label5.sprite = dataSlot5.symbolImage.sprite);
                clone.DOScale(new Vector3(0.25f, 0.25f, 1), speedStamp).OnStepComplete(() => DestroyImmediate(clone.gameObject));
            }
        }
        else if (supplies.symbols.Count >= 5 && dataSlot5.data != null)
        {
            dataSlot5.data.transform.Find("Wrong").gameObject.SetActive(true);
            dataSlot5.NotCorrect();
            dataSlot5.symbolImage.sprite = emptySlotSprite;
        }


        if (supplies.symbols.Count >= 6 && supplies.symbols.Contains(dataSlot6.symbol))
        {
            correctCount++;

            if (dataSlot6.symbolImage.GetComponent<Button>().interactable)
            {
                dataSlot6.symbolImage.GetComponent<Button>().interactable = false;
                dataSlot6.data.transform.Find("Correct").gameObject.SetActive(true);
                RectTransform clone = Instantiate(
                     dataSlot6.symbolImage.gameObject,
                     dataSlot6.symbolImage.transform.position,
                     dataSlot6.symbolImage.transform.rotation,
                     transform
                     ).GetComponent<RectTransform>();

                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(supplies.label6.transform.position);
                clone.sizeDelta = new Vector2(360, 300);
                clone.DOAnchorMax(viewportPoint, speedStamp);
                clone.DOAnchorMin(viewportPoint, speedStamp);
                clone.DOAnchorPos(new Vector2(0, 0), speedStamp).OnStepComplete(() => supplies.label6.sprite = dataSlot6.symbolImage.sprite);
                clone.DOScale(new Vector3(0.25f, 0.25f, 1), speedStamp).OnStepComplete(() => DestroyImmediate(clone.gameObject));
            }
        }
        else if (supplies.symbols.Count >= 6 && dataSlot6.data != null)
        {
            dataSlot6.data.transform.Find("Wrong").gameObject.SetActive(true);
            dataSlot6.NotCorrect();
            dataSlot6.symbolImage.sprite = emptySlotSprite;
        }

        if (symbolCount == correctCount)
        {
            supplies.correntSymbol = true;
        }
        else
        {
            supplies.correntSymbol = false;
        }
        
    }
}
