using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FloatyControls : MonoBehaviourPunCallbacks
{
    #region Variables
    //Movement
    public float speed;
    public float min_distance_swipe;
    public GameObject body;
    public AudioSource pu_sound;
    private FixedJoystick joystick;
    public float rot_speed;
    public GameObject cam_parent;
    public GameObject r_arm;
    public Camera cam;
    public float distance = 6.0f;
    public float height = 2.0f;
    public float height_damping = 2.0f;
    public float cam_speed;
    [HideInInspector] public ProfileData player_profile;
    [HideInInspector] public bool awayTeam;
    public TextMeshPro user_header;
    public Transform obstruction;
    private Vector3 deceleration = new Vector3(.5f, 1f, .5f);
    private Rigidbody rb;
    private Vector2 finger_down_position;
    private Vector2 finger_up_position;
    float m_deadZone = 0.1f;
    public float m_hoverForce = 9.0f;
    public float m_hoverHeight = 2.0f;
    public GameObject[] m_hoverPoints;
    public float m_forwardAcl = 100.0f;
    public float m_backwardAcl = 25.0f;
    float m_currThrust = 0.0f;
    public float m_turnStrength = 10f;
    float m_currTurn = 0.0f;
    public GameObject m_leftAirBrake;
    public GameObject m_rightAirBrake;

    int m_layerMask;


    //Health
    public int starting_health = 100;                            // The amount of health the player starts the game with.
    public int current_health;                                   // The current health the player has.
    public Slider health_slider;                                 // Reference to the UI's health bar.
    public Image damage_image;
    public GameObject healthUI;
    public float flash_speed = 5f;                               // The speed the damage_image will fade at.
    public Color flash_colour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damage_image is set to, to flash.
    private TextMeshProUGUI user_text;
    public GameObject username;

    private ArenaManager manager;
    private bool is_dead;                                                // Whether the player is dead.
    private bool damaged;

    #endregion Variables

    #region UnityCallbacks
    private void Awake()
    {
        joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
        current_health = starting_health;
        is_dead = false;
        if (photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            m_layerMask = 1 << LayerMask.NameToLayer("Characters");
            m_layerMask = ~m_layerMask;
        }
    }

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<ArenaManager>();
        cam_parent.SetActive(photonView.IsMine);
        pu_sound.gameObject.SetActive(photonView.IsMine);
        healthUI.SetActive(photonView.IsMine);
        if (photonView.IsMine)
        {
            user_text = GameObject.Find("HUD/Username/Text").GetComponent<TextMeshProUGUI>();
            user_text.text = PhotonLauncher.my_profile.username;
            photonView.RPC("SyncProfile", RpcTarget.All, PhotonLauncher.my_profile.username, PhotonLauncher.my_profile.level, PhotonLauncher.my_profile.xp, PhotonLauncher.my_profile.color);
            if (GameSettings.GameMode == GameMode.DEATHMATCH)
            {
                photonView.RPC("SyncTeam", RpcTarget.All, GameSettings.IsAwayTeam);
            }
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.layer = 11;
            ChangeLayerRecursively(transform, 11);
        }
    }

    private void ChangeLayerRecursively(Transform p_trans, int p_layer)
    {
        p_trans.gameObject.layer = p_layer;
        foreach (Transform t in p_trans) ChangeLayerRecursively(t, p_layer);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            username.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            user_header.color = new Color(1, 1, 1, 0);
            if (Pause.paused) return;
            // Main Thrust
            m_currThrust = 0.0f;
            float aclAxis = joystick.Vertical;
            if (aclAxis > m_deadZone)
                m_currThrust = aclAxis * m_forwardAcl;
            else if (aclAxis < -m_deadZone)
                m_currThrust = aclAxis * m_backwardAcl;

            // Turning
            m_currTurn = 0.0f;
            float turnAxis = joystick.Horizontal;
            if (Mathf.Abs(turnAxis) > m_deadZone)
                m_currTurn = turnAxis;
            if (damaged && current_health > 10)
            {
                damage_image.color = flash_colour;
            }
            else
            {
                damage_image.color = Color.Lerp(damage_image.color, Color.clear, flash_speed * Time.deltaTime);
            }
            damaged = false;
        }
    }

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            //CamControls();
            ViewObstructed();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (Pause.paused) return;
            //  Hover Force
            RaycastHit hit;
            for (int i = 0; i < m_hoverPoints.Length; i++)
            {
                var hoverPoint = m_hoverPoints[i];
                if (Physics.Raycast(hoverPoint.transform.position,
                                    -Vector3.up, out hit,
                                    m_hoverHeight,
                                    m_layerMask))
                    rb.AddForceAtPosition(Vector3.up
                      * m_hoverForce
                      * (1.0f - (hit.distance / m_hoverHeight)),
                                              hoverPoint.transform.position);
                else
                {
                    if (transform.position.y > hoverPoint.transform.position.y)
                        rb.AddForceAtPosition(
                          hoverPoint.transform.up * m_hoverForce,
                          hoverPoint.transform.position);
                    else
                        rb.AddForceAtPosition(
                          hoverPoint.transform.up * -m_hoverForce,
                          hoverPoint.transform.position);
                }
            }

            // Forward
            if (Mathf.Abs(m_currThrust) > 0)
                rb.AddForce(transform.forward * m_currThrust);

            // Turn
            if (m_currTurn > 0)
            {
                rb.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
            }
            else if (m_currTurn < 0)
            {
                rb.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
            }
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
                        Rotating(touch);
                    }
                }
                else if ((touch.position.x > Screen.width * 0.33f) && (touch.position.y < Screen.height * 0.2f))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        finger_up_position = touch.position;
                        finger_down_position = touch.position;
                    }
                }
            }
            //rb.velocity = Vector3.Scale(rb.velocity, deceleration);
            rb.AddForce(Vector3.down * 15);
        }
    }
    #endregion

    #region Health
    public void TakeDamage(int amount, int p_actor)
    {
        if (photonView.IsMine)
        {
            damaged = true;
            current_health -= amount;
            health_slider.value = current_health;
            if (current_health <= 0 && !is_dead)
            {
                cam_parent.SetActive(false);
                manager.Respawn();
                manager.ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
                if (p_actor >= 0) manager.ChangeStat_S(p_actor, 0, 1);
                PhotonNetwork.Destroy(transform.root.gameObject);
            }
        }

    }

    [PunRPC]
    void fallDamage()
    {
        if (photonView.IsMine)
        {
            damaged = true;
            current_health = 0;
            health_slider.value = current_health;
            if (current_health <= 0 && !is_dead)
            {
                cam_parent.SetActive(false);
                manager.Respawn();
                manager.ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
                PhotonNetwork.Destroy(transform.root.gameObject);
            }
        }
    }
    #endregion

    #region Movement

    void OnDrawGizmos()
    {

        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            var hoverPoint = m_hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position,
                                -Vector3.up, out hit,
                                m_hoverHeight,
                                m_layerMask))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hoverPoint.transform.position,
                               hoverPoint.transform.position - Vector3.up * m_hoverHeight);
            }
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

    public void Rotating(Touch touch)
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                r_arm.transform.Rotate(touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f, 0f);
                cam.transform.Rotate(-touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f, 0f);
                float z = cam.transform.eulerAngles.z;
                cam.transform.Rotate(0, 0, -z);
            }
            else
            {
                body.transform.Rotate(0f, touch.deltaPosition.y * Time.deltaTime * rot_speed, 0f);
                cam.transform.Rotate(0f, touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f);
                username.transform.Rotate(0f, touch.deltaPosition.x * Time.deltaTime * rot_speed, 0f);
                float z = cam.transform.eulerAngles.z;
                cam.transform.Rotate(0, 0, -z);
            }
        }
    }
    #endregion

    #region Camera
    /*public void CamControls()
    {
        float wanted_height = head.transform.position.y + height;
        float current_height = cam.transform.position.y;
        float current_rotation_angle = cam.transform.eulerAngles.y;
        Quaternion current_rotation = Quaternion.Euler(0, current_rotation_angle, 0);
        current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
        cam.transform.position = head.transform.position;
        cam.transform.position -= current_rotation * Vector3.forward * distance;
        cam.transform.position = new Vector3(cam.transform.position.x, current_height, cam.transform.position.z);
    }*/

    public void ViewObstructed()
    {
        if (Physics.Raycast(cam.transform.position, transform.position - cam.transform.position, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                if (hit.collider.gameObject.tag == "Environment" && obstruction != null && obstruction != hit.transform)
                {
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    obstruction = hit.collider.transform;
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
                else if (hit.collider.gameObject.tag == "Environment" && obstruction == null)
                {
                    obstruction = hit.collider.transform;
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
                else if (hit.collider.gameObject.tag == "Environment" && obstruction == hit.transform) { }
                else if (obstruction == null) { }
                else
                {
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    obstruction = null;
                }
            }
        }
    }

    #endregion

    #region Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Reset")
        {
            photonView.RPC("fallDamage", RpcTarget.All);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ExtraHealth"))
        {
            pu_sound.Play();
            current_health += 50;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("SpeedUp"))
        {
            pu_sound.Play();
            speed += 30;
            other.gameObject.SetActive(false);
        }

    }
    #endregion Collisions

    public void TrySync()
    {
        if (!photonView.IsMine) return;
        photonView.RPC("SyncProfile", RpcTarget.All, PhotonLauncher.my_profile.username, PhotonLauncher.my_profile.level, PhotonLauncher.my_profile.xp, PhotonLauncher.my_profile.color);
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            photonView.RPC("SyncTeam", RpcTarget.All, GameSettings.IsAwayTeam);
        }
    }

    [PunRPC]
    private void SyncProfile(string p_username, int p_level, int p_xp, string col)
    {
        player_profile = new ProfileData(p_username, p_level, p_xp, col);
        user_header.text = player_profile.username;
    }

    [PunRPC]
    private void SyncTeam(bool p_awayTeam)
    {
        awayTeam = p_awayTeam;

        if (awayTeam)
        {
            user_header.color = Color.red;
        }
        else
        {
            user_header.color = Color.blue;
        }
    }


}

