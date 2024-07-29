using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Wiring : MonoBehaviour
{

    public GridLayout grid;
    public Tilemap tileMap;
    public Tilemap secondLayer;
    public Tilemap circuitBoard;
    public List<WirePath> wirePaths;
    public TileBase[] redWires;
    public TileBase[] yellowWires;
    public TileBase[] greenWires;
    public TileBase[] blueWires;
    private string[] wireColors;
    private int colorIndex;
    private Vector3Int previousTile;
    public PowerSource[] powerSources;
    private int currentWirePathIndex;
    private int mode;
    private bool mouseHeld;

    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
        wirePaths = new List<WirePath>();
        wireColors = new string[4] {"red", "yellow", "green", "blue"};
        colorIndex = -1;
        AddNewWirePath();
        //color = wireColors[colorIndex];

        //============== Testing ========================

    }

    // Update is called once per frame
    void Update()
    {
        if(wirePaths.Count == 0){
            AddNewWirePath();
        }

        Vector3Int targetTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(colorIndex == 0){
            UserWireInput(targetTile, redWires);
        }
        else if(colorIndex == 1){
            UserWireInput(targetTile, yellowWires);
        }
        else if(colorIndex == 2){
            UserWireInput(targetTile, greenWires);
        }
        else if(colorIndex == 3){
            UserWireInput(targetTile, blueWires);
        }
    }

    void AddNewWirePath(){
        GameObject wire = new GameObject("WirePath");
        WirePath wp = wire.AddComponent<WirePath>();
        wirePaths.Add(wp);
        currentWirePathIndex = wirePaths.Count - 1;
        wirePaths[wirePaths.Count-1].color = wireColors[NextColor()];
    }

    public int SearchAllWiresReturnIndex(Vector3Int targetTile){
        bool match = false;
        int matchedPath = -1;
        for(int i = 0; i < wirePaths.Count; i++){
            WirePath tempPath = wirePaths[i];
            for(int j = 0; j < tempPath.path.Count; j++){
                if(targetTile == tempPath.path[j]){
                    match = true;
                    matchedPath = i;
                    break;
                }
            }
            if(match)
                break;
        }
        return matchedPath;
    }

    public bool SearchAllWiresReturnTruth(Vector3Int targetTile){
        bool match = false;
        for(int i = 0; i < wirePaths.Count; i++){
            WirePath tempPath = wirePaths[i];
            for(int j = 0; j < tempPath.path.Count; j++){
                if(targetTile == tempPath.path[j]){
                    match = true;
                    break;
                }
            }
            if(match)
                break;
        }
        return match;
    }

    void FirstWire(WirePath currentWirePath, Vector3Int firstTile, TileBase[] wireColor){
        if(circuitBoard.HasTile(firstTile) && !SearchAllWiresReturnTruth(firstTile)){
            for(int i = 0; i < powerSources.Length; i++){
                Vector3Int power1 = grid.WorldToCell(powerSources[i].transform.GetChild(0).position);
                Vector3Int power2 = grid.WorldToCell(powerSources[i].transform.GetChild(1).position);

                //Power Source is horizontal
                if(powerSources[i].direction == 1){
                    
                    //Left outlet
                    if(firstTile == new Vector3Int(power1.x - 1, power1.y, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[5]);
                        previousTile = firstTile;
                    }
                    //top outlets
                    else if(firstTile == new Vector3Int(power1.x, power1.y+1, 0) || firstTile == new Vector3Int(power2.x, power2.y+1, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[2]);
                        previousTile = firstTile;
                    }
                    //bottom outlets
                    else if(firstTile == new Vector3Int(power1.x, power1.y-1, 0) || firstTile == new Vector3Int(power2.x, power2.y-1, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[3]);
                        previousTile = firstTile;
                    }
                    //Right outlet
                    if(firstTile == new Vector3Int(power2.x + 1, power2.y, 0)){
                        currentWirePath.path.Add(firstTile);
                        //Debug.Log(currentWirePath.path.ToString());
                        tileMap.SetTile(firstTile, wireColor[4]);
                        previousTile = firstTile;
                    }
                }

                //Power source is vertical
                else{
                    //top outlet
                    if(firstTile == new Vector3Int(power1.x, power1.y+1, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[2]);
                        previousTile = firstTile;
                    }
                    //left outlets
                    else if(firstTile == new Vector3Int(power1.x-1, power1.y, 0) || firstTile == new Vector3Int(power2.x-1, power2.y, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[5]);
                        previousTile = firstTile;
                    }
                    //bottom outlet
                    else if(firstTile == new Vector3Int(power2.x, power2.y-1, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[3]);
                        previousTile = firstTile;
                    }
                    //right outlets
                    else if(firstTile == new Vector3Int(power1.x+1, power1.y, 0) || firstTile == new Vector3Int(power2.x+1, power2.y, 0)){
                        currentWirePath.path.Add(firstTile);
                        tileMap.SetTile(firstTile, wireColor[4]);
                        previousTile = firstTile;
                    }
                }
            }
        }
    }

     void DrawPath(WirePath currentWirePath, Vector3Int targetTile, TileBase[] wireColor){

        //if(circuitBoard.HasTile(targetTile)){
                //Previous is down facing end
                if(tileMap.GetTile(previousTile) == wireColor[3]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWiresReturnTruth(targetTile)){
                            //wire turns right
                            //turn prev tile into bottom left corner, turn current tile to right facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[12]);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire turns left
                            //turn prev tile into bottom right corner, turn current tile to left facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[13]);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire continues down
                            //turn prev tile into vertical straight, turn current tile to down facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[0]);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.path.Add(targetTile);
                            }
                        }
                        else{
                            
                            //wire reverts to previous vertical straight
                            //replace vertical stright with down facing end
                            if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && tileMap.GetTile(targetTile) == wireColor[0]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //sDebug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous upper left corner
                            //replace upper left corner with right facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && tileMap.GetTile(targetTile) == wireColor[10]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous upper right corner
                            //replace upper right corner with left facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && tileMap.GetTile(targetTile) == wireColor[11]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }

                    else{
                        //Debug.Log("Removing");
                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0)){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                        }
                    }
                }

                    //Prev tile is up facing end
                else if(tileMap.GetTile(previousTile) == wireColor[2]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWiresReturnTruth(targetTile)){    
                            //wire turns right
                            //turn prev into upper left corner, turn current to right facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1,previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[10]);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire turns left
                            //turn prev into upper right corner, turn current to left facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[11]);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire continues up
                            //turn prev into vertical straight, turn current to up facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[0]);
                                tileMap.SetTile(targetTile, wireColor[2]);
                            currentWirePath.path.Add(targetTile);
                            }
                        }
                        
                        else{
                            
                            //wire reverts to previous vertical straight
                            //replace vertical stright with up facing end
                            if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && tileMap.GetTile(targetTile) == wireColor[0]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous lower left corner
                            //replace lower left corner with right facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && tileMap.GetTile(targetTile) == wireColor[12]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous lower right corner
                            //replace lower right corner with left facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && tileMap.GetTile(targetTile) == wireColor[13]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }
                    else{
                        //Debug.Log("Removing");
                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int( previousTile.x, previousTile.y - 1, 0)){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                        }
                    }
                }

                    //Prev is right facing end
                else if(tileMap.GetTile(previousTile) == wireColor[4]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWiresReturnTruth(targetTile)){    
                            //Wire continues right
                            //turn prev into horizontal straight, turn current to right facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[1]);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire turns up
                            //turn prev into bottom right corner, turn current to up facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[13]);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.path.Add(targetTile);

                            }

                            //wire turns down
                            //turn prev into upper right corner, turn current to down facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[11]);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.path.Add(targetTile);
                            }
                        }
                        else{
                            
                            //wire reverts to previous horizontal straight
                            //replace horizontal stright with right facing end
                            if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[1]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[4]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous lower left corner
                            //replace lower left corner with down facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[12]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous upper left corner
                            //replace upper left corner with up facing end
                            else if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[10]){
                                //Debug.Log(tileMap.GetTile(targetTile));
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }
                    else{
                        //Debug.Log("Removing");
                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0)){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                        }
                    }
                }

                //Prev is left facing end
                else if(tileMap.GetTile(previousTile) == wireColor[5]){
                    if(circuitBoard.HasTile(targetTile)){
                        if(!SearchAllWiresReturnTruth(targetTile)) {   
                            //wire continues left
                            //turn prev into horizontal straight, turn current to left facing end
                            if(targetTile == new Vector3Int(previousTile.x - 1, previousTile.y, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[1]);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire turns up
                            //turn prev into bottom left corner, turn current to up facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y + 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[12]);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.path.Add(targetTile);
                            }

                            //wire turns down
                            //turn prev into upper left corner, turn current to down facing end
                            else if(targetTile == new Vector3Int(previousTile.x, previousTile.y - 1, 0) && !tileMap.HasTile(targetTile)){
                                tileMap.SetTile(previousTile, wireColor[10]);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.path.Add(targetTile);
                            }
                        }
                        else{
                            
                            //wire reverts to previous horizontal straight
                            //replace horizontal stright with left facing end
                            if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[1]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[5]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to vertical straight");
                            }

                            //wire reverts to previous lower right corner
                            //replace lower left corner with down facing end
                            else if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[13]){
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[3]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                //Debug.Log("downward end revert to upper left corner");
                            }

                            //wire reverts to previous upper right corner
                            //replace upper left corner with up facing end
                            else if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == wireColor[11]){
                                //Debug.Log(tileMap.GetTile(targetTile));
                                tileMap.SetTile(previousTile, null);
                                tileMap.SetTile(targetTile, wireColor[2]);
                                currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                                // Debug.Log("downward end revert to upper right corner");
                            }
                        }
                    }
                    else{
                        //Debug.Log("Removing");
                        //wire removed from power cell
                        //set prev tile to null
                        if(targetTile == new Vector3Int(previousTile.x + 1, previousTile.y, 0) && tileMap.GetTile(targetTile) == null){
                            tileMap.SetTile(previousTile, null);
                            currentWirePath.path.RemoveAt(currentWirePath.path.Count-1);
                        }
                    }
                }
            
            previousTile = targetTile;
        //}
    }

    void Solder(Vector3Int targetTile, TileBase[] wireColor){
        if(wirePaths[currentWirePathIndex].path.Count != 0){    
            wirePaths[currentWirePathIndex].soldered = true;
            for(int i = 2; i < 6; i++){
                if(tileMap.GetTile(targetTile) == wireColor[i]){
                    tileMap.SetTile(targetTile, wireColor[i+4]);
                    if(wirePaths[wirePaths.Count-1].path.Count != 0)
                        AddNewWirePath();
                    break;
                    
                }
            }
        }
    }

    void Unsolder(Vector3Int targetTile, TileBase[] wireColor){
        
        bool allSoldered = true;

        if(wirePaths[wirePaths.Count-1].path.Count == 0){
            RemoveWireFromPath(wirePaths[wirePaths.Count-1]);
            Debug.Log(true);
        }

        for(int i = 0; i < wirePaths.Count; i++){
            if(!wirePaths[i].soldered){
                allSoldered = false;
                break;
            }
        }

        //Debug.Log(allSoldered);

        if(allSoldered){
            currentWirePathIndex = SearchAllWiresReturnIndex(targetTile);
            wirePaths[currentWirePathIndex].soldered = false;
            for(int i = 6; i < 10; i++){
                if(tileMap.GetTile(targetTile) == wireColor[i]){
                    tileMap.SetTile(targetTile, wireColor[i-4]);
                    
                }
            }
            
        }
        else{
            SendMessage("Cannot unsolder unless all other wires are soldered.");
        }
    }

    void RemoveWireFromTile(Vector3Int targetTile){
        WirePath removedPath = wirePaths[SearchAllWiresReturnIndex(targetTile)];
        for(int i = 0; i < removedPath.path.Count; i++){
            tileMap.SetTile(removedPath.path[i], null);
        }
        wirePaths.Remove(removedPath);
    }

    void RemoveWireFromPath(WirePath wirePath){
        for(int i = 0; i < wirePath.path.Count; i++){
            tileMap.SetTile(wirePath.path[i], null);
        }
        wirePaths.Remove(wirePath);
        Destroy(wirePath.gameObject);

        for(int i = 0; i < wirePaths.Count; i++){
            if(!wirePaths[i].soldered){
                currentWirePathIndex = i;
                break;
            }
        }
    }

    void SendMessage(string message){
        Debug.Log(message);
    }

    int NextColor(){
        if(colorIndex < 4){
            colorIndex++;
        }
        else{
            colorIndex = 0;
        }
        Debug.Log(wireColors[colorIndex]);
        return colorIndex;
    }

    void SetColorIndex(string color){
        if(color == "red"){
            colorIndex = 0;
        }
        else if(color == "yellow"){
            colorIndex = 1;
        }
        else if(color == "green"){
            colorIndex = 2;
        }
        else if(color == "blue"){
            colorIndex = 3;
        }
    }


    void UserWireInput(Vector3Int targetTile, TileBase[] wireColor){
        
        WirePath tempPath;

        if(Input.GetMouseButtonDown(0)){
            if(SearchAllWiresReturnTruth(targetTile)){
                SetColorIndex(wirePaths[SearchAllWiresReturnIndex(targetTile)].color);
            }
            mouseHeld = true;
        }
        if(Input.GetMouseButtonUp(0)){
            mouseHeld = false;
            if(mode == 1 && tileMap.HasTile(targetTile)){
                tempPath = wirePaths[SearchAllWiresReturnIndex(targetTile)];
                if(targetTile == tempPath.path[tempPath.path.Count-1]){
                    //Debug.Log(tempPath.path.ToString());
                    if(!tempPath.soldered){
                        Solder(targetTile, wireColor);
                    }
                    else {
                        //Debug.Log("Unsoldering");
                        Unsolder(targetTile, wireColor);
                    }
                }
            }
        }

        if(mouseHeld){
            
            if(mode == 0){  
                //Debug.Log(circuitBoard.HasTile(targetTile));
                //if(!wirePaths[currentWirePathIndex].path.Contains(targetTile)){
                    if(wirePaths[wirePaths.Count-1].path.Count == 0){
                        FirstWire(wirePaths[currentWirePathIndex], targetTile, wireColor);
                    }
                    else{
                        DrawPath(wirePaths[currentWirePathIndex], targetTile, wireColor);
                    }
                //}
                
            }
            
            else if(mode == 2 && tileMap.HasTile(targetTile)){
                RemoveWireFromPath(wirePaths[SearchAllWiresReturnIndex(targetTile)]);
            }
            
        }

    }

    public void SetWiringMode(){
        mode = 0;
    }

    public void SetSolderingMode(){
        mode = 1;
    }

    public void SetRemovalMode(){
        mode = 2;
    }
}
