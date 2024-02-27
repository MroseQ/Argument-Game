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
        if(!GameObject.Find("Video Player").GetComponent<VideoPlayer>().isPlaying && start)
        {
            SP.storyProgress = 0;
            SP.plotID = 0;
            SP.lifes = 5;
            Application.Quit();
        }
    }
    public IEnumerator Starting()
    {
        yield return new WaitForSeconds(2);
        start = true;
    }
}
