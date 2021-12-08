using System;
using UnityEngine;

[Serializable]
public class EnemyPack
{
    public UnitData[] enemyDatas;
    public Vector3[] offsetPositions;

    public int probabilityWeight = 1;   // Unused for now
    public int minFloor = -1;           // Unused for now
    public int maxFloor = -1;           // Unused for now

    public void SpawnEnemies(Vector3 position)
    {
        GameObject enemyPrefab = GameManager.GetEnemyPrefab();
        if (enemyPrefab == null)
            return;

        // Extend the position array if not long enough
        if (offsetPositions.Length < enemyDatas.Length)
        {
            Vector3[] newOffsetPosition = new Vector3[enemyDatas.Length];
            for (int i = 0; i < offsetPositions.Length; i++)
                newOffsetPosition[i] = offsetPositions[i];
            offsetPositions = newOffsetPosition;
        }

        for(int i = 0; i < enemyDatas.Length; i++)
        {
            GameObject enemy = UnityEngine.Object.Instantiate(enemyPrefab, position + offsetPositions[i], Quaternion.identity);
            enemy.GetComponent<Enemy>().Setup(enemyDatas[i]);
        }
    }

    public bool OkForThisFloor(int currentFloor)    // Min and Max are inclusive
    {
        // Min ok ?
        if (currentFloor < minFloor)
            return false;

        // Max ok ? => not inf (<=0) and above = not ok
        if (maxFloor > 0 && currentFloor > maxFloor)
            return false;

        return true;
    }
}
