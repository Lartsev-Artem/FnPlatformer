using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuControll : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private DieCanvas _DieCanvas;
    [SerializeField] private FinishCanvas _FinishCanvas;

   private bool _menu_is_active;
    public bool Menu_is_active { get { return _menu_is_active;  } }
    public EventTrigger trigger;
    private void Start()
    {
        _menu = GameObject.Find("MenuPauseCanvas");
        if (_menu)
        {
            _menu.SetActive(false);
            _menu_is_active = false;
        }
        _DieCanvas = FindObjectOfType<DieCanvas>();
        _DieCanvas.gameObject.SetActive(false);

        _FinishCanvas = FindObjectOfType<FinishCanvas>();
       if(_FinishCanvas) _FinishCanvas.gameObject.SetActive(false);

#if UNITY_ANDROID
        // Resources.Load("AndroidCanvas");
        Transform canvas = Resources.Load<Transform>("Canvas\\AndroidCanvas");
        canvas= Instantiate(canvas, Vector3.zero, Quaternion.identity);

        canvas.gameObject.GetComponentInChildren<Button>().onClick.AddListener(FindObjectOfType<ShootCharacter>().ShootAndroid);

        canvas.Find("Exit").GetComponent<Button>().onClick.AddListener(ContinueOrPause);


        trigger = canvas.gameObject.GetComponentInChildren<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { FindObjectOfType<StatesCharachter>().ShieldDown(); });
        trigger.triggers.Add(entry);

        trigger = canvas.gameObject.GetComponentInChildren<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { FindObjectOfType<StatesCharachter>().ShieldUp(); });
        trigger.triggers.Add(entry);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ContinueOrPause();
    }


    public void ContinueOrPause()
    {
        _menu_is_active = !_menu_is_active;
        _menu.SetActive(_menu_is_active);

        Time.timeScale = _menu_is_active ? 0.0F : 1.0F;
    }

    public void StartDie()
    {
        _DieCanvas.gameObject.SetActive(true);
        Time.timeScale = 0.0F;
    }

    public void StartFinish(int number_scene)
    {

        _FinishCanvas.gameObject.SetActive(true);
        _FinishCanvas._number_next_scene = number_scene;
        Time.timeScale = 0.0F;
    }
}