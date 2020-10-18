 using UnityEngine;

public class HeadControl : MonoBehaviour
{

    
    private Rigidbody hb;

    
    void Start()
    {
        hb = GetComponent<Rigidbody>();
    }




    private void OnCollisionEnter(Collision other)
    {
        hb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void OnCollisionStay(Collision collision)
    {
        hb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

}



