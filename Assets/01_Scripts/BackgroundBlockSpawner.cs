using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBlockSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject backGroundBlockPrefab;
    [SerializeField]
    private int orderInLayer;


    public BackgroundBlock[] SpawnBlocks(Vector2Int blockCount, Vector2 blockHalf)
    {
        BackgroundBlock[] blocks = new BackgroundBlock[blockCount.x * blockCount.y];
        for(int y = 0; y < blockCount.y; y++)
        {
            for(int x = 0; x < y; x++)
            {
                float px = -blockCount.x * 0.5f + blockHalf.x + x;
                float py = blockCount.y * 0.5f - blockHalf.y - y;
                Vector3 position = new Vector3(px, py, 0);
                GameObject clone = Instantiate(backGroundBlockPrefab, position,Quaternion.identity,transform);

                clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

                blocks[y * blockCount.x + x] = clone.GetComponent<BackgroundBlock>();
            }
        }
        return blocks;
    }
}
