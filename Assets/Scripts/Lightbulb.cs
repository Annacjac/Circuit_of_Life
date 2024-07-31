using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lightbulb : MonoBehaviour
{
    private bool isOn;
    public Vector3Int lightPosition;
    private Vector3Int connectionCell;
    public Sprite[] lightSprites;
    public SpriteRenderer upperSprite;
    public SpriteRenderer lowerSprite;
    public ObjectsAndData data;


    
    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.Find("Data").GetComponent<ObjectsAndData>();
        isOn = false;
        lightPosition = data.grid.WorldToCell(transform.GetChild(1).position);
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
        if(data.wiringMap1.HasTile(connectionCell)){
            //Debug.Log(true);
            for(int i = 2; i < 10; i++){
                if(data.wiringMap1.GetTile(connectionCell) == data.wiring.redWires[i] || 
                    data.wiringMap1.GetTile(connectionCell) == data.wiring.yellowWires[i] ||
                    data.wiringMap1.GetTile(connectionCell) == data.wiring.greenWires[i] ||
                    data.wiringMap1.GetTile(connectionCell) == data.wiring.blueWires[i]){
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

    public bool GetIsOn(){
        return isOn;
    }

}
