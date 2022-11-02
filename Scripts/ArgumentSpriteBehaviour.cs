using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArgumentSpriteBehaviour : MonoBehaviour
{
    float speed;
    public StoryProgress SP;
    int argumentPlaceHolder = -1, argumentRender;
    public GameObject destination;
    public TextAsset spritesText;
    public string[] spriteLines;
    public float radius;
    public float radiants;
    public Camera renderCamera;
    public GameObject corelatedText;
    public bool startNew, isPlaying;

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
            argumentRender = renderCamera.GetComponent<CoreArgumentLoader>().SP.dialogLineCount[SP.storyProgress - 2] - 1;
        }
    }

    void LateUpdate()
    {
        if(!renderCamera.GetComponent<CoreArgumentLoader>().pauseGame && !renderCamera.GetComponent<CoreArgumentLoader>().rewind && GameObject.Find("Debate").GetComponent<DebateStartScript>().ended)
            {
            if (argumentPlaceHolder != SP.argumentID && argumentRender != SP.argumentID)
            {
                isPlaying = true;
                argumentPlaceHolder = SP.argumentID;
                
                //Determining the direction and start position of a sprite in scene "Argument".
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
                    // Playing the corelated audio with a corelated sprite.
                    var audioComponents = renderCamera.GetComponents<AudioSource>();
                    var spriteName = spriteLines[SP.argumentID].Split('-', System.StringSplitOptions.RemoveEmptyEntries);
                    if (SP.argumentID != 0 && SP.argumentID != 1 && SP.argumentID != 57 && SP.argumentID != 52) //Playing audioclips of a sprite.
                    {
                        audioComponents[1].PlayOneShot(Resources.Load<AudioClip>("Audio/" + spriteName[0] + "/" + spriteName[1]));
                    }

                    //Playing only one audioclip for these 4 SP.argumentIDs
                    if (gameObject.name == "SpriteMid" && (SP.argumentID == 0 || SP.argumentID == 1 || SP.argumentID == 52 || SP.argumentID == 57)) 
                    {
                        audioComponents[1].PlayOneShot(Resources.Load<AudioClip>("Audio/" + spriteName[0] + "/" + spriteName[1]));
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
                    
                    // Setting where should the start position of a sprite be and where its destination should be.
                    transform.localPosition = new Vector3(Mathf.Sin(radiants) * radius, Mathf.Cos(radiants) * radius, 0);
                    destination.transform.localPosition = new Vector3(-Mathf.Sin(radiants) * radius / 1.3f, -Mathf.Cos(radiants) * radius / 1.3f, 0);
                    if (argumentRender != SP.argumentID)
                    {
                        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprites/" + spriteLines[SP.argumentID]); //Loading correct sprite.
                    }
                    var spriteColor = GetComponent<SpriteRenderer>().color;
                    GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
                    var corelatedTextColor = corelatedText.GetComponent<TextMeshPro>().color;
                    corelatedText.GetComponent<TextMeshPro>().color = new Color(corelatedTextColor.r, corelatedTextColor.g, corelatedTextColor.b, 1f);  
                }

                // Moving the sprites towards the destination in a part they should be displayed.
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
        if (renderCamera.GetComponent<CoreArgumentLoader>().rewind)
        {
            argumentPlaceHolder = -1;
        }
    }
    public IEnumerator ChangeAlpha() // Changing alpha of sprites so the SP.argumentID progresses.
    {
        StartCoroutine(isPlayingChange());
        yield return new WaitUntil(() => !isPlaying);
        StartCoroutine(Destroyer());
        var alpha = 1f;
        var corelatedTextColor = corelatedText.GetComponent<TextMeshPro>().color;
        var spriteColor = GetComponent<SpriteRenderer>().color;
        while (alpha > 0f) {
            alpha -= 2f * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            corelatedText.GetComponent<TextMeshPro>().color = new Color(corelatedTextColor.r, corelatedTextColor.g, corelatedTextColor.b, alpha);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Destroyer() // Destroys the hitboxex if they weren't clicked.
    {
        yield return new WaitUntil(() => !renderCamera.GetComponent<CoreArgumentLoader>().rewind);
        Destroy(GameObject.Find("LeftHitbox(Clone)"));
        Destroy(GameObject.Find("MidHitbox(Clone)"));
        Destroy(GameObject.Find("RightHitbox(Clone)"));
    }

    public IEnumerator isPlayingChange() // Checking for change of a bool "isPlaying". The change of this bool starts to load new sprites and text.
    {
        yield return new WaitForSecondsRealtime(5f);
        yield return new WaitUntil(() => !renderCamera.GetComponent<CoreArgumentLoader>().pauseGame);
        isPlaying = false;
    }
}
