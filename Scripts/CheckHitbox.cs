using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckHitbox : MonoBehaviour
{
    public StoryProgress SP;
    public Camera renderCamera;
    public int[] winnings = new int[] { -1, -1, -1, -1};
    private List<int>lastPhaseWinnings = new();
    private CrosshairManagement crosshairManagement;
    private float speed;
    public float tick;
    private ScreenFading failBoxFade;
    private Transform focus;
    void Start()
    {
        failBoxFade = GameObject.Find("FailPanel").GetComponent<ScreenFading>();
        speed = 5;
        renderCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        SP = Resources.Load("ActualStoryProgress")as StoryProgress;
        if (name == "LeftHitbox(Clone)")
        {
            focus = GameObject.Find("LeftFocus").transform;
            winnings[0] = 11;
            winnings[1] = 27;
            winnings[2] = 57;
            if (SP.storyProgress == 19)
            {
                lastPhaseWinnings.Add(54);
            }
        } else if (name == "MidHitbox(Clone)")
        {
            focus = GameObject.Find("MidFocus").transform;
            winnings[0] = 17;
            winnings[1] = 20;
            winnings[2] = 43;
            winnings[3] = 50;
            if (SP.storyProgress == 19)
            {
                lastPhaseWinnings.Add(53);
            }
        }
        else
        {
            focus = GameObject.Find("RightFocus").transform;
            winnings[0] = 4;
            winnings[1] = 31;
            winnings[2] = 36;
            if (SP.storyProgress == 19)
            {
                lastPhaseWinnings.Add(55);
                lastPhaseWinnings.Add(56);
            }
        }
        crosshairManagement = GameObject.Find("CrosshairManager").GetComponent<CrosshairManagement>();
    }

    private void OnMouseOver()
    {
        if (!renderCamera.GetComponent<TextLoader>().pauseGame&&!renderCamera.GetComponent<TextLoader>().rewind)
        {
            crosshairManagement.crosshairObject.GetComponent<Animator>().SetBool("OnTarget", true);
        }
        else
        {
            crosshairManagement.crosshairObject.GetComponent<Animator>().SetBool("OnTarget", false);
        }
    }

    private void OnMouseExit()
    {
        crosshairManagement.crosshairObject.GetComponent<Animator>().SetBool("OnTarget", false);
    }

    private void OnDestroy()
    {
        crosshairManagement.crosshairObject.GetComponent<Animator>().SetBool("OnTarget", false);
    }

    void OnMouseDown()
    {
        if (!renderCamera.GetComponent<TextLoader>().pauseGame)
        {
            StartCoroutine(Zoom());
        }
    }

    public IEnumerator Zoom()
    {
        float distanceX,distanceY;
        speed = 4f;
        tick = 0;
        if (!renderCamera.GetComponent<TextLoader>().pauseGame)
        {
            renderCamera.GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Zoom"), 0.5f);
            renderCamera.GetComponent<TextLoader>().pauseGame = true;
            renderCamera.GetComponent<Animator>().SetTrigger("Mid");
            while (tick < 3f)
            {
                yield return new WaitForEndOfFrame();
                distanceX = focus.position.x - renderCamera.transform.position.x;
                distanceY = focus.position.y - renderCamera.transform.position.y;
                Vector3 distance = new(distanceX, distanceY,renderCamera.transform.position.z);
                if (Vector3.Distance(renderCamera.transform.position, distance) > 0.25f)
                {
                    renderCamera.transform.position = Vector3.MoveTowards(renderCamera.transform.position, distance, speed * Time.deltaTime);
                }
                tick+=Time.deltaTime;
            }
        }
        if(winnings.Contains(SP.argumentID))
        {
            GameObject.Find("Core").transform.SetParent(GameObject.Find("Main Camera").transform);
            renderCamera.GetComponent<TextLoader>().changeScene = true;
        }
        else if (lastPhaseWinnings.Contains(SP.argumentID)) 
        {
            GameObject.Find("Core").transform.SetParent(GameObject.Find("Main Camera").transform);
            StartCoroutine(renderCamera.GetComponent<TextLoader>().Continue());
            StartCoroutine(renderCamera.GetComponent<ObjectionSequence>().ObjectionLite());
            yield return new WaitUntil(() => renderCamera.GetComponent<ObjectionSequence>().objectionPlay);
            failBoxFade.SetColorScreen(0.03f, 0.9f, 0.1f);
            renderCamera.GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Damage"), 1.4f); //dŸwiêk zmieniæ
            renderCamera.GetComponent<TextLoader>().keyWinnings.Remove(SP.argumentID);
        }
        else
        {
            tick = 0;
            renderCamera.GetComponent<TextLoader>().pauseGame = false;
            renderCamera.GetComponent<TextLoader>().time = 0;
            failBoxFade.SetColorScreen(0.92f, 0.03f, 0);
            renderCamera.GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Damage"), 1.4f);
            SP.lifes--;
            StartCoroutine(renderCamera.GetComponent<TextLoader>().PlayBack());
            renderCamera.GetComponent<Animator>().SetTrigger("Fail");
            while (tick < 3f)
            {
                yield return new WaitForEndOfFrame();
                Vector3 destination = new(0, -0.5f, renderCamera.transform.position.z);
                if (Vector3.Distance(renderCamera.transform.position, destination) > 0.02f)
                {
                    renderCamera.transform.position = Vector3.MoveTowards(renderCamera.transform.position, destination, 4 * speed * Time.deltaTime);
                }
                tick += Time.deltaTime;
            }
            renderCamera.transform.position = new Vector3(0f, -0.5f, -10);
        }
    } 
}
