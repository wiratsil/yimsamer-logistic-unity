using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quest_Logistic : Singleton<Quest_Logistic>
{
    private GameObject item;

    public bool task;

    public GameObject symbolPanel;
    public GameObject shippingPanel;
    public GameObject label;
    [Space]
    private Material material;
    public Material outline;
    private float posY;
    public TextMeshPro itemLabel;
    public GameObject pointer;
    public Animator beltAni;
    [Space]
    public float idleTime;
    public GameObject tapStart;
    [Space]
    public AudioSource audioSource;
    public AudioClip audio_PreProductAppear;
    public AudioClip aduio_Packed;

    private void Awake()
    {
        Application.targetFrameRate = 300;
    }

    private void Start()
    {
        StartCoroutine(LogisticTask());
    }

    private void Update()
    {
        if (!tapStart.activeInHierarchy)
        {
            if (idleTime < 30)
            {
                idleTime += Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.anyKey)
            {
                idleTime = 0;
            }
        }
    }

    IEnumerator LogisticTask()
    {
        yield return new WaitUntil(() => Input.anyKey);
        tapStart.SetActive(false);
        // สุ่มไอเท็ม
        RandomSupplies();
        yield return null;
        audioSource.PlayOneShot(audio_PreProductAppear);
        Supplies supplies = FindObjectOfType<Supplies>();
        itemLabel.text = supplies.productName;
        task = false;
        // เคลื่อนที่ภายในท่อจากบนลงล่าง
        yield return new WaitForSeconds(2);
        supplies.GetComponent<Rigidbody2D>().gravityScale = 0;
        //supplies.transform.DOMove(new Vector3(-12.5f, -3.25f, 0), 2).OnStepComplete(() => task = true);
        //yield return new WaitUntil(() => task == true);
        task = false;
        // เคลื่อนที่มาในสายพาน
        posY = supplies.transform.position.y;
        supplies.transform.DOMove(new Vector3(-2.5f, posY, 0), 2).OnStepComplete(() => task = true) ;
        beltAni.SetTrigger("Belt_Active");
        yield return new WaitUntil(() => task == true);
        beltAni.SetTrigger("Belt_Stop");
        supplies.transform.DOShakeScale(60, fadeOut: false, strength: 0.1f, randomness: 15, vibrato: 5);
        // รอเลือกชนิดกล่องให้ถูกต้อง
        material = supplies.GetComponent<SpriteRenderer>().material;
        supplies.GetComponent<SpriteRenderer>().material = outline;
        pointer.SetActive(true);
        yield return new WaitUntil(() => Packaging() == true);

        task = false;
        //แพ็คเข้ากล่อง
        audioSource.PlayOneShot(aduio_Packed);
        supplies.Pack();
        supplies.GetComponent<SpriteRenderer>().material = material;
        supplies.GetComponent<Rigidbody2D>().gravityScale = 1;
        supplies.GetComponent<BoxCollider2D>().size = new Vector2(3.5f, 2.45f);
        yield return new WaitForSeconds(2);
        posY = supplies.transform.position.y;
        //เลื่อนสู่ขั้นตอนต่อไป
        //supplies.transform.DOMove(new Vector3(0, posY, 0), 2).OnStepComplete(() => task = true);
        //yield return new WaitUntil(() => task == true);
        //task = false;
        //เลือกไปติดสัญลักษณ์
        Camera.main.transform.DOMove(new Vector3(41.5f, 0, -10), 2);
        supplies.transform.DOMove(new Vector3(35, posY, 0), 2).OnStepComplete(() => task = true);
        beltAni.SetTrigger("Belt_Active");
        yield return new WaitUntil(() => task == true);
        beltAni.SetTrigger("Belt_Stop");
        task = false;
        symbolPanel.SetActive(true);
        yield return new WaitUntil(() => supplies.correntSymbol);
        yield return new WaitForSeconds(1.1f);
        symbolPanel.SetActive(false);
        //เลื่อนไปเลือกจุดหมาย
        Camera.main.transform.DOMove(new Vector3(58.5f, 0, -10), 2);
        supplies.transform.DOMove(new Vector3(52, posY, 0), 2).OnStepComplete(() => task = true);
        beltAni.SetTrigger("Belt_Active");
        yield return new WaitUntil(() => task == true);
        beltAni.SetTrigger("Belt_Stop");
        task = false;

        shippingPanel.SetActive(true);
        yield return new WaitUntil(() => supplies.comfrimShipping);

        yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void RandomSupplies()
    {
        GameObject[] suppliesList = Resources.LoadAll<GameObject>("Logistic/Prefab/Items"); ;
        GameObject supplies = suppliesList[Random.Range(0, suppliesList.Length)];
        GameObject clone = Instantiate(supplies,new Vector3(-12.5f, 5f, 0),Quaternion.identity);
        Instantiate(label,clone.transform);
    }

    bool Packaging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000);
            if (hit.collider != null && hit.collider.GetComponent<Supplies>())
            {
                item = hit.collider.gameObject;
                pointer.SetActive(false);
                BoxCollider2D boxCollider2D = item.GetComponent<BoxCollider2D>();
                boxCollider2D.size = new Vector2(0.5f , boxCollider2D.size.y);
                item.transform.DOKill();
            }

        }
        if (Input.GetMouseButton(0))
        {
            if (item != null)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                item.transform.position = new Vector2(pos.x, pos.y);
            }
        }
        if (Input.GetMouseButtonUp(0) && item != null)
        {
            if (item.GetComponent<Supplies>().correntType)
            {
                return true;
            }
            item.transform.position = new Vector2(-2.5f, posY);
            BoxCollider2D boxCollider2D = item.GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(3.5f, boxCollider2D.size.y);
            item.transform.DOShakeScale(60, fadeOut: false, strength: 0.1f, randomness: 15, vibrato: 5);
            item = null;

        }
        return false;
    }

}
