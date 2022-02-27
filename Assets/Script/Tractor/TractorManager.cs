using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum Engine
{
    None, Small, Middle, Large
}

public enum Wheel
{
    None, Standard, HighClearance, Crawler, Mixed
}

public enum Tail
{
    None, Rice, Corn, Potato, Grass
}

public class TractorManager : Singleton<TractorManager>
{
    private Engine engine;
    //private Wheel wheel;
    private Tail tail;
    [Space]
    private Engine engineSelected;
    private Wheel wheelSelected;
    private Tail tailSelected;

    public Tractor tractor;
    [Space]
    public GameObject engineChoice;
    public GameObject wheelChoice;
    public GameObject tailChoice;
    [Space]
    public bool engineCorrect;
    public bool tailCorrect;
    [Space]
    public bool processing = false;
    [Space]
    public TextMeshProUGUI textMeadow;
    public TextMeshProUGUI textProduct;
    public TextMeshProUGUI textProductCount;
    public TextMeshProUGUI textMaxCount;
    public Slider fuelBar;
    public float maxFuel;
    public Slider progressBar;
    public float maxDes;
    public Image iconProduct;
    public Image iconProductM;
    [Space]
    public Image areaIcon;
    public Sprite areaSIcon;
    public Sprite areaMIcon;
    public Sprite areaLIcon;
    [Space]
    public float desAreaS;
    public float desAreaM;
    public float desAreaL;
    [Space]
    public GameObject areaS;
    public GameObject areaM;
    public GameObject areaL;
    [Space]
    public GameObject productRice, productCorn, productPotato, productGrass;
    public Sprite iconRice, iconCorn, iconPotato, iconGrass;
    public GameObject spawnAreaS;
    public GameObject spawnAreaM;
    public GameObject spawnAreaL;
    public Transform[] point;
    private int productCount;
    public int ProductCount { get { return productCount; } set { productCount = value; textProductCount.text = value.ToString(); } }
    public int maxCount;
    public Camera cam1;
    public Camera cam2;
    public bool camFollow;
    [Space]
    public GameObject result;
    public GameObject resultSeccess;
    public GameObject resultFail1; // น้ำมันหมดซะแล้ว
    public GameObject resultFail2; // ผลผลิตเสียหาย
    public GameObject resultFail3; // ผลผลิตเสียหาย น้ำมันรถก็หมดแล้ว
    public GameObject resultFail4; // เก็บเกี่ยวสำเร็จแต่ รถใช้เชื้อเพลิงมากเกินไป
    public GameObject resultFail5; // ผลผลิตเสียหาย รถใช้เชื้อเพลิงมากเกินไป
    [Space]
    public GameObject textGuide;
    [Space]
    public Button confirm;
    public TextMeshProUGUI textEngie;
    public TextMeshProUGUI textTail;

    private Button engineButton;
    private Button tailButton;
    [Space]
    public Button startEngine;
    public Button startTail;
    [Space]
    public CanvasGroup canvas2;

    void Awake()
    {
        Application.targetFrameRate = 300;
        Display.onDisplaysUpdated += ActivateDisplays;
        tractor = FindObjectOfType<Tractor>();
    }

    void Start()
    {
        ActivateDisplays();
        if (!PlayerPrefs.HasKey("Cam1"))
        {
            PlayerPrefs.SetInt("Cam1", 0);
            PlayerPrefs.SetInt("Cam2", 1);
        }
        else
        {
            cam1.targetDisplay = PlayerPrefs.GetInt("Cam1");
            cam2.targetDisplay = PlayerPrefs.GetInt("Cam2");
        }
        cam2.transform.parent = tractor.transform;
        RandomProposition();

        SelectEngine(1);
        SelectTail(1);
        confirm.interactable = false;
        startEngine.onClick.Invoke();
        startTail.onClick.Invoke();
#if UNITY_ANDROID || UNITY_IOS
        cam1.targetDisplay = 0;
        cam2.targetDisplay = 0;
        cam1.gameObject.SetActive(false);
#endif
    }

    void Update()
    {
        if (tractor.transform.position.x > 5 && camFollow)
        {
            cam1.transform.position = new Vector3(tractor.transform.position.x + 10, 30, -10);
        }
        progressBar.value = ((tractor.transform.position.x * 100) / maxDes) / 100;
        fuelBar.value = (((maxFuel - tractor.transform.position.x) * 100) / maxFuel) / 100;

        if (processing && !tractor.Move && !result.activeInHierarchy)
        {
            result.SetActive(true);
            camFollow = false;
            tractor.wheelCrtl.SetTrigger("Stop");

            if (engineCorrect && tailCorrect)
            {
                tractor.destination = tractor.destination + 50;
                tractor.speed = 1000;
                tractor.Move = true;
            }
            if (!engineCorrect)
            {
                foreach (Animator a in tractor.engineCrtl)
                {
                    a.SetTrigger("Broken");
                }
            }
        }

        if (processing && !tractor.Move && result.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchDisplay();
        }
    }


    public void ActivateDisplays()
    {
        Debug.Log(Display.displays.Length);
        for (int i = 1; i < Display.displays.Length; i++)
        {
            if (!Display.displays[i].active)
            {
                Display.displays[i].Activate();
            }
        }
    }

    public void SwichDisplayOnMobile()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (cam1.gameObject.activeInHierarchy)
        {
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
        }
        else
        {
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
        }
#else
    SwitchDisplay();
#endif
    }

    public void SwitchDisplay()
    {
        if (cam1.targetDisplay == 0)
        {
            cam1.targetDisplay = 1;
            cam2.targetDisplay = 0;
            PlayerPrefs.SetInt("Cam1", 1);
            PlayerPrefs.SetInt("Cam2", 0);
        }
        else
        {
            cam1.targetDisplay = 0;
            cam2.targetDisplay = 1;
            PlayerPrefs.SetInt("Cam1", 0);
            PlayerPrefs.SetInt("Cam2", 1);
        }

    }

    public void RandomProposition()
    {
        engine = (Engine)Random.Range(1,4);
        tail = (Tail)Random.Range(1, 5);

        switch (engine)
        {
            case Engine.None:
                break;
            case Engine.Small:
                textMeadow.text = "ทุ่งหญ้าขนาดเล็ก";
                maxCount = 10;
                textMaxCount.text = "/10";
                areaIcon.sprite = areaSIcon;
                tractor.destination = desAreaS;
                areaS.SetActive(true);
                point = spawnAreaS.GetComponentsInChildren<Transform>();
                break;
            case Engine.Middle:
                textMeadow.text = "ทุ่งหญ้าขนาดกลาง";
                maxCount = 15;
                textMaxCount.text = "/15";
                areaIcon.sprite = areaMIcon;
                tractor.destination = desAreaM;
                areaM.SetActive(true);
                point = spawnAreaM.GetComponentsInChildren<Transform>();
                break;
            case Engine.Large:
                textMeadow.text = "ทุ่งหญ้าขนาดใหญ่";
                maxCount = 20;
                textMaxCount.text = "/20";
                areaIcon.sprite = areaLIcon;
                tractor.destination = desAreaL;
                areaL.SetActive(true);
                point = spawnAreaL.GetComponentsInChildren<Transform>();
                break;
        }
        maxDes = tractor.destination;
        maxFuel = tractor.destination + 10;

        switch (tail)
        {
            case Tail.None:
                break;

            case Tail.Rice:
                textProduct.text = "ข้าว";
                SpawnProduct(productRice);
                iconProduct.sprite = iconRice;
                iconProductM.sprite = iconRice;
                break;

            case Tail.Corn:
                textProduct.text = "ข้าวโพด/ถั่ว";
                SpawnProduct(productCorn);
                iconProduct.sprite = iconCorn;
                iconProductM.sprite = iconCorn;
                break;


            case Tail.Potato:
                textProduct.text = "หัวมัน";
                SpawnProduct(productPotato);
                iconProduct.sprite = iconPotato;
                iconProductM.sprite = iconPotato;
                break;

            case Tail.Grass:
                textProduct.text = "อ้อย";
                SpawnProduct(productGrass);
                iconProduct.sprite = iconGrass;
                iconProductM.sprite = iconGrass;
                break;
        }
    }

    public void SpawnProduct(GameObject product)
    {
        for (int i = 1; i < point.Length ; i++)
        {
            Instantiate(product, point[i].position, Quaternion.identity,point[i]);
        }
    }

    public void SelectEngine(int index)
    {
        if (processing)
            return;
        confirm.interactable = true;

        engineSelected = (Engine)index;
        tractor.customize.engineSmall.SetActive(false);
        tractor.customize.engineMiddle.SetActive(false);
        tractor.customize.engimeLarge.SetActive(false);

        switch (engineSelected)
        {
            case Engine.None:
                engineChoice.SetActive(true);
                wheelChoice.SetActive(false);
                tailChoice.SetActive(false);
                break;
            case Engine.Small:
                tractor.customize.engineSmall.SetActive(true);
                textEngie.text = "เครื่องยนต์ขนาดเล็ก <30 แรงม้า";
                break;
            case Engine.Middle:
                tractor.customize.engineMiddle.SetActive(true);
                textEngie.text = "เครื่องยนต์ขนาดกลาง 30-60 แรงม้า";
                break;
            case Engine.Large:
                tractor.customize.engimeLarge.SetActive(true);
                textEngie.text = "เครื่องยนต์ขนาดใหญ่ >60 แรงม้า";
                break;
        }

        if (engine == engineSelected)
        {
            engineCorrect = true;
        }
        else
        {
            engineCorrect = false;
        }
    }
    public void SelectWheel(int index)
    {
        if (processing)
            return;

        wheelSelected = (Wheel)index;

        tractor.customize.wheelStandard.SetActive(false);
        tractor.customize.wheelHighClearance.SetActive(false);
        tractor.customize.wheelCrawler.SetActive(false);
        tractor.customize.wheelMixed.SetActive(false);

        switch (wheelSelected)
        {
            case Wheel.None:
                engineChoice.SetActive(false);
                wheelChoice.SetActive(true);
                tailChoice.SetActive(false);
                break;

            case Wheel.Standard:
                tractor.customize.wheelStandard.SetActive(true);
                break;

            case Wheel.HighClearance:
                tractor.customize.wheelHighClearance.SetActive(true);
                break;

            case Wheel.Crawler:
                tractor.customize.wheelCrawler.SetActive(true);
                break;

            case Wheel.Mixed:
                tractor.customize.wheelMixed.SetActive(true);
                break;
        }
    }
    public void SelectTail(int index)
    {
        if (processing)
            return;
        confirm.interactable = true;

        tailSelected = (Tail)index;

        tractor.customize.tailRice.SetActive(false);
        tractor.customize.tailCorn.SetActive(false);
        tractor.customize.tailPotato.SetActive(false);
        tractor.customize.tailGrass.SetActive(false);

        switch (tailSelected)
        {
            case Tail.None:
                engineChoice.SetActive(false);
                wheelChoice.SetActive(false);
                tailChoice.SetActive(true);
                break;

            case Tail.Rice:
                tractor.customize.tailRice.SetActive(true);
                textTail.text = "หัวเก็บเกี่ยวข้าว";
                break;


            case Tail.Corn:
                tractor.customize.tailCorn.SetActive(true);
                textTail.text = "หัวเก็บเกี่ยวข้าวโพด/ถั่ว";
                break;


            case Tail.Potato:
                tractor.customize.tailPotato.SetActive(true);
                textTail.text = "หัวเก็บเกี่ยวหัวมัน";
                break;

            case Tail.Grass:
                tractor.customize.tailGrass.SetActive(true);
                textTail.text = "หัวเก็บเกี่ยวอ้อย";
                break;

        }
        if (tail == tailSelected)
        {
            tailCorrect = true;
        }
        else
        {
            tailCorrect = false;
        }
    }

    public void Confrim()
    {
        processing = true;
        tractor.Move = true;
        tractor.wheelCrtl.SetTrigger("Move");
        confirm.interactable = false;
        confirm.gameObject.SetActive(false);
        camFollow = true;
        canvas2.interactable = false;
        textGuide.SetActive(false);

        if (engineCorrect && tailCorrect)
        {
            Debug.Log("Success");
            resultSeccess.SetActive(true);
            tractor.productResult = "Good";
        }
        else if (!engineCorrect && (int)engineSelected < (int)engine && tailCorrect)
        {
            Debug.Log("Engine samll than area");
            tractor.destination = tractor.destination / 2;
            maxFuel = tractor.destination;
            resultFail1.SetActive(true);
            tractor.productResult = "Good";
        }
        else if (engineCorrect && !tailCorrect)
        {
            Debug.Log("Tail Fail");
            maxFuel = tractor.destination;
            resultFail2.SetActive(true);
            tractor.productResult = "Bad";
        }
        else if (!engineCorrect && (int)engineSelected > (int)engine && !tailCorrect)
        {
            Debug.Log("Engine more than area : Tail fail");
            maxFuel = tractor.destination;
            resultFail3.SetActive(true);
            tractor.productResult = "Bad";
        }
        else if (!engineCorrect && (int)engineSelected > (int)engine && tailCorrect)
        {
            Debug.Log("Engine more than area");
            maxFuel = tractor.destination;
            resultFail4.SetActive(true);
            tractor.productResult = "Good";
        }
        else if (!engineCorrect && (int)engineSelected < (int)engine && !tailCorrect)
        {
            Debug.Log("Engine small than area");
            tractor.destination = tractor.destination * 0.5f;
            maxFuel = tractor.destination;
            resultFail5.SetActive(true);
            tractor.productResult = "Bad";
        }
    }

    public void HighlightEngineButton(Button but)
    {
        if (engineButton != null)
        {
            ColorBlock colorsA = engineButton.colors;
            colorsA.normalColor = Color.clear;
            colorsA.disabledColor = Color.clear;
            engineButton.colors = colorsA;
            engineButton = null;
        }
        Button b = but;
        ColorBlock colorsB = b.colors;
        colorsB.normalColor = new Color(255,255,255,255);
        colorsB.disabledColor = new Color(255, 255, 255, 255);
        but.colors = colorsB;
        engineButton = but;
    }

    public void HighlightTailButton(Button but)
    {
        if (tailButton != null)
        {
            ColorBlock colorsA = tailButton.colors;
            colorsA.normalColor = Color.clear;
            colorsA.disabledColor = Color.clear;
            tailButton.colors = colorsA;
            tailButton = null;
        }
        Button b = but;
        ColorBlock colorsB = b.colors;
        colorsB.normalColor = new Color(255, 255, 255, 255);
        colorsB.disabledColor = new Color(255, 255, 255, 255);
        but.colors = colorsB;
        tailButton = but;
    }
}
