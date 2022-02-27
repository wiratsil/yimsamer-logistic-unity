using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
    }

    private void Update()
    {
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist ,transform.position.y ,transform.position.z);

    }
}
