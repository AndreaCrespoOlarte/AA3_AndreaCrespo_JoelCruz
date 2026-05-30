using NUnit.Framework.Constraints;
using UnityEngine;

public class gerstner : MonoBehaviour
{

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
    private Vector2[,] initialPositions;
    private float time;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cells = new GameObject[width, height];
        initialPositions = new Vector2[width, height];

        direccion = direccion.normalized; 

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                cells[x, z] = cell;
                initialPositions[x, z] = pos;
            }
        }
    }

    // Update is called once per frame
    void Update()
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
}

