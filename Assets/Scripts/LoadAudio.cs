using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class LoadAudio : MonoBehaviour
{
#if UNITY_ANDROID
    public string audioName = "track.mp3";
#else
    public string audioName = "track.mp3";
#endif

    private string path;
    
    public AudioSource audioSource;
    public AudioClip audioClip;

    private IEnumerator Start()
    {
        path = Path.Combine(Application.persistentDataPath, audioName);
        audioSource = GetComponent<AudioSource>();
        Debug.Log(Application.persistentDataPath);

        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);

        req.SendWebRequest();


        while (req.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        
        if (req.result != UnityWebRequest.Result.ProtocolError)
        audioClip = DownloadHandlerAudioClip.GetContent(req);


        if (audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

      //  Destroy(this);
    }

}