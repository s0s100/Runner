using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkParentLazer : MonoBehaviour
{
    // Lazer can't see parent object scripts
    [SerializeField]
    private LazerGenerator lazerGenerator;

    public void ActivateLazer()
    {
        lazerGenerator.Activate();
    }

    public void DeactivateLazer()
    {
        lazerGenerator.Deactivate();
    }
}
