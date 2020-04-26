using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    private int Jumped;
    private Rigidbody rb;
    private int count;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        if (rb.transform.position.y == 0.5f)
            Jumped = 0;
        if (Input.GetKeyDown("space") && Jumped < 2)
        {
            Vector3 jump = new Vector3(0.0f, 200.0f, 0.0f);
            rb.AddForce(jump);
            Jumped += 1;
        }
    }


}
