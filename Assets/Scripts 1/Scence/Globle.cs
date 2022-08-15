using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globle
{
    // private int[][] map = new int[3][];
    // private int[] roomTree = new int [9];
    private static int roomCount = 0;
    private static bool isGoingToRoom;
    private static int enemyCount;

    public static void toNextRoom()
    {
        isGoingToRoom = true;
    }
    public static void inTheNextRoom()
    {
        isGoingToRoom = false;
    }
    public static bool getIsGoRoom()
    {
        return isGoingToRoom;
    }
    public static int getRoom()
    {
        return roomCount;
    }
    public static void goRoom()
    {
        roomCount++;
    }
    public static void enemyAdd()
    {
        enemyCount++;
    }
    public static void enemySubtract()
    {
        enemyCount--;
    }
    public static void enemyReturn()
    {
        enemyCount = 0;
    }
    public static int getEnemy()
    {
        return enemyCount;
    }

}
