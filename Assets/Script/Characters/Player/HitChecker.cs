using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitChecker : MonoBehaviour
{
    [SerializeField]float radius;
    [SerializeField] LayerMask hitMask;
    [SerializeField] bool isHit;

    #region Properties
    public float Radius
    {
        get { return radius; }
    }

    public LayerMask HitMask
    {
        get { return hitMask; }
    }

    public bool IsHit
    {
        get { return isHit; }
        set { isHit = value; }
    }
    #endregion

    // Update is called once per frame
    private void Update()
    {
        HitCheck();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void HitCheck()
    {
        isHit = Physics2D.OverlapCircle(transform.position, radius, hitMask);
    }
}
