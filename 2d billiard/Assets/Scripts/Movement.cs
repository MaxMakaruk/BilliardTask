using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Rigidbody2D rb;
    LineRenderer lr;
    Transform trans;

    public Vector2 touchPosition;
    public Vector2 touchPosition1;
    public Vector2 touchPosition2;

    private Vector2 ballPos, mousePos, direction;


    public void ShowTrajectory()
    {
        Vector3[] points = new Vector3[2];
        points[0] = ballPos;
        points[1] = 2f * mousePos - ballPos;
        lr.positionCount = 2;
        lr.SetPositions(points);
    }

    
    public void ShowTrajectory(Vector3 ballHit, Vector3 ballTraj)
    {
        Vector3[] points = new Vector3[3];
        points[0] = ballPos;
        points[1] = ballHit;
        points[2] = ballTraj;        
        lr.positionCount = 3;
        lr.SetPositions(points);
    }
    public void ShowTrajectory(Vector3 ballHit, Vector3 ballTraj, Vector3 perp)
    {
        Vector3[] points = new Vector3[5];
        points[0] = ballPos;
        points[1] = ballHit;
        points[2] = ballTraj;
        points[3] = ballHit;
        points[4] = perp;        
        lr.positionCount = 5;
        lr.SetPositions(points);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        trans = GetComponent<Transform>();
    }

    
    void Update()
    {
        ballPos = new Vector2(trans.position.x, trans.position.y);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPosition = mousePos;
        touchPosition1 = ballPos;
        direction = (mousePos - ballPos) * 2f;
        
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.CircleCast(ballPos, trans.lossyScale.x / 2, direction, Vector2.Distance(ballPos, mousePos) * 2f);
            if (hit)
            {
                var hitPoint = hit.point + trans.lossyScale.x / 2f * hit.normal;
                if (hit.rigidbody)
                {
                                     
                    
                    var perpendicular1 = hitPoint + trans.lossyScale.x  * new Vector2(hit.normal.y, -hit.normal.x);
                    var perpendicular2 = hitPoint + trans.lossyScale.x  * new Vector2(-hit.normal.y, hit.normal.x);
                    if (Vector2.Distance(ballPos, perpendicular1) > Vector2.Distance(ballPos, perpendicular2))
                    {
                        ShowTrajectory(hitPoint, hitPoint + trans.lossyScale.x * -3f * hit.normal, perpendicular1);
                    }
                    else
                        ShowTrajectory(hitPoint, hitPoint + trans.lossyScale.x * -3f * hit.normal, perpendicular2);
                    
                }
                else
                    ShowTrajectory(hitPoint, Vector2.Reflect(direction / 8, hit.normal) + hitPoint); ;
            }
            else
                ShowTrajectory();

        }
        
        
        if (Input.GetMouseButtonUp(0))
        {
            rb.AddForce(direction, ForceMode2D.Impulse);
            lr.positionCount = 0;
        }

    }
}
