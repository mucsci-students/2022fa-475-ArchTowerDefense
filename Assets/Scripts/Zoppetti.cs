using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoppetti : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // anim.SetFloat("speed", GetComponent<GraphwayTest>().speed);
    }
}
