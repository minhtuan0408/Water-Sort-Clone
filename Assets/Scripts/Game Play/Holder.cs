using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public Liquid LiquidPrefab;
    public Transform StackLiquid;

    public float MaxTotalValue = 1.6f;
    public bool IsPicked;

    private Coroutine _moveCoroutine;
    private List<Liquid> _liquids = new List<Liquid>();
    public Vector3 OriginalPoint { get; private set; }
    public Vector2 PickPoint { get; private set; }

    public Transform[] DropPoint;
    public Liquid TopLiquid => _liquids.LastOrDefault();
    

    public bool IsFull => _liquids.Sum(l => l.Value) >= MaxTotalValue;
    public void Init()
    {
        OriginalPoint = transform.position;
        IsPicked = false;
        PickPoint = transform.position + 0.5f * Vector3.up;
    }


    private void StopMoveIfAlready()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
    }

    public void MoveTo(Vector2 point, float speed = 1)
    {
        StopMoveIfAlready();

        _moveCoroutine = StartCoroutine(MoveToEnumerator(point, speed));
    }

    public void PickThis()
    {
        if (IsPicked)
        {
            throw new InvalidOperationException();
        }
        IsPicked = true;

        MoveTo(PickPoint, speed: 5);
    }
    
    public void UndoPickedThis()
    {
        IsPicked = false;
        MoveTo(OriginalPoint, speed: 5);
    }

    private IEnumerator MoveToEnumerator(Vector2 point, float speed = 1)
    {
        while ((Vector2)transform.position != point)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                point,
                speed * Time.deltaTime
            );
            yield return null;
        }
    }

    public void AddLiquid (int groupId, float value = 0)
    {
        var topPoint = GetTopPoint();
        var liquid = Instantiate(LiquidPrefab, StackLiquid);
        liquid.transform.position = topPoint;
        liquid.Value = value;
        liquid.GroupId = groupId;
        _liquids.Add(liquid);
    }
    public Vector2 GetTopPoint() 
    {
        return transform.TransformPoint(_liquids.Sum(l => l.Value) * Vector2.up);
    }

    //public IEnumerator DropAction(Holder Take) 
    //{
    //    // lấy bên phải - take trái
    //    float dir = transform.position.x - Take.transform.position.x;
    //    float tiltAngle = (dir < 0) ? -45f : 45f;

    //    StartCoroutine(MoveToEnumerator(point: Take.DropPoint[0].position, speed:5));

    //    yield return null;
    //}


    public IEnumerator Drop(Holder Take)
    {
        float maxValueTaked = Take._liquids.Sum(l => l.Value);

        float availableSpace = MaxTotalValue - maxValueTaked;
        Liquid top = TopLiquid;

        if (top == null)
            yield break;

        float valueTaked = Mathf.Min(top.Value, availableSpace);
        int groupId = top.GroupId;

        if (maxValueTaked < 0.01f && Take.TopLiquid == null)
        {
            top.Value -= valueTaked;
            print("Lấy hết");
            if (top.Value <= 0)
            {
                _liquids.Remove(top);
                Destroy(top.gameObject);
            }
            Take.AddLiquid(groupId, valueTaked);
        }
        else
        {
            Take.TopLiquid.Value += valueTaked;
            top.Value -= valueTaked;
            print("Lấy một phần");
            if (top.Value  <.1f)
            {
                _liquids.Remove(top);
                Destroy(top.gameObject);
            }
        }

        float ehe = Take._liquids.Sum(l => l.Value);
        print(Take.IsFull + " " + ehe);

        yield return null;
    }

    public IEnumerator TransferLiquidSmoothly(Liquid from, Liquid to, float value, float duration)
    {
        float elapsed = 0f;
        float startFrom = from.Value;
        float startTo = to.Value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float newFrom = Mathf.Lerp(startFrom, startFrom - value, t);
            float newTo = Mathf.Lerp(startTo, startTo + value, t);

            from.Value = newFrom;
            to.Value = newTo;



            yield return null;
        }

        from.Value = startFrom - value;
        to.Value = startTo + value;

        if (from.Value <= 0.01f)
        {
            _liquids.Remove(from);
            Destroy(from.gameObject);
        }
    }

}
