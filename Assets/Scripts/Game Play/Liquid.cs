using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private int _groupId;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color[] _groupColors = new Color[0];
    private float _value;

    public int GroupId
    {
        get => _groupId;
        set
        {
            _groupId = value;
            _renderer.color = _groupColors[value];
        }
    }

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            _renderer.transform.localScale = new Vector3(1, value, 1);
        }
    }
}
