using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSpriteRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spriteBox;
    public StoryProgress SP;
    public GameObject renderCamera;
    [SerializeField]
    private GameObject coloredScreen;
    public List<Sprite> spriteList = new();
    public int plotPlaceholder;
    public bool blueFade, whiteFade;
    void Start()
    {
        plotPlaceholder = SP.plotID -1;
    }

    void LateUpdate()
    {
        if (renderCamera.GetComponent<TextLoader>().plotIDplaceholder != -1)
        {
            bool silenced = false;
            
            if (SP.plotID == 57 || SP.plotID == 60 || SP.plotID == 62 || SP.plotID == 64) silenced = true;
            if (SP.dialogSprite[SP.plotID].Contains('*'))
            {
                silenced = true;
                SP.dialogSprite[SP.plotID] = SP.dialogSprite[SP.plotID].Replace("*","");
            }
            spriteBox.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprites/" + SP.dialogSprite[SP.plotID]);
            if (plotPlaceholder != SP.plotID)
            {
                var audioClip = SP.dialogSprite[SP.plotID].Split('-', System.StringSplitOptions.RemoveEmptyEntries);
                var components = renderCamera.GetComponents<AudioSource>();
                plotPlaceholder = SP.plotID;
                if(SP.plotID == 3 && !whiteFade)
                {
                    whiteFade = true;
                    StartCoroutine(ColorScreenFade(new Color(1f, 1f, 1f, 1f), 1.2f));
                    components[1].PlayOneShot(Resources.Load<AudioClip>("Audio/Zoom"),1.2f);
                    components[0].clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack 18");
                    components[0].Play();
                }
                if (SP.plotID == 65 && !blueFade)
                {
                    blueFade = true;
                    StartCoroutine(ColorScreenFade(new Color(0f, 0.41f, 1f, 0.95f), 0.2f));
                    components[0].clip = Resources.Load<AudioClip>("Audio/Tracks/Soundtrack 14");
                    components[0].Play();
                }
                if (!silenced && audioClip.Length > 1)
                {
                    components[1].clip = Resources.Load<AudioClip>("Audio/" + audioClip[0].Trim('-') + "/" + audioClip[1]);
                    components[1].Play();
                }
            }
        }
    }

    public IEnumerator ColorScreenFade(Color initialColor,float waitTime)
    {
        coloredScreen.GetComponent<ScreenFading>().SetColorScreen(initialColor.r, initialColor.g, initialColor.b);
        coloredScreen.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(waitTime);

        whiteFade = false; 
        blueFade = false; 
    }
}
