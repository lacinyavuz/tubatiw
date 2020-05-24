using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateHelper;
using System;

public class CreateMap : MonoBehaviour
{
    public Sprite Player1BoxSprite;
     public Sprite Player2BoxSprite;
    int xBorder,yBorder;
    public int xLenght,yLenght;
    public int StartingPositionX =0,StartingPositionY = 0;
    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        GameState.TileArray = new Tile[xLenght,yLenght];
        xBorder=StartingPositionX + xLenght-1;
        yBorder = StartingPositionY + yLenght-1;
        createBoxRecursively(StartingPositionX,StartingPositionY);
        InitializePlayers();
    }

    private void InitializePlayers()
    {
        GameState.TileArray[5,0].OwnedPlayer = 1;
        GameState.TileArray[5,10].OwnedPlayer =2;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void createBoxRecursively(int x,int y){
         GameState.AddTile(Instantiate(box, new Vector3(x, y, 0), Quaternion.identity),x,y);
         if(x  < xBorder){
            createBoxRecursively(++x,y);
         }else if(y < yBorder){
             x =StartingPositionX;
             createBoxRecursively(x,++y);
         }

    }
}
