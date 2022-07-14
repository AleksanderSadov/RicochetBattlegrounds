using System.Collections.Generic;
using Unity.Ricochet.Game;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    public List<Actor> actors;
    public GameObject player;

    private void Awake()
    {
        actors = new List<Actor>();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
