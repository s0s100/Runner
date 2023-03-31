using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimatorHelper : MonoBehaviour
{
    public void DeleteParent()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
