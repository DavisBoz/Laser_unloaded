using UnityEngine;

public class RayGun : MonoBehaviour
{
    public float shoot_rate;
    int shootable_mask;
    public GameObject m_shot_prefab;
    public int damage_amount;
    RaycastHit hit;
    Ray shoot_ray;
    float range = 1000.0f;
    public AudioSource laser_sound;

    private float shoot_rate_time;


    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootable_mask = LayerMask.GetMask("Shootable");
    }

        void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.time > shoot_rate_time)
            {
                laser_sound.Play();
                shootRay();
                shoot_rate_time = Time.time + shoot_rate;
            }
        }

    }

    void shootRay()
    {
        shoot_ray.origin = transform.position;
        shoot_ray.direction = transform.forward;
        if (Physics.Raycast(shoot_ray, out hit, range, shootable_mask))
        {
            
            GameObject laser = GameObject.Instantiate(m_shot_prefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            EnemyHealth enemy_health = hit.collider.GetComponent<EnemyHealth>();

            // If the EnemyHealth component exist...
            if (enemy_health != null)
            {
                // ... the enemy should take damage.
                enemy_health.TakeDamage(damage_amount);
            }
            
            GameObject.Destroy(laser, 2f);

        }
        else if(Physics.Raycast(shoot_ray, out hit, range))
        {
            GameObject laser = GameObject.Instantiate(m_shot_prefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);
        }

    }



}
