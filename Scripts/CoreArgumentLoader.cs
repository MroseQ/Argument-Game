using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreArgumentLoader : MonoBehaviour
{
    public StoryProgress SP;
    public int argumentPlaceHolder;
    public GameObject leftTextBox, midTextBox, rightTextBox, lifes;
    public TextMeshPro leftMesh, midMesh, rightMesh;
    public TextAsset leftText, midText, rightText;
    public string[] leftLines, midLines, rightLines;
    public string[][] dialogLines;
    public bool canClick, fadeInactive, rewind, pauseGame, changeScene;

    private void Start()
    {
        if (SP.storyProgress == 3)
        {
            SP.lifes = 5; // After completing the training, the lifes number is reverted to original.
        }

        var splitFile = new string[] { "\r\n", "\r", "\n" };

        switch (SP.storyProgress) // Changing the volume value for background music in every "Argument" scene.
        {
            case 1:
            case 3:
            case 5:
            case 7:
            case 9:
                GetComponents<AudioSource>()[0].volume = 0.09f;
                break;

            case 11:
            case 13:
                GetComponents<AudioSource>()[0].volume = 0.06f;
                break;

            case 15:
            case 17:
                GetComponents<AudioSource>()[0].volume = 0.075f;
                break;

            case 19:
                GetComponents<AudioSource>()[0].volume = 0.25f;
                break;
        }

        if (SP.storyProgress == 1)
        {
            argumentPlaceHolder = -1;
            SP.argumentID = -1;
        }
        else
        {
            argumentPlaceHolder = SP.dialogLineCount[SP.storyProgress - 2] - 1;
            SP.argumentID = SP.dialogLineCount[SP.storyProgress - 2] - 1;
        }

        //Starting the sounds
        GetComponents<AudioSource>()[0].clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack " + SP.storyProgress);
        if (SP.storyProgress == 19) GetComponents<AudioSource>()[0].time = 1f;
        GetComponents<AudioSource>()[0].Play();
        GetComponents<AudioSource>()[1].clip = Resources.Load<AudioClip>("Audio/StartDebaty");
        GetComponents<AudioSource>()[1].PlayDelayed(0.5f);
        
        //Loading text files.
        leftLines = leftText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        midLines = midText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        rightLines = rightText.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(); // Way to exit the app.
        if (!rightTextBox.GetComponent<TextDisplayEffect>().isPlaying
                && GameObject.Find("CubeFading").GetComponent<MeshRenderer>().material.color.a == 0f
                && !rewind && !pauseGame && GameObject.Find("Debate").GetComponent<DebateStartScript>().ended)
        {
            rightTextBox.GetComponent<TextDisplayEffect>().isPlaying = true;

            // Checks for a loop. If not, load next text lines.
            if (SP.argumentID == SP.dialogLineCount[SP.storyProgress] - 1 && !rewind) 
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
        lifes.GetComponent<TextMeshPro>().text = SP.lifes.ToString(); // Refreshing the life counter.
    }

    private void LateUpdate()
    {
        if (changeScene)
        {
            changeScene = false;
            SP.storyProgress++;
            StartCoroutine(ChangeToDialog());
        }
        if (SP.lifes <= 0)
        {
            StartCoroutine(ChangeToGameOver()); 
        }
    }

    public IEnumerator ChangeToDialog() //Changes to the next part of a story.
    {
        GetComponent<ObjectionSequence>().start = true;
        yield return new WaitForSecondsRealtime(4);
        SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator PlayBack() // Reverts progress to the start of a scene and starts a new debate loop.
    {
        GameObject.Find("SpriteMid").GetComponent<ArgumentSpriteBehaviour>().isPlaying = false;
        GameObject.Find("SpriteRight").GetComponent<ArgumentSpriteBehaviour>().isPlaying = false;
        GameObject.Find("SpriteLeft").GetComponent<ArgumentSpriteBehaviour>().isPlaying = false;
        rewind = true;
        SP.argumentID = argumentPlaceHolder;
        leftMesh.text = "";
        rightMesh.text = "";
        midMesh.text = "";
        yield return new WaitUntil(() => leftMesh.color.a <= 0);
        yield return new WaitForSeconds(2.3f);
        rightTextBox.GetComponent<TextDisplayEffect>().isPlaying = false;
        rewind = false;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator ChangeToGameOver() // Changes scene to "GameOver".
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadSceneAsync(2);
    }
}