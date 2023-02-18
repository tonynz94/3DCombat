using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    private void Update()
    {
        //ม฿ทย
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            //Physics.gravity : new Vector3( 0f, -9.81f, 0f);
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
