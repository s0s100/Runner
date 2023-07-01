using UnityEngine;

public class DroneController : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        Destroy(this.gameObject, existanceTime);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Movement()
    {
        transform.position += speed * Vector3.right * Time.deltaTime;
    }
}
