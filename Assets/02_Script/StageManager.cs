using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Transform floor;

    public float minSpawnPos;
    public float maxSpawnPos;
    public float interval;
    public float spawnTime;

    public float speed;

    public void initialize()
    {
        spawnTime = 10f;
    }

    private void Update()
    {

    }

    public Transform SpawnColumn()
    {
        float spawnRange = Random.Range(minSpawnPos, maxSpawnPos);
        Vector2 spawnpos = new Vector2(5, spawnRange);
        return ColumnPool.Instance.SpawnColumn(spawnpos);
    }

    public IEnumerator SpawnAndMoveColumn()
    {
        while(true)
        {
            if (GameManager.Instance.isGameover) break;

            Transform column;
            //»ý¼º
            spawnTime += Time.deltaTime;
            if (spawnTime > interval)
            {
                column = SpawnColumn();
                IEnumerator moveColumn = MoveColumn(column);
                StartCoroutine(moveColumn);

                spawnTime = 0;
            }

            yield return null;
        }
    }

    IEnumerator MoveColumn(Transform columnTransform)
    {
        while (true)
        {
            if (GameManager.Instance.isGameover) break;

            columnTransform.position += Vector3.left * Time.deltaTime * speed;

            if (columnTransform.position.x < -4f)
            {
                ColumnPool.Instance.DespawnColumn(columnTransform);
                break;
            }


            yield return null;
        }
    }

    public IEnumerator MoveFloor()
    {
        while (true)
        {
            floor.position += Vector3.left * Time.deltaTime * speed;

            if (floor.position.x < -3.3275f)
            {
                floor.position = new Vector2(3.3275f, floor.position.y);
            }

            yield return null;
        }
    }
}
