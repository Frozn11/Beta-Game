using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPS file", menuName = "NPS Files Archive")]

public class NPS : ScriptableObject
{
    public string name;
    [TextArea(3, 12)]
    public string[] dialogue;
    [TextArea(3, 15)]
    public string[] playerDialogue;
}
