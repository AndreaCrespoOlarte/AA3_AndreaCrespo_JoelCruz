using NUnit.Framework.Constraints;
using UnityEngine;

public class gerstner : MonoBehaviour
{
    public int initialX = 0;
    public int initialY = 0;
    public int width = 10;
    public int height = 10;
    public float spacing = 2f;
    public GameObject cellPrefab;

    public float amplitud = 1f;
    public float longitudOnda = 10f;
    public Vector2 direccion = new Vector2(1f, 0f); 
    public float periodo  = 2f;

    public float stepTime = 0.01f;

    private GameObject[,] cells;
    private Vector3[,] initialPositions;
    private float time;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cells = new GameObject[width, height];
        initialPositions = new Vector3[width, height];

        direccion = direccion.normalized; 

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3((x + initialX) * spacing, 0f, (z + initialY) * spacing);
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                cells[x, z] = cell;
                initialPositions[x, z] = pos;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += stepTime;

        float k_mag = (2f * Mathf.PI) / longitudOnda;
        Vector2 k = direccion * k_mag;

        float omega = 2f * Mathf.PI / periodo;

        for (int x = 0; x < width; x++) 
        {
            for (int z = 0; z < height; z++) 
            {
                Vector3 basePos = initialPositions[x, z];

                float fase = (k.x * basePos.x + k.y * basePos.z) - omega * time;
                
                float offsetX = -(k.x / k_mag) * amplitud * Mathf.Sin(fase);
                float offsetZ = -(k.y / k_mag) * amplitud * Mathf.Sin(fase);

                float offsetY = amplitud * Mathf.Cos(fase);

                Vector3 newPos = new Vector3(
                    basePos.x + offsetX, 
                    basePos.y + offsetY,
                    basePos.z + offsetZ
                );

                cells[x, z].transform.position = newPos;
            }
                
        }

    }

    public float GetGerstnerWaterHeight(Vector3 buoyPos)
    {
        GameObject closestCell = null;
        float minDistanceSqr = Mathf.Infinity;

        // Iteramos sobre la matriz de celdas que ya calculaste en Gerstner.cs
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 cellPos = cells[x, z].transform.position;

                // Calculamos distancia solo en el plano XZ (horizontal)
                float distSqr = Mathf.Pow(cellPos.x - buoyPos.x, 2) + Mathf.Pow(cellPos.z - buoyPos.z, 2);

                if (distSqr < minDistanceSqr)
                {
                    minDistanceSqr = distSqr;
                    closestCell = cells[x, z];
                }
            }
        }

        if (closestCell != null)
        {
            return closestCell.transform.position.y; // Retorna la altura Y real actual
        }

        return initialY;
    }
}

