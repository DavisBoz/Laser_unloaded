using UnityEngine;

public class PlatformXmorph : MonoBehaviour
{
    private float xpos = 2f;
    private float xmax;
    private float xmin = .2f;
    private Transform plane;
    bool growing = false;

    // Start is called before the first frame update
    void Start()
    {
        plane = gameObject.transform;
        xmax = plane.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xpos = plane.localScale.x;
        if (growing)
        {
            plane.localScale = new Vector3(xpos + .01f, plane.localScale.y, plane.localScale.z);
        }
        else
        {
            plane.localScale = new Vector3(xpos - .01f, plane.localScale.y, plane.localScale.z);
        }
        if (xpos >= xmax)
        {
            growing = false;
        }
        else if (xpos <= xmin)
        {
            growing = true;
        }
    }
}
