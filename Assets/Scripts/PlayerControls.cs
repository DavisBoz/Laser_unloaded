using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    public float JumpHeight;
    public float fallspeed;
    public GameObject head;

    private Rigidbody rb;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private float ypos;
    private bool falling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        ypos = rb.transform.position.y;

        if (moveVertical >= 0)
        {
            rb.AddForce(Vector3.forward * speed * moveVertical);
        }
        rb.AddForce(Vector3.right * speed * moveHorizontal);

        if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }

        if (ypos <= .5f)
            falling = false;
        if (ypos >= 1.5f || Input.GetKeyUp("space"))
            falling = true;

        if (Input.GetKey("space") && !falling && ypos > .25f)
        {
            rb.velocity += Vector3.up * JumpHeight;
        }
        else if (falling)
        {
            rb.velocity += Vector3.down * fallspeed;
        }

        rb.velocity = Vector3.Scale(rb.velocity, deceleration);
    }


    void Update()
    {
        head.transform.position = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // Beginning Double damage
        /*
        if(other.gameObject.CompareTag("DoubleDmg"))
        {
            other.gameObject.SetActive(false);
            m_shot = GetComponent<ShotBehavior>();
            m_shot.damageAmount *= 2;
            print("Doubled damage, from " + m_shot.damageAmount / 2 + " to " + m_shot.damageAmount);
        }
        */

        // Extra Health
        if(other.gameObject.CompareTag("ExtraHealth"))
        {
            other.gameObject.SetActive(false);
            rb.gameObject.GetComponent<PlayerHealth>().currentHealth += 50;
            print("Player got extra health");

        }

        if(other.gameObject.CompareTag("SpeedUp"))
        {
            other.gameObject.SetActive(false);
            speed += 30;
            print("Player got more speed");
        }
    }

}
