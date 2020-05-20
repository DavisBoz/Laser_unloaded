using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    public float jump_height;
    public static int gravity_direction = 1;
    public static int previous_direction = 1;
    public static bool g_changed = false;
    public AudioSource BB8Jump;
    public GameObject head;
    public GameObject muzzle;
    public Vector3 current_gravity = Vector3.down;
    //EnemyHealth dmg;
    RayGun m_shot;
    int jumps;

    private Vector3 jump_direction = Vector3.up;
    private Vector3 right_horizontal = Vector3.up;
    private Vector3 top_horizontal = Vector3.left;
    private Vector3 left_horizontal = Vector3.down;
    private Vector3 bottom_horizontal = Vector3.right;
    private Vector3 current_horizontal = Vector3.right;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private Rigidbody rb;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, current_gravity, .5f))
                rb.AddForce( jump_direction * jump_height, ForceMode.Impulse);
                if (jumps % 10 == 0)
                {
                    BB8Jump.Play();
                    jumps += 1;
                }
                else
                    jumps += 1;
        }
        
        if (moveVertical >= 0)
        {
            rb.AddForce(Vector3.forward * speed * moveVertical);
        }

        rb.AddForce(current_horizontal * speed * moveHorizontal);
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
        if (other.gameObject.CompareTag("DoubleDamage"))
        {
            other.gameObject.SetActive(false);
            m_shot = muzzle.GetComponent<RayGun>();
            m_shot.damageAmount *= 2;
            print("Doubled damage, from " + m_shot.damageAmount / 2 + " to " + m_shot.damageAmount);
        }
        
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

        /*if (other.gameObject.CompareTag("DoubleXP"))
        {
            other.gameObject.SetActive(false);
            dmg = GetComponent<EnemyHealth>();
            dmg.scoreValue *= 2;
            print("Doubled XP, from " + dmg.scoreValue / 2 + " to " + dmg.scoreValue);
        }*/

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
