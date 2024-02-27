using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;

public class ColoringText : MonoBehaviour
{
    [SerializeField] private string nameToLoad;
    public string text;
    public string[] convertedText;
    private string colouredText;
    private GameObject hitboxObject=null;
    [SerializeField] private StoryProgress SP;
    void LateUpdate()
    {
        StopAllCoroutines();
        if(SP.storyProgress == 19)
        {
            if(SP.argumentID == 57 && Camera.main.GetComponent<TextLoader>().keyWinnings.Count != 0)
            {
                TrimLocks("*");
            }
            if (!Camera.main.GetComponent<TextLoader>().keyWinnings.Contains(SP.argumentID) && SP.argumentID != 57)
            {
                TrimLocks(";");
            }
        }
        StartCoroutine(EndFrame());
    }

    void TrimLocks(string separator)
    {
        text = GetComponent<TextMeshPro>().text;
        text = text.Replace(separator, "");
        GetComponent<TextMeshPro>().text = text;
    }

    public IEnumerator EndFrame()
    {
        text = GetComponent<TextMeshPro>().text;
        convertedText = text.Split(';');
        if(convertedText.Length == 1)
        {
            convertedText = text.Split('*');
        }
        text = convertedText[0];
        if (convertedText.Length > 1)
        {
            colouredText = convertedText[1];
            convertedText[1] = "<color=#ebd914>" + convertedText[1] + "</color>";
            text += convertedText[1];
            if (hitboxObject == null) 
            { 
                hitboxObject = Instantiate(Resources.Load<GameObject>("Prefabs/" + nameToLoad + "Hitbox")); 
            }
            ChangeHitboxSize(colouredText, hitboxObject, convertedText[0].Length);
        }
        if (convertedText.Length > 2)
        {
            text += convertedText[2];
        }
        GetComponent<TextMeshPro>().text = text;
        yield return new WaitForEndOfFrame();
    }

    private void ChangeHitboxSize(string colouredText,GameObject hitbox, int firstPartLength = 0)
    {
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        int charIndex = firstPartLength;

        Vector3 maxBottomLeft = Vector3.zero, maxTopLeft = Vector3.zero, maxBottomRight = Vector3.zero, maxTopRight = Vector3.zero;
        foreach(char c in colouredText)
        {
            TMP_CharacterInfo charInfo = textMeshPro.textInfo.characterInfo[charIndex];
            if (charIndex == firstPartLength)
            {
                maxBottomLeft = textMeshPro.transform.TransformPoint(charInfo.bottomLeft);
                maxTopLeft = textMeshPro.transform.TransformPoint(charInfo.topLeft);
                maxBottomRight = textMeshPro.transform.TransformPoint(charInfo.bottomRight);
                maxTopRight = textMeshPro.transform.TransformPoint(charInfo.topRight);
            }
            else
            {
                Vector3 nowBottomLeft = textMeshPro.transform.TransformPoint(charInfo.bottomLeft);
                Vector3 nowBottomRight = textMeshPro.transform.TransformPoint(charInfo.bottomRight);
                Vector3 nowTopLeft = textMeshPro.transform.TransformPoint(charInfo.topLeft);
                Vector3 nowTopRight = textMeshPro.transform.TransformPoint(charInfo.topRight);

                if(nowBottomLeft.x < maxBottomLeft.x)
                {
                    maxBottomLeft.x = nowBottomLeft.x;
                }
                if(nowBottomLeft.y < maxBottomLeft.y)
                {
                    maxBottomLeft.y = nowBottomLeft.y;
                }

                if(nowBottomRight.x > maxBottomRight.x)
                {
                    maxBottomRight.x = nowBottomRight.x;
                }
                if(nowBottomRight.y < maxBottomRight.y)
                {
                    maxBottomRight.y = nowBottomRight.y;
                }

                if(nowTopLeft.x < maxTopLeft.x)
                {
                    maxTopLeft.x = nowTopLeft.x;
                }
                if(nowTopLeft.y > maxTopLeft.y)
                {
                    maxTopLeft.y = nowTopLeft.y;
                }

                if(nowTopRight.x > maxTopRight.x)
                {
                    maxTopRight.x = nowTopRight.x;
                }
                if(nowTopRight.y > maxTopRight.y)
                {
                    maxTopRight.y = nowTopRight.y;
                }
            }
            
            charIndex++;
        }

        Vector3 centerCharPoint = (maxBottomLeft + maxBottomRight + maxTopLeft + maxTopRight) / 4;
        
        //Moving the hitbox to show it before any other obscuring obstacles like SpeedUpBox, letting the player to click the hitbox while speeding up.
        centerCharPoint.z = -9; 

        float charWidthX = maxTopRight.x - maxTopLeft.x;
        float charWidthY = maxTopRight.y - maxBottomRight.y;
        
        hitbox.transform.localPosition = Vector3.Lerp(hitbox.transform.localPosition,centerCharPoint,1);
        hitbox.transform.localScale = new(charWidthX, charWidthY, hitbox.transform.localScale.z);
    }
}
