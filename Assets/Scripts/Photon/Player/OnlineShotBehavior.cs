using UnityEngine;

public class OnlineShotBehavior : MonoBehaviour
{
    public Vector3 m_target;
    public GameObject collisionExplosion;
    public float speed;

    // Update is called once per frame
    void Update()
    {
            // transform.position += transform.forward * Time.deltaTime * 300f;// The step size is equal to speed times frame time.
            float step = speed * Time.deltaTime;

            if (m_target != null)
            {
                if (transform.position == m_target)
                {
                    GameObject.FindGameObjectWithTag("LocalMuzzle").GetComponent<MultiplayerRaygun>().explode(transform.position, transform.rotation);
                    Destroy(gameObject);
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, m_target, step);
            }
    }


    public void setTarget(Vector3 target)
    {
        m_target = target;
    }

}
