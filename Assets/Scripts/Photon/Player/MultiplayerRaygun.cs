using UnityEngine;
using Photon.Pun;

public class MultiplayerRaygun : MonoBehaviourPun
{
    public float shoot_rate;
    public LayerMask can_shoot;
    public int damage_amount = 20;
    public GameObject bullet;
    RaycastHit hit;
    Ray shoot_ray;
    float range = 1000.0f;
    public GameObject collisionExplosion;
    public AudioClip laser;


    private float shoot_rate_time;
    private Vector2 finger_down_position;
    private Vector2 finger_up_position;


    

    void Update()
    {
        // transform.position += transform.forward * Time.deltaTime * 300f;// The step size is equal to speed times frame time.
        if (photonView.IsMine)
        {
            gameObject.tag = "LocalMuzzle";
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
        }
        else
            gameObject.tag = "Muzzle";
    }


    [PunRPC]
    void shootRay()
    {
        shoot_ray.origin = transform.position;
        shoot_ray.direction = transform.forward;
        AudioSource laser_sound = gameObject.AddComponent<AudioSource>();
        laser_sound.clip = laser;
        laser_sound.spatialBlend = 1;
        laser_sound.minDistance = 25;
        laser_sound.maxDistance = 100;
        laser_sound.volume = 0.41f;
        if (Physics.Raycast(shoot_ray, out hit, range, can_shoot))
        {
            laser_sound.Play();
            GameObject laser = Instantiate(bullet, transform.position, transform.rotation);
            laser.GetComponent<OnlineShotBehavior>().setTarget(hit.point);
            MultiplayerHealth enemy_health = hit.collider.gameObject.GetComponent<MultiplayerHealth>();
            if (enemy_health != null)
            { 
                enemy_health.TakeDamage(damage_amount);
            }
            GameObject.Destroy(laser,2f);

        }
        else if(Physics.Raycast(shoot_ray, out hit, range))
        {
            laser_sound.Play();
            GameObject laser = Instantiate(bullet, transform.position, transform.rotation);
            laser.GetComponent<OnlineShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);
        }
    }

    public void explode(Vector3 position, Quaternion rotation)
    {
        photonView.RPC("Explode", RpcTarget.All, position, rotation);
    }

    [PunRPC]
    void Explode(Vector3 position, Quaternion rotation)
    {
        if (collisionExplosion != null)
        {
            GameObject explosion = Instantiate(collisionExplosion, position, rotation);
            //Destroy(gameObject);
            GameObject.Destroy(explosion, 1f);
        }
    }

    
}
