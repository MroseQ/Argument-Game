using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreDialogLoader : MonoBehaviour
{

    public StoryProgress SP;
    public int plotIDplaceholder = -1, renderPlot;
    public GameObject plotBox, charBox;
    public TextMeshPro charMesh, plotMesh;
    public TextAsset dialogText;
    public string[] spriteLines;
    public string[][] dialogLines;
    public bool canClick, fadeInactive, pauseGame, changeScene;
    public List<string[]> tuple = new List<string[]>();

    private void Start()
    {
        if (SP.storyProgress == 0)
        {
            SP.dialogSprite = new List<string>();
        }
        var splitFile = new string[] { "\r\n", "\r", "\n" };
        var splitLine = new char[] { ';' };
        switch (SP.storyProgress) // Changing the volume value for background music in every "DialogBox" scene.
        {
            case 4:
            case 10:
                GetComponents<AudioSource>()[0].volume = 0.085f;
                break;
            case 6:
                GetComponents<AudioSource>()[0].volume = 0.065f;
                break;
            case 8:
            case 14:
                GetComponents<AudioSource>()[0].volume = 0.09f;
                break;
            case 12:
            case 16:
                GetComponents<AudioSource>()[0].volume = 0.07f;
                break;

        }
            if (SP.storyProgress > 0) GameObject.Find("CubeFading").GetComponent<MeshRenderer>().enabled = false;
            
            // Spliting a line in file.
            foreach (string line in dialogText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries))
            {
                tuple.Add(line.Split(splitLine, System.StringSplitOptions.None));
            }
            dialogLines = tuple.ToArray();
             
            charMesh = charBox.GetComponent<TextMeshPro>();
            plotMesh = plotBox.GetComponent<TextMeshPro>();

            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack " + SP.storyProgress);
            GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(); // A way to exit the app.
            if (GameObject.Find("CubeFading").GetComponent<MeshRenderer>().material.color.a == 0f)
            {
                if (plotIDplaceholder != SP.plotID) //Checking if there was a mouse click.
                {
                    plotIDplaceholder = SP.plotID;
                    if (renderPlot == SP.dialogLineCount[SP.storyProgress])
                    {
                        GameObject.Find("CubeFading").GetComponent<MeshRenderer>().enabled = true;
                        GetComponent<ScreenFading>().fadeOut = false;
                        canClick = false;
                        renderPlot = 0;
                        changeScene = true;
                        if (SP.plotID == 68)
                        {
                            Application.Quit();
                        }
                    }
                    try
                    {
                        if (GetComponent<ScreenFading>().fadeOut)
                        {
                            ReplaceDialogText(dialogLines, SP.plotID); 
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && canClick) //If clicked on the mouse, program progresses with the story.
                {
                    SP.plotID++;
                    renderPlot++;
                    canClick = false;
                }
            }
    }

    private void LateUpdate()
    {
        if (changeScene)
        {
            changeScene = false;
            SP.storyProgress++;
            StartCoroutine(ChangeToArgument());
        }
    }

    private void ReplaceDialogText(string[][] dialogLines, int number) // Replacing text in DialogBox scene.
    {
        var dashSplit = dialogLines[number][0].Split('-', System.StringSplitOptions.RemoveEmptyEntries);
        if (dashSplit.Length == 1)
        {
            charMesh.text = dialogLines[number][0];
            SP.dialogSprite.Add("None");
        }
        else
        {
            charMesh.text = dashSplit[0];
            var otherSplit = dashSplit[0].Split(':', System.StringSplitOptions.RemoveEmptyEntries);
            SP.dialogSprite.Add(otherSplit[0] + '-' + dashSplit[1]);
        }
        plotBox.GetComponent<TextDisplayEffect>().Run(plotMesh.text = dialogLines[number][1]);
    }

    public IEnumerator ChangeToArgument() //Changing scene to "Argument".
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadSceneAsync(1);
    }
}

