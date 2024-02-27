using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOverSettings : MonoBehaviour
{
    [SerializeField] private CrosshairManagement crosshairManagement;
    [SerializeField] private GameObject crosshair;

    private bool currentlyOn;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            crosshairManagement.enabled = false;
            crosshair.SetActive(false);
            Cursor.visible = true;
            currentlyOn = true;
        }
        if (!EventSystem.current.IsPointerOverGameObject()&& currentlyOn == true)
        {
            currentlyOn = false;
            crosshairManagement.enabled = true;
            crosshair.SetActive(true);
        }
    }
}
