using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 targetPosition;

    public static void Create(Vector3 spawnPosition, Vector3 targetPosition)
    {
        Transform projectileTransform = Instantiate(GameAssets.Instance.pfProjectile, spawnPosition ,Quaternion.identity);

        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        projectile.Setup(targetPosition);
    }

    private void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveSpeed = 20f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float angle = UtilsClass.GetAngleFromVectorFloat(moveDirection);
        transform.eulerAngles = new Vector3(0, 0, angle);

        float destroySelfDistance = 1f;
        if (Vector3.Distance(transform.position,targetPosition)< destroySelfDistance)
        {
            Destroy(gameObject);
        }
    }
}
