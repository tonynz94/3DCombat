using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//하나의 동작이라고 생각하면 될듯
public class PlayerFreeLookState : PlayerBaseState
{
    //stringToHaSH는 런타임에 실행되기 때문에 CONST는 불가

     //자유시점
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;

    private float timer;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;

        stateMachine.Animator.Play(FreeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine , 0));
            return;
        }

        Vector3 movement = CalculateMoveMent();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        //Debug.Log(stateMachine.InputReader.MovementValue);

        if(stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return; 
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if(!stateMachine.Targeter.SelectTarget()) { return; }

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private Vector3 CalculateMoveMent()
    {
        //카메라 기준으로
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //W => y : 1
        //S =>  y : -1
      
        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, 
            Quaternion.LookRotation(movement), 
            deltaTime  * stateMachine.RotationSmoothValue);
    }

}
