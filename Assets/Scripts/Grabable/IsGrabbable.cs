using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsGrabbable : MonoBehaviour
{
    private Rigidbody Rigidbody;
    [SerializeField]
    public bool Grabbable, Switchable, IsWalkableBox;
    public bool Switched = false;
    public bool IsGrabbed = false;

    [SerializeField]
    private GameObject BoxTarget;
    NavMeshAgent BoxAgent;


    public void Switch()
    {
        if (Switched)
            Switched = false;
        else
            Switched = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Switched = false;
        IsGrabbed = false;
        if (IsWalkableBox)
        {
            BoxTarget.transform.SetParent(null);
            BoxAgent = GetComponent<NavMeshAgent>();
        }
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWalkableBox)
        {
            if (!IsGrabbed)
            {
                BoxAgent.SetDestination(BoxTarget.transform.position);
            }

            if (Vector3.Distance(transform.position, BoxTarget.transform.position) < 1 && IsGrabbed == false)
            {
                IsGrabbed = true;
                Rigidbody.isKinematic = false;
            }
        }
    }
}
