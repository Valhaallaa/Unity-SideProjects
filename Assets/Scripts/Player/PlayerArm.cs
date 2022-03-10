using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    public LayerMask Mask;
    private Camera mainCamera;
    private bool GrabbingItem;
    public bool IsPlayerDead;
    private GameObject ArmTarget,ArmIdle, HeldObject,RangeRing, Player;
    RaycastHit GrabTarget;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        ArmTarget = GameObject.Find("ArmTarget");
        ArmIdle = GameObject.Find("ArmIdlePos");
        RangeRing = GameObject.Find("RangeRing");
        RangeRing.SetActive(false);
        ArmTarget.transform.SetParent(null);
        GrabbingItem = false;
        Player = GameObject.Find("Player");
    }
    IEnumerator DisplayRangeRing()
    {
        RangeRing.SetActive(true);
        yield return new WaitForSeconds(3);
        RangeRing.SetActive(false);
    }

    private void ArmIdleMovement()
    {
        if(Vector3.Distance(ArmTarget.transform.position,ArmIdle.transform.position) > 0.5f)
            ArmTarget.transform.position = Vector3.MoveTowards(ArmTarget.transform.position, ArmIdle.transform.position, 0.05f);
    }
    private void GrabIdleMovement()
    {
        if (Vector3.Distance(GrabTarget.transform.position, ArmIdle.transform.position) > 0.5f) {
            GrabTarget.transform.position = Vector3.MoveTowards(GrabTarget.transform.position, ArmIdle.transform.position, 0.05f);
            GrabTarget.transform.rotation = Player.transform.rotation;
                }
    }
    private void TryGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(cameraRay, out GrabTarget, 20, Mask, QueryTriggerInteraction.Ignore))
                if(GrabTarget.transform.GetComponent<IsGrabbable>())
                   if (GrabTarget.transform.GetComponent<IsGrabbable>().Grabbable == true ) {
                    if (Vector3.Distance(transform.position, GrabTarget.point) <= 4)
                    {
                        GrabTarget.transform.GetComponent<IsGrabbable>().IsGrabbed = true;
                        ArmTarget.transform.position = GrabTarget.point;
                        GrabbingItem = true;
                        ArmTarget.transform.SetParent(GrabTarget.transform);
                        GrabTarget.transform.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    else {
                        StartCoroutine("DisplayRangeRing");
                    }
                }
            
        }
    }
    private void DropObject()
    {
        if (Input.GetKeyDown(KeyCode.R) || IsPlayerDead)
        {

            ArmTarget.transform.SetParent(null);
            GrabbingItem = false;
            if(GrabTarget.transform.GetComponent<IsGrabbable>().IsWalkableBox == false)
                GrabTarget.transform.GetComponent<Rigidbody>().isKinematic = false;
            GrabTarget.transform.GetComponent<IsGrabbable>().IsGrabbed = false;

        }
    }
    

    // Update is called once per frame
    void Update()
    {

        if (!GrabbingItem) {
            ArmIdleMovement();
            TryGrab();
        }
        if (GrabbingItem)
        {
            GrabIdleMovement();
            DropObject();
        }
        if (IsPlayerDead)
        {
            DropObject();
            enabled = false;
            ArmTarget.GetComponent<Rigidbody>().isKinematic = false;
        }

    }

}
