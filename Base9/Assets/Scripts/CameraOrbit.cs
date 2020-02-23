using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField]
    private float maxAngle;

    [SerializeField]
    private float height;

    [SerializeField]
    private Transform focusPoint;

    private bool bOrbiting;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = focusPoint.position + new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                bOrbiting = true;
                Debug.Log("Touch orbiting..");
            }
        }

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) && bOrbiting)
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
        if (Input.GetMouseButtonUp(0) && bOrbiting)
        {
            bOrbiting = false;
            Debug.Log("Click orbiting ended.");
        }
    }
}
