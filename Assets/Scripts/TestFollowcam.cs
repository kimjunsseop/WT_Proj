using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollowcam: MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    public float smoothSpeed = 10f;
    void Start()
    {
        if(target != null)
        {
            offset = transform.position - target.position;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
