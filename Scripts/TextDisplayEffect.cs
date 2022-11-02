using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextDisplayEffect : MonoBehaviour
{
    private float speed = 25f;
    public bool isPlaying;
    public StoryProgress SP;

    public void Run(string text)
    {
        SP = Resources.Load<StoryProgress>("ActualStoryProgress");
        TypeText(text);
    }

    public IEnumerator SwitchNext() //Sending message to program after some time to let the next lines appear.
    {
        GameObject Camera = GameObject.Find("Main Camera");
        var lifes = SP.lifes;
        yield return new WaitForSecondsRealtime(5.8f);
        if (!Camera.GetComponent<CoreArgumentLoader>().changeScene && !Camera.GetComponent<CoreArgumentLoader>().pauseGame && !Camera.GetComponent<CoreArgumentLoader>().rewind && lifes == SP.lifes)
        {
            isPlaying = false;
        }
    }

    private void TypeText(string text)
    {

        if (SceneManager.GetActiveScene().name == "Argument")
        {
            StartCoroutine(TextChanging(!GameObject.Find("Main Camera").GetComponent<CoreArgumentLoader>().rewind, text));
            StartCoroutine(SwitchNext());
            GameObject.Find("Main Camera").GetComponent<CoreArgumentLoader>().canClick = true;
        }
        else
        {
            StartCoroutine(TextChanging(true, text));
            GameObject.Find("Main Camera").GetComponent<CoreDialogLoader>().canClick = true;
        }
        
    }

    private IEnumerator TextChanging(bool condition, string text) //The animation of text apperance.
    {
        float t = 0;
        int charIndex = 0;
        while (charIndex < text.Length && condition)
        {
            t += Time.deltaTime * speed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, text.Length);
            GetComponent<TextMeshPro>().text = text.Substring(0, charIndex);
            yield return null;
        }
        GetComponent<TextMeshPro>().text = text;
    }
}