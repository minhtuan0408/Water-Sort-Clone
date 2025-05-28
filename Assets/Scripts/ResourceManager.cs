using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitResourceData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}

public partial class ResourceManager
{
    [SerializeField] private TextAsset lvAssets;
    private readonly List<Level> _levels = new List<Level>();

    public LevelGroup group;

    // load dữ liệu từ Json vào mảng
    private void InitResourceData()
    {
        group = JsonUtility.FromJson<LevelGroup>(lvAssets.text);
        _levels.Clear();
        _levels.AddRange(group.levels);

    }

    // lấy level trong mảng
    public static Level GetLevel(int no)
    {
        return Instance._levels.FirstOrDefault(l => l.no == no);
    }

    // lấy danh sách level trong mảng
    public static IEnumerable<Level> GetLevels()
    {
        return Instance._levels;
    }

    // check xem có bị lock hay không
    public static bool IsLevelLocked(int no)
    {
        var completedLevel = GetCompletedLevel();
        return no > completedLevel + 1;
    }

    // Mở khoá nếu hoàn thành
    public static int GetCompletedLevel()
    {
        return PlayerPrefs.GetInt($"Level_Complete");
    }
}
