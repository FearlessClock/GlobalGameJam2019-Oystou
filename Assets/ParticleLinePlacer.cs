using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLinePlacer : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float lineSizeOffset;
    void Start()
    {
        float distanceBetweenPoints = (point1.position - point2.position).magnitude;
        float ratio = distanceBetweenPoints / this.transform.localScale.magnitude - lineSizeOffset;

        Vector3 direction = (point2.position - point1.position).normalized;
        float angle = Vector3.SignedAngle(direction, this.transform.rotation * Vector3.right, -Vector3.forward);
        this.transform.localScale = Vector3.one * ratio;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.position = point1.position + direction * distanceBetweenPoints / 2;

    }
}
