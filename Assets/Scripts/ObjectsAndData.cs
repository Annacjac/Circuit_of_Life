using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ObjectsAndData : MonoBehaviour
{
    public PowerSource[] powerSources;
    public Lightbulb[] lightbulbs;
    public Switch[] switches;
    public GridLayout grid;
    public Wiring wiring;
    public Tilemap wiringMap1;
    public Tilemap wiringMap2;
    public Tilemap circuitBoard;
    public Dictionary<Vector3Int, Object> componentDictionary;

    void Awake(){
        grid = GameObject.FindWithTag("Grid").GetComponent<GridLayout>();
    }
    
    void Start(){
        GameObject[] psTemp = GameObject.FindGameObjectsWithTag("PowerSource");
        ConvertPowerSources(psTemp);
        GameObject[] lbTemp = GameObject.FindGameObjectsWithTag("Lightbulb");
        ConvertLightbulbs(lbTemp);
        GameObject[] sTemp = GameObject.FindGameObjectsWithTag("Switch");
        ConvertSwitches(sTemp);
        wiring = GameObject.FindWithTag("Wiring").GetComponent<Wiring>();
        wiringMap1 = GameObject.Find("Wiring1").GetComponent<Tilemap>();
        wiringMap2 = GameObject.Find("Wiring2").GetComponent<Tilemap>();
        circuitBoard = GameObject.Find("Board").GetComponent<Tilemap>();
        CreateComponentDictionary();
    }

    void CreateComponentDictionary(){
        componentDictionary = new Dictionary<Vector3Int, Object>();

        for(int i = 0; i < lightbulbs.Length; i++){
            Vector3Int upper = grid.WorldToCell(lightbulbs[i].transform.GetChild(0).position);
            Vector3Int lower = grid.WorldToCell(lightbulbs[i].transform.GetChild(1).position);
            componentDictionary.Add(upper, lightbulbs[i]);
            componentDictionary.Add(lower, lightbulbs[i]);
        }
        
        for(int i = 0; i < powerSources.Length; i++){
            Vector3Int part1 = grid.WorldToCell(powerSources[i].transform.GetChild(0).position);
            Vector3Int part2 = grid.WorldToCell(powerSources[i].transform.GetChild(1).position);
            componentDictionary.Add(part1, powerSources[i]);
            componentDictionary.Add(part2, powerSources[i]);
        }
        
        for(int i = 0; i < switches.Length; i++){
            Vector3Int sw = grid.WorldToCell(switches[i].transform.position);
            componentDictionary.Add(sw, switches[i]);
        }
    }

    void ConvertPowerSources(GameObject[] ps){
        powerSources = new PowerSource[ps.Length];
        for(int i = 0; i < ps.Length; i++){
            powerSources[i] = ps[i].GetComponent<PowerSource>();
        }
    }

    void ConvertLightbulbs(GameObject[] lb){
        lightbulbs = new Lightbulb[lb.Length];
        for(int i = 0; i < lb.Length; i++){
            lightbulbs[i] = lb[i].GetComponent<Lightbulb>();
        }
    }

    void ConvertSwitches(GameObject[] s){
        switches = new Switch[s.Length];
        for(int i = 0; i < s.Length; i++){
            switches[i] = s[i].GetComponent<Switch>();
        }
    }
}
