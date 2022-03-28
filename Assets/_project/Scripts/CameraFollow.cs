using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;

    private bool allowFollow;
    
    void Start()
    {
        if (objectToFollow != null)
            allowFollow = true;
    }
    
    void Update()
    {
        if (allowFollow)
        {
            var followTransform = objectToFollow.transform.position;
            var cameraTransform = transform;
            cameraTransform.position = new Vector3(followTransform.x, followTransform.y, cameraTransform.position.z);
        }
    }
}
