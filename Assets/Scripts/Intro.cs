using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    private float a;
    public UILabel skipText;
    public UITexture uitexture;
    public VideoPlayer video;
    public bool isPlaying;
    void Start()
    {
        skipText.gameObject.SetActive(false);
        video.loopPointReached += OnFinish;
        if (video)
        {
            video.Play();

            Invoke("StartPlay",1.5f);
        }
    }
    void OnFinish(VideoPlayer vp)
    {
        SceneManager.LoadScene("FirstScene");
    }
    public void StartPlay()
    {
        isPlaying = true;
        skipText.gameObject.SetActive(true);
        Invoke("hideText",4.25f);
    }
    public void hideText()
    {
        
        skipText.gameObject.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }
        uitexture.mainTexture = video.texture;
        if (Input.GetKeyDown(KeyCode.Mouse1) )
        {
            OnFinish(null);
        }
        
    }
}
