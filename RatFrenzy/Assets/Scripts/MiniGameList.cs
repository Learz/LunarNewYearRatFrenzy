using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateMiniGameList", order = 1)]
public class MiniGameList : ScriptableObject
{
    public List<MiniGame> gamesList;
}
[System.Serializable]
public class MiniGame
{
    public string gameName;
    public string description;
    public Sprite thumbnail;
    public string sceneName;
    public List<Binding> bindings;
}
[System.Serializable]
public class Binding
{
    public enum Action
    {
        Jump,
        Interact,
        Move
    }
    public Action action;
    public string description;
}