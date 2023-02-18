using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Target : MonoBehaviour
{
    public Action<Target> OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
