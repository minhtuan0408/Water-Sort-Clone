using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] private LevelTileUI _levelTileUIPrefab;
    [SerializeField] private RectTransform _content;
    private readonly List<LevelTileUI> _tiles = new List<LevelTileUI>();

    private void Awake()
    {
        LoadList();
    }
    private void LoadList()
    {
        var list = ResourceManager.GetLevels().ToList();

        foreach (var level in list)
        { 
            var levelTileUI = Instantiate(_levelTileUIPrefab, _content);
            levelTileUI.MViewModel = new LevelTileUI.ViewModel
            {
                Level = level,
                Locked = ResourceManager.IsLevelLocked(level.no),
                Completed = ResourceManager.GetCompletedLevel() >= level.no
            };

            levelTileUI.Clicked += OnTileClicked;
            _tiles.Add(levelTileUI);
        }
    }
    private void OnTileClicked(LevelTileUI tileUI)
    {
        if (tileUI.MViewModel.Locked)
            return;

        GameManager.LoadGame(new LoadGameData
        {
            Level = tileUI.MViewModel.Level,
        });
    }

}
