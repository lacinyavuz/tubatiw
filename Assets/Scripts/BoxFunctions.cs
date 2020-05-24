using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.StateHelper;
using UnityEngine;

public class BoxFunctions : MonoBehaviour
{
  private static readonly int LEFTCLICK = 0;
  private static readonly int RIGHTCLICK = 1;
  private static readonly int NOONE = 0;
  private static readonly int PLAYER1 = 1;
  private static readonly int PLAYER2 = 2;
  public Sprite EmptyTileBox;
  public Sprite Player1Box;
  public Sprite AvailableMoveBox;
  public Sprite Player2Box;
  public Sprite BlockedBox;
  public Sprite Player1FortifiedBox;
  public Sprite Player2FortifiedBox;
  public Sprite Player1DamagedFortifiedBox;
  public Sprite Player2DamagedFortifiedBox;
  public Sprite Player1HeavyDamageFortifiedBox;
  public Sprite Player2HeavyDamageFortifiedBox;
  private Sprite[] spriteArray = new Sprite[3];
  private Sprite[,] FortifySpritesArray = new Sprite[3, 4];

  private int state = 0;
  // Start is called before the first frame update
  void Start()
  {
    spriteArray[0] = EmptyTileBox;
    spriteArray[1] = Player1Box;
    spriteArray[2] = Player2Box;

    FortifySpritesArray[1, 1] = Player1HeavyDamageFortifiedBox;
    FortifySpritesArray[1, 2] = Player1DamagedFortifiedBox;
    FortifySpritesArray[1, 3] = Player1FortifiedBox;

    FortifySpritesArray[2, 1] = Player2HeavyDamageFortifiedBox;
    FortifySpritesArray[2, 2] = Player2DamagedFortifiedBox;
    FortifySpritesArray[2, 3] = Player2FortifiedBox;
  }

  // Update is called once per frame
  void Update()
  {
    var currentBoxCoordinates = GetCurrentPoint();
    bool success = false;
    var boxSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    if (Input.GetMouseButtonDown(LEFTCLICK))
    {
      Vector3 vp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      var point = PointConverter.VectorConverter(vp);
      BoxCollider2D col = this.gameObject.GetComponent<BoxCollider2D>();
      if (col.OverlapPoint(vp))
      {

        if (GameState.BoxAvailableForAttack(currentBoxCoordinates))
        {
          success = GameState.AttackTile(point, spriteArray[GameState.currentPlayer]);
        }
        else if (boxSpriteRenderer.sprite == spriteArray[GameState.currentPlayer])
        {
          Debug.Log("asdads" + (boxSpriteRenderer.sprite == spriteArray[GameState.currentPlayer]).ToString());
          success = GameState.FortifyTile(point);
        }
      }
    }
    else if (Input.GetMouseButtonDown(RIGHTCLICK))
    {
      Vector3 vp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      var point = PointConverter.VectorConverter(vp);
      BoxCollider2D col = this.gameObject.GetComponent<BoxCollider2D>();
      if (col.OverlapPoint(vp))
      {
        if (boxSpriteRenderer.sprite == AvailableMoveBox)
        {
          success = GameState.BlockTile(point);
          if (success)
            boxSpriteRenderer.sprite = BlockedBox;
        }
      }
    }
    if (success)
    {
      GameState.switchTurn(EmptyTileBox);
    }
    UpdateBoxState(currentBoxCoordinates, boxSpriteRenderer);
  }

  private void UpdateBoxState((int x, int y) point, SpriteRenderer boxSpriteRenderer)
  {
    var tileProperties = GameState.TileArray[point.x, point.y];
    if (tileProperties.OwnedPlayer == NOONE)
    {
      bool isNeighbour = GameState.CheckNeighbourhood(point);
      if (GameState.CheckBlocked(point))
      {
        boxSpriteRenderer.sprite = BlockedBox;
      }
      else if (GameState.CheckNeighbourhood(point))
      {
        boxSpriteRenderer.sprite = AvailableMoveBox;
      }
      else
      {
        boxSpriteRenderer.sprite = EmptyTileBox;
      }
    }
    else if (tileProperties.Status > 1)
    {
      boxSpriteRenderer.sprite = FortifySpritesArray[tileProperties.OwnedPlayer, tileProperties.Status]; ;
    }
    else
    {
      boxSpriteRenderer.sprite = spriteArray[tileProperties.OwnedPlayer]; ;
    }
  }
  private (int, int) GetCurrentPoint()
  {
    var pos = this.gameObject.GetComponent<Transform>().position;
    return PointConverter.VectorConverter(pos);
  }
}

