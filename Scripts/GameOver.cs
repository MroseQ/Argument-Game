using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameOver : MonoBehaviour
{
    public StoryProgress SP;
    public bool start;

    private void Start()
    {
        StartCoroutine(Starting());
    }
    void Update()
    {
        if(!GameObject.Find("Video Player").GetComponent<VideoPlayer>().isPlaying && start) //Exits the app.
        {
            Application.Quit();
        }
    }
    public IEnumerator Starting()
    {
        yield return new WaitForSeconds(2); //Waits for the video to start.
        start = true;
    }
}
