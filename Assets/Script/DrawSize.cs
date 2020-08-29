using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSize : MonoBehaviour
{
    [SerializeField] Vector2 viewRange;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, viewRange);
    }
}
