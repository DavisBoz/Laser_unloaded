using UnityEngine;
using Photon;
using Photon.Pun;

public class SuperSync : MonoBehaviourPun, IPunObservable
{
    public Vector3 obj_position;
    public Vector3 obj_scale;
    public Quaternion obj_rotation;
    private Rigidbody rb;
    private float lerp_speed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            if (rb.velocity.magnitude > 0)
            {
                UpdateTransform();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.transform.position);
            stream.SendNext(rb.transform.localScale);
            stream.SendNext(rb.transform.rotation);
        }
        else if (stream.IsReading)
        {
            obj_position = (Vector3)stream.ReceiveNext();
            obj_scale = (Vector3)stream.ReceiveNext();
            obj_rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void UpdateTransform()
    {
        rb.transform.position = Vector3.Lerp(rb.transform.position, obj_position, lerp_speed * Time.deltaTime);
        rb.transform.localScale = Vector3.Lerp(rb.transform.localScale, obj_scale, lerp_speed * Time.deltaTime);
        rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, obj_rotation, lerp_speed * Time.deltaTime);
    }
}
