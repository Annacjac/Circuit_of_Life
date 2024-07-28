using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lightbulb : MonoBehaviour
{
    private bool isOn;
    public Vector3Int lightPosition;
    public GridLayout grid;
    public Wiring wiring;
    private Vector3Int connectionCell;
    public Sprite[] lightSprites;
    public SpriteRenderer upperSprite;
    public SpriteRenderer lowerSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        lightPosition = grid.WorldToCell(transform.GetChild(1).position);
        connectionCell = new Vector3Int(lightPosition.x, lightPosition.y - 1, 0);
        Debug.Log(connectionCell);
    }

    // Update is called once per frame
    void Update()
    {
        DetectWire();
    }

    void TurnOn(){
        isOn = true;
        upperSprite.sprite = lightSprites[2];
        lowerSprite.sprite = lightSprites[3];

    }

    void TurnOff(){
        isOn = false;
        upperSprite.sprite = lightSprites[0];
        lowerSprite.sprite = lightSprites[1];
    }

    void DetectWire(){
        bool wireFound = false;
        if(wiring.SearchAllWires(connectionCell)){
            Debug.Log(true);
            for(int i = 2; i < 10; i++){
                if(wiring.tileMap.GetTile(connectionCell) == wiring.redWires[i] || 
                    wiring.tileMap.GetTile(connectionCell) == wiring.yellowWires[i] ||
                    wiring.tileMap.GetTile(connectionCell) == wiring.greenWires[i] ||
                    wiring.tileMap.GetTile(connectionCell) == wiring.blueWires[i]){
                    wireFound = true;
                    break;
                }
                if(wireFound)
                    break;
            }
        }

        if(wireFound){
            TurnOn();
        }
        else{
            TurnOff();
        }
    }

}
