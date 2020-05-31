using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float mult;


    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime  * mult);
    }
}
