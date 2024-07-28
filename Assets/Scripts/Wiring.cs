using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wiring : MonoBehaviour
{
    private string color;
    private bool soldered;
    public GridLayout grid;
    public TileBase[] redWires;
    public TileBase[] yellowWires;
    public TileBase[] greenWires;
    public TileBase[] blueWires;
    private string[] wireColors;
    private int colorIndex;
    public Tilemap tileMap;
    public Tilemap secondLayer;
    public Tilemap circuitBoard;
    private Vector3Int previousTile;
    public PowerSource[] powerSources;
    private int currentWirePathIndex;
    private ArrayList allWirePaths;
    private bool mouseHeld;
    private int mode; //0=wiring, 1=soldering
    int mouseUpCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
        allWirePaths = new ArrayList();
        wireColors = new string[4] {"red", "yellow", "green", "blue"};
        colorIndex = 0;
        color = wireColors[colorIndex];
    }

    // Update is called once per frame
    void Update()
    {

        if(allWirePaths.Count == 0){
            allWirePaths.Add(new ArrayList());
            currentWirePathIndex = 0;
        }
        //Debug.Log(allWirePaths.Count);
        if(color == wireColors[0])
            UserWireInput(redWires);
        if(color == wireColors[1])
            UserWireInput(yellowWires);
        if(color == wireColors[2])
            UserWireInput(greenWires);
        if(color == wireColors[3])
            UserWireInput(blueWires);


        
    }

    public bool SearchAllWires(Vector3Int targetTile){
        bool match = false;
        for(int i = 0; i < allWirePaths.Count; i++){
            ArrayList tempList = (ArrayList)allWirePaths[i];
            for(int j = 0; j < tempList.Count; j++){
                if(targetTile == (Vector3Int)tempList[j]){
                    match = true;
                    break;
                }
            }
            if(match)
                break;
        }
        return match;
    }

    void FirstWire(ArrayList currentWirePath, Vector3Int firstTile, TileBase[] wireColor){
        
        grid = transform.parent.GetComponentInParent<GridLayout>();

        //check if it's adjacent to a power source
        if(circuitBoard.HasTile(firstTile) && !SearchAllWires(firstTile)){
            for(int i = 0; i < powerSources.Length; i++){
                Vector3Int power1 = grid.WorldToCell(powerSources[i].transform.GetChild(0).position);
                Vector3Int power2 = grid.WorldToCell(powerSources[i].transform.GetChild(1).position);

                //Power Source is horizontal
                if(powerSources[i].direction == 1){
                    //Left outlet
                    if(firstTile == new Vector3Int(power1.x - 1, power1.y, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[5]);
                        previousTile = firstTile;
                    }
                    //top outlets
                    else if(firstTile == new Vector3Int(power1.x, power1.y+1, 0) || firstTile == new Vector3Int(power2.x, power2.y+1, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[2]);
                        previousTile = firstTile;
                    }
                    //bottom outlets
                    else if(firstTile == new Vector3Int(power1.x, power1.y-1, 0) || firstTile == new Vector3Int(power2.x, power2.y-1, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[3]);
                        previousTile = firstTile;
                    }
                    //Right outlet
                    else if(firstTile == new Vector3Int(power2.x + 1, power2.y, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[4]);
                        previousTile = firstTile;
                    }
                }

                //Power source is vertical
                else{
                    //top outlet
                    if(firstTile == new Vector3Int(power1.x, power1.y+1, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[2]);
                        previousTile = firstTile;
                    }
                    //left outlets
                    else if(firstTile == new Vector3Int(power1.x-1, power1.y, 0) || firstTile == new Vector3Int(power2.x-1, power2.y, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[5]);
                        previousTile = firstTile;
                    }
                    //bottom outlet
                    else if(firstTile == new Vector3Int(power2.x, power2.y-1, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[3]);
                        previousTile = firstTile;
                    }
                    //right outlets
                    else if(firstTile == new Vector3Int(power1.x+1, power1.y, 0) || firstTile == new Vector3Int(power2.x+1, power2.y, 0)){
                        currentWirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[4]);
                        previousTile = firstTile;
                    }
                }
            }

        }
    }

    void DrawPath(ArrayList currentWirePath, Vector3Int targetTile, TileBase[] wireColor){

        if(circuitBoard.HasTile(targetTile)){
                //Previous is down facing end
                if(tileMap.GetTile(previousTile) == wireColor[3]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWires(targetTile)){
                            //wire turns right
                            //turn prev tile into bottom left corner, turn current tile to right facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[12]);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.Add(targetTile);
                            }

                            //wire turns left
                            //turn prev tile into bottom right corner, turn current tile to left facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[13]);
                                tileMap.SetTile(targetTile, wireColor[5]);
                            currentWirePath.Add(targetTile);
                            }

                            //wire continues down
                            //turn prev tile into vertical straight, turn current tile to down facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[0]);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.Add(targetTile);
                            }
                        }
                        else{
                            //wire reverts to previous vertical straight
                            //replace vertical stright with down facing end
                            if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && tileMap.GetTile(targetTile) == wireColor[0]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous upper left corner
                            //replace upper left corner with right facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && tileMap.GetTile(targetTile) == wireColor[10]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous upper right corner
                            //replace upper right corner with left facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && tileMap.GetTile(targetTile) == wireColor[11]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }

                    else{
                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0)){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.RemoveAt(currentWirePath.Count-1);
                        }
                    }
                }

                    //Prev tile is up facing end
                else if(tileMap.GetTile(previousTile) == wireColor[2]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWires(targetTile)){    
                            //wire turns right
                            //turn prev into upper left corner, turn current to right facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1,previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[10]);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.Add(targetTile);
                            }

                            //wire turns left
                            //turn prev into upper right corner, turn current to left facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[11]);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.Add(targetTile);
                            }

                            //wire continues up
                            //turn prev into vertical straight, turn current to up facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[0]);
                                tileMap.SetTile(targetTile, wireColor[2]);
                            currentWirePath.Add(targetTile);
                            }
                        }
                        
                        else{
                            //wire reverts to previous vertical straight
                            //replace vertical stright with up facing end
                            if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && tileMap.GetTile(targetTile) == wireColor[0]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous lower left corner
                            //replace lower left corner with right facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && tileMap.GetTile(targetTile) == wireColor[12]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous lower right corner
                            //replace lower right corner with left facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && tileMap.GetTile(targetTile) == wireColor[13]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }
                    else{
                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int( previousTile.x, previousTile.y - 1, 0)){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.RemoveAt(currentWirePath.Count-1);
                        }
                    }
                }

                    //Prev is right facing end
                else if(tileMap.GetTile(previousTile) == wireColor[4]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWires(targetTile)){    
                            //Wire continues right
                            //turn prev into horizontal straight, turn current to right facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[1]);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.Add(targetTile);
                            }

                            //wire turns up
                            //turn prev into bottom right corner, turn current to up facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[13]);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.Add(targetTile);

                            }

                            //wire turns down
                            //turn prev into upper right corner, turn current to down facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[11]);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.Add(targetTile);
                            }
                        }
                        else{
                            //wire reverts to previous horizontal straight
                            //replace horizontal stright with right facing end
                            if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[1]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous lower left corner
                            //replace lower left corner with down facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[12]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous upper left corner
                            //replace upper left corner with up facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[10]){
                                //Debug.Log(tileMap.GetTile(targetTile));
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }
                    else{

                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0)){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.RemoveAt(currentWirePath.Count-1);
                        }
                    }
                }

                //Prev is left facing end
                else if(tileMap.GetTile(previousTile) == wireColor[5]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWires(targetTile)) {   
                            //wire continues left
                            //turn prev into horizontal straight, turn current to left facing end
                            if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[1]);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.Add(targetTile);
                            }

                            //wire turns up
                            //turn prev into bottom left corner, turn current to up facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[12]);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.Add(targetTile);
                            }

                            //wire turns down
                            //turn prev into upper left corner, turn current to down facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[10]);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.Add(targetTile);
                            }
                        }
                        else{
                            //wire reverts to previous horizontal straight
                            //replace horizontal stright with left facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[1]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous lower right corner
                            //replace lower left corner with down facing end
                            else if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[13]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous upper right corner
                            //replace upper left corner with up facing end
                            else if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[11]){
                                //Debug.Log(tileMap.GetTile(targetTile));
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.RemoveAt(currentWirePath.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }
                    else{

                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == null){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.RemoveAt(currentWirePath.Count-1);
                        }
                    }
                }
            
            string currentWirePathString = "";
            for(int i = 0; i < currentWirePath.Count; i++){
                currentWirePathString = currentWirePathString + " " + currentWirePath[i];
            }
            //Debug.Log(currentWirePathString);
            //Debug.Log(targetTile);
            previousTile = targetTile;
        }
    }

    void RemoveWirePath(Vector3Int targetTile){
            bool wireFound = false;
            ArrayList tempArray = new ArrayList();
            for(int i = 0; i < allWirePaths.Count; i++){
                tempArray = (ArrayList)allWirePaths[i];
                for(int j = 0; j < tempArray.Count; j++){
                    if(targetTile == (Vector3Int)tempArray[j]){
                        allWirePaths.RemoveAt(i);
                        currentWirePathIndex = allWirePaths.Count-1;
                        for(int k = 0; k < tempArray.Count; k++)
                            tileMap.SetTile((Vector3Int)tempArray[k], null);
                        wireFound = true;
                        break;
                    }
                }
                if(wireFound)
                    break;
            }
            
            // if(currentWirePath.Contains(targetTile)){
            //     for(int i = currentWirePath.Count; i > 0; i--){
            //         tileMap.SetTile((Vector3Int)currentWirePath[i-1], null);
            //     }
            //     allWirePaths.Remove(currentWirePath);

            // }

            // currentWirePathIndex = allWirePaths.Count - 1;
        
    }

    void Solder(ArrayList currentWirePath, Vector3Int targetTile, TileBase[] wireColor){
        
        for(int i = 2; i < 6; i++){
            if(tileMap.GetTile(targetTile) == wireColor[i]){
                tileMap.SetTile(targetTile, wireColor[i+4]);
            }
        }

        soldered = true;
    }

    void Unsolder(ArrayList currentWirePath, Vector3Int targetTile, TileBase[] wireColor){
        ArrayList lastWire = (ArrayList)allWirePaths[allWirePaths.Count-1];
        //if(lastWire.Count == 0){    
            for(int i = 6; i < 10; i++){
                if(tileMap.GetTile(targetTile) == wireColor[i]){
                    tileMap.SetTile(targetTile, wireColor[i-4]);
                }
            }

            soldered = false;
        
    }


    // ArrayList SearchWirePaths(ArrayList targetWire, ArrayList wirePaths){
    //     ArrayList matchedWire = new ArrayList();
    //     for(int i = 0; i < wirePaths.Count; i++){
    //         if(targetWire == wirePaths[i]){
    //             matchedWire = (ArrayList)wirePaths[i];
    //         }
    //     }
    //     return matchedWire;
    // }

    // returns the index of allWirePaths that contains the target tile
    
    // public int SearchWirePathFromTile(Vector3Int targetTile){
    //     int matchIndex = -1;
    //     Vector3Int currentElement;
    //     for(int i = 0; i < allWirePaths.Count; i++){
    //         ArrayList currentList = (ArrayList)allWirePaths[i];
    //         for(int j = 0; j < currentList.Count; j++){
    //             currentElement = (Vector3Int)currentList[j];
    //             if(targetTile == currentElement){
    //                 matchIndex = i;
    //                 break;
    //             }
    //         }
    //         if (matchIndex != -1) break;
    //     }
    //     Debug.Log(targetTile);
    //     return matchIndex;
        
    // }

    public void ClearBoard(){
        tileMap.ClearAllTiles();
        allWirePaths.Clear();
        allWirePaths.Add(new ArrayList());
        currentWirePathIndex = 0;
    }


    void findTile(Vector3Int targetTile){
        bool match = false;
        
    }

    void NextColor(){
        if(colorIndex < 3){
            colorIndex++;
        }
        else{
            colorIndex = 0;
        }
        color = wireColors[colorIndex];
        Debug.Log(color);
    }

    void NewWirePath(){
        allWirePaths.Add(new ArrayList());
        NextColor();
        soldered = false;
        currentWirePathIndex = allWirePaths.Count-1;
        
    }


    void UserWireInput(TileBase[] wireColor){

        Vector3Int targetTile; 
        

        if(Input.GetMouseButtonDown(0)){
            mouseHeld = true;
        }
        if(Input.GetMouseButtonUp(0)){
            mouseHeld = false;
            if(mode == 1){
                Debug.Log(currentWirePathIndex);
                targetTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //mouseUpCounter++;
                ArrayList tempArray = (ArrayList)allWirePaths[currentWirePathIndex];

                if(targetTile == (Vector3Int)tempArray[tempArray.Count-1] && !soldered){
                    Solder((ArrayList)allWirePaths[currentWirePathIndex], targetTile, wireColor);
                    NewWirePath();
                    //Debug.Log(false);
                }
                // else if(targetTile == (Vector3Int)tempArray[tempArray.Count-1] && soldered){
                //     //Unsolder((ArrayList)allWirePaths[currentWirePathIndex], targetTile, wireColor);
                //     //RemoveWirePath((ArrayList)allWirePaths[currentWirePathIndex], targetTile);
                // }
            }
                
        }

        if(mouseHeld){
            targetTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(mode == 0){  
                ArrayList tempList = (ArrayList)allWirePaths[currentWirePathIndex];
                if(tempList.Count == 0){
                    FirstWire((ArrayList)allWirePaths[currentWirePathIndex], targetTile, wireColor);
                }
                else{
                    DrawPath((ArrayList)allWirePaths[currentWirePathIndex], targetTile, wireColor);
                }
            }
            
            else if(mode == 2){
                RemoveWirePath(targetTile);
            }
            
        }
        
    }

    

    public void ToWiringMode(){
        mode = 0;
        //Debug.Log(mode);
    }

    public void ToSolderingMode(){
        mode = 1;
        //Debug.Log(mode);
    }

    public void ToRemovalMode(){
        mode = 2;
        //Debug.Log(mode);
    }

    public int GetMode(){
        return mode;
    }

}
