using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrangeSystem : MonoBehaviour
{
    private Vector2Int blockCount;
    private Vector2 blockHalf;
    private BackgroundBlock[] backgroundBlocks;
    private StageController stageController;

    public void SetUp(Vector2Int blockCount, Vector2 blockHalf, BackgroundBlock[] backgroundBlocks, StageController stageController)
    {
        this.blockCount = blockCount;
        this.blockHalf = blockHalf;
        this.backgroundBlocks = backgroundBlocks;
        this.stageController = stageController;
    }

    public bool TryArrangementBlock(DragBlock block)
    {
        for(int i =0; i < block.ChildBlocks.Length; ++i)
        {
            Vector3 position = block.transform.position + block.ChildBlocks[i];

            if (!IsBlockInsideMap(position)) return false;

            if (!IsOtherBlockInThisBlock(position)) return false;
        }
        for (int i = 0; i < block.ChildBlocks.Length; ++i)
        {
            Vector3 position = block.transform.position + block.ChildBlocks[i];
            backgroundBlocks[PositionToIndex(position)].FillBlock(block.Color);
        }

        stageController.AfterBlockArrangement(block);

        return true;
    }

    private bool IsBlockInsideMap(Vector2 position)
    {
        if (position.x < -blockCount.x * 0.5f + blockHalf.x || position.x > blockCount.x * 0.5f - blockHalf.x || position.y < -blockCount.y * 0.5f + blockHalf.y || position.y > blockCount.y * 0.5f - blockHalf.y)
        {
            return false;
        }
        return true;
    }
    private int PositionToIndex(Vector2 position)
    {
        float x = blockCount.x * 0.5f - blockHalf.x + position.x;
        float y = blockCount.y * 0.5f - blockHalf.y - position.y;

        return (int)(y * blockCount.x + x);
    }

    private bool IsOtherBlockInThisBlock(Vector2 position)
    {
        int index = PositionToIndex(position);

        if (backgroundBlocks[index].BlockState == BlockState.Fill)
        {
            return false;
        }
        return true;
    }
}
