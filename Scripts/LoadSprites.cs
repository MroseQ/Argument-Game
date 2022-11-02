using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSprites : MonoBehaviour
{
    public List<Sprite> list;
    void Update() //Loads all sprites at start of the scene.
    {
        for (int i = 0;i< list.Count; i++)
        {
            GetComponent<SpriteRenderer>().sprite = list[i];
        }
    }
}
