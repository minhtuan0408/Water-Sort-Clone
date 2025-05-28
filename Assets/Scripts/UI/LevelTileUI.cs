using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelTileUI : MonoBehaviour, IPointerClickHandler
{
    public struct ViewModel
    {
        public Level Level { get; set; }
        public bool Locked { get; set; }
        public bool Completed { get; set; }
    }

    public event Action<LevelTileUI> Clicked;

    [SerializeField] private TextMeshProUGUI _txt;
    [SerializeField] private GameObject _lockMark;


    private ViewModel _mViewModel;

    public ViewModel MViewModel
    {
        get => _mViewModel;
        set
        {
            _txt.text = value.Level.no.ToString();
            _lockMark.SetActive(value.Locked);
            _mViewModel = value;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }

}
