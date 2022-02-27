using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GetVolumeSettings : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource audioSrc;


    [Header("Tags")]
    [SerializeField] private string sliderTag = "";

    [Header("Parametrs")]
    [SerializeField] private float volume;

    public float Volume
    {
        get { return volume; }
    }

    private void Start()
    {
        GetVolumeSettings[] objs = FindObjectsOfType<GetVolumeSettings>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();

        this.volume = FindObjectOfType<SaveSystem>().GetComponent<SaveSystem>().GetDataVolume();

        this.audioSrc.volume = this.volume;

        SetVolume(volume);

    }

    public void  SetVolume(float vol)
    {  
        this.volume = vol;
        this.audioSrc.volume = this.volume;
        GameObject.FindGameObjectWithTag(sliderTag).GetComponent<Slider>().value = audioSrc.volume;

    }
}
