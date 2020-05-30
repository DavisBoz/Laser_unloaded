using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    public float jump_height;
    public static int gravity_direction = 1;
    //public static int previous_direction = 1;
    public static int count;
    public static bool g_changed = false;
    public GameObject head;
    public GameObject muzzle;
    public Vector3 current_gravity = Vector3.down;
    RayGun m_shot;
    string prev_tag;
    float move_horizontal;
    float move_vertical;
    bool can_jump;
    bool grounded;

    private Vector3 jump_direction = Vector3.up;
    private Vector3 right_horizontal = Vector3.up;
    private Vector3 top_horizontal = Vector3.left;
    private Vector3 left_horizontal = Vector3.down;
    private Vector3 bottom_horizontal = Vector3.right;
    private Vector3 current_horizontal = Vector3.right;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private Rigidbody rb;
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        move_horizontal = Input.GetAxis("Horizontal");
        move_vertical = Input.GetAxis("Vertical");

        if (can_jump)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(jump_direction * jump_height, ForceMode.Impulse);
            can_jump = false;
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            SceneManager.LoadScene(0);
        }

        if (move_vertical >= 0)
        {
            rb.AddForce(Vector3.forward * speed * move_vertical);
        }

        rb.AddForce(current_horizontal * speed * move_horizontal);
        rb.velocity = Vector3.Scale(rb.velocity, deceleration);

        

        rb.AddForce(current_gravity * 30);
        rb.AddForce(Vector3.forward * speed);

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            can_jump = true;
        }

        /*if (g_changed)
        {
            previous_direction = gravity_direction;
            g_changed = false;
        }*/

        head.transform.position = transform.position;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Right_wall"))
        {
            grounded = true;
            if (prev_tag != "Right_wall")
            {
                //g_changed = true;
                gravity_direction = 2;
                current_horizontal = right_horizontal;
                current_gravity = Vector3.right;
                jump_direction = Vector3.left;
                deceleration = new Vector3(1f, .5f, .5f);
                head.transform.rotation = Quaternion.Euler(0, -90, 45);
                prev_tag = "Right_wall";
            }
        }

        if (other.gameObject.CompareTag("Left_wall"))
        {
            grounded = true;
            if (prev_tag != "Left_wall")
            {
                //g_changed = true;
                gravity_direction = 4;
                current_horizontal = left_horizontal;
                current_gravity = Vector3.left;
                jump_direction = Vector3.right;
                deceleration = new Vector3(1f, .5f, .5f);
                head.transform.rotation = Quaternion.Euler(0, 90, -135);
                prev_tag = "Left_wall";
            }
        }

        if (other.gameObject.CompareTag("Top_wall"))
        {
            grounded = true;
            if (prev_tag != "Top_wall")
            {
                //g_changed = true;
                gravity_direction = 3;
                current_horizontal = top_horizontal;
                current_gravity = Vector3.up;
                jump_direction = Vector3.down;
                deceleration = new Vector3(.5f, 1f, .5f);
                head.transform.rotation = Quaternion.Euler(90, -135, 0);
                prev_tag = "Top_wall";
            }
        }

        if (other.gameObject.CompareTag("Bottom_wall"))
        {
            grounded = true;
            if (prev_tag != "Bottom_wall")
            {
                //g_changed = true;
                gravity_direction = 1;
                current_horizontal = bottom_horizontal;
                current_gravity = Vector3.down;
                jump_direction = Vector3.up;
                deceleration = new Vector3(.5f, 1f, .5f);
                head.transform.rotation = Quaternion.Euler(-90, -45, 0);
                prev_tag = "Bottom_wall";
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoubleDamage"))
        {
            other.gameObject.SetActive(false);
            m_shot = muzzle.GetComponent<RayGun>();
            m_shot.damage_amount *= 2;
            count += 1;
            //print("Doubled damage, from " + m_shot.damageAmount / 2 + " to " + m_shot.damageAmount);
        }

        if (other.gameObject.CompareTag("ExtraHealth"))
        {
            other.gameObject.SetActive(false);
            rb.gameObject.GetComponent<PlayerHealth>().current_health += 50;
            count += 1;
            //print("Player got extra health");

        }

        if (other.gameObject.CompareTag("SpeedUp"))
        {
            other.gameObject.SetActive(false);
            speed += 30;
            count += 1;
            //print("Player got more speed");
        }

        if (other.gameObject.CompareTag("DoubleXP"))
        {
            other.gameObject.SetActive(false);
            ScoreManager.score *= 2;
            count += 1;
            //print("Doubled XP, from " + ScoreManager.score / 2 + " to " + ScoreManager.score);
        }

        
    }

}

        

            