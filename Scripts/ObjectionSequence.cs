using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ObjectionSequence : MonoBehaviour
{
    public GameObject core, canvas;
    public float speed;
    public bool dontRepeat, start;
    public List<Vector3> sizeList;
    public List<GameObject> gameObjects;
    public List<UnityEngine.Color> colorList;
    public StoryProgress SP;
    public bool objectionPlay;
    [SerializeField] private PostProcessVolume postProcessVolume;
    private Bloom bloom;
    private LensDistortion lensDistortion;
    private Vector3 coreStartPosition;
    private Vector3 coreSize;
    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "DialogBox") {
            if(SP.storyProgress == 0)
            {
                GameObject.Find("Core").SetActive(false);
            }
            core.transform.rotation = Quaternion.Euler(new Vector3(0,0,SP.coreRotationZ));
            for(int i = 0; i < 5; i++)
            {
                //core.transform.localScale = new Vector3(1.16f, 1.16f, 1);
                core.transform.position = new Vector3(transform.position.x, transform.position.y, core.transform.position.z);
                gameObjects[i].transform.localScale = new Vector3(gameObjects[i].transform.localScale.x, SP.wideGameObjects[i], gameObjects[i].transform.localScale.z);
            }
            StartCoroutine(FadeOut());
            StartCoroutine(Wider());
        }
        else
        {
            bloom = postProcessVolume.profile.GetSetting<Bloom>();
            lensDistortion = postProcessVolume.profile.GetSetting<LensDistortion>();

        }
        if(SP.storyProgress == 19)
        {
            coreStartPosition = core.transform.position;
            coreSize = core.transform.localScale;
            foreach(GameObject obj in gameObjects)
            {
                obj.TryGetComponent(out TextMeshPro component);
                if (component)
                {
                    colorList.Add(component.color);
                    sizeList.Add(component.transform.localScale);
                }
                else
                {
                    colorList.Add(obj.GetComponent<MeshRenderer>().material.color);
                    sizeList.Add(obj.transform.localScale);
                }
            }
        }
        
    }

    void Update()
    {
        if (start)
        {
            float ortographicSize = GetComponent<Camera>().orthographicSize;
            Vector3 newScale = core.transform.localScale;
            newScale.x = ortographicSize / 4.4f;
            newScale.y = ortographicSize / 4.4f;
            core.transform.localScale = newScale;
            Vector2 newPos = Vector2.MoveTowards(core.transform.position, gameObject.transform.position, speed*Time.deltaTime);
            core.transform.position = new Vector3(newPos.x,newPos.y,core.transform.position.z);
            if (Mathf.Abs(core.transform.position.x-gameObject.transform.position.x) < 8f && !dontRepeat)
            {
                if (!GetComponents<AudioSource>()[1].isPlaying && !objectionPlay)
                {
                    objectionPlay = true;
                    GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Igiari"), 1.5f);
                }
                dontRepeat = true;
                StartCoroutine(Wider());
            }
        }
    }

    public IEnumerator ObjectionLite()
    {
        float tick = 0;
        while(tick < 2f)
        {
            float ortographicSize = GetComponent<Camera>().orthographicSize;
            Vector3 newScale = core.transform.localScale;
            newScale.x = ortographicSize / 4.4f;
            newScale.y = ortographicSize / 4.4f;
            core.transform.localScale = newScale;
            Vector2 newPos = Vector2.MoveTowards(core.transform.localPosition, new(0, 0), speed * Time.deltaTime);
            core.transform.localPosition = new Vector3(newPos.x, newPos.y, core.transform.localPosition.z);
            if (Mathf.Abs(core.transform.position.x - gameObject.transform.position.x) < 8f && !dontRepeat)
            {
                GetComponent<TextLoader>().pauseGame = false;
                GetComponent<TextLoader>().time = 0;
                if (!objectionPlay)
                {
                    objectionPlay = true;
                    GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Igiari"), 1.5f);
                }
                dontRepeat = true;
                StartCoroutine(Wider(3));
            }
            tick += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(0f, -0.5f, -10);
        GetComponent<TextLoader>().ResetItems();
        StartCoroutine(FadeOut(true));
        
    }

    public Vector3 ReturnPosition()
    {
        float x = core.transform.position.x;
        if (x > gameObject.transform.position.x)
        {
            x = Mathf.Log(x, 1f / 2f) + 4f + gameObject.transform.position.x;
        }
        else
        {
            x = 0;
        }
        return new Vector3(x, 0, 0);
    }


    private IEnumerator Wider(int speed = 1)
    {
        while (gameObjects[4].GetComponent<MeshRenderer>().material.color.a > 0f)
        {
            if (bloom != null && start)
            {
                bloom.intensity.value -= Time.deltaTime * 5;
                if(lensDistortion.intensity.value > 0)
                {
                    lensDistortion.intensity.value = (lensDistortion.intensity.value - Time.deltaTime * 5 <= 0) ? 0 : lensDistortion.intensity.value - Time.deltaTime * 21;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                gameObjects[i].transform.localScale += new Vector3(0, speed * Time.deltaTime * Random.Range(5.5f, 6f) / (i + 1), 0);
                SP.wideGameObjects[i] = gameObjects[i].transform.localScale.y;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    private IEnumerator FadeOut()
    {
        foreach (GameObject obj in gameObjects)
        {
            StartCoroutine(FadeFunction(obj));
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(1f);
        Destroy(core);
    }

    private IEnumerator FadeOut(bool dontDestroy)
    {
        foreach (GameObject obj in gameObjects)
        {
            float ortographicSize = GetComponent<Camera>().orthographicSize;
            Vector3 newScale = core.transform.localScale;
            newScale.x = ortographicSize / 4.4f;
            newScale.y = ortographicSize / 4.4f;
            core.transform.localScale = newScale;
            StartCoroutine(FadeFunction(obj));
            yield return new WaitForSecondsRealtime(0.1f);
        }
        core.transform.SetParent(null);
        GetComponent<Animator>().SetTrigger("Fail");
        yield return new WaitUntil(() => gameObjects[^1].GetComponent<TextMeshPro>().color.a < 0.01f);
        
        
        

        core.transform.position = coreStartPosition;
        core.transform.localScale = coreSize;
        int index = 0;
        foreach(GameObject obj in gameObjects)
        {
            obj.TryGetComponent(out TextMeshPro component);
            if (component)
            {
                component.color = colorList[index];
                component.transform.localScale = sizeList[index];
            }
            else
            {
                obj.GetComponent<MeshRenderer>().material.color = colorList[index];
                obj.transform.localScale = sizeList[index];
            }
            index++;
        }
        dontRepeat = false;
        objectionPlay = false;
        StopAllCoroutines();
    }

    private IEnumerator FadeFunction(GameObject obj)
    {
        UnityEngine.Color color;
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
            color = new UnityEngine.Color(color.r, color.g, color.b, fadeAmount);
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
