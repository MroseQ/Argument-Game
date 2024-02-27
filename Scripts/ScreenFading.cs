using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFading : MonoBehaviour
{
    [SerializeField] private float maxAlpha;
    public bool fadeOut = true;

    void Update()
    {
        Color color = GetComponent<Image>().color;
        if (color.a > 0f && color.a <= maxAlpha || (SceneManager.GetActiveScene().name == "DialogBox" && !Camera.main.GetComponent<DialogSpriteRenderer>().whiteFade && !Camera.main.GetComponent<DialogSpriteRenderer>().blueFade))
        {
            if (fadeOut)
            {
                color = new Color(color.r, color.g, color.b, (color.a - Time.deltaTime) > 0f ? color.a - Time.deltaTime:0f);
                GetComponent<Image>().color = color;
            }
            else
            {
                color = new Color(color.r, color.g, color.b, (color.a + Time.deltaTime) < maxAlpha ? color.a + Time.deltaTime : maxAlpha);
                GetComponent<Image>().color = color;
            }
        }
    }

    public void SetColorScreen(float r, float g, float b)
    {
        GetComponent<Image>().color = new Color(r, g, b, maxAlpha);
        fadeOut = true;
    }

    public void RevertToBlack()
    {
        GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
    }

    public IEnumerator SwitchOffWhenTransparent()
    {
        yield return new WaitUntil(() => GetComponent<Image>().color.a == 0f);
        gameObject.SetActive(false);
    }
}
