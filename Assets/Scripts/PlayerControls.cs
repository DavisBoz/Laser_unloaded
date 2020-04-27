using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    private Rigidbody rb;
    public GameObject head;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.AddForce(Vector3.right * speed * moveHorizontal);
        rb.AddForce(Vector3.forward * speed * moveVertical);


        if (moveHorizontal==0 && moveVertical==0)
        {
            rb.angularVelocity = Vector3.zero;
        }


        if (Input.GetKeyDown("space") && rb.transform.position.y <= 0.5f)
        {
            Vector3 jump = new Vector3(0.0f, 200.0f, 0.0f);
            rb.AddForce(jump);
        }
    }


     void Update()
    {
        head.transform.position = transform.position;
    }

}
