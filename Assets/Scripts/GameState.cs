using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.StateHelper
{

  public class GameState
  {
    public static Tile[,] TileArray;
    public static int turnNumber { get; private set; }

    public static int currentPlayer { get { return (turnNumber % 2) + 1; } }
    public static void switchTurn(Sprite bluebox)
    {
      turnNumber++;
    }
    public static void AddTile(GameObject tileObj, int x, int y)
    {
      TileArray[x, y] = new Tile
      {
        OwnedPlayer = 0,
        Status = 0,
      };
    }

    public static bool CheckNeighbourhood((int x, int y) point, bool allPoints = false)
    {
      if (TileArray[point.x, point.y].OwnedPlayer == currentPlayer || (TileArray[point.x, point.y].OwnedPlayer != 0 && !allPoints))
        return false;
      bool result = false;
      int horMax = TileArray.GetLength(0);
      int verMax = TileArray.GetLength(1);
      //east
      if (point.x + 1 < horMax)
      {
        result |= TileArray[point.x + 1, point.y].OwnedPlayer == currentPlayer;
      }
      //west
      if (point.x - 1 >= 0)
      {
        result |= TileArray[point.x - 1, point.y].OwnedPlayer == currentPlayer;
      }
      //north
      if (point.y + 1 < verMax)
      {
        result |= TileArray[point.x, point.y + 1].OwnedPlayer == currentPlayer;
      }
      //south
      if (point.y - 1 >= 0)
      {
        result |= TileArray[point.x, point.y - 1].OwnedPlayer == currentPlayer;
      }
      return result;
    }

    internal static int GetOwnedPlayer((int x, int y) point)
    {
      return TileArray[point.x, point.y].OwnedPlayer;

    }

    public static bool AttackTile((int x, int y) point, Sprite currentPlayerSprite)
    {
      if (TileArray[point.x, point.y].OwnedPlayer != currentPlayer)
      {
        TileArray[point.x, point.y].Status--;
        if (TileArray[point.x, point.y].Status < 1)
        {
          TileArray[point.x, point.y].Status = 0;
          TileArray[point.x, point.y].OwnedPlayer = currentPlayer;
        }
        return true;
      }
      return false;
    }

    internal static bool BoxAvailableForAttack((int x, int y) point)
    {
      return CheckNeighbourhood(point, true) &&
      TileArray[point.x, point.y].Status != -1;

    }

    public static bool FortifyTile((int x, int y) point)
    {
      if (TileArray[point.x, point.y].Status == 0)
      {
        TileArray[point.x, point.y].Status = 3;
        return true;
      }
      return false;
    }

    internal static bool BlockTile((int x, int y) point)
    {
      if (TileArray[point.x, point.y].OwnedPlayer == 0)
      {
        TileArray[point.x, point.y].Status = -1;
        return true;
      }
      return false;
    }

    internal static bool CheckBlocked((int x, int y) point)
    {
      return TileArray[point.x, point.y].Status == -1;
    }
  }
  public class Tile
  {
    public int Status { get; set; }
    public int OwnedPlayer { get; set; }
  }
  public class PointConverter
  {
    public static (int, int) VectorConverter(Vector3 vp)
    {
      return ((int)Math.Round(vp.x), (int)Math.Round(vp.y));
    }

  }

}