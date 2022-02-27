using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
public class TerrainGeneration : MonoBehaviour
{
    /*После проверки генератора сделать массив в SaveSystem на стартовое число генерации. И тем самым левелы с 5-20 сделать псевдо рандомныими*/

    [SerializeField] private int Width;
    [SerializeField] private Transform Block; 

    [SerializeField] private List<Transform> Objects;
    [SerializeField] private List<float> Weight;

    [SerializeField] private Transform _charachter_transform;
    [SerializeField] private int _cur_x_position;
    [SerializeField] private int _cur_y_position;

    [SerializeField] private Transform _diespace_transform;
    [SerializeField] private bool is_rand;




    private ulong m_rand = 0; // начальное значение (в Save если ==0 то уровень не сохранен)
             
    private void Awake()
    {
        _charachter_transform = FindObjectOfType<StatesCharachter>().transform;
        _cur_x_position = (int)transform.position.x;
        _cur_y_position = (int)transform.position.y;

        _diespace_transform = FindObjectOfType<StaticDieSpace>().transform;

        Block = Resources.Load<Transform>("Block");
        Objects = LoadObject(new List<string> {
            "Enemy1",
            "Enemy2_1", "Enemy2_2", "Enemy2_3", 
            "Enemy3_1", "Enemy3_2", "Enemy3_3",
            "Enemy4_1", "Enemy4_2", "Enemy4_3",
            "Heart", "Energy",
            "EmtyObject"
        });
   
    }

    private void LevelSettings()
    {
        SaveSystem _save = FindObjectOfType<SaveSystem>();

       

        if (_save.GetStartNumberToLevel(_save.GetPrevScene()) == 0)
        {// число генерации для выбранной сцены
            m_rand = GenerateDateNumber();
            is_rand = true;
            Width = 1000;

            int dif = _save.GetDifficult(); if (dif == -1) dif = 0;
            switch (dif)
            {     

                // Enemy1  Enemy2_(1_2_3) Enemy3_(1_2_3) Heart Energy EmtyObject
                case 0:
                    Weight = new List<float> { 1f, 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0.5f, 0.5f, 1f };
                    break;
                case 1:
                    Weight = new List<float> { 1f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0.4f, 0.4f, 0.5f };
                    break;
                case 2:
                    Weight = new List<float> { 1f,  0f, 0f, 1f,  0f, 0f, 1f,  0f, 0f, 0.9f,  0.35f, 0.35f, 0.6f };
                    break;
            }
        }
        else {
            int num_level = _save.GetPrevScene();
            m_rand = _save.GetStartNumberToLevel(num_level);
            is_rand = false;
            Width = 100;

            if (num_level - _save.GetCountStaticLevel() < 3)
                // Enemy1  Enemy2_(1_2_3) Enemy3_(1_2_3) Heart Energy EmtyObject
                Weight = new List<float> { 1f, 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0.5f, 0.5f, 1f };
            else if (num_level - _save.GetCountStaticLevel() < 6)
                Weight = new List<float> { 1f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0.5f, 0.5f, 0.9f };
            else if (num_level - _save.GetCountStaticLevel() < 9)
                Weight = new List<float> { 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0.9f, 0.35f, 0.35f, 0.6f };
            else if (num_level - _save.GetCountStaticLevel() == 9)
            {
                _save.GetWeight(ref Weight, ref Width);
            }
        }
    }
    private void Start()
    {
        LevelSettings();
        NewGenerate();   
    }

    private void FixedUpdate()
    {
        if (_cur_x_position - _charachter_transform.position.x < 50)
        {
            ContinueGenerate();
            _diespace_transform.position = new Vector2(_diespace_transform.position.x + 50, _diespace_transform.position.y);
        }
    }

    private List<Transform> LoadObject(List<string> name_objects)
    {
        List<Transform> objects = new List<Transform>(7);

        for (int i = 0; i < name_objects.Count; i++)
        {
            objects.Add(Resources.Load<Transform>(name_objects[i]));
        }
        return objects;
    }

    private void NewGenerate()
    {
        //_cur_y_position= (int)transform.position.y; // block_zero;
        int checkStep;
        int currentLength = GenerateNumber() % 3 + 2; //[2,4];

        int start = _cur_x_position; //(int)transform.position.x;


       
        for (int i = 0; i < Width; i++) // second floor
        {

            // генерация платформы
            for (int k = 0; k < currentLength; k++)
            {
                Instantiate(Block, new Vector3(k + i + start, _cur_y_position, 0), Quaternion.identity);

            }
            //спаун чего -либо
            {
                int xx = i + currentLength / 2;
                int index = 0;
                do {
                    index = RandomChoiceFollowingDistribution(Weight);
                } while (currentLength<=2 && index==4); // большой монстр не спауница на маленьком блоке
             

                 Instantiate(Objects[index], new Vector3(xx + start, _cur_y_position + 2, 0), Quaternion.identity); 
            }

            i += currentLength - 1;
            //  выбираем сдвиг
            {
                int buf;
                do
                {
                    buf = GenerateNumber();
                    if (buf % 2 == 0)// 50%
                    {                 
                        checkStep = buf % 4; // [-2,3] 
                    }
                    else 
                    {
                        checkStep = -buf % 3;
                    }
                } while (checkStep == 0);

                _cur_y_position += checkStep;

                _cur_x_position += currentLength;
                // выбор длины следующий платформы 
                currentLength = GenerateNumber() % 4 + 2;
            }
        }

        if (!is_rand)
        {
            // генерация платформы
            for (int k = 0; k < 5; k++)
                Instantiate(Block, new Vector3(k + _cur_x_position, _cur_y_position, 0), Quaternion.identity);

            Transform finish = Resources.Load<Transform>("Parts\\Finish");
            Instantiate(finish, new Vector3(3 + _cur_x_position, _cur_y_position+2, 0), Quaternion.identity);

            for (int k = 0; k < 4; k++)
                for (int j = 0; j < 7; j++)
                    Instantiate(Block, new Vector3(5 +k + _cur_x_position, _cur_y_position+j, 0), Quaternion.identity);

            Destroy(this);
        }
    }

    private void ContinueGenerate()
    {
        int checkStep;
        int currentLength = GenerateNumber() % 3 + 2; //[2,4];

        int start = _cur_x_position;// (int)transform.position.x;



            // генерация платформы
            for (int k = 0; k < currentLength; k++)
            {
                 Instantiate(Block, new Vector3(k + start, _cur_y_position, 0), Quaternion.identity);

            }
            //спаун чего -либо
            {
                int xx = currentLength / 2;
                int index = 0;
                do
                {
                    index = RandomChoiceFollowingDistribution(Weight);
                } while (currentLength <= 2 && index == 4); // большой монстр не спауница на маленьком блоке


                Instantiate(Objects[index], new Vector3(xx + start, _cur_y_position + 2, 0), Quaternion.identity);
            }

            //  выбираем сдвиг
            {
                int buf;
                do
                {
                    buf = GenerateNumber();
                    if (buf % 2 == 0)// 50%
                    {
                        checkStep = buf % 4; // [-2,3] 
                    }
                    else
                    {
                        checkStep = -buf % 3;
                    }
                } while (checkStep == 0);

                _cur_y_position += checkStep;
                _cur_x_position += currentLength;

                // выбор длины следующий платформы 
                currentLength = GenerateNumber() % 4 + 2;
            }
    }
    private int GenerateNumber()
    {
        m_rand = xorshift64s(new xorshift64s_state(m_rand));
        int result = (int)xorshift64s(new xorshift64s_state(m_rand)) % 10;
        if (result >= 0)
            return result;
        else
            return -result; 
    }

    ulong xorshift64s(xorshift64s_state state)
    {
        ulong x = state.a;  /* The state must be seeded with a nonzero value. */
        x ^= x >> 12; // a
        x ^= x << 25; // b
        x ^= x >> 27; // c
        state.a = x;
        return x * 2685821657736338717; 
    }

    private ulong GenerateDateNumber()
    {
        System.DateTime date = System.DateTime.Now;
        string buf = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() 
            + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
        return System.Convert.ToUInt64(buf);
    }
    private   float GenerateIntToFloat(float start, float end)
    {
        float result = start;
        m_rand = xorshift64s(new xorshift64s_state(m_rand));
        ulong number = xorshift64s(new xorshift64s_state(m_rand));//сгенерировали случайное целое число

        number = number % 1000000;
        float buf = (float)number / 100000;
        while (buf > end) // ужасно не оптимизировано. (Спасает, что это Start(). Придумать алгоритм генерации float чисел !!!)
        {
            m_rand = xorshift64s(new xorshift64s_state(m_rand));
            number = xorshift64s(new xorshift64s_state(m_rand));//сгенерировали случайное целое число
            number = number % 1000000;
             buf = (float)number / 100000;
        }
        return buf;
    }
  
    public   int RandomChoiceFollowingDistribution(List<float> probabilities) //static
    {

        // Sum to create CDF:
    float[] cdf = new float[probabilities.Count];
        float sum = 0;
        for (int i = 0; i < probabilities.Count; ++i)
        {
            cdf[i] = sum + probabilities[i];
            sum = cdf[i];
        }
        // Choose from CDF:
        // float cdf_value = Random.Range(0.0f, cdf[probabilities.Count - 1]);
               float cdf_value = GenerateIntToFloat(0.0f, cdf[probabilities.Count-1]);
      
        int index = System.Array.BinarySearch(cdf, cdf_value); // откуда +1!!!????
  

        if (index < 0)
        {
          //  Debug.Log("VOOo");
            index = ~index; // if not found (probably won't be) BinarySearch returns bitwise complement of next-highest index.        
        }

        return index;
    }
}
 
class xorshift64s_state
{
    public ulong a;
    public xorshift64s_state(ulong x)
    {
        a = x;
    }
};

