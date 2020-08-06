using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed;
    [SerializeField] Transform[] transforms = new Transform[4];
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform transform in transforms)
        {
            transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 targetVector;
            float x;
            float y;
            x = LimitedXPosition();
            y = LimitedYPosition();
            targetVector = new Vector3(x, y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetVector, smoothSpeed * Time.deltaTime);
        }
    }

    private float LimitedYPosition()
    {
        float y;
        if (target.position.y > transforms[0].position.y)
        {
            y = transforms[0].position.y;
        }
        else if (target.position.y < transforms[2].position.y)
        {
            y = transforms[2].position.y;
        }
        else
        {
            y = target.position.y;
        }

        return y;
    }
    private float LimitedXPosition()
    {
        float x;
        if (target.position.x < transforms[0].position.x)
        {
            x = transforms[0].position.x;
        }
        else if (target.position.x > transforms[1].position.x)
        {
            x = transforms[1].position.x;
        }
        else
        {
            x = target.position.x;
        }

        return x;
    }
}
