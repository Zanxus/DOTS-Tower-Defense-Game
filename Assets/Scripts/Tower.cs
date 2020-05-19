using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IPlaceable
{
    private Vector3 projectileShootFromPosition;

    private void Awake()
    {
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(projectileShootFromPosition);
            Projectile.Create(projectileShootFromPosition, UtilsClass.GetMouseWorldPosition());
        }
    }

    public void Place()
    {
        
    }
}
