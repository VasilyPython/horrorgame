using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public GameObject[] cubes = new GameObject[6];
    public float activationInterval = 600f; 

    private void Start()
    {
        
        if (cubes.Length != 6)
        {
            Debug.LogError("Not enough exits.");
            return;
        }

        StartCoroutine(ActivateCubesPeriodically());
    }

    IEnumerator ActivateCubesPeriodically()
    {
        while (true)
        {
            ActivateRandomCubes(3);
            yield return new WaitForSeconds(activationInterval);

            DeactivateAllCubes();
            yield return new WaitForSeconds(1f);
        }
    }

    void ActivateRandomCubes(int count)
    {
        // Create a list to store cube indices
        List<int> cubeIndices = new List<int>();

        // Populate the list with indices
        for (int i = 0; i < cubes.Length; i++)
        {
            cubeIndices.Add(i);
        }

        // Shuffle the list of cube indices
        ShuffleList(cubeIndices);

        // Activate the first 'count' cubes in the shuffled list
        for (int i = 0; i < count; i++)
        {
            int index = cubeIndices[i];
            cubes[index].SetActive(true);
        }
    }

    void DeactivateAllCubes()
    {
        foreach (var cube in cubes)
        {
            cube.SetActive(false);
        }
    }

    // Fisher-Yates shuffle algorithm to shuffle a list
    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

