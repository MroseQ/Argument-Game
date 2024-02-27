using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArgumentSpriteBehaviour : MonoBehaviour
{
    float speed;
    public StoryProgress SP;
    int argumentPlaceHolder = -1, argumentRender;
    public GameObject destination, corelatedText;
    public ScreenFading failFade;
    public TextAsset spritesText;
    public string[] spriteLines;
    public float radius;
    public float radiants;
    public Camera renderCamera;
    public bool startNew;
    public TextLoader textLoader;

    void Start()
    {
        var splitFile = new string[] { "\r\n", "\r", "\n" };
        spriteLines = spritesText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        if (SP.storyProgress == 1)
        {
            argumentRender = -1;
        }
        else
        {
            argumentRender = renderCamera.GetComponent<TextLoader>().dialogLineCount[SP.storyProgress - 2] - 1;
        }
    }
    void LateUpdate()
    {
        if(!renderCamera.GetComponent<TextLoader>().pauseGame && !renderCamera.GetComponent<TextLoader>().rewind && GameObject.Find("DebateStart").GetComponent<DebateStartScript>().ended)
            {
            if (argumentPlaceHolder != SP.argumentID && argumentRender != SP.argumentID)
            {
                argumentPlaceHolder = SP.argumentID;
                radius = 11f;
                radiants = Random.Range(1,3);
                if (radiants == 1)
                {
                    radiants = Random.Range(35f, 55f);
                }
                else
                {
                    radiants = Random.Range(-55f, -35f);
                }
                if (SP.argumentID != -1)
                {
                    var audioComponents = renderCamera.GetComponents<AudioSource>();
                    var spriteName = spriteLines[SP.argumentID].Split('-', System.StringSplitOptions.RemoveEmptyEntries);
                    if (SP.argumentID != 0 && SP.argumentID != 1 && SP.argumentID != 57 && SP.argumentID != 52)
                    {
                        audioComponents[1].PlayOneShot(Resources.Load<AudioClip>("Audio/" + spriteName[0] + "/" + spriteName[1]));
                    }
                    if (gameObject.name == "SpriteMid" && (SP.argumentID == 0 || SP.argumentID == 1))
                    {
                        audioComponents[1].PlayOneShot(Resources.Load<AudioClip>("Audio/" + spriteName[0] + "/" + spriteName[1]));
                    }
                    if(gameObject.name == "SpriteMid" && (SP.argumentID == 52 || SP.argumentID == 57))
                    {
                        StartCoroutine(PlayAudioMultipleTimes(audioComponents[1],spriteName,0.035f));
                    }
                }
                StartCoroutine(ChangeAlpha());
                startNew = false;
            }
            else
            {
                if (!startNew)
                {
                    startNew = true;
                    transform.localPosition = new Vector3(Mathf.Sin(radiants) * radius, Mathf.Cos(radiants) * radius, 0);
                    destination.transform.localPosition = new Vector3(-Mathf.Sin(radiants) * radius / 1.3f, -Mathf.Cos(radiants) * radius / 1.3f, 0);
                    if (argumentRender != SP.argumentID)
                    {
                        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprites/" + spriteLines[SP.argumentID]);
                    }
                    var spriteColor = GetComponent<SpriteRenderer>().color;
                    GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
                    var corelatedTextColor = corelatedText.GetComponent<TextMeshPro>().color;
                    corelatedText.GetComponent<TextMeshPro>().color = new Color(corelatedTextColor.r, corelatedTextColor.g, corelatedTextColor.b, 1f);  
                }
                if (Mathf.Abs(transform.localPosition.x) >= Mathf.Abs(transform.localPosition.y))
                {
                    speed = (Mathf.Pow(transform.localPosition.x, 2) + 0.1f) * Time.deltaTime;

                }
                else
                {
                    speed = (Mathf.Pow(transform.localPosition.y, 2) + 0.1f) * Time.deltaTime;
                }
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination.transform.localPosition, speed);
            }
        }
        if (renderCamera.GetComponent<TextLoader>().rewind)
        {
            argumentPlaceHolder = -1;
        }
    }

    public IEnumerator ChangeAlpha()
    {
        yield return new WaitUntil(() => textLoader.time >= textLoader.fadeTime || textLoader.time == 0);
        yield return new WaitUntil(() => !renderCamera.GetComponent<TextLoader>().pauseGame);
        int savedID = SP.argumentID;
        var alpha = 1f;
        var corelatedTextColor = corelatedText.GetComponent<TextMeshPro>().color;
        var spriteColor = GetComponent<SpriteRenderer>().color;
        while (alpha > 0f && (savedID==SP.argumentID || renderCamera.GetComponent<TextLoader>().rewind)) {
            alpha -= 2f * Time.deltaTime;
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
                alpha -= 2f * Time.deltaTime;
            }
            GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            corelatedText.GetComponent<TextMeshPro>().color = new Color(corelatedTextColor.r, corelatedTextColor.g, corelatedTextColor.b, alpha);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator PlayAudioMultipleTimes(AudioSource source, string[] name, float delay)
    {
        source.PlayOneShot(Resources.Load<AudioClip>("Audio/" + name[0] + "/" + name[1]));
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(Resources.Load<AudioClip>("Audio/" + name[0] + "/" + name[1]));
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(Resources.Load<AudioClip>("Audio/" + name[0] + "/" + name[1]));
    }
}
