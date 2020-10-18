using UnityEngine;

// Place the script in the Camera-Control group in the component menu
[AddComponentMenu("Camera-Control/Smooth Follow CSharp")]

public class SmoothFollowLevel2 : MonoBehaviour
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
    public float height_damping = 2.0f;
    public float speed;

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        float wanted_height = target.position.y + height;
        float current_rotation_angle = transform.eulerAngles.y;
        float current_height = transform.position.y;

        // Damp the height
        current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion current_rotation = Quaternion.Euler(0, current_rotation_angle, 0);
        
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        

        // Set the height of the camera

        if (target.GetComponent<PlayerControlsLevel2>().current_gravity == Vector3.down)
        {
            transform.position = target.position;
            transform.position -= current_rotation * Vector3.forward * distance;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed);
            transform.position = new Vector3(transform.position.x, current_height, transform.position.z);
        }
        else if (target.GetComponent<PlayerControlsLevel2>().current_gravity == Vector3.right)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed);
            wanted_height = target.position.x - height;
            current_height = transform.position.x;

            // distance meters behind the target
            transform.position = target.position;
            transform.position -= current_rotation * Vector3.forward * distance;

            // Damp the height
            current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
            transform.position = new Vector3(current_height, transform.position.y, transform.position.z);
        }
        else if (target.GetComponent<PlayerControlsLevel2>().current_gravity == Vector3.up)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed);
            wanted_height = target.position.y - height;
            current_height = transform.position.y;

            // distance meters behind the target
            transform.position = target.position;
            transform.position -= current_rotation * Vector3.forward * distance;

            // Damp the height
            current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, current_height, transform.position.z);
        }
        else if (target.GetComponent<PlayerControlsLevel2>().current_gravity == Vector3.left)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 270), Time.deltaTime * speed);
            wanted_height = target.position.x + height;
            current_height = transform.position.x;

            // distance meters behind the target
            transform.position = target.position;
            transform.position -= current_rotation * Vector3.forward * distance;

            // Damp the height
            current_height = Mathf.Lerp(current_height, wanted_height, height_damping * Time.deltaTime);
            transform.position = new Vector3(current_height, transform.position.y, transform.position.z);
        }
    }
}