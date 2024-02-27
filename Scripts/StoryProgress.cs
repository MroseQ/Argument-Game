using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActualStoryProgress",menuName = "Create new StoryProgress")]
public class StoryProgress : ScriptableObject
{
    public int storyProgress = 0;
    public int plotID = 0;
    public int argumentID = 0;
    public float[] wideGameObjects;
    public int lifes = 5;
    public List<string> dialogSprite = new List<string>();
    public float musicPrevValue = 1f;
    public float characterPrevValue = 1f;
    public float coreRotationZ = 20f;
}
