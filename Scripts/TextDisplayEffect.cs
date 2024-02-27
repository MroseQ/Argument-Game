using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextDisplayEffect : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private StoryProgress SP;
    public bool isPlaying;
    public TextLoader textLoader;
    public void Run(string text)
    {
        textLoader.time = 0;
        StartCoroutine(TypeText(text));
        if(SceneManager.GetActiveScene().name == "DialogBox")
        {
            GameObject.Find("Main Camera").GetComponent<TextLoader>().canClick = false;
        }
    }

    public IEnumerator SwitchNext()
    {
        yield return new WaitUntil(() => textLoader.time >= textLoader.fadeTime);
        Destroy(GameObject.Find("LeftHitbox(Clone)"));
        Destroy(GameObject.Find("MidHitbox(Clone)"));
        Destroy(GameObject.Find("RightHitbox(Clone)"));
        yield return new WaitUntil(() => textLoader.time >= textLoader.changeTime);
        GameObject Camera = GameObject.Find("Main Camera");
        if (!Camera.GetComponent<TextLoader>().changeScene && !Camera.GetComponent<TextLoader>().pauseGame && !Camera.GetComponent<TextLoader>().rewind)
        {
            isPlaying = false;
        }
    }

    private IEnumerator TypeText(string text)
    {
        float t = 0;
        int charIndex = 0;
        int savedID = SP.argumentID;
        if (SceneManager.GetActiveScene().name == "Argument")
        {
            StartCoroutine(SwitchNext());
        }
        while(charIndex < text.Length && !GameObject.Find("Main Camera").GetComponent<TextLoader>().rewind && savedID==SP.argumentID)
        {
            t += Time.deltaTime * speed;
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
                t += Time.deltaTime * speed;
            }
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, text.Length);
            GetComponent<TextMeshPro>().text = text.Substring(0, charIndex);
            yield return null;
        }
        GetComponent<TextMeshPro>().text = text;
        GameObject.Find("Main Camera").GetComponent<TextLoader>().canClick = true;
    }
}
