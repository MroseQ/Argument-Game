using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLighting : MonoBehaviour
{
    public int ID;
    [SerializeField]private StoryProgress SP;
    private GameObject childNode;

    private void Start()
    {
        childNode = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (ID < SP.argumentID)
        {
            ChangeMaterial("Materials/PreviousProgress");
        }else if(ID == SP.argumentID)
        {
            ChangeMaterial("Materials/NowProgress");

        }else
        {
            ChangeMaterial("Materials/NextProgress");
        }
    }

    void ChangeMaterial(string path)
    {
        Material m = Resources.Load<Material>(path);
        gameObject.GetComponent<MeshRenderer>().material = m;
        childNode.GetComponent<MeshRenderer>().material = m;
    }
}
