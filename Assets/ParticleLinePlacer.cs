using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLinePlacer : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float lineSizeOffset;
    public ParticleSystem parts;
    public bool isActive;
    public SwitchController activeSwitch;
    void Start()
    {
        activeSwitch.OnCloseToSwitch += OnCloseToSwitch;
        CalculatePlacement();
    }

    private void OnCloseToSwitch(int ID, bool enter)
    {
        if (enter)
        {
            parts.Play();
        }
        else
        {
            parts.Stop();
        }
    }

    private void Update()
    {
        CalculatePlacement();
    }

    public void CalculatePlacement()
    {
        float distanceBetweenPoints = (point1.position - point2.position).magnitude;
        float ratio = distanceBetweenPoints / 1.8f - lineSizeOffset;
        Vector3 direction = (point2.position - point1.position).normalized;
        float angle = Vector3.SignedAngle(direction, Vector3.right, -Vector3.forward);
        this.transform.localScale = Vector3.one * ratio;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.position = point1.position + direction * distanceBetweenPoints / 2;
    }
}
