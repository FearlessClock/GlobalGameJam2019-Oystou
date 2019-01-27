using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterFlyController : MonoBehaviour
{
    Vector3 target;
    public float randomMovingDistance;
    public float closeChangeDirectionDistance;
    Vector3 initPosition;
    public float speed;

    private void Start()
    {
        initPosition = this.transform.position;
        target = initPosition + Random.insideUnitSphere * randomMovingDistance;
        target.z = initPosition.z;
    }

    private void Update()
    {
        if(Vector3.Distance(target, this.transform.position) < closeChangeDirectionDistance)
        {
            target = initPosition + Random.insideUnitSphere * randomMovingDistance;
            target.z = initPosition.z;
        }

        this.transform.position += (target - this.transform.position).normalized * speed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(initPosition, randomMovingDistance);
        Gizmos.DrawSphere(target, 0.4f);
    }
}
