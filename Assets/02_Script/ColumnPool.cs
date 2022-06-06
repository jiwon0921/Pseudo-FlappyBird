using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour
{
    public static ColumnPool Instance;

    public GameObject column;
    public int columnAmount;
    public List<Transform> columns;
    public List<Transform> spawnedColumns;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void initilaze()
    {
        columns = new List<Transform>();
        spawnedColumns = new List<Transform>();

        if(columnAmount>0)
        {
            for (int i = 0; i < columnAmount; i++)
            {
                AddColumn();
            }
        }
    }

    public Transform AddColumn()
    {
        Transform newColumn = Instantiate(column, this.transform).transform;
        newColumn.transform.localPosition = Vector3.zero;
        newColumn.gameObject.SetActive(false);
        columns.Add(newColumn);

        return newColumn;
    }

    public Transform SpawnColumn(Vector2 position)
    {
        if(columns.Count > 0)
        {
            Transform result = columns[columns.Count - 1];
            columns.RemoveAt(columns.Count - 1);
            spawnedColumns.Add(result);
            result.SetParent(null);
            result.position = position;
            result.gameObject.SetActive(true);
            return result;
        }
        else
        {
            AddColumn();
        }

        //log
        return null;
    }

    public void DespawnColumn(Transform spawnedColumn)
    {
        columns.Add(spawnedColumn);
        spawnedColumns.Remove(spawnedColumn);
        spawnedColumn.SetParent(this.transform);
        spawnedColumn.localPosition = Vector3.zero;
        spawnedColumn.gameObject.SetActive(false);
    }

    public void DespawnAll()
    {
        if(spawnedColumns.Count > 0)
        {
            for (int i = spawnedColumns.Count - 1; i >= 0; i--)
            {
                DespawnColumn(spawnedColumns[i]);
            }
        }
    }
}
