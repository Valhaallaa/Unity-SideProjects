using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegMovement : MonoBehaviour
{
    [SerializeField]
    GameObject RayOrigin;
    public LayerMask Mask;


    public float LegRange;
    public float LegSpeed = 0.1f;
    private float LegDistance, MiddleDist;
    private Vector3 MiddlePos;
    private Vector3 LegTarget;
    private bool LegMoving, ReachedMid;
    private void LegRaycast() 
    { 
        
        Ray Ray = new Ray(RayOrigin.transform.position,-RayOrigin.transform.up);
        Debug.DrawRay(RayOrigin.transform.position,-RayOrigin.transform.up, Color.green);
        RaycastHit RayLegTarget;
        if (Physics.Raycast(Ray, out RayLegTarget, 5, Mask, QueryTriggerInteraction.Ignore))
        {
            Vector3 SetPos = RayLegTarget.point;
            LegTarget = SetPos;
        }
        else
        {
            LegTarget = new Vector3(RayOrigin.transform.localPosition.x + 1f, RayOrigin.transform.localPosition.y, RayOrigin.transform.localPosition.z);
        }
    }

    private void LegMovement()
    {
        if (LegDistance >= LegRange)
        {
            LegMoving = true;
        }
        if (LegMoving)
        {
            if (!ReachedMid)
            {
                transform.position = Vector3.MoveTowards(transform.position, MiddlePos, LegSpeed / 2);
                MiddleDist = Vector3.Distance(transform.position, MiddlePos);
                if (MiddleDist <= 0.2f)
                {
                    ReachedMid = true;
                }
            }
            else if (ReachedMid)
            {
                transform.position = Vector3.MoveTowards(transform.position, LegTarget, LegSpeed / 2);
                if (LegDistance <= 0.2f)
                {
                    LegMoving = false;
                    ReachedMid = false;
                }
            }
        }
    }

    private void GetLegDistance()
    {
        LegDistance = Vector3.Distance(LegTarget, transform.position);
        if (LegDistance >= LegRange)
        {
            GetLegMiddle();
        }
    }

    private void GetLegMiddle()
    {
        MiddlePos = Vector3.Lerp(LegTarget, transform.position, 0.5f);
        MiddlePos.y = MiddlePos.y + 0.3f;
    }


    // Start is called before the first frame update
    void Start()
    {
        /*
        RayOrigin.transform.SetParent(GameObject.Find("Player").transform);
        RayOrigin.transform.localPosition = new Vector3(0,1,-0.25f);*/
    }

    // Update is called once per frame
    void Update()
    {
        LegRaycast();
        GetLegDistance();
        LegMovement();
    }
}
