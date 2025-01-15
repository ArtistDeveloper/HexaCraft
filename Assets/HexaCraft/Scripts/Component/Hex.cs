using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HexaCraft
{
    public struct HexPos
    {
        public int Q;
        public int R;
        public int S;

        public HexPos(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public HexPos(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
        }
    }

    public enum HexTileType
    {
        Blank,
        Path,
        Obstacle,
    }


    public class Hex : MonoBehaviour
    {
        public GameObject HexTile { get; private set; }
        
        public HexPos Pos { get; private set; }

        [field: SerializeField]
        public HexTileType HexType { get; set; }


        /// <summary>
        /// Axial coordinate Hex 초기화
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <param name="hexType"></param>
        public void Initialize(int q, int r, HexTileType hexType)
        {
            Debug.Assert(q + r + (-q - r) == 0);

            HexPos hexPos = new HexPos(q, r, -q - r);
            Pos = hexPos;
            HexTile = this.gameObject;
            HexType = hexType;
        }

        /// <summary>
        /// Cube coordinate Hex 초기화
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <param name="s"></param>
        /// <param name="hexType"></param>
        public void Initialize(int q, int r, int s, HexTileType hexType)
        {
            Debug.Assert(q + r + s == 0);

            HexPos hexPos = new HexPos(q, r, s);
            Pos = hexPos;
            HexTile = this.gameObject;
            HexType = hexType;
        }
    }
}
