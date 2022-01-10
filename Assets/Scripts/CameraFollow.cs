using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    //setting up variables for position offset und movement values
    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform followedObject;
    private Vector3 toPosition;

    // Called after Update()
    void LateUpdate()
    {
        //calculating target position:
        //start at followed object position, add offset in up direction to
        //it and subtract distanceAway times followedObject.forward to move it behind the camera
        toPosition = followedObject.position + Vector3.up * distanceUp - followedObject.forward * distanceAway;
        //update position with lerp to create a smooth interpolation.
        //Often important when moving a camera.
        //moving from current position to toPosition, interpolated over time and smooth scale
        transform.position = Vector3.Lerp(
            transform.position,
            toPosition,
            Time.deltaTime * smooth);

        //LookAt() function applied on transform makes
        //the forward vector face the position of the target object
        transform.LookAt(followedObject);
    }
}
