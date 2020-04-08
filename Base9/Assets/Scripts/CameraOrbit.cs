using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
    [Header("External Ref")]
    [SerializeField]
    private Transform focusPoint;

    [SerializeField]
    private float maxAngle;

    [SerializeField]
    private float height;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float damp;


    private bool bOrbiting;
    private Vector3 velocity;
    private Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = focusPoint.position + new Vector3(0, height, 0);
        rotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                bOrbiting = true;
                Debug.Log("Touch orbiting..");
            }
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) && bOrbiting)
        {
            bOrbiting = false;
            Debug.Log("Touch orbiting ended.");
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                bOrbiting = true;
                Debug.Log("Click orbiting...");
            }
        }
        else if (Input.GetMouseButtonUp(0) && bOrbiting)
        {
            bOrbiting = false;
            Debug.Log("Click orbiting ended.");
        }
    }

    /*
     * void LateUpdate()
    {
        // get velocity from mouse movement
        if (bOrbiting)
        {
            velocity.x += speed * Input.GetAxis("Mouse X");
            velocity.y += speed * Input.GetAxis("Mouse Y");
        }

        // rotate camera
        transform.RotateAround(focusPoint.position, Vector3.up, velocity.x);
        transform.RotateAround(focusPoint.position, Vector3.right, velocity.y);

        Quaternion quat = transform.rotation;
        Vector3 euler = transform.rotation.eulerAngles;
        if (euler.x < 90 - maxAngle)
            euler.x = 90 - maxAngle;
        else if (euler.x > 90 + maxAngle)
            euler.x = 90 + maxAngle;

        if (euler.y > maxAngle)
            euler.y = maxAngle;
        else if (euler.y < -maxAngle)
            euler.y = -maxAngle;

        quat.eulerAngles = euler;
        transform.rotation = quat;
        
        // damping
        velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * damp);
        velocity.y = Mathf.Lerp(velocity.y, 0, Time.deltaTime * damp);
    }
    */
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
