using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;
    public List<Target> targets = new List<Target>();

    private Camera mainCamera;

    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);
        target.OnDestroyed += RemoveTarget;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0)
            return false;

        Target closestTarget = null;
        float closestTargetDistance = float.MaxValue;

        foreach (Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            //카메라 영역 밖이라면 target잡을 필요가 없음.
            if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) 
            {
                continue;
            }

            Vector2 distanceFromCenter = viewPos - new Vector2(0.5f , 0.5f);
            if (closestTargetDistance > distanceFromCenter.sqrMagnitude)
            {
                closestTarget = target;
                closestTargetDistance = distanceFromCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
