using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WirePath : MonoBehaviour
{
    public List<Vector3Int> path;
    public string color;
    public bool soldered;
    public bool active;
    //public Wiring wiring;

    public WirePath()
    {
        // Ensure path is initialized here if needed
        path = new List<Vector3Int>();
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        path = new List<Vector3Int>();
        soldered = false;
        active = true;
        color = "red";
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void DrawPath(Vector3Int targetTile, ){

    // }
}
