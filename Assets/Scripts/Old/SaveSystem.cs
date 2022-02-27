using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private string _path_save = "";
    private SaveLevel _save_level;
    private int _number_of_levels = 15;
    public int Number_of_levels { get { return _number_of_levels; } }

    private void Awake()
    {
        // var path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "_EditorCache");

        // _path_save = Path.Combine("C:\\Users\\Artem\\Desktop\\игра\\FnPlatformer", "SaveFile.json");
#if UNITY_ANDROID
        _path_save = Path.Combine(Application.persistentDataPath, "SaveFile.json");
#else
        _path_save = Path.Combine(Application.persistentDataPath, "SaveFile.json");
#endif

        if (File.Exists(_path_save))
            _save_level = JsonUtility.FromJson<SaveLevel>(File.ReadAllText(_path_save));
        else
            _save_level = new SaveLevel(_number_of_levels);

    }


    public int GetRecord()
    {
        return _save_level.Record;
    }
    public int GetDifficult()
    {
        return _save_level.Difficult;
    }
    public void SetDifficult(int dif)
    {
        _save_level.Difficult = dif;
    }
    public void SetRecord(int score )
    {
        _save_level.Record = score;
    }
    public int GetPrevScene()
    {
        return _save_level.Number_prev_level;
    }
    public void SetPrevScene(int num)
    {
        _save_level.Number_prev_level = num;
    }
    public void SaveCurResult(int num_of_star)
    {
        _save_level.SetStar(num_of_star);
        _save_level.OpenNewLevel();
    }
    public void GetDataLevel(ref int n,  ref bool[] is_active,  ref int[] stars)
    {
        n = _number_of_levels;
        is_active = new bool[n];
        stars = new int[n];

        _save_level.GetLevel(n, ref is_active, ref stars);
    }
    public void SetDataVolume(float vol)
    {
        _save_level.Volume = vol;
    }
    public float GetDataVolume()
    {
        return _save_level.Volume;
    }
    public void ExitGame()
    {
        File.WriteAllText(_path_save, JsonUtility.ToJson(_save_level));
    }

    private void OnDestroy()
    {
        ExitGame();
    }

    private void OnApplicationQuit()
    {
        ExitGame();
    }

    public void ClearSave()
    {
        _save_level.Clear();
        _save_level = new SaveLevel(_number_of_levels);
        FindObjectOfType<GetVolumeSettings>().GetComponent<GetVolumeSettings>().SetVolume(_save_level.Volume);
    }

    public ulong GetStartNumberToLevel(int i)
    {
        return _save_level.GetStartNumber(i);

    }
    public int GetCountStaticLevel()
    {
        return _save_level.NumStatic;

    }

    public void GetWeight (ref List<float> _weight, ref int _width)
    {
       const int size = 13 + 1; //13 --  число префабов генерации + Width
        float[] buf = new float[size];  
        _save_level.GetWeightLastLevel(size, ref buf);

        for (int i = 0; i < size-1; i++)
            _weight.Add(buf[i]);

        _width = (int)buf[size-1];
    }
}

[SerializeField]
public class SaveLevel
{
    [SerializeField] private const int _max_size = 15;
    [SerializeField] private const int _count_static_level = 5;

    [SerializeField] private bool[] _is_active_levels;
    [SerializeField] private int[] _number_stars;
    [SerializeField] private ulong[] _start_number_for_level = 
    {
        0, 0, 0, 0, 0 ,
        25920211803,  259202119633,  259202119817,  25920211994,   2592021191147,  
        259202119164, 2592021191751, 2592021191735, 2592021192111, 2592021192141
    };

    [SerializeField] private float[] _last_level_weight = { 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0.9f, 0.35f, 0.35f, 0.6f, 100 };

   [SerializeField] private int _number_prev_level;

    [SerializeField] private float _volume;
    [SerializeField] private int _record;
    [SerializeField] private int _difficult;

    public SaveLevel(int n = 1)
    {
        _is_active_levels = new bool[n];
        _number_stars = new int[n];

        _is_active_levels[0] = true;
        _number_stars[0] = 0;
        for (int i = 1; i < n; i++)
        {
            _is_active_levels[i] = false;
            _number_stars[i] = 0;
        }

        Volume = 0.5f;
        _number_prev_level = 0;
        _record = 0;
        _difficult = -1;  // -1 => не разу не вызывался рандомный уровень
    }

    public int NumStatic { get { return _count_static_level; } }
    public int Record { get { return _record; } set { _record = value; } }
    public int Difficult { get { return _difficult; } set { _difficult = value; } }

    public float Volume
    {
        get { return _volume; }
        set { if (Mathf.Abs(value) <= 1) _volume = value; }
    }
    public int Number_prev_level
    {
        get { return _number_prev_level; }
        set {  _number_prev_level = value; }
    }

    public void Clear()
    {
       _is_active_levels= null;
        _number_stars = null;
        _volume = 0;
        _number_prev_level = 0;
        _record = 0;
        _difficult = -1;

    }

    public void GetLevel(int n, ref bool[] is_active, ref int[] stars)
    {
        if (is_active != null && stars != null)
            for (int i = 0; i < n; i++)
            {
                is_active[i] = _is_active_levels[i];
                stars[i] = _number_stars[i];
            }
    }
    public void SetLevel(int n, ref bool[] is_active, ref int[] stars)
    {
        if (is_active != null && stars != null)
            for (int i = 0; i < n; i++)
            {
                _is_active_levels[i] = is_active[i];
                _number_stars[i] = stars[i];
            }
    }

    public void SetStar(int num_star)
    {
        _number_stars[Number_prev_level] = num_star;
    }
    public void OpenNewLevel()
    {
        if (Number_prev_level + 1 < _max_size)
        {
            _is_active_levels[Number_prev_level + 1] = true;
        }
    }

    public ulong GetStartNumber(int i)
    {
        if (i < _max_size)
        {
            return _start_number_for_level[i];
        }
        else return 0;
        
    }

    public void GetWeightLastLevel(int size, ref float[] weigth)
    {
        // size<=_last_level_weight // в последним хранится длина мира генерации
        for (int i = 0; i < size; i++)
            weigth[i] = _last_level_weight[i];
    }
}
