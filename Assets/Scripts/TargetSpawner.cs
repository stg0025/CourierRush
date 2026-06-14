using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject packageTarget;
    public GameObject deliveryTarget;
    public WorldSpawner worldSpawner;

    float minSpawnDistance = 10f;
    float collectDistance = 1f;
    public bool hasPackage = false;

    void Start()
    {
        deliveryTarget.SetActive(false);
        Invoke(nameof(SpawnTargetInitial), 0.1f);
    }

    void SpawnTargetInitial()
    {
        SpawnOnRoad(packageTarget, true, Vector3.zero, 0f);
    }

    void SpawnOnRoad(GameObject obj, bool ignoreDistance)
    {
        SpawnOnRoad(obj, ignoreDistance, Vector3.zero, 0f);
    }

    void SpawnOnRoad(GameObject obj, bool ignoreDistance, Vector3 avoidPosition, float avoidRadius)
    {
        List<Vector3> candidates = new List<Vector3>();
        foreach (var kvp in worldSpawner.GetActiveChunks())
        {
            GameObject chunkObject = kvp.Value;
            if (chunkObject == null) continue;
            if (!ignoreDistance && Vector3.Distance(player.position, chunkObject.transform.position) < minSpawnDistance)
                continue;

            Tilemap road = chunkObject.transform.Find("Road")?.GetComponent<Tilemap>();
            if (road == null) continue;

            foreach (Vector3Int cell in road.cellBounds.allPositionsWithin)
            {
                if (road.HasTile(cell))
                {
                    Vector3 worldPos = road.GetCellCenterWorld(cell);
                    if (avoidRadius <= 0f || Vector3.Distance(worldPos, avoidPosition) > avoidRadius)
                        candidates.Add(worldPos);
                }
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("No valid spawn positions found!");
            return;
        }

        obj.transform.position = candidates[Random.Range(0, candidates.Count)];
        obj.SetActive(true);
    }

    void Update()
    {
        if (player == null || worldSpawner == null) return;

        if (!hasPackage)
        {
            if (packageTarget.activeSelf)
            {
                if (Vector3.Distance(player.position, packageTarget.transform.position) < collectDistance)
                {
                    packageTarget.SetActive(false);
                    hasPackage = true;
                    AudioManager.Instance.PlayPickup();
                    SpawnOnRoad(deliveryTarget, false, packageTarget.transform.position, minSpawnDistance);
                }
                else
                {
                    Vector2Int coord = worldSpawner.WorldToGrid(packageTarget.transform.position);
                    if (!worldSpawner.IsChunkActive(coord))
                        SpawnOnRoad(packageTarget, false);
                }
            }
        }
        else
        {
            if (deliveryTarget.activeSelf)
            {
                if (Vector3.Distance(player.position, deliveryTarget.transform.position) < collectDistance)
                {
                    deliveryTarget.SetActive(false);
                    hasPackage = false;
                    AudioManager.Instance.PlayDelivery();
                    GameManager.Instance.AddScore();
                    SpawnOnRoad(packageTarget, false, deliveryTarget.transform.position, minSpawnDistance);
                }
                else
                {
                    Vector2Int coord = worldSpawner.WorldToGrid(deliveryTarget.transform.position);
                    if (!worldSpawner.IsChunkActive(coord))
                        SpawnOnRoad(deliveryTarget, false);
                }
            }
        }
    }
}