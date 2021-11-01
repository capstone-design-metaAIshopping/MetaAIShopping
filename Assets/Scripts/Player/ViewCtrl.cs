using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ViewCtrl : MonoBehaviour
{
    public static ViewCtrl _Instance;
    Rigidbody rb;
    NavMeshAgent ag;

    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
       // rb = GetComponent<Rigidbody>();
        //ag = GetComponent<NavMeshAgent>();

        // Disable agent control of the transform
       // ag.updatePosition = false;
       // ag.updateRotation = false;
    }
    // Update is called once per frame
    void Update()
    {
        //rb.velocity = ag.velocity;
       // ag.nextPosition = rb.position;
        /*if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition = new Vector3(0f, 1.6f, 0f);
        }
        if(Input.GetKey(KeyCode.B))
        {
            transform.localPosition = new Vector3(0f, 5.611877f, 0f);
        }
        if(Input.GetKey(KeyCode.C))
        {
            transform.localPosition = new Vector3(0f, 9.614086f, 0f);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.localPosition = new Vector3(0f, 14.80789f, 0f);
        }
      */
    }

    public void Move1F()
    {
        //transform.localPosition = new Vector3(0f, 1.6f, 0f);
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    public void Move2F()
    {
        transform.localPosition = new Vector3(0f, 4.011877f, 0f);
    }
    public void Move3F()
    {
        transform.localPosition = new Vector3(0f, 8.014086f, 0f);
    }

    public void Move4F()
    {
        transform.localPosition = new Vector3(0f, 13.20789f, 0f);
    }
}
