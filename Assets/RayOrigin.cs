using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RayOrigin : MonoBehaviour
{
    Rigidbody Rigidbody;
    NavMeshAgent BoxAgent;
    private IsGrabbable Cube;

    private RayOrigin AltLegRay;
    public LayerMask Mask;
    private bool IsBox;
    public Vector3 RayPos,LegPos,MidPos,LegTarget;

    [SerializeField]
    private GameObject Leg, AltLeg;
    [SerializeField]
    private GameObject RayPoint;

    public bool MoveLeg, AtMid;
    
    public float PopupCD;
    private void RayCast()
    {
        RayPos = RayPoint.transform.position;
        Ray Ray = new Ray(RayPos, -transform.up);
        Debug.DrawRay(RayPos, -transform.up, Color.green);
        RaycastHit RayLegTarget;

        if (AltLeg != null) {
            if (AltLegRay.MoveLeg == false)
            {
                if (Physics.Raycast(Ray, out RayLegTarget, 2, Mask, QueryTriggerInteraction.Ignore))
                {

                    LegTarget = RayLegTarget.point;

                    if (Vector3.Distance(LegPos, LegTarget) >= 1 && !MoveLeg)
                    {
                        MoveLeg = true;
                        AtMid = false;
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(Ray, out RayLegTarget, 2, Mask, QueryTriggerInteraction.Ignore))
            {

                LegTarget = RayLegTarget.point;

                if (Vector3.Distance(LegPos, LegTarget) >= 1.2 && !MoveLeg)
                {
                    MoveLeg = true;
                    AtMid = false;
                }
            }
        }
    }
    private void LegMovement()
    {
        if (!AtMid)
        {
            Leg.transform.position = Vector3.MoveTowards(Leg.transform.position, MidPos, 0.03f);
            if (Vector3.Distance(Leg.transform.position, MidPos) <= 0.1f)
            {
                AtMid = true;
                Debug.Log("AtMid");
            }
        }
        if(AtMid)
        {
            Leg.transform.position = Vector3.MoveTowards(Leg.transform.position, LegTarget, 0.03f);
            if (Vector3.Distance(Leg.transform.position, LegTarget) <= 0.1f)
            {
                MoveLeg = false;
                Debug.Log("AtTarget");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MoveLeg = false;
        AtMid = false;
        IsBox = false;
        Cube = gameObject.GetComponentInParent<IsGrabbable>();
        BoxAgent = GetComponentInParent<NavMeshAgent>();
        Rigidbody = GetComponentInParent<Rigidbody>();
        if(AltLeg != null)
        {
            AltLegRay = AltLeg.GetComponent<RayOrigin>();
        }
    }

    private void BeBox()
    {
        Leg.SetActive(false);
        BoxAgent.enabled = false;
        Leg.transform.SetParent(transform.parent);
        //Leg.transform.position = LegTarget;
        MoveLeg = false;
        AtMid = false;
        MidPos = transform.position;
        
    }
    private void CheckIfBox()
    {
        if (!Cube.IsGrabbed)
        {
            Rigidbody.isKinematic = false;
            if (PopupCD > 0)
            {
                PopupCD -= Time.deltaTime;
                Leg.transform.SetParent(null);
                Leg.transform.position = transform.position;
            }
            if (PopupCD <= 0)
            {
                Rigidbody.isKinematic = true;
                IsBox = false;
            }
        }
        else if (Cube.IsGrabbed)
        {
            
            PopupCD = 3;
            IsBox = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckIfBox();
        LegPos = Leg.transform.position;
        if (!IsBox) {
            RayCast();
            Leg.SetActive(true);
            BoxAgent.enabled = true;
            
            
            
            if (MoveLeg)
                LegMovement();
            else if (!MoveLeg)
            {
                MidPos = Vector3.Lerp(Leg.transform.position, LegTarget, 0.5f);
                MidPos = new Vector3(MidPos.x, MidPos.y + 0.3f, MidPos.z);
            }
        }
        else
        {
            BeBox();
        }
        
    }
}
