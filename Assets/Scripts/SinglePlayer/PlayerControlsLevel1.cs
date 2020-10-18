 using UnityEngine;

public class PlayerControlsLevel1 : MonoBehaviour
{

    public float speed;
    public float jump_height;
    public float min_distance_swipe;
    public static int gravity_direction = 1;
    public static bool g_changed = false;
    public GameObject head;
    public GameObject muzzle;
    public GameObject bottom;
    public GameObject top;
    public GameObject left;
    public GameObject right;
    public AudioSource pu_sound;
    public Vector3 current_gravity = Vector3.down;
    RayGun m_shot;
    bool grounded;

    private Vector3 jump_direction = Vector3.up;
    private Vector3 right_horizontal = Vector3.up;
    private Vector3 top_horizontal = Vector3.left;
    private Vector3 left_horizontal = Vector3.down;
    private Vector3 bottom_horizontal = Vector3.right;
    private Vector3 current_horizontal = Vector3.right;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private Rigidbody rb;

    private Vector2 finger_down_position;
    private Vector2 finger_up_position;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
            head.transform.position = transform.position;
    }

    void FixedUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                finger_up_position = touch.position;
                finger_down_position = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                finger_down_position = touch.position;
                if (current_horizontal == Vector3.right)
                {
                    transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * 0.005f, transform.position.y, transform.position.z);
                }
                else if (current_horizontal == Vector3.left)
                {
                    transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * -0.005f, transform.position.y, transform.position.z);
                }
                else if (current_horizontal == Vector3.up)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + touch.deltaPosition.x * 0.005f, transform.position.z);
                }
                else if (current_horizontal == Vector3.down)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + touch.deltaPosition.x * -0.005f, transform.position.z);
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                finger_down_position = touch.position;
                if ((Mathf.Abs(finger_down_position.y - finger_up_position.y)) > (Mathf.Abs(finger_down_position.x - finger_up_position.x)) && grounded && ((finger_down_position.y - finger_up_position.y) > min_distance_swipe))
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    rb.AddForce(jump_direction * jump_height, ForceMode.Impulse);
                    grounded = false;
                }
            }
        }


        rb.velocity = Vector3.Scale(rb.velocity, deceleration);

        rb.AddForce(current_gravity * 15);
        rb.AddForce(Vector3.forward * speed);


    }



private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Right_wall"))
        {
            grounded = true;
            gravity_direction = 2;
            current_horizontal = right_horizontal;
            current_gravity = Vector3.right;
            jump_direction = Vector3.left;
            deceleration = new Vector3(1f, .5f, .5f);
            head.transform.rotation = Quaternion.Euler(0, -90, 45);
            bottom.SetActive(false);
            top.SetActive(false);
            right.SetActive(true);
        }

        if (other.gameObject.CompareTag("Left_wall"))
        {
            grounded = true;
            gravity_direction = 4;
            current_horizontal = left_horizontal;
            current_gravity = Vector3.left;
            jump_direction = Vector3.right;
            deceleration = new Vector3(1f, .5f, .5f);
            head.transform.rotation = Quaternion.Euler(0, 90, -135);
            bottom.SetActive(false);
            top.SetActive(false);
            left.SetActive(true);
        }

        if (other.gameObject.CompareTag("Top_wall"))
        {
            grounded = true;
            gravity_direction = 3;
            current_horizontal = top_horizontal;
            current_gravity = Vector3.up;
            jump_direction = Vector3.down;
            deceleration = new Vector3(.5f, 1f, .5f);
            head.transform.rotation = Quaternion.Euler(90, -135, 0);
            left.SetActive(false);
            right.SetActive(false);
            top.SetActive(true);
        }

        if (other.gameObject.CompareTag("Bottom_wall"))
        {
            grounded = true;
            gravity_direction = 1;
            current_horizontal = bottom_horizontal;
            current_gravity = Vector3.down;
            jump_direction = Vector3.up;
            deceleration = new Vector3(.5f, 1f, .5f);
            head.transform.rotation = Quaternion.Euler(-90, -45, 0);
            left.SetActive(false);
            right.SetActive(false);
            top.SetActive(false);
            bottom.SetActive(true);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoubleDamage"))
        {
            pu_sound.Play();
            other.gameObject.SetActive(false);
            m_shot = muzzle.GetComponent<RayGun>();
            m_shot.damage_amount *= 2;
            ScoreManager.pickups_used += 1;
        }

        if (other.gameObject.CompareTag("ExtraHealth"))
        {
            pu_sound.Play();
            other.gameObject.SetActive(false);
            rb.gameObject.GetComponent<PlayerHealth>().current_health += 50;
            ScoreManager.pickups_used += 1;
        }

        if (other.gameObject.CompareTag("SpeedUp"))
        {
            pu_sound.Play();
            other.gameObject.SetActive(false);
            speed += 30;
            ScoreManager.pickups_used += 1;
        }

        if (other.gameObject.CompareTag("DoubleXP"))
        {
            pu_sound.Play();
            other.gameObject.SetActive(false);
            ScoreManager.score *= 2;
            ScoreManager.pickups_used += 1;
        }

        
    }

}



