using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : MonoBehaviour
{
    [Space]
    public Rigidbody2D rigid;
    [SerializeField]
    private bool move;
    public bool Move {
        get { return move; }
        set { move = value;
            if (move)
            {
                rigid.constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
    public float speed;
    public float destination = 20;

    [System.Serializable]
    public class Customize
    {
        public GameObject engineSmall, engineMiddle, engimeLarge;
        public GameObject wheelStandard, wheelHighClearance, wheelCrawler, wheelMixed;
        public GameObject tailRice, tailCorn, tailPotato, tailGrass;
    }
    public Customize customize;
    public string productResult;

    public Animator[] engineCrtl;
    public Animator wheelCrtl;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void SetEngine()
    {

    }

    public void SetWheel()
    {

    }

    public void SetTail()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Product")
        {
            if (productResult == "Good")
            {
                TractorManager.Instance.ProductCount++;
            }
            //collision.gameObject.SetActive(false);
            collision.enabled = false;
            collision.GetComponent<Animator>().SetTrigger(productResult);
        }
    }

    public void Movement()
    {
        if (Move && transform.position.x < destination)
        {
            rigid.velocity = Vector2.right * speed * Time.deltaTime;
        }
        else {
            Move = false;
        }

    }
}
