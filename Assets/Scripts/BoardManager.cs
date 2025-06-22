using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class BoardManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject fishPrefab, treePrefab, tornadoPrefab, tree1Prefab, cactusPrefab, emeraldPrefab, wheatPrefab;
    public TextMeshProUGUI rules;
    GameObject[] tiles;
    //tile prefabs
    long dirtBB = 0, desertBB = 0; //tornado & Cactus
    long pastureBB = 0, woodBB = 0; //trees
    long waterBB = 0; //fish
    long rockBB = 0; //Emerald
    long grainBB = 0;//wheat

    //Object Prefabs
    long fishBB = 0;
    long treeBB = 0;
    long tree1BB = 0;
    long tornadoBB = 0;
    long cactusBB = 0;
    long emeraldBB = 0;
    long wheatBB = 0;


    void Start()
    {
        tiles = new GameObject[64];
        for (int r = 0; r < 8; ++r) //++r gets new value
        {
            for (int c = 0; c < 8; ++c)
            {
                int randomTile = UnityEngine.Random.Range(0, tilePrefabs.Length);
                Vector3 pos = new Vector3(c, 0, r);
                GameObject tile = Instantiate(
                        tilePrefabs[randomTile],
                        pos,
                        Quaternion.identity
                );
                tile.name = tile.tag + "_" + r + "_" + c;
                tiles[r * 8 + c] = tile;
                if (tile.tag == "Dirt")
                {
                    dirtBB = SetCellState(dirtBB, r, c);
                    PrintBB("Dirt", dirtBB);
                }
                else if (tile.tag == "Water")
                {
                    waterBB = SetCellState(waterBB, r, c);
                    PrintBB("Water", waterBB);
                }
                else if (tile.tag == "Pasture")
                {
                    pastureBB = SetCellState(pastureBB, r, c);
                    PrintBB("Pasture", pastureBB);
                }
                else if (tile.tag == "Desert")
                {
                    desertBB = SetCellState(desertBB, r, c);
                    PrintBB("Desert", desertBB);
                }
                else if (tile.tag == "Grain")
                {
                    grainBB = SetCellState(grainBB, r, c);
                    PrintBB("Grain", grainBB);
                }
                else if (tile.tag == "Rock")
                {
                    rockBB = SetCellState(rockBB, r, c);
                    PrintBB("Rock", rockBB);
                }
                else if (tile.tag == "Woods")
                {
                    woodBB = SetCellState(woodBB, r, c);
                    PrintBB("Woods", woodBB);
                }
            }
        }
        //spawn canvas for text
        GameObject TextCanvas = new GameObject();
        TextCanvas.name = "Canvas";
        TextCanvas.AddComponent<Canvas>();
        TextCanvas.AddComponent<CanvasScaler>();
        TextCanvas.AddComponent<GraphicRaycaster>();
        //Canvas is taken from GameObject to Overlay
        Canvas canvas;
        canvas = TextCanvas.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //Creates TMP Game object
        GameObject TextTMP = new GameObject();
        TextTMP.transform.parent = TextCanvas.transform;//parent to the canvas
        TextTMP.AddComponent<TextMeshProUGUI>();
        //Text Properties like font, position etc.
        //Spawn rules in-game
        rules = TextTMP.GetComponent<TextMeshProUGUI>();
        rules.transform.position = new Vector3(150, 1000, 0);
        rules.text = "SPAWN RULES:\n - Trees, 2 secs\n - fish, 1 sec\n - tornado, 6 secs\n - cactus, 1 sec\n - wheat, 4 secs\n - emerald, 3 secs";
        rules.fontSize = 25;
        rules.alignment = TextAlignmentOptions.TopLeft;

        /* == SPAWN RULES == 
         * - Trees, 2 secs
         * - fish, 1 sec
         * - tornado ,6 secs
         * - cactus, 1 sec
         * - wheat, 4 secs
         * - emerald, 3 secs
         */

        InvokeRepeating("PlantTree", 0.2f, 2.0f);
        InvokeRepeating("WaterFish", 0.1f, 1.0f);
        InvokeRepeating("DirtTornado", 0.6f, 6.0f);
        InvokeRepeating("DesertCactus", 0.1f, 1.0f);
        InvokeRepeating("WoodTree1", 0.2f, 2.0f);
        InvokeRepeating("RockEmerald", 0.3f, 3.0f);
        InvokeRepeating("GrainWheat", 0.4f, 4.0f);
    }

    void PrintBB(String name, long BB)
    {
        Debug.Log(name + ": " + Convert.ToString(BB, 2).PadLeft(64, '0'));
    }
    long SetCellState(long Bitboard, int row, int col)
    {
        long newBit = 1L << (row * 8 + col);
        return (Bitboard |= newBit);
    }
    bool GetCellState(long Bitboard, int row, int col)
    {
        long mask = 1L << (row * 8 + col);
        return ((Bitboard & mask) != 0);
    }
    int CellCount(long bitboard) // Count how many Cells is created base on tags
    {
        int count = 1;
        long bb = bitboard;
        while (bb != 0)
        {
            bb &= bb - 1;
            count++;
        }
        return count;
    }
    void PlantTree()
    {
        int rr = UnityEngine.Random.Range(0, 8);//random row
        int rc = UnityEngine.Random.Range(0, 8);//random column
        if (GetCellState(pastureBB, rr, rc))
        {
            GameObject tree = Instantiate(treePrefab);
            tree.transform.parent = tiles[rr * 8 + rc].transform;
            tree.transform.localPosition = Vector3.zero;
            treeBB = SetCellState(treeBB, rr, rc);
        }
    }
    void WaterFish()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(waterBB, rr, rc))
        {
            GameObject fish = Instantiate(fishPrefab);
            fish.transform.parent = tiles[rr * 8 + rc].transform;
            fish.transform.localPosition = new Vector3(0, 1, 0);//changed the position of Y.
            fishBB = SetCellState(fishBB, rr, rc);
        }
    }
    void DirtTornado()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(dirtBB, rr, rc))
        {
            GameObject tornado = Instantiate(tornadoPrefab);
            tornado.transform.parent = tiles[rr * 8 + rc].transform;
            tornado.transform.localPosition = new Vector3(0, 1, 0);
            tornadoBB = SetCellState(tornadoBB, rr, rc);
        }
    }

    void DesertCactus()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(desertBB, rr, rc))
        {
            GameObject cactus = Instantiate(cactusPrefab);
            cactus.transform.parent = tiles[rr * 8 + rc].transform;
            cactus.transform.localPosition = new Vector3(0, 1, 0);
            cactusBB = SetCellState(cactusBB, rr, rc);
        }
    }

    void WoodTree1()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(woodBB, rr, rc))
        {
            GameObject tree1 = Instantiate(tree1Prefab);
            tree1.transform.parent = tiles[rr * 8 + rc].transform;
            tree1.transform.localPosition = Vector3.zero;
            tree1BB = SetCellState(tree1BB, rr, rc);
        }
    }

    void RockEmerald()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(rockBB, rr, rc))
        {
            GameObject emerald = Instantiate(emeraldPrefab);
            emerald.transform.parent = tiles[rr * 8 + rc].transform;
            emerald.transform.localPosition = new Vector3(0, 3, 0);
            emeraldBB = SetCellState(emeraldBB, rr, rc);
        }
    }

    void GrainWheat()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(grainBB, rr, rc))
        {
            GameObject wheat = Instantiate(wheatPrefab);
            wheat.transform.parent = tiles[rr * 8 + rc].transform;
            wheat.transform.localPosition = new Vector3(0, 1, 0);
            wheatBB = SetCellState(wheatBB, rr, rc);
        }
    }

}
