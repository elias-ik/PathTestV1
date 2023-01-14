using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

public class SpaceShipScript : MonoBehaviour
{

    public Vector3 startPosition;

    public Vector3 startDirection;

    public Vector3 endPosition;

    public Vector3 endDirection;

    public float velocityMps = 10;
    public float accelerationMpss = 1;


    private Vector3 TravelDirection { get; set; }
    private float TurnRadius => (velocityMps * velocityMps) / accelerationMpss;
    private List<Action> gizmoActions = new();
    private List<Vector3> posList = new();
    void Start()
    {
        transform.position = startPosition;
        TravelDirection = startDirection.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        gizmoActions.Clear();
        Move();
        DrawDebug();
    }

    void Move()
    {
        var prev = transform.position;
        transform.position = (TravelDirection * (velocityMps * Time.deltaTime)) + prev;
        posList.Add(prev);
        DoGizmo(() =>
        {
            Gizmos.color = Color.magenta;
            for (var i = 0; i < posList.Count - 1; )
            {
                Gizmos.DrawLine(posList[i], posList[++i]);
            }
        });
    }
    
    void DrawDebug()
    {
        DoGizmo(() =>
        {
            Gizmos.color = Color.green;
            DrawArrow.ForGizmo(startPosition, startPosition + startDirection.normalized * 10);
            Gizmos.color = Color.red;
            DrawArrow.ForGizmo(endPosition, endPosition + endDirection.normalized * 10);

            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(transform.position, TurnRadius);
            // Gizmos.DrawWireSphere(endPosition, TurnRadius);

        });
    }

    private void OnDrawGizmos()
    {
        gizmoActions.ForEach(ga => ga.Invoke());
    }

    private void DoGizmo(Action a)
    {
        gizmoActions.Add(a);
    }
    
}
