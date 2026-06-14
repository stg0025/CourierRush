using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    public Transform player;
    public Transform packageTarget;
    public Transform deliveryTarget;
    public TargetSpawner targetSpawner;

    void Update()
    {
        if (player == null) return;

        Transform current = targetSpawner.hasPackage ? deliveryTarget : packageTarget;

        Vector3 direction = current.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}