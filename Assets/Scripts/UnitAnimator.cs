using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    //temporary
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform knifeTransform;


    private void Awake() 
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
            shootAction.OnAim += ShootAction_OnAim;
        }

        if(TryGetComponent<KnifeAction>(out KnifeAction knifeAction))
        {
            knifeAction.OnKnifeActionStarted += KnifeAction_OnKnifeActionStarted;
            knifeAction.OnKnifeActionCompleted += KnifeAction_OnKnifeActionCompleted;
        }
    }

    private void Start() 
    {
        EquipRifle();
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile =bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;
        
        bulletProjectile.Setup(targetUnitShootAtPosition, e.didHitTarget);
    }

    private void ShootAction_OnAim(object sender, EventArgs e)
    {
        animator.SetTrigger("Aim");
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void KnifeAction_OnKnifeActionStarted(object sender, EventArgs e)
    {
        EquipKnife();
        animator.SetTrigger("KnifeSlash");
    }

    private void KnifeAction_OnKnifeActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void EquipKnife()
    {
        knifeTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        knifeTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
