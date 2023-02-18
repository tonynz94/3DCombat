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
        //previousFrameTime �� 1�� �� �� ����

        //normalizeTime > previousFrameTime �� ���ִ� ������ 
        //1 -> 2�� �������� �Ѿ�°Ͱ� ���ÿ� ������ Ÿ���� ���ϸ� ���� �幰�� 2���� �������� �ƴ� 1���� 1.0f�� ��ȯ�Ҷ��� ���� �ִ�
        //�̸� �����ϱ� ���� ������ ���̴�. 
        //1�� ��ȯ�Ѵٴ°� ������ �޺��� �����ٶ�� ���ε� ����ó�� ���� �幰�� �ִϸ��̼��� �̵��߿� 1�� ���Դٸ� ���ܹ����� ������ ���� ���� ���̴�.
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
        //���� �޺��� ������
        if (attack.ComboStateIndex == -1) { return; }

        //���� �޺� ���� �ð��� ������ �ʾҴٸ� ������
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

    //�ִϸ��̼� ���̴� ��� �ٸ��� ������ 0~1 ���̷� ��ֶ����� �����ش�.
    private float GetNormalizeTime()
    {
        //�ִϸ��̼��� ���¸� �ΰ����� ���� ���� �ִ� 
        //�ִϸ��̼� -> �� �ٸ� �ִϸ��̼����� �̵� �϶� �ΰ����� ���¸� ���´�.

        //0�� base Layer�� �̾߱��Ѵ�.
        AnimatorStateInfo currentInfo=stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo =stateMachine.Animator.GetNextAnimatorStateInfo(0);

        //�Ǵٸ� �ִϸ��̼����� �̵������� �˻�.
        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            //���⿡ �Դٴ°� �� �ٸ� Attack Enter�� ������ �ߴٴ� ��
            //�׷��� ������ �Ϳ� ���� ��ֶ����� �ð��� ��ȯ���ش�.
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
