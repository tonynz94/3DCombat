using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;

    private Attack attack;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine) 
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizeTime = GetNormalizeTime();
        //previousFrameTime 가 1이 될 수 있음

        //normalizeTime > previousFrameTime 를 해주는 이유는 
        //1 -> 2번 공격으로 넘어가는것과 동시에 프레임 타임을 구하면 아주 드물게 2번의 프레임이 아닌 1번의 1.0f를 반환할때가 가끔 있다
        //이를 방지하기 위해 설정한 것이다. 
        //1를 반환한다는건 마지막 콤보가 끝나다라는 뜻인데 위에처럼 아주 드물게 애니메이션이 이동중에 1로 나왔다면 끊겨버리는 현상을 막기 위한 것이다.
        if (normalizeTime >= previousFrameTime && normalizeTime < 1f)
        {
            if (true)
            {
                TryComboAttack(normalizeTime);
            }
        }
        else
        {
            //
            // go back to locotiontion
        }

        previousFrameTime = normalizeTime;
    }

    private void TryComboAttack(float normlizedTime)
    {
        //다음 콤보가 없을때
        if (attack.ComboStateIndex == -1) { return; }

        //다음 콤보 어택 시간이 지나지 않았다면 무반응
        if (normlizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex
            )
        );
    }

    public override void Exit()
    {
        
    }

    //애니메이션 길이는 모두 다르기 때문에 0~1 사이로 노멀라이즈 시켜준다.
    private float GetNormalizeTime()
    {
        //애니메이션은 상태를 두가지를 가질 수가 있다 
        //애니메이션 -> 또 다른 애니메이션으로 이동 일때 두가지의 상태를 갖는다.

        //0은 base Layer를 이야기한다.
        AnimatorStateInfo currentInfo=stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo =stateMachine.Animator.GetNextAnimatorStateInfo(0);

        //또다른 애니메이션으로 이동중인지 검사.
        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            //여기에 왔다는건 또 다른 Attack Enter에 진입을 했다는 것
            //그래서 진입한 것에 대한 노멀라이즈 시간을 반환해준다.
            return nextInfo.normalizedTime;
        }
        else if(!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
