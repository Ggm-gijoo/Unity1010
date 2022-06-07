using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBlock : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curveMovement;
    [SerializeField]
    private AnimationCurve curveScale;

    private BlockArrangeSystem blockArrangeSystem;

    private float appearTime = 0.3f;
    private float returnTime = 0.1f;

    [field:SerializeField]
    public Vector2Int BlockCount { private set; get; }

    public Color Color { private set; get; }
    public Vector3[] ChildBlocks { private set; get; }

    public void SetUp(BlockArrangeSystem blockArrangeSystem,Vector3 parentPosition)
    {
        this.blockArrangeSystem = blockArrangeSystem;
        Color = GetComponentInChildren<SpriteRenderer>().color;

        ChildBlocks = new Vector3[transform.childCount];
        for (int i = 0; i < ChildBlocks.Length; ++i)
        {
            ChildBlocks[i] = transform.GetChild(i).localPosition;
        }
        StartCoroutine(OnMoveTo(parentPosition,appearTime));
    }

    private void OnMouseDown()
    {
        StopCoroutine("OnScaleTo");
        StartCoroutine("OnScaleTo", Vector3.one);
    }

    private void OnMouseDrag()
    {
        Vector3 gap = new Vector3(0, BlockCount.y + 1, 10);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition)+gap;
    }

    private void OnMouseUp()
    {
        float x = Mathf.RoundToInt(transform.position.x-BlockCount.x%2*0.5f)+BlockCount.x % 2*0.5f;
        //�������� x��(float)���� x�� ��ġ�� ����� ��ġ�� Ȧ������ ¦������ üũ�Ѵ�.
        //���� Ȧ����� �������� x������ 1�� �����ϰ�, �̸� �ݿø��Ͽ� ������ ���·� �����.
        //�׸��� �� ���� �ٽ� ���Ѵ�.
        float y = Mathf.RoundToInt(transform.position.y - BlockCount.y % 2 * 0.5f) + BlockCount.y % 2 * 0.5f;

        transform.position = new Vector3(x, y, 0);

        bool isSuccess = blockArrangeSystem.TryArrangementBlock(this);

        if (!isSuccess)
        {
            StopCoroutine("OnScaleTo");
            StartCoroutine("OnScaleTo", Vector3.one * 0.75f);
            StartCoroutine(OnMoveTo(transform.parent.position, returnTime));
        }
    }

    public IEnumerator OnMoveTo(Vector3 end, float time)
    {
        Vector3 start = transform.position;
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(start, end, curveMovement.Evaluate(percent));

            yield return null;
        }
    }

    public IEnumerator OnScaleTo(Vector3 end)
    {
        Vector3 start = transform.localScale;
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / returnTime;

            transform.localScale = Vector3.Lerp(start, end, curveScale.Evaluate(percent));

            yield return null;
        }
    }
}
