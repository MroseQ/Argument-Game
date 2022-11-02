using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFading : MonoBehaviour
{
    public bool fadeOut;
    public GameObject cubeFading;
    private Color newColor;
    private float fadeAmount;
    public bool ifRed;
    void Start()
    {
        newColor = cubeFading.GetComponent<MeshRenderer>().material.color;
    }

    void Update()
    {
        var condition = ConditionCheck();
        if (condition)
        {
            // Check which fade should accur and apply changes every frame.

            if (fadeOut)
            {
                fadeAmount = newColor.a - (1f * Time.deltaTime);
                if (fadeAmount < 0f) fadeAmount = 0f;
            }
            else
            {
                fadeAmount = newColor.a + (1f * Time.deltaTime);
                if (fadeAmount > 1f) fadeAmount = 1f;
            }
            newColor = new Color(newColor.r, newColor.g, newColor.b, fadeAmount);
            cubeFading.GetComponent<MeshRenderer>().material.color = newColor;
        }
    }

    private bool ConditionCheck()
    {
        // Check for special events, where the screen should fade with a different color.

        bool condition;
        if (SceneManager.GetActiveScene().name == "DialogBox")
        {
            condition = !GetComponent<DialogSpriteRenderer>().whiteFade && !GetComponent<DialogSpriteRenderer>().blueFade;
        }
        else
        {
            condition = !ifRed;
        }
        return condition;
    }
}
