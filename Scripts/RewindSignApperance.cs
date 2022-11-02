using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindSignApperance : MonoBehaviour
{
    public List<GameObject> triangles = new List<GameObject>();
    public GameObject renderCamera;
    private bool previousEnd = true;

    void Update()
    {
        if (renderCamera.GetComponent<CoreArgumentLoader>().rewind && previousEnd)
        {
            StartCoroutine(SwitchOffOn());
        }
        if (!renderCamera.GetComponent<CoreArgumentLoader>().rewind) //Switch offs the triangles when the rewind finishes.
        {
            foreach (GameObject triangle in triangles)
            {
                var coloredNow = triangles[0].GetComponent<MeshRenderer>().material.color;
                triangle.GetComponent<MeshRenderer>().material.color = new Color(coloredNow.r, coloredNow.g, coloredNow.b, 0f);
            }
        }
    }

    public IEnumerator SwitchOffOn() //Switches trianagles visibility between on and off.
    {
        previousEnd = false;
        var coloredNow = triangles[0].GetComponent<MeshRenderer>().material.color;
        foreach (GameObject triangle in triangles)
        {
            triangle.GetComponent<MeshRenderer>().material.color = new Color(coloredNow.r, coloredNow.g, coloredNow.b, 0.6f);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject triangle in triangles){
            triangle.GetComponent<MeshRenderer>().material.color = new Color(coloredNow.r, coloredNow.g, coloredNow.b, 0f);
        }
        yield return new WaitForSeconds(0.5f);
        previousEnd = true;
    }
}
