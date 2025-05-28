using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {get; private set;}
    public Camera Camera;

    [SerializeField] private Holder _holderPrefab;

    private bool isDropping;
    public State CurrentState { get; private set; } = State.None;

    public List<Holder> _holders = new List<Holder>();

    public Level Level { get; private set; }
    private void Awake()
    {
        Instance = this;
        CurrentState = State.Playing;

        Level = GameManager.LoadGameData.Level;

        LoadLevel();
    }

    private List<List<LiquidData>> GetLiquidDataFromLevel(Level level)
    {
        var result = new List<List<LiquidData>>();

        foreach (var holderColumn in level.map)
        {
            var liquidList = new List<LiquidData>();
            foreach (var groupId in holderColumn.values)
            {
                liquidList.Add(new LiquidData
                {
                    groupId = groupId,
                    value = 0.4f
                });
            }
            result.Add(liquidList);
        }

        return result;
    }


    private void LoadLevel()
    {
        var listPos = PositionForHolders(Level.map.Count, 1.2f);
        var liquidDataLists = GetLiquidDataFromLevel(Level);
        for (var i = 0; i < 5; i++)
        {
            var holder = Instantiate(_holderPrefab, listPos[i], Quaternion.identity);

            _holders.Add(holder);
            holder.Fill(liquidDataLists[i]);
        }
    }

    public List<Vector2> PositionForHolders(int count, float distance)
    {
        var positions = new List<Vector2>();

        int maxPerRow = 3;
        int rows = Mathf.CeilToInt(count / (float)maxPerRow);

        Vector2 center = transform.position;

        for (int row = 0; row < rows; row++)
        {
            int itemsInRow = (row == rows - 1) ? count - row * maxPerRow : maxPerRow;

            float rowY = center.y - row * (distance + 1.5f); // cách đều theo chiều dọc
            float totalWidth = (itemsInRow - 1) * distance;
            float startX = center.x - totalWidth / 2f;

            for (int i = 0; i < itemsInRow; i++)
            {
                float x = startX + i * distance;
                positions.Add(new Vector2(x, rowY));
            }
        }

        return positions;
    }


    private void Update()
    {
        if (CurrentState != State.Playing) 
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var collider = Physics2D.OverlapPoint(Camera.ScreenToWorldPoint(Input.mousePosition));
            if (collider != null)
            {
                Holder holder = collider.GetComponent<Holder>();
                PickThisHolder(holder);
            }
        }
    }

    private void PickThisHolder(Holder holder)
    {
        if (isDropping) return;

        // lấy Holder nếu IsPicked = True;

        var nextHolder = _holders.FirstOrDefault(x => x.IsPicked);
        if (nextHolder != null && nextHolder != holder)
        {
            print(holder.TopLiquid.GroupId + " " + nextHolder.TopLiquid.GroupId);
            print(nextHolder);
            print("Next holder " + nextHolder.IsFull + " " + nextHolder.IsPicked);
            print("holder " + holder.IsFull + " " + holder.IsPicked);
            if (holder.TopLiquid == null || holder.TopLiquid.GroupId == nextHolder.TopLiquid.GroupId && !holder.IsFull)
            {
                //isDropping = true;
                StartCoroutine(nextHolder.Drop(Take:holder));

                nextHolder.UndoPickedThis();
            }
            else
            {
                nextHolder.UndoPickedThis();
            }
        }
        else if (holder.TopLiquid != null)
        {
            if (!holder.IsPicked)
                holder.PickThis();
            else
            {
                holder.UndoPickedThis();
            }
        }
    }
}



public enum State
{
    None,
    Playing,
    Over
}

public struct LiquidData
{
    public int groupId;
    public float value;
}

[System.Serializable]
public class LevelGroup { public List<Level> levels; }
[System.Serializable]
public class Level 
{ 
    public int no; 
    public List<HoldersColumn> map; 

    public int TotalHoldersColumn => map.Count;
}
[System.Serializable]
public class HoldersColumn { public List<int> values; }


