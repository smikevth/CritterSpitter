using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    public int Speaker; //0 for Charles 1 for Larry
    [TextAreaAttribute]
    public string Text;
}
