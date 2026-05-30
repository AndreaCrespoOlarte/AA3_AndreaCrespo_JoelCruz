using UnityEngine;

public class sinusoidal : MonoBehaviour
{

    public int width = 5;
    public int height = 5;
    public float spacing = 2f;
    public GameObject cellPrefab;

    public float amplitud = 1f;
    public float longitudOnda = 10f;
    public Vector2 direccion = new Vector2(1f, 0f); 
    public float periodo  = 2f;
    public float faseInicial  = 0f;

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

        float velocidad = longitudOnda / periodo; 

        for (int x = 0; x < width; x++) 
        {
            for (int z = 0; z < height; z++) 
            {
                Vector3 basePos = initialPositions[x, z];

                float posicionProyectada = basePos.x * direccion.x + basePos.z * direccion.y;

                float k = (2f * Mathf.PI) / longitudOnda;
                float insideSin = k * (posicionProyectada - velocidad * time) + faseInicial;

                float offsetY = amplitud * Mathf.Sin(insideSin);

                Vector3 newPos = new Vector3(basePos.x, basePos.y + offsetY, basePos.z);

                cells[x, z].transform.position = newPos;

            }
                
        }

    }
}
