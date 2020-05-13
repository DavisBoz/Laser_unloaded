using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    public float JumpHeight;
    public float fallspeed;
    public GameObject head;
    public GameObject muzzle;
    public int cont_speed;
    RayGun m_shot;
    public AudioSource BB8Jump;
    public static int gravity_direction = 1;
    public static int previous_direction = 1;
    // 1 = down
    // 2 = right
    // 3 = up
    // 4 = left

    private Vector3 right_horizontal = Vector3.up;
    private Vector3 top_horizontal = Vector3.left;
    private Vector3 left_horizontal = Vector3.down;
    private Vector3 bottom_horizontal = Vector3.right;
    private Vector3 current_horizontal = Vector3.right;


    public Vector3 current_gravity = Vector3.down;

    private Vector3 jump_direction = Vector3.up;

    public static bool g_changed = false;
    private Rigidbody rb;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private Vector3 pos;
    private bool grounded = false;

    GameObject cam;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        pos = rb.transform.position;

        if (moveVertical >= 0)
        {
            rb.AddForce(Vector3.forward * speed * moveVertical);
        }
        rb.AddForce(current_horizontal * speed * moveHorizontal);

        /*if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }*/

        if (Physics.Raycast(pos, current_gravity, 1f))
            grounded = true;
        else
            grounded = false;

        if (Input.GetKey("space") && grounded)
        {
            rb.AddForce(jump_direction * JumpHeight);
            BB8Jump.Play();
        }

        rb.velocity = Vector3.Scale(rb.velocity, deceleration);

        rb.AddForce(current_gravity * 50);


    }


    void Update()
    {
        if (g_changed)
        {
            previous_direction = gravity_direction;
            g_changed = false;
        }

        head.transform.position = transform.position;
        rb.AddForce(Vector3.forward * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        // Beginning Double damage
        
        if(other.gameObject.CompareTag("DoubleDamage"))
        {
            other.gameObject.SetActive(false);
            m_shot = muzzle.GetComponent<RayGun>();
            m_shot.damageAmount *= 2;
            print("Doubled damage, from " + m_shot.damageAmount / 2 + " to " + m_shot.damageAmount);
        }
        

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

        if (other.gameObject.CompareTag("Right_wall"))
        {
            g_changed = true;
            gravity_direction = 2;
            current_horizontal = right_horizontal;
            current_gravity = Vector3.right;
            jump_direction = Vector3.left;
            deceleration = new Vector3(1f, .5f, .5f);
            print(head.transform.rotation);
            head.transform.rotation = Quaternion.Euler(0, -90, 45);
        }
        if (other.gameObject.CompareTag("Left_wall"))
        {
            g_changed = true;
            gravity_direction = 4;
            current_horizontal = left_horizontal;
            current_gravity = Vector3.left;
            jump_direction = Vector3.right;
            deceleration = new Vector3(1f, .5f, .5f);
            head.transform.rotation = Quaternion.Euler(0, 90, 45);
        }
        if (other.gameObject.CompareTag("Top_wall"))
        {
            g_changed = true;
            gravity_direction = 3;
            current_horizontal = top_horizontal;
            current_gravity = Vector3.up;
            jump_direction = Vector3.down;
            deceleration = new Vector3(.5f, 1f, .5f);
            head.transform.rotation = Quaternion.Euler(90, 45, 0);
        }
        if (other.gameObject.CompareTag("Bottom_wall"))
        {
            g_changed = true;
            gravity_direction = 1;
            current_horizontal = bottom_horizontal;
            current_gravity = Vector3.down;
            jump_direction = Vector3.up;
            deceleration = new Vector3(.5f, 1f, .5f);
            head.transform.rotation = Quaternion.Euler(-90, -45, 0);
            print(head.transform.forward);
        }
    }

}
