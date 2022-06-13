using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private BackgroundBlockSpawner backgroundBlockSpawner;
    [SerializeField]
    private BackgroundBlockSpawner foregroundBlockSpawner;
    [SerializeField]
    private DragBlockSpawner dragBlockSpawner;
    [SerializeField]
    private BlockArrangeSystem blockArrangeSystem;

    public int CurrentScore { private set; get; }
    public int HighScore { private set; get; }

    private BackgroundBlock[] backgroundBlocks;
    private int currentDragBlockCount;

    private readonly Vector2Int blockCount = new Vector2Int(10, 10);
    private readonly Vector2 blockHalf = new Vector2(0.5f, 0.5f);
    private readonly int maxDragBlockCount = 3;

    private List<BackgroundBlock> filledBlockList;

    private void Awake()
    {
        CurrentScore = 0;
        HighScore = PlayerPrefs.GetInt("HighScore");

        filledBlockList = new List<BackgroundBlock>();

        backgroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        backgroundBlocks = new BackgroundBlock[blockCount.x * blockCount.y];
        backgroundBlocks = foregroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        blockArrangeSystem.SetUp(blockCount, blockHalf, backgroundBlocks, this);

        StartCoroutine(SpawnDragBlock());
    }

    private IEnumerator SpawnDragBlock()
    {
        currentDragBlockCount = maxDragBlockCount;

        dragBlockSpawner.SpawnBlocks();

        yield return new WaitUntil(() => IsCompleteSpawnBlocks());
    }

    public bool IsCompleteSpawnBlocks()
    {
        int count = 0;

        for (int i = 0; i < dragBlockSpawner.BlockSpawnPosition.Length; ++i)
        {
            if (dragBlockSpawner.BlockSpawnPosition[i].childCount != 0 && dragBlockSpawner.BlockSpawnPosition[i].GetChild(0).localPosition == Vector3.zero)
            {
                count++;
            }
        }
        return count == dragBlockSpawner.BlockSpawnPosition.Length;
    } 

    


    public void AfterBlockArrangement(DragBlock block)
    {
        StartCoroutine("OnAfterBlockArrangement", block);
    }

    public IEnumerator OnAfterBlockArrangement (DragBlock block)
    {
        Destroy(block.gameObject);

        int filledLineCount = CheckFilledLine();

        int lineScore = filledLineCount == 0 ? 0 : (int)(Mathf.Pow(2, filledLineCount - 1) * 10);
        CurrentScore += block.ChildBlocks.Length + lineScore;

        yield return StartCoroutine(DestroyFilledBlocks(block));

        currentDragBlockCount--;
        if(currentDragBlockCount <= 0)
        {
            yield return StartCoroutine(SpawnDragBlock());
        }

        yield return new WaitForEndOfFrame();

        if(IsGameOver())
        {
            Debug.Log("게임 오버");

            if(CurrentScore>= HighScore)
            {
                PlayerPrefs.SetInt("HighScore", CurrentScore);
            }
        }


    }

    private int CheckFilledLine()
    {
        int filledLineCount = 0;

        filledBlockList.Clear();

        for(int y = 0; y < blockCount.y; ++y)
        {
            int fillBlockCount = 0;
            for (int x = 0; x < blockCount.x; ++x)
            {
                if (backgroundBlocks[y * blockCount.x + x].gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                {
                    fillBlockCount++;
                }
                //if (backgroundBlocks[y * blockCount.x + x].BlockState == BlockState.Fill)
                //    fillBlockCount++;
            }
            if (fillBlockCount == blockCount.x)
            {
                for(int x = 0; x < blockCount.x; ++x)
                {
                    filledBlockList.Add(backgroundBlocks[y * blockCount.x + x]);
                }
                filledLineCount++;
            }

        }

        for (int x = 0; x < blockCount.x; ++x)
        {
            int fillBlockCount = 0;
            for (int y = 0; y < blockCount.y; ++y)
            {
                if (backgroundBlocks[y * blockCount.x + x].gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                {
                    fillBlockCount++;
                }
                //if (backgroundBlocks[y * blockCount.x + x].BlockState == BlockState.Fill)
                //    fillBlockCount++;
            }
            if (fillBlockCount == blockCount.y)
            {
                for (int y = 0; y < blockCount.y; ++y)
                {
                    filledBlockList.Add(backgroundBlocks[y * blockCount.x + x]);
                }
                filledLineCount++;
            }

        }

        return filledLineCount;
    }

    private IEnumerator DestroyFilledBlocks(DragBlock block)
    {
        filledBlockList.Sort((a,b)=>(a.transform.position - block.transform.position).sqrMagnitude.CompareTo((b.transform.position - block.transform.position).sqrMagnitude));

        for (int i = 0; i < filledBlockList.Count; ++i)
        {
            filledBlockList[i].EmptyBlock();

            yield return new WaitForSeconds(0.01f);
        }

        filledBlockList.Clear();
    }

    private bool IsGameOver()
    {
        int dragBlockCount = 0;
        for(int i = 0; i < dragBlockSpawner.BlockSpawnPosition.Length; i++)
        {
            if(dragBlockSpawner.BlockSpawnPosition[i].childCount !=0)
            {
                dragBlockCount++;

                if (blockArrangeSystem.IsPossibleArrangement(dragBlockSpawner.BlockSpawnPosition[i].GetComponentInChildren<DragBlock>()))
                {
                    return false;
                }
            }
        }

        return dragBlockCount != 0;
    }
}
