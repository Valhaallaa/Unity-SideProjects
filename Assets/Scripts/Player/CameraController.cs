using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float RotSpeed;
    private GameObject Player;

    private void rotation()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0, RotSpeed * Time.deltaTime, 0, Space.World);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, -RotSpeed * Time.deltaTime, 0, Space.World);
       
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        rotation();
        transform.position = Player.transform.position;
    }

}
