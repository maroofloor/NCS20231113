using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Rigidbody2D rigid;

    float speed = 5f;
    Vector3 LRVec = Vector3.zero;
    Vector3 jumpVec = Vector3.up;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            rigid.velocity = jumpVec * 30;
        }
    }

    //void FixedUpdate()
    //{
    //    LRVec.x = Input.GetAxisRaw("Horizontal");
    //    rigid.velocity = LRVec * speed;

        
    //}
}
