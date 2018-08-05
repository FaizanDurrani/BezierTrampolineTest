using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
    [SerializeField] private Transform _base;
    [SerializeField] private int _subDivisions;
    [SerializeField] private float _topWidth, _bottomWidth;
    private LineRenderer _lineRenderer;

    private float _halfWidth;
    private float _bHalfWidth;

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _halfWidth = _topWidth / 2;
        _bHalfWidth = _bottomWidth / 2;

        var point1 = transform.position + (Vector3.left * _halfWidth);
        var point2 = _base.position + (Vector3.left * _bHalfWidth);
        var point3 = _base.position + (Vector3.right * _bHalfWidth);
        var point4 = transform.position + (Vector3.right * _halfWidth);

        point2.x = Mathf.Clamp(point2.x, point1.x, point4.x);
        point3.x = Mathf.Clamp(point3.x, point1.x, point4.x);

        Vector3[] bPoints = {point1, point2, point3, point4};

        _lineRenderer.positionCount = _subDivisions + 1;
        for (int i = 0; i <= _subDivisions; i++)
            _lineRenderer.SetPosition(i, GetPoint(bPoints, 1f / _subDivisions * i));
    }

    private Vector3 GetPoint(Vector3[] pts, float t)
    {
        float omt = 1f - t,
            omt2 = omt * omt,
            t2 = t * t;

        // 4 Vector3.Lerp(), simplified
        return pts[0] * (omt2 * omt) +
               pts[1] * (3f * omt2 * t) +
               pts[2] * (3f * omt * t2) +
               pts[3] * (t2 * t);
    }
}