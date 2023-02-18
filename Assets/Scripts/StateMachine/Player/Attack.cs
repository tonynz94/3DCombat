using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//콤보 어택을 위한 것
[Serializable]
public class Attack
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }
}
