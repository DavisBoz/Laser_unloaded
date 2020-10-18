using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerControllerOnline : MonoBehaviourPun
{

    public float speed;
    public float jump_height;
    public float min_distance_swipe;
    public static int gravity_direction = 1;
    public static bool g_changed = false;
    public GameObject head;
    public GameObject head2;
    public GameObject muzzle;
    public AudioSource pu_sound;
    private FixedJoystick joystick;
    public Camera cam;
    public GameObject cam_parent;
    public float rot_speed;
    public Vector3 current_gravity = Vector3.down;
    public float distance = 6.0f;
    public float height = 2.0f;
    public float height_damping = 2.0f;
    public float cam_speed;
    bool grounded;
    bool right;
    bool left;
    bool bottom;
    bool top;

    private Vector3 jump_direction = Vector3.up;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private Rigidbody rb;
    private Vector2 finger_down_position;
    private Vector2 finger_up_position;

    private void Awake()
    {
        joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
    }
    void Start()
    {
            cam_parent.gameObject.SetActive(photonView.IsMine);
            pu_sound.gameObject.SetActive(photonView.IsMine);
            rb = GetComponent<Rigidbody>();
            if(!photonView.IsMine) gameObject.layer = 11;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (joystick.Horizontal != 0f || joystick.Vertical != 0f)
            {
                Rolling();
            }
            head.transform.position = transform.position;
        }
    }

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            float wanted_height = gameObject.transform.position.y + height;
            float current_height = cam.transform.position.y;
            float current_rotation_angle = cam.transform.eulerAngles.y;
            Quaternion current_rotation = Quaternion.Euler(0, current_rotation_angle, 0);
            current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
            if (bottom)
            {
                cam.transform.position = transform.position;
                cam.transform.position -= current_rotation * Vector3.forward * distance;
                cam.transform.position = new Vector3(cam.transform.position.x, current_height, cam.transform.position.z);
            }
            else if (left)
            {
                wanted_height = gameObject.transform.position.x + height;
                current_height = cam.transform.position.x;
                cam.transform.position = gameObject.transform.position;
                cam.transform.position -= current_rotation * Vector3.forward * distance;
                current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
                cam.transform.position = new Vector3(current_height, cam.transform.position.y, cam.transform.position.z);
            }
            else if (right | top)
            {
                wanted_height = gameObject.transform.position.x - height;
                current_height = cam.transform.position.x;
                cam.transform.position = gameObject.transform.position;
                cam.transform.position -= current_rotation * Vector3.forward * distance;
                current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
                cam.transform.position = new Vector3(current_height, cam.transform.position.y, cam.transform.position.z);
            }
           
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x > Screen.width * 0.33f && (touch.position.y > Screen.height * 0.2f))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        finger_up_position = touch.position;
                        finger_down_position = touch.position;
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        finger_down_position = touch.position;
                        if (SwipeDistanceCheckMet())
                        {
                            if (IsVerticalSwipe())
                            {
                                if (bottom)
                                {
                                    //head2.transform.RotateAround(head2.transform.position, head2.transform.up, touch.deltaPosition.y * Time.deltaTime * rot_speed);
                                    //cam.transform.RotateAround(cam.transform.position, -cam.transform.right, touch.deltaPosition.y * Time.deltaTime * rot_speed);
                                    head2.transform.Rotate(0f, touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f);
                                    cam.transform.Rotate(-touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z);
                                }
                                else if (right)
                                {
                                    head2.transform.Rotate(0f, touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f);
                                    cam.transform.Rotate(-touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z+90);
                                }
                                else if (left)
                                {
                                    head2.transform.Rotate(0f, touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f);
                                    cam.transform.Rotate(-touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z+270);
                                }
                                else if (top)
                                {
                                    head2.transform.Rotate(0f, touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f);
                                    cam.transform.Rotate(-touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z+180);
                                }
                            }
                            else
                            {
                                if (bottom)
                                {
                                    head.transform.Rotate(0f, 0f, touch.deltaPosition.x * Time.deltaTime * rot_speed);
                                    cam.transform.Rotate(0f, touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z);
                                }
                                else if(right)
                                {
                                    head.transform.Rotate(0f, 0f, touch.deltaPosition.x * Time.deltaTime * rot_speed);
                                    cam.transform.Rotate(0f, touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z+90);
                                }
                                else if (left)
                                {
                                    head.transform.Rotate(0f, 0f, touch.deltaPosition.x * Time.deltaTime * rot_speed);
                                    cam.transform.Rotate(0f, touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z+270);
                                }
                                else if (top)
                                {
                                    head.transform.Rotate(0f, 0f, touch.deltaPosition.x * Time.deltaTime * rot_speed);
                                    cam.transform.Rotate(0f, touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f);
                                    float z = cam.transform.eulerAngles.z;
                                    cam.transform.Rotate(0, 0, -z+180);
                                }
                            }
                        }
                    }
                }
                else if((touch.position.x > Screen.width * 0.33f) && (touch.position.y < Screen.height * 0.2f))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        finger_up_position = touch.position;
                        finger_down_position = touch.position;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        finger_down_position = touch.position;
                        if ((finger_up_position == finger_down_position) && (grounded))
                        {
                            rb.velocity = new Vector3(0, 0, 0);
                            rb.AddForce(jump_direction * jump_height, ForceMode.Impulse);
                            grounded = false;
                        }
                    }
                }
            }
            rb.velocity = Vector3.Scale(rb.velocity, deceleration);
            rb.AddForce(current_gravity * 15);
        }
    }


    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > min_distance_swipe || HorizontalMovementDistance() > min_distance_swipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(finger_down_position.y - finger_up_position.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(finger_down_position.x - finger_up_position.x);
    }

    public void Rolling()
    {
        Vector3 forceDirection = cam.transform.forward;
        forceDirection = new Vector3(forceDirection.x, 0, forceDirection.z);
        if (current_gravity == Vector3.down)
        {

            Vector3 TurnDirection = cam.transform.right;
            TurnDirection = new Vector3(TurnDirection.x, 0, TurnDirection.z);
            rb.AddForce(forceDirection.normalized * speed * (joystick.Vertical));
            rb.AddForce(TurnDirection.normalized * speed * (joystick.Horizontal));
        }
        else if (current_gravity == Vector3.right)
        {

            Vector3 TurnDirection = -cam.transform.up;
            TurnDirection = new Vector3(0, TurnDirection.x, TurnDirection.z);
            rb.AddForce(forceDirection.normalized * speed * (joystick.Vertical));
            rb.AddForce(TurnDirection.normalized * speed * (joystick.Horizontal));
        }
        else if (current_gravity == Vector3.left)
        {

            Vector3 TurnDirection = -cam.transform.up;
            TurnDirection = new Vector3(0, TurnDirection.x, TurnDirection.z);
            rb.AddForce(forceDirection.normalized * speed * (joystick.Vertical));
            rb.AddForce(TurnDirection.normalized * speed * (joystick.Horizontal));
        }
        else if (current_gravity == Vector3.up)
        {

            Vector3 TurnDirection = -cam.transform.right;
            TurnDirection = new Vector3(TurnDirection.x, 0, TurnDirection.z);
            rb.AddForce(forceDirection.normalized * speed * (joystick.Vertical));
            rb.AddForce(TurnDirection.normalized * speed * (joystick.Horizontal));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Right_wall"))
        {
            grounded = true;
            left = false;
            bottom = false;
            top = false;
            if (!right)
            {
                gravity_direction = 2;
                current_gravity = Vector3.right;
                jump_direction = Vector3.left;
                deceleration = new Vector3(1f, .5f, .5f);
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed);
                head.transform.rotation = Quaternion.Euler(0, -90, 45);
                right = true;
            }
        }

        if (other.gameObject.CompareTag("Left_wall"))
        {
            grounded = true;
            right = false;
            bottom = false;
            top= false;
            if (!left)
            {
                gravity_direction = 4;
                current_gravity = Vector3.left;
                jump_direction = Vector3.right;
                deceleration = new Vector3(1f, .5f, .5f);
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(0, 0, 270), Time.deltaTime * speed);
                head.transform.rotation = Quaternion.Euler(0, 90, -135);
                left = true;
            }
        }

        if (other.gameObject.CompareTag("Top_wall"))
        {
            grounded = true;
            right = false;
            left = false;
            bottom = false;
            if (!top)
            {
                gravity_direction = 3;
                current_gravity = Vector3.up;
                jump_direction = Vector3.down;
                deceleration = new Vector3(.5f, 1f, .5f);
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed);
                head.transform.rotation = Quaternion.Euler(90, -135, 0);
                top = true;
            }
        }

        if (other.gameObject.CompareTag("Bottom_wall"))
        {
            grounded = true;
            right = false;
            left = false;
            top = false;
            if (!bottom)
            {
                gravity_direction = 1;
                current_gravity = Vector3.down;
                jump_direction = Vector3.up;
                deceleration = new Vector3(.5f, 1f, .5f);
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed);
                head.transform.rotation = Quaternion.Euler(-90, -45, 0);
                bottom = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
        
    }

}



