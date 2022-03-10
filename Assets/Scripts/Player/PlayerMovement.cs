using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerMovement : MonoBehaviour
{
    private bool SpiderMode;
    public LayerMask Mask;
    NavMeshAgent HumanoidAgent;
    private Camera mainCamera;
    private GameObject PlayerMoveTarget;
    [SerializeField]
    private GameObject PlayerHips;
    public float distanceToTarget;
    Animator Animator;
    float CurrentSpeed;

    private PlayerArm PlayerArm;
    private GameObject MoveRing;
    private float RangeRingCD;
    public bool Isdead;
    // Start is called before the first frame update
    void Start()
    {
        HumanoidAgent = GetComponent<NavMeshAgent>();
        mainCamera = FindObjectOfType<Camera>();
        transform.SetParent(null);
        PlayerMoveTarget = GameObject.Find("PlayerMoveTarget");
        PlayerMoveTarget.transform.SetParent(null);
        Animator = GetComponent<Animator>();
        GetComponentInChildren<Rigidbody>().isKinematic = true;
        PlayerArm = GameObject.Find("ArmBase").GetComponent<PlayerArm>();
        MoveRing = GameObject.Find("MoveRing");
        MoveRing.SetActive(false);
    }


    private void DisplayMoveRing()
    {
        if (RangeRingCD > 0)
            MoveRing.SetActive(true);
        else
            MoveRing.SetActive(false);
    }

    private void Die()
    {
        Animator.enabled = false;
        HumanoidAgent.enabled = false;
        GetComponentInChildren<Rigidbody>().isKinematic = false;
        PlayerArm.IsPlayerDead = true;
        GetComponent<PlayerMovement>().enabled = false;

    }

    private void Pathfinding()
    {
        HumanoidAgent.SetDestination(PlayerMoveTarget.transform.position);

        distanceToTarget = Vector3.Distance(PlayerMoveTarget.transform.position, transform.position);
        if (distanceToTarget < 0.2f)
        {
            HumanoidAgent.isStopped = true;

        }

        if (distanceToTarget >= 0.2f)
        {
            HumanoidAgent.isStopped = false;
        }

    }
    private void updateTargetPos()
    {
        if (Input.GetMouseButton(1))
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit MoveTarget;
            if (Physics.Raycast(cameraRay, out MoveTarget, 20, Mask, QueryTriggerInteraction.Ignore))
            {
                PlayerMoveTarget.transform.position = MoveTarget.point;
                RangeRingCD = 3;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        DisplayMoveRing();
        if (RangeRingCD > 0)
            RangeRingCD -= Time.deltaTime;
        updateTargetPos();
        if (!SpiderMode)
        {
            Pathfinding();
            CurrentSpeed = HumanoidAgent.velocity.magnitude / HumanoidAgent.speed;
            Animator.SetFloat("Speed", CurrentSpeed, .1f, Time.deltaTime);
        }
        else if (SpiderMode)
        {

        }
        if (Isdead)
            Die();
    }
}