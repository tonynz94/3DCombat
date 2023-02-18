using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }

    //계속 동작을 추가해주며 됨
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action CancelEvent;
    public bool IsAttacking;

    private Controls controls;

    void Start()
    {
        //컨트롤러를 우리에 맞게 연결하는 것
        controls = new Controls();

        //연결하여 액션이 실행될때 OnJump 실행
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //눌렸는지
        if(!context.performed) { return; }

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    //씨네머씬에서 처리해주고 있음
    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
       if(!context.performed) { return; }

        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if(context.canceled)
        {
            IsAttacking = false;
        }
    }
}
