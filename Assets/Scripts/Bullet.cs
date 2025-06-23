using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 initialPosition;
    private Vector3 targetDirection;
    public float bulletSpeed;
    public void Init(Vector2 initialPosition, Vector2 targetDirection)
    {
        this.initialPosition = initialPosition;
        this.targetDirection = targetDirection.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, targetDirection, Color.yellow);
        transform.position += targetDirection * bulletSpeed * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
