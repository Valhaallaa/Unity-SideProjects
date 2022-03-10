using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 3;

    Rigidbody Rigidbody;
    private void Movement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            //transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime, Space.Self);
            Rigidbody.AddForce(Vector3.forward * MoveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime, Space.Self);
            Rigidbody.AddForce(-Vector3.forward * MoveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime, Space.Self);
            Rigidbody.AddForce(Vector3.right * MoveSpeed * Time.deltaTime, ForceMode.Impulse);

        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.Translate(-Vector3.right * MoveSpeed * Time.deltaTime, Space.Self);
            Rigidbody.AddForce(-Vector3.right * MoveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Rigidbody.velocity.y == 0)
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 5, Rigidbody.velocity.y);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
}
