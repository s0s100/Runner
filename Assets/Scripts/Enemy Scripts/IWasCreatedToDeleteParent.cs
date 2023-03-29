using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWasCreatedToDeleteParent : MonoBehaviour
{
    public void DeleteParent()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
