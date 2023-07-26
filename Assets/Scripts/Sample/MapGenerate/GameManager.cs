using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // This is the map object
    public GameObject map;
    // This is the prefab list
    public GameObject[] prefabList;
    // This is the map size
    public int[] mapSize = new int[3] { 36, 6, 36 };
    // This is the map array
    public int[,,] mapArray;
    // This is the sea width
    public int seaWidth = 3;
    // This is the sea height
    public float seaHeight = 0.5f;
    // This is the sea power
    public int seaPower = 2;
    // This is the wave speed
    public float waveSpeed = 0.5f;
    // This is min land height
    public int minLandHeight = 2;
    // This is the smoothness of the map
    public int smoothness = 20;
    public GameObject supply;

    public GameObject bomb;
    public GameObject bomber;

    // Return the perlin noise value which is between 2 and prefabList.Length
    // The smoothness is the smoothness of the map

    int GetPerlinNoise(int x, int z)
    {
        float noise = Mathf.PerlinNoise((float)x / smoothness, (float)z / smoothness);
        noise = Mathf.Pow(noise, 3);  // Raise the Perlin noise result to a power
        noise = noise * (mapSize[1] - minLandHeight) + minLandHeight; // Make sure the height is between minLandHeight and maxHeight
        return Mathf.RoundToInt(noise);
    }


    void GenerateMapArray()
    {
        // Temp variables
        int tempHeight;
        // Init the map array
        mapArray = new int[mapSize[0], mapSize[1], mapSize[2]];
        // Generate the map array
        for (int i = 0; i < mapSize[0]; i++)
        {
            for (int k = 0; k < mapSize[2]; k++)
            {
                // If the map block is in the sea, the map height will be 1
                if (i < seaWidth || i >= mapSize[0] - seaWidth || k < seaWidth || k >= mapSize[2] - seaWidth)
                {
                    // Fill the map block with the sea
                    mapArray[i, 0, k] = 0;
                    // Fill the rest map block with the air
                    for (int j = 1; j < mapSize[1]; j++)
                        mapArray[i, j, k] = 1;
                }
                else
                {
                    // Generate the map height

                    tempHeight = GetPerlinNoise(i, k);
                    // Debug.Log(tempHeight);
                    // Fill the map block with the ground
                    for (int j = 0; j < tempHeight; j++)
                    {
                        mapArray[i, j, k] = 2;
                        if (j == tempHeight - 1)
                        {
                            mapArray[i, j, k] = 3;
                        }
                        if (j == 0)
                        {
                            mapArray[i, j, k] = 4;
                        }
                    }

                    // Fill the rest map block with the air
                    for (int j = tempHeight; j < mapSize[1]; j++)
                        mapArray[i, j, k] = 1;
                }
            }
        }
    }


    float GetSinValue(float x, float z)
    {
        return seaHeight * Mathf.Sin(x / seaPower) * Mathf.Sin(z / seaPower);
    }


    void InitSeaCube()
    {
        for (int i = 0; i < mapSize[0]; i++)
        {
            for (int k = 0; k < mapSize[2]; k++)
            {
                // If the map block is the sea
                if (i < seaWidth || i >= mapSize[0] - seaWidth || k < seaWidth || k >= mapSize[2] - seaWidth)
                {
                    // Init the height of the sea cube with the sin value
                    MapGenerator.mapBlockArray[i, 0, k].transform.position = new Vector3(i, GetSinValue(i, k), k);
                    // Set the movement direction of the sea cube
                    if (GetSinValue(i, k) < GetSinValue(i + 1, k + 1))
                        EventAdder.AddEvent(MapGenerator.mapBlockArray[i, 0, k], "SeaMovement", new string[3] { "moveUp", "seaHeight", "waveSpeed" }, new object[3] { 1, seaHeight, waveSpeed });
                    else
                        EventAdder.AddEvent(MapGenerator.mapBlockArray[i, 0, k], "SeaMovement", new string[3] { "moveUp", "seaHeight", "waveSpeed" }, new object[3] { -1, seaHeight, waveSpeed });
                }
            }
        }
    }
    IEnumerator SupplyDrop(GameObject supply)
    {
        while (true)
        {
            // Wait for some seconds
            yield return new WaitForSeconds(5f);

            // Generate a random position within the map bounds
            int x = Random.Range(0, mapSize[0]);
            int y = Random.Range(20, 30);
            int z = Random.Range(0, mapSize[2]);

            // Instantiate the Supply prefab at the random position
            GameObject bombObject = Instantiate(supply, new Vector3(x, y, z), Quaternion.identity);
        }

    }
    IEnumerator BombDrop(GameObject bomb, GameObject Bomber, float v, float h)
    {
        while (true)
        {
            // Wait for some seconds
            // Time it should take for the bomber to reach the target position
            float journeyTime = 5f;



            yield return new WaitForSeconds(journeyTime);
            // Generate a random position within the map bounds

            int x = Random.Range(0, mapSize[0]);
            int y = Random.Range(40, 50);
            int z = Random.Range(0, mapSize[2]);
            // Start time of the journey
            float startTime = Time.time;

            // Instantiate the Supply prefab at the random position
            GameObject BomberObject = Instantiate(Bomber, new Vector3(v, h, 20), Quaternion.identity);



            while (Time.time < startTime + journeyTime)
            {
                // Calculate the fraction of the journey completed
                float fractionComplete = (Time.time - startTime) / journeyTime;

                // Set the bomber's position to be a point along the line between its start and target positions
                BomberObject.transform.position = Vector3.Lerp(BomberObject.transform.position, new Vector3(v, h, 76), fractionComplete);

                // Wait for the next frame before reevaluating the position
                yield return null;
            }
            Destroy(BomberObject);
            Instantiate(bomb, new Vector3(x, y, z), Quaternion.identity);
            Instantiate(bomb, new Vector3(x, y, z), Quaternion.identity);
            Instantiate(bomb, new Vector3(x, y, z), Quaternion.identity);
            Instantiate(bomb, new Vector3(x, y, z), Quaternion.identity);
        }

    }

    private void Awake() {
         // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        GenerateMapArray();
        MapGenerator.GenerateMap(map, mapSize, mapArray, prefabList);
        InitSeaCube();
    }

    void Start()
    {

        StartCoroutine(SupplyDrop(supply));
        StartCoroutine(SupplyDrop(supply));
        StartCoroutine(SupplyDrop(supply));
        StartCoroutine(BombDrop(bomb, bomber, 25, 80));
        StartCoroutine(BombDrop(bomb, bomber, 56, 56));
        StartCoroutine(BombDrop(bomb, bomber, 100, 90));
    }

}
