using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextLoader : MonoBehaviour
{
    public StoryProgress SP;
    public int plotIDplaceholder = -1, renderPlot, argumentPlaceHolder;
    public int[] dialogLineCount;
    public float time,  fadeTime, changeTime;
    public Scene scene;
    public GameObject leftTextBox, midTextBox, rightTextBox, plotBox, charBox, lifes, speedUpBox ,cubeFading;
    public TextMeshPro leftMesh, midMesh, rightMesh, charMesh, plotMesh;
    public TextAsset leftText, midText, rightText, dialogText, spriteText;
    public string[] leftLines, midLines, rightLines, spriteLines;
    public string[][] dialogLines;
    public bool canClick, fadeInactive, rewind, pauseGame, changeScene;
    public List<string[]> tuple = new List<string[]>();
    public AudioControl musicControl;
    public List<int> keyWinnings = new() { 53, 54, 55, 56 };

    private void Start()
    {
        if (SP.storyProgress == 0)
        {
            SP.lifes = 3;
            SP.dialogSprite = new List<string>();
}
        var splitFile = new string[] { "\r\n", "\r", "\n" };
        spriteLines = spriteText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        scene = SceneManager.GetActiveScene();
        var splitLine = new char[] { ';' };
        if (scene.name == "DialogBox")
        {
            if(SP.storyProgress == 2)
            {
                SP.lifes = 3;
            }
            if (SP.storyProgress > 0) cubeFading.GetComponent<Image>().enabled = false;
            foreach (string line in dialogText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries))
            {
                tuple.Add(line.Split(splitLine, System.StringSplitOptions.None));
            }
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack " + SP.storyProgress);
            GetComponent<AudioSource>().Play();
            dialogLines = tuple.ToArray();
            charMesh = charBox.GetComponent<TextMeshPro>();
            plotMesh = plotBox.GetComponent<TextMeshPro>();
            //ReplaceDialogText(dialogLines, SP.plotID);
        }
        else
        {
            if(SP.storyProgress == 21)
            {
                Application.Quit();
            }
            time = 0;
            fadeTime = 5.3f;
            changeTime = 6f;
            if (SP.storyProgress == 1)
            {
                argumentPlaceHolder = -1;
                SP.argumentID = -1;
            }
            else
            {
                argumentPlaceHolder = dialogLineCount[SP.storyProgress - 2] - 1;
                SP.argumentID = dialogLineCount[SP.storyProgress - 2] - 1;
            }
            
            SpawnArgumentProgress();
            cubeFading.GetComponent<ScreenFading>().fadeOut = true;
            StartCoroutine(cubeFading.GetComponent<ScreenFading>().SwitchOffWhenTransparent());
            GetComponents<AudioSource>()[0].clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack " + SP.storyProgress);
            GetComponents<AudioSource>()[0].Play();
            leftLines = leftText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
            midLines = midText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
            rightLines = rightText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        }
        switch (SP.storyProgress)
        {
            case 10:
            case 12:
            case 16:
            case 1:
            case 3:
            case 4:
            case 5:
            case 18:
            case 0:
            case 2:
                musicControl.SetBaseValue(0.1f);
                break;
            case 6:
            case 7:
            case 8:
            case 9:
            case 15:
            case 17:
            case 14:
            case 20:
                musicControl.SetBaseValue(0.09f);
                break;     
            case 11:
            case 13:
                musicControl.SetBaseValue(0.08f);
                break;
           
            case 19:
                StartCoroutine(ChangePitch(GetComponent<AudioSource>()));
                musicControl.SetBaseValue(0.09f);
                break;
        }
    }

    private IEnumerator ChangePitch(AudioSource source)
    {
        source.pitch = 0.76f;
        while(source.pitch < 1f)
        {
            source.pitch += 0.0001f;
            yield return new WaitForEndOfFrame();
        }
        source.pitch = 1;
    }

    private void SpawnArgumentProgress()
    {
        int amount = dialogLineCount[SP.storyProgress] - SP.argumentID-1;
        GameObject.Find("Lifes").transform.SetParent(GameObject.Find("Canvas").transform, true);
        for(int i = 1; i <= amount; i++) 
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("PreFabs/ProgressNode"));
            obj.GetComponent<ProgressLighting>().ID = SP.argumentID + i;
            obj.transform.position = new(7.5f-0.5f*(amount-i), -4.7f, -5f);
            obj.transform.SetParent(GameObject.Find("Canvas").transform,true);
        }
    }

    private void Update()
    {
        if (scene.name == "DialogBox")
        {
            if (cubeFading.GetComponent<Image>().color.a == 0f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && canClick && !EventSystem.current.IsPointerOverGameObject())
                {
                    SP.plotID++;
                    renderPlot++;
                    canClick = true;
                }
                if (plotIDplaceholder != SP.plotID)
                {
                    plotIDplaceholder = SP.plotID;
                    if (renderPlot == dialogLineCount[SP.storyProgress])
                    {
                        cubeFading.GetComponent<Image>().enabled = true;
                        cubeFading.GetComponent<ScreenFading>().RevertToBlack();
                        cubeFading.GetComponent<ScreenFading>().fadeOut = false;
                        canClick = false;
                        renderPlot = 0;
                        changeScene = true;
                    }
                    try
                    {
                        if (cubeFading.GetComponent<ScreenFading>().fadeOut)
                        {
                            ReplaceDialogText(dialogLines, SP.plotID);
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e);
                    }
                }
            }
        }
        else
        {
            if (!rightTextBox.GetComponent<TextDisplayEffect>().isPlaying
                && cubeFading.GetComponent<Image>().color.a == 0f
                && !rewind && !pauseGame && GameObject.Find("DebateStart").GetComponent<DebateStartScript>().ended)
            {
                rightTextBox.GetComponent<TextDisplayEffect>().isPlaying = true;
                if (SP.argumentID == dialogLineCount[SP.storyProgress] - 1 && !rewind)
                {
                    rewind = true;
                    StartCoroutine(PlayBack());
                }
                else
                {
                    SP.argumentID++;
                    leftTextBox.GetComponent<TextDisplayEffect>().Run(leftLines[SP.argumentID]);
                    midTextBox.GetComponent<TextDisplayEffect>().Run(midLines[SP.argumentID]);
                    rightTextBox.GetComponent<TextDisplayEffect>().Run(rightLines[SP.argumentID]);
                }
            }
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && !rewind && !pauseGame && !GetComponent<ObjectionSequence>().dontRepeat
                && GameObject.Find("DebateStart").GetComponent<DebateStartScript>().ended)
            {
                time += 2*Time.deltaTime;
                GetComponents<AudioSource>()[1].pitch = 1.5f;
                speedUpBox.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl) || rewind ||pauseGame)
            {
                GetComponents<AudioSource>()[1].pitch = 1f;
                speedUpBox.SetActive(false);
            }
            if (!rewind && !pauseGame)
            {
                time+=Time.deltaTime;
            }
            lifes.GetComponent<TextMeshPro>().text = SP.lifes.ToString();
        }
    }

    private void LateUpdate()
    {
        if (changeScene)
        {
            changeScene = false;
            SP.storyProgress++;
            if (scene.name == "DialogBox")
            {
                StartCoroutine(ChangeToArgument());
            }
            else
            {
                StartCoroutine(ChangeToDialog());
            }
        }
        if (SP.lifes <= 0)
        {
            StartCoroutine(ChangeToGameOver());
        }
    }

    private void ReplaceDialogText(string[][] dialogLines, int index)
    {
        if (dialogLines[index].Length == 3)
        {
            charMesh.text = dialogLines[index][0];
            SP.dialogSprite.Add(dialogLines[index][2]+"*");
        }
        else
        {
            var dashSplit = dialogLines[index][0].Split('-', System.StringSplitOptions.RemoveEmptyEntries);
            if (dashSplit.Length == 1)
            {
                charMesh.text = dialogLines[index][0];
                SP.dialogSprite.Add("None");
            }
            else
            {
                charMesh.text = dashSplit[0];
                SP.dialogSprite.Add(dialogLines[index][0].Replace(":", ""));
            }
        }
        plotBox.GetComponent<TextDisplayEffect>().Run(plotMesh.text = dialogLines[index][1]);
    }

    private IEnumerator FadeOutMusic()
    {
        while (GetComponent<AudioSource>().volume > 0f)
        {
            GetComponent<AudioSource>().volume -= Time.deltaTime/50;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<AudioSource>().volume = 0;
    }

    public IEnumerator ChangeToDialog()
    {
        StartCoroutine(FadeOutMusic());
        GetComponent<ObjectionSequence>().start = true;
        yield return new WaitForSecondsRealtime(1);
        SP.coreRotationZ = GetComponent<ObjectionSequence>().core.transform.localRotation.eulerAngles.z;
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator ChangeToArgument()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadSceneAsync(1);
    }

    public IEnumerator PlayBack()
    {
        rewind = true;
        SP.argumentID = argumentPlaceHolder;
        leftMesh.text = "";
        rightMesh.text = "";
        midMesh.text = "";
        yield return new WaitForSeconds(3f);
        Destroy(GameObject.Find("LeftHitbox(Clone)"));
        Destroy(GameObject.Find("MidHitbox(Clone)"));
        Destroy(GameObject.Find("RightHitbox(Clone)"));
        rightTextBox.GetComponent<TextDisplayEffect>().isPlaying = false;
        rewind = false;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Continue()
    {
        yield return new WaitForSeconds(3f);
        
        rightTextBox.GetComponent<TextDisplayEffect>().isPlaying = false;
        yield return new WaitForEndOfFrame();
    }

    public void ResetItems()
    {
        time = fadeTime;
        Destroy(GameObject.Find("LeftHitbox(Clone)"));
        Destroy(GameObject.Find("MidHitbox(Clone)"));
        Destroy(GameObject.Find("RightHitbox(Clone)"));
        leftMesh.text = "";
        rightMesh.text = "";
        midMesh.text = "";
    }

    public IEnumerator ChangeToGameOver()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadSceneAsync(2);
    }
}