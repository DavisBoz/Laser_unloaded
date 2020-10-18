using UnityEngine;

public class PlatformZmorph : MonoBehaviour
{
    private float zpos = 1.02f;
    private float zmax;
    private float zmin = .01f;
    private Transform plane;
    bool growing = false;

    // Start is called before the first frame update
    void Start()
    {
        plane = gameObject.transform;
        zmax = plane.localScale.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        zpos = plane.localScale.z;
        if (growing)
        {
            plane.localScale = new Vector3(plane.localScale.x, plane.localScale.y, zpos + .01f);
        }
        else
        {
            plane.localScale = new Vector3(plane.localScale.x, plane.localScale.y, zpos - .01f);
        }
        if (zpos >= zmax)
        {
            growing = false;
        }
        else if (zpos <= zmin)
        {
            growing = true;
        }
    }
}

