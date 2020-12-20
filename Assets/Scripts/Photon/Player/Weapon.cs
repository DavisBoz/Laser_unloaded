using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;

public class Weapon : MonoBehaviourPun
{
    #region Variables
    public Transform muzzle;
    public LayerMask can_shoot;
    public int damage_amount = 20;
    public float shoot_rate;
    public GameObject bullet;
    RaycastHit hit;
    Ray shoot_ray;
    float range = 1000.0f;
    public AudioClip a_laser;
    public AudioSource hitmarker_sound;
    public Image hitmarker_image;

    private Color clear_white = new Color(1, 1, 1, 0);
    private float hitmarker_delay;
    private float shoot_rate_time;
    private Vector2 finger_down_position;
    private Vector2 finger_up_position;


    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        hitmarker_image.color = clear_white;
    }

    void Update()
    {
        if (Pause.paused && photonView.IsMine) return;
        if (photonView.IsMine)
        {
            foreach (Touch touch in Input.touches)
            {
                if ((touch.position.x > Screen.width * 0.33f) && (touch.position.y > Screen.height * 0.2f))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        finger_up_position = touch.position;
                        finger_down_position = touch.position;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        finger_down_position = touch.position;
                        if ((finger_up_position == finger_down_position) && (Time.time > shoot_rate_time))
                        {
                            photonView.RPC("shootRay", RpcTarget.All);
                            shoot_rate_time = Time.time + shoot_rate;
                        }
                    }
                }
            }
            if (hitmarker_delay > 0)
            {
                hitmarker_delay -= Time.deltaTime;
            }
            else if (hitmarker_image.color.a > 0)
            {
                hitmarker_image.color = Color.Lerp(hitmarker_image.color, clear_white, Time.deltaTime * hitmarker_delay);
            }
        }
    }
    #endregion

    #region Shooting
    [PunRPC]
    void shootRay()
    {
        shoot_ray.origin = muzzle.position;
        shoot_ray.direction = muzzle.forward;
        AudioSource laser_sound = gameObject.AddComponent<AudioSource>();
        laser_sound.clip = a_laser;
        laser_sound.spatialBlend = 1;
        laser_sound.minDistance = 25;
        laser_sound.maxDistance = 100;
        laser_sound.volume = 0.41f;
        if (Physics.Raycast(shoot_ray, out hit, range, can_shoot))
        {
            laser_sound.Play();
            GameObject laser = Instantiate(bullet, muzzle.position, muzzle.rotation);
            laser.GetComponent<OnlineShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);
            if (photonView.IsMine)
            {
                if (hit.collider.gameObject.layer == 11)
                {
                    bool applyDamage = false;

                    if (GameSettings.GameMode == GameMode.ARENA)
                    {
                        applyDamage = true;
                    }

                    if (GameSettings.GameMode == GameMode.DEATHMATCH)
                    {
                        if (hit.collider.transform.CompareTag("Head"))
                        {
                            if (hit.collider.transform.root.GetChild(0).gameObject.GetComponent<PlayerControllerOnline>().awayTeam != GameSettings.IsAwayTeam)
                            {
                                applyDamage = true;
                            }
                        }
                        else
                        {
                            if (hit.collider.gameObject.GetComponent<PlayerControllerOnline>().awayTeam != GameSettings.IsAwayTeam)
                            {
                                applyDamage = true;
                            }
                        }
                        
                    }

                    if (applyDamage)
                    {
                        if (hit.collider.transform.CompareTag("Head"))
                        {
                            int headshot = 50;
                            hit.collider.transform.root.GetChild(0).gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, headshot, PhotonNetwork.LocalPlayer.ActorNumber);
                        }
                        else
                        {
                            hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, damage_amount, PhotonNetwork.LocalPlayer.ActorNumber);
                        }
                        //show hitmarker
                        hitmarker_image.color = Color.white;
                        hitmarker_sound.Play();
                        hitmarker_delay = 0.5f;
                    }
                }
            }

        }
        else if(Physics.Raycast(shoot_ray, out hit, range))
        {
            laser_sound.Play();
            GameObject laser = Instantiate(bullet, muzzle.position, muzzle.rotation);
            laser.GetComponent<OnlineShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);
        }
    }

    [PunRPC]
    private void TakeDamage(int p_damage, int p_actor)
    {
        GetComponent<PlayerControllerOnline>().TakeDamage(p_damage, p_actor);
    }

    public void explode(string explosion, Vector3 position, Quaternion rotation)
    {
        photonView.RPC("Explode", RpcTarget.All, explosion, position, rotation);
    }

    [PunRPC]
    void Explode(string collisionExplosion, Vector3 position, Quaternion rotation)
    {
        GameObject explosion = Instantiate((GameObject)Resources.Load(collisionExplosion), position, rotation);
        Destroy(explosion, 1f);
    }
    #endregion

}
