using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBlockSpawner : MonoBehaviour
{
    [SerializeField]
    private BlockArrangeSystem blockArrangeSystem;
    [SerializeField]
    private Transform[] blockSpawnPosition;
    [SerializeField]
    private GameObject[] blockPrefabs;
    [SerializeField]
    private Vector3 spawnGapAmount = new Vector3(10, 0, 0);
    public Transform[] BlockSpawnPosition => blockSpawnPosition;

    public void SpawnBlocks()
    {
        StartCoroutine("OnSpawnBlocks");
    }

    private IEnumerator OnSpawnBlocks()
    {
        for(int i = 0; i < blockSpawnPosition.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);

            int index = Random.Range(0, blockPrefabs.Length);
            Vector3 spawnPosition = blockSpawnPosition[i].position + spawnGapAmount;

            GameObject clone = Instantiate(blockPrefabs[index], spawnPosition, Quaternion.identity, blockSpawnPosition[i]);

            clone.GetComponent<DragBlock>().SetUp(blockArrangeSystem, blockSpawnPosition[i].position);
        }
    }
}
