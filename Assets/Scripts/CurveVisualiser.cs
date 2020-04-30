using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CurveVisualiser : MonoBehaviour
{
    // config variables
    [SerializeField] [Tooltip("If this is not the first curve in a path, drag previous curve here.")]
                     GameObject previousCurve = null;
    [SerializeField] Transform[] controlPoints = new Transform[4];
    [SerializeField] int numberOfSpheres = 20;
    [SerializeField] float gizmoRadius = 0.25f;

    private Vector2 gizmosPosition;

    // create cache
    Transform ctrlPoint0;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += (1f / numberOfSpheres))
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                             3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                             3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                             Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, gizmoRadius);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
                        new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
                        new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }

    private void Update()
    {
        SetStartControlPoint();
    }

    private void SetStartControlPoint()
    {
        ctrlPoint0 = transform.Find("Ctrl Point 0");

        if (previousCurve)
        {
            ctrlPoint0.transform.position = previousCurve.GetComponent<CurveVisualiser>().GetLastControlPoint().transform.position;
        }
    }

    public Transform GetLastControlPoint() { return transform.Find("Ctrl Point 3"); }

}
