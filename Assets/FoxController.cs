using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    Vector3 target;
    public float randomMovingDistance;
    public float closeChangeDirectionDistance;
    Vector3 initPosition;
    public float speed;
    public float positionWaitTime;

    private bool calculatingNewPosition = false;

    private void Start()
    {
        initPosition = this.transform.position;
        target = initPosition + Random.insideUnitSphere * randomMovingDistance;
        target.z = initPosition.z;
    }

    private void Update()
    {
        if (!calculatingNewPosition && Vector3.Distance(target, this.transform.position) < closeChangeDirectionDistance)
        {
            StartCoroutine("WaitAtPosition");
        }

        this.transform.position += (target - this.transform.position).normalized * speed * Time.deltaTime;
    }

    private IEnumerator WaitAtPosition()
    {
        calculatingNewPosition = true;
        yield return new WaitForSeconds(positionWaitTime);
        target = initPosition + Random.insideUnitSphere * randomMovingDistance;
        target.z = initPosition.z;
        this.transform.localScale = new Vector3(Mathf.Sign((target - this.transform.position).normalized.x), 1, 1);
        calculatingNewPosition = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(initPosition, randomMovingDistance);
        Gizmos.DrawSphere(target, 0.4f);
    }
}
