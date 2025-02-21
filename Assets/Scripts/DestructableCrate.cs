using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroy;

    [SerializeField] private Transform crateDestroyedPrefab;
    [SerializeField] private float partsDestroyDelay = 4f;
    private GridPosition gridPosition;

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab,transform.position,transform.rotation);
        OnAnyDestroy.Invoke(this,EventArgs.Empty);
        Destroy(gameObject);
        ApplayExplosionToChildren(crateDestroyedTransform, 100f,transform.position, 10);
    }

    private void ApplayExplosionToChildren(Transform root,float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce,explosionPosition,explosionRange);
                Destroy(child.gameObject,partsDestroyDelay);
            }

            ApplayExplosionToChildren(child,explosionForce,explosionPosition,explosionRange);
        }
    }
}
