using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private int _groupId;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color[] _groupColors = new Color[0];

    public float Value;

    private void Start()
    {
        _renderer.color = _groupColors[_groupId];
        _renderer.enabled = true;
        _renderer.transform.localScale = new Vector3(1, Value, 1);
    }
}
