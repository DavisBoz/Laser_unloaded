using UnityEngine;
using System.Collections;

// Place the script in the Camera-Control group in the component menu
[AddComponentMenu("Camera-Control/Smooth Follow CSharp")]

public class SmoothFollow : MonoBehaviour
{
    /*
    This camera smoothes out rotation around the y-axis and height.
    Horizontal Distance to the target is always fixed.

    There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

    For every of those smoothed values we calculate the wanted value and the current value.
    Then we smooth it using the Lerp function.
    Then we apply the smoothed values to the transform's position.
    */

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we 
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        float wantedHeight = target.position.y + height;
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        

        // Set the height of the camera

        if (target.GetComponent<PlayerControls>().current_gravity == Vector3.down)
        {
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        }
        else if (target.GetComponent<PlayerControls>().current_gravity == Vector3.right)
        {
            print("right wall");
            transform.rotation = Quaternion.Euler(0, 0, 90);
            wantedHeight = target.position.x - height;
            currentHeight = transform.position.x;

            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            transform.position = new Vector3(currentHeight, transform.position.y, transform.position.z);
        }
        else if (target.GetComponent<PlayerControls>().current_gravity == Vector3.up)
        {
            print("top wall");
            transform.rotation = Quaternion.Euler(0, 0, 180);
            wantedHeight = target.position.y - height;
            currentHeight = transform.position.y;

            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        }
        else if (target.GetComponent<PlayerControls>().current_gravity == Vector3.left)
        {
            print("left wall");
            transform.rotation = Quaternion.Euler(0, 0, 270);
            wantedHeight = target.position.x + height;
            currentHeight = transform.position.x;

            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            transform.position = new Vector3(currentHeight, transform.position.y, transform.position.z);
        }
    }
}