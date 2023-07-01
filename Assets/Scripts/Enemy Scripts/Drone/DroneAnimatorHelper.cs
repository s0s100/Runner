using UnityEngine;

public class DroneAnimatorHelper : MonoBehaviour
{
    public void DeleteParent()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
