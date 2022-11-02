using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectionSequence : MonoBehaviour
{
    public GameObject core;
    public float speed;
    public bool dontRepeat, start;
    public List<GameObject> gameObjects;
    public StoryProgress SP;
    public bool objectionPlay;
    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "DialogBox") {
            if(SP.storyProgress == 0)
            {
                GameObject.Find("Core").SetActive(false);
            }
            for(int i = 0; i < 5; i++)
            {
                gameObjects[i].transform.localScale = new Vector3(gameObjects[i].transform.localScale.x, SP.wideGameObjects[i], gameObjects[i].transform.localScale.z);
            }
            StartCoroutine(FadeOut());
        }
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (Application.isPlaying && start) //Changing the position for "Core" gameObject.
        {
            core.transform.localScale = new Vector3(1.5f / GetComponent<Camera>().orthographicSize, 1.5f / GetComponent<Camera>().orthographicSize,1);
            core.transform.Translate(ReturnPosition() * Time.deltaTime * speed);
            if (core.transform.position.x < 8f && !dontRepeat)
            {
                if (!GetComponents<AudioSource>()[1].isPlaying && !objectionPlay)
                {
                    objectionPlay = true;
                    GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Igiari"), 1.5f); //Playing audio for objection phase.
                }
                dontRepeat = true;
                core.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,core.transform.position.z);
                StartCoroutine(Wider());
            }
        }
        if (scene.name == "DialogBox" && !dontRepeat) //Setting correctly the "Core" gameObject on DialogBox secne.
        {
            dontRepeat = true;
            core.transform.localScale = new Vector3(1.158f, 1.158f, 1);
            core.transform.position = new Vector3(transform.position.x, transform.position.y,core.transform.position.z);
        }
    }
    public Vector3 ReturnPosition() //Calculating the position for the next frame.
    {
        float x = core.transform.position.x;
        if (x > gameObject.transform.position.x)
        {
            x = Mathf.Log(x, 1f / 2f) - 4f - gameObject.transform.position.x;
        }
        else
        {
            x = 0;
        }
        return new Vector3(x, 0);
    }
    private IEnumerator FadeOut() // Making the "Core" fade out.
    {
        StartCoroutine(Wider());
        foreach (GameObject obj in gameObjects)
        {
            if (gameObjects[5] == obj)
            {
                StartCoroutine(FadeFunction(obj));
            }
            else
            {
                StartCoroutine(FadeFunction(obj));
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        Destroy(core);
    }
    private IEnumerator Wider() // Making the components of "Core" wider every frame.
    {
        while (gameObjects[4].GetComponent<MeshRenderer>().material.color.a > 0f)
        {
            for (int i = 0; i < 5; i++)
            {
                gameObjects[i].transform.localScale += new Vector3(0, Time.deltaTime * Random.Range(5.5f, 6f) / (i + 1), 0);
                SP.wideGameObjects[i] = gameObjects[i].transform.localScale.y;
            }
            if (SceneManager.GetActiveScene().name == "Argument")
            {
                GetComponents<AudioSource>()[0].volume -= 0.00025f;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    private IEnumerator FadeFunction(GameObject obj) // Made to set new values of fade to "Core" gameObject components.
    {
        Color color;
        float fadeAmount;
        obj.TryGetComponent(out TextMeshPro component);
        if (component)
        {
            color = component.color;
        }
        else
        {
            color = obj.GetComponent<MeshRenderer>().material.color;
        }
        while (color.a > 0f)
        {
            fadeAmount = color.a - (1f * Time.deltaTime * 2);
            if (fadeAmount < 0f) fadeAmount = 0f;
            color = new Color(color.r, color.g, color.b, fadeAmount);
            if (component)
            {
                component.color = color;
            }
            else
            {
                obj.GetComponent<MeshRenderer>().material.color = color;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
