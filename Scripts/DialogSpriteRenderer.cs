using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSpriteRenderer : MonoBehaviour
{
    public GameObject spriteBox;
    public StoryProgress SP;
    public GameObject renderCamera;
    public List<Sprite> spriteList = new();
    public int plotPlaceholder;
    public bool blueFade, whiteFade;

    private void Start()
    {
        plotPlaceholder = SP.plotID - 1;
    }

    private void FixedUpdate()
    {
        if (renderCamera.GetComponent<CoreDialogLoader>().plotIDplaceholder != -1 && plotPlaceholder != SP.plotID)
        {
            var audioClip = SP.dialogSprite[SP.plotID].Split('-', System.StringSplitOptions.RemoveEmptyEntries);
            var components = renderCamera.GetComponents<AudioSource>();
            plotPlaceholder = SP.plotID;
            spriteBox.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprites/" + SP.dialogSprite[SP.plotID]);
            
            // Playing the audioclips of every sprite visible in "DialogBox" scene
            if (audioClip.Length > 1 && SP.plotID != 49 && SP.plotID != 52 && SP.plotID != 54 && SP.plotID != 56)
            {
                components[1].clip = Resources.Load<AudioClip>("Audio/" + audioClip[0] + "/" + audioClip[1]);
                components[1].Play();
            }
            if (SP.plotID == 3 && !whiteFade) //Exception for SP.plotID == 3 when the white fade needs to appear with different sound and different background music.
            {
                whiteFade = true;
                StartCoroutine(ColorScreenFade(new Color(1f, 1f, 1f, 1f), 1.2f));
                components[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Zoom"), 1.5f);
                components[0].clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack 18");
                components[0].Play();
            }

            if (SP.plotID == 57 && !blueFade) //Exception for SP.plotID == 57 when the blue fade needs to appear with different background music.
            {
                blueFade = true;
                StartCoroutine(ColorScreenFade(new Color(0f, 0.41f, 1f, 0.95f), 0.15f));
                components[0].clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack 14");
                components[0].Play();
            }
        }
    }

    public IEnumerator ColorScreenFade(Color thisColor, float WaitTime) // Made to change alpha of a colored screen. 
    {
        renderCamera.GetComponent<ScreenFading>().cubeFading.GetComponent<MeshRenderer>().material.color = thisColor;
        renderCamera.GetComponent<ScreenFading>().cubeFading.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(WaitTime);
        while (renderCamera.GetComponent<ScreenFading>().cubeFading.GetComponent<MeshRenderer>().material.color.a > 0)
        {
            var fadeAmount = renderCamera.GetComponent<ScreenFading>().cubeFading.GetComponent<MeshRenderer>().material.color.a - (1f * Time.deltaTime);
            if (fadeAmount < 0f) fadeAmount = 0f;
            var color = renderCamera.GetComponent<ScreenFading>().cubeFading.GetComponent<MeshRenderer>().material.color;
            renderCamera.GetComponent<ScreenFading>().cubeFading.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, fadeAmount);
            yield return new WaitForEndOfFrame();
        }
        whiteFade = false;
        blueFade = false;
    }
}