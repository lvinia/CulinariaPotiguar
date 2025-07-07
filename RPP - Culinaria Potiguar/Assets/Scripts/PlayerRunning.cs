using System.Collections.Generic;
using UnityEngine;

public class PlayerRunning : MonoBehaviour
{
    public float speed;
    public bool isGrounded = true;
    
    private Animator anim;
    public Rigidbody2D rig;
    
    public LayerMask LayerGround;
    public Transform checkGround;
    public string isGroundBool = "eChao";
    
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        MovimentaPlayer();
    }

    
    void Update()
    {
        
    }

    private void MovimentaPlayer()
    {
        transform.Translate(new Vector3(speed, 0, 0));
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(speed, 0, 0));

        if(Physics2D.OverlapCircle(checkGround.transform.position, 0.2f, LayerGround))
        {
            anim.SetBool(isGroundBool, true);
            isGrounded = true;
        }
        else
        {
            anim.SetBool(isGroundBool, false);
            isGrounded = false;
        }
    }
}
