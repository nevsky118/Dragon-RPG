using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    bool isInDirectMode = false;

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 0.5f;


    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDistanation, clickPoint;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDistanation = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) // G for gamepad
        {
            isInDirectMode = !isInDirectMode; // toggle mode
            currentDistanation = transform.position; // clear the clicktarget
        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement(); // mouse movement
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;
        thirdPersonCharacter.Move(movement, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            print("Cursor raycast hit layer: " + cameraRaycaster.currentLayerHit);
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDistanation = clickPoint;
                    currentDistanation = ShortDestination(clickPoint, walkMoveStopRadius);  // So not set in default case
                    break;
                case Layer.Enemy:
                    currentDistanation = ShortDestination(clickPoint, attackMoveStopRadius);  // So not set in default case
                    break;
                default:
                    print("Shouldn't be here");
                    return;
            }
        }
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDistanation - transform.position;
        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {
        // Draw movement gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDistanation);
        Gizmos.DrawSphere(currentDistanation, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        // Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }

}

