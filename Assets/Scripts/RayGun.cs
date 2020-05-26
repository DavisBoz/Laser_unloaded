using UnityEngine;

public class RayGun : MonoBehaviour
{
    public float shootRate;
    int shootableMask;
    public GameObject m_shotPrefab;
    public int damageAmount;
    RaycastHit hit;
    Ray shoot_ray;
    float range = 1000.0f;
    public AudioSource laserSound;

    private float m_shootRateTimeStamp;


    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
    }

        void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Time.time > m_shootRateTimeStamp)
            {
                laserSound.Play();
                shootRay();
                m_shootRateTimeStamp = Time.time + shootRate;
            }
        }

    }

    void shootRay()
    {
        shoot_ray.origin = transform.position;
        shoot_ray.direction = transform.forward;
        if (Physics.Raycast(shoot_ray, out hit, range, shootableMask))
        {
            
            GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage(damageAmount);
            }
            
            GameObject.Destroy(laser, 2f);

        }
        else if(Physics.Raycast(shoot_ray, out hit, range))
        {
            GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);
        }

    }



}
