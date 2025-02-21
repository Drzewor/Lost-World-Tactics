using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start() 
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GranadeProjectile.OnAnyGranadeExploded += GranadeProjectile_OnAnyGranadeExploded;
        KnifeAction.OnAnyKnifeHit += KnifeAction_OnAnyKnifeHit;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }

    private void GranadeProjectile_OnAnyGranadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5);
    }
    
    private void KnifeAction_OnAnyKnifeHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(0.5f);
    }
}
