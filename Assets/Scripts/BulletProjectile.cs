using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;
    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition, bool didHitTarget)
    {
        if(!didHitTarget)
        {
            float randomVerticalOffset = Random.Range(-4,4);
            targetPosition += Vector3.left * randomVerticalOffset;
            float randomHorizontalOffset = Random.Range(-0.5f,0.5f);
            targetPosition += Vector3.up * randomHorizontalOffset;
        }
        this.targetPosition = targetPosition;
    }

    private void Update() 
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;

            Instantiate(bulletHitVfxPrefab,targetPosition,Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
