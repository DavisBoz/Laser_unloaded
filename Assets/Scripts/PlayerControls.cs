using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    private int Jumped;
    private Rigidbody rb;
    private int count;
    public GameObject head;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        GetComponent<Rigidbody>().AddForce(Vector3.forward * speed * moveVertical);
        GetComponent<Rigidbody>().AddForce(Vector3.right * speed * moveHorizontal);

        if(moveHorizontal==0 && moveVertical==0)
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (rb.transform.position.y == 0.5f)
            Jumped = 0;
        if (Input.GetKeyDown("space") && Jumped < 2)
        {
            Vector3 jump = new Vector3(0.0f, 200.0f, 0.0f);
            rb.AddForce(jump);
            Jumped += 1;
        }
    }


    private void Update()
    {
        head.transform.position = transform.position;
    }


}
