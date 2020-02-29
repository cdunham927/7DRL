using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float spd;
    Rigidbody2D bod;

    void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inp.magnitude != 0)
        {
            bod.AddForce(new Vector2(inp.x * spd * Time.deltaTime, inp.y * spd * Time.deltaTime));
        }
    }
}
