using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;

public class CrosshairManagement : MonoBehaviour
{
    [SerializeField] public GameObject crosshairObject;
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out MousePos point);

    [SerializeField]private int randomSavedX,randomSavedY;

    private bool delaying; 

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePos
    {
        public int x;
        public int y;
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        randomSavedX = 0;
        randomSavedY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        if (Application.isFocused && !delaying)
        {
            StartCoroutine(DelayChange(0.02f));
        }
        /*Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshairObject.transform.position = new(pos.x,pos.y,crosshairObject.transform.position.z);*/
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshairObject.transform.position = Vector3.MoveTowards(crosshairObject.transform.position, new(pos.x, pos.y, crosshairObject.transform.position.z), 1f);
    }

    private IEnumerator DelayChange(float time)
    {
        delaying = true;
        yield return new WaitForSeconds(time);
        GetCursorPos(out MousePos currentMousePos);
        randomSavedY = Random.Range(randomSavedY > -5 ? randomSavedY - 3 : randomSavedY + 5, randomSavedY < 5 ? randomSavedY + 4 : randomSavedY - 6);
        randomSavedX = Random.Range(randomSavedX > -5 ? randomSavedX - 3 : randomSavedX + 5, randomSavedX < 5 ? randomSavedX + 4 : randomSavedX - 6);
        SetCursorPos(currentMousePos.x + randomSavedX, currentMousePos.y + randomSavedY);
        delaying = false;
    }
}
