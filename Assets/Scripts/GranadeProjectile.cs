using System;
using Unity.Mathematics;
using UnityEngine;

public class GranadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGranadeExploded;

    [SerializeField] private Transform granadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    private Vector3 targetPosition;
    private Action OnGranadeBehaviorComplete;
    private float totalDistance;
    private Vector3 positionXZ;

    private void Update() 
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        float movespeed = 15f;
        positionXZ += moveDir * movespeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ,targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x,positionY,positionXZ.z);

        float reachedTargetDistance = 0.2f;
    
        if(Vector3.Distance(positionXZ, targetPosition) <= reachedTargetDistance)
        {
            float damageRadius = 3f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
                if(collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {
                    destructableCrate.Damage();
                }
            }

            OnGranadeBehaviorComplete();
            OnAnyGranadeExploded.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(granadeExplodeVfxPrefab,targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGranadeBehaviorComplete)
    {
        this.OnGranadeBehaviorComplete = OnGranadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ,targetPosition);
    }
}
