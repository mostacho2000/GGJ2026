using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/PlatformStats")]
public class PlatformStats : ScriptableObject
{
    public List<float> platformSpeeds;
    public List<float> timesBeforeNextMove;
}
