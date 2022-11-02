using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckHitbox : MonoBehaviour
{
    public StoryProgress SP;
    public Camera renderCamera;
    public int[] winnings = new int[] { -1, -1, -1, -1};
    void Start() // Determining on which SP.argumentID should a hitbox react to progress the story. 
    {
        renderCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        SP = Resources.Load("ActualStoryProgress")as StoryProgress;
        if (name == "LeftHitbox(Clone)")
        {
            winnings[0] = 10;
            winnings[1] = 26;
            winnings[2] = 43;
        } else if (name == "MidHitbox(Clone)")
        {
            winnings[0] = 17;
            winnings[1] = 20;
            winnings[2] = 46;
        }
        else
        {
            winnings[0] = 4;
            winnings[1] = 31;
            winnings[2] = 35;
            winnings[3] = 57;
        }
        
    }
    void OnMouseDown()
    {
        if (!renderCamera.GetComponent<CoreArgumentLoader>().pauseGame)
        {
            StartCoroutine(Zoom()); 
        }
    }

    public IEnumerator Zoom() //Zooming and focusing the camera on a hitbox.
    {
        float distanceX,distanceY;
        if (!renderCamera.GetComponent<CoreArgumentLoader>().pauseGame)
        {
            renderCamera.GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Zoom"), 0.7f);
            renderCamera.GetComponent<CoreArgumentLoader>().pauseGame = true;
            for (float i = 105; i > 0; i--)
            {
                yield return new WaitForEndOfFrame();
                distanceX = gameObject.transform.position.x - renderCamera.transform.position.x;
                distanceY = gameObject.transform.position.y - renderCamera.transform.position.y;
                renderCamera.transform.Translate(new Vector3(distanceX * i / 3096f, distanceY * i / 5504f));
                if (renderCamera.orthographicSize - (i / 19.5f * Time.deltaTime) < 2.385979f)
                {
                    renderCamera.orthographicSize = 2.385979f;
                }
                else
                {
                    renderCamera.orthographicSize -= i / 19.5f * Time.deltaTime;
                }
            }
        }
        yield return new WaitForSecondsRealtime(2);

        // By clicking on the hitbox, the program checks if the SP.argumentID corresponds to any winnings.
        if (SP.argumentID == winnings[0] || SP.argumentID == winnings[1] || SP.argumentID == winnings[2] || SP.argumentID == winnings[3]) 
        {
            renderCamera.GetComponent<CoreArgumentLoader>().changeScene = true; // Starting to change scene.
        }

        else // If the hitbox was clicked but SP.argumentID didn't correspond to any winnings, the "taking damage" sequence is activated.
        {
            if (SP.storyProgress == 19)
            {
                renderCamera.GetComponents<AudioSource>()[0].Play();
            }
            renderCamera.GetComponent<CoreArgumentLoader>().pauseGame = false;
            renderCamera.GetComponent<ScreenFading>().ifRed = true;
            renderCamera.GetComponents<AudioSource>()[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Damage"),1.7f);
            SP.lifes--;
            Color redColor;
            GameObject.Find("CubeFading").GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f, 1f);
            StartCoroutine(renderCamera.GetComponent<CoreArgumentLoader>().PlayBack());
            
            // Reverting the zoom and returning back the camera position to its origin.
            // Creating the flashing red screen which symbolise the player receiving a damage.
            for (float i = 105; i > 0; i--) 
            {
                yield return new WaitForEndOfFrame();
                float fadeAmount = GameObject.Find("CubeFading").GetComponent<MeshRenderer>().material.color.a;
                fadeAmount -= 2f * Time.deltaTime;
                if (fadeAmount < 0f) fadeAmount = 0f;
                redColor = new Color(1f, 0f, 0f, fadeAmount);
                GameObject.Find("CubeFading").GetComponent<MeshRenderer>().material.color = redColor;
                distanceX = 0f - renderCamera.transform.position.x;
                distanceY = -0.5f - renderCamera.transform.position.y;
                renderCamera.transform.Translate(new Vector3(distanceX * i / 1000f, distanceY * i / 1000f));
                if(renderCamera.orthographicSize + (i / 20f * Time.deltaTime) > 4.400071f)
                {
                    renderCamera.orthographicSize = 4.400071f;
                }
                else
                {
                    renderCamera.orthographicSize += i / 20f * Time.deltaTime;
                }
            }
            renderCamera.GetComponent<ScreenFading>().ifRed = false;
            renderCamera.transform.position = new Vector3(0f, -0.5f, -10);
        }
    }
}
