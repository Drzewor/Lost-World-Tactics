using UnityEngine;

public class UnitCorpse : MonoBehaviour
{
    float destroyTimer = 10f;

    void Update()
    {
        destroyTimer -= Time.deltaTime;

        if(destroyTimer >= 8f)
        {
            return;
        }

        float movingDownSpeed = 0.1f;
        transform.position += Vector3.down * movingDownSpeed * Time.deltaTime;

        if(destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
