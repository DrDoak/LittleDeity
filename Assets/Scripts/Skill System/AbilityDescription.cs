using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class AbilityDescription : ScriptableObject {

    public List<string> AbilityNames;
    public List<string> AbilityDescriptions;
    public List<Image> AbilityImages;
}
