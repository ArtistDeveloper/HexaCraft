using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace HexaCraft
{
    struct PQNode : IComparable<PQNode>
    {
        public int F;
        public int G;
        public int Q;
        public int R;

        public int CompareTo(PQNode other)
        {
            if (F == other.F)
                return 0;
            return F < other.F ? 1 : -1;
        }
    }

    public class PathFinder
    {
        public List<HexPos> AStar(Hex[,] hexTiles, HexPos start, HexPos dest)
        {
            int hexRange = hexTiles.GetLength(0);

            // UL, L, DL, DR, R, UR
            int[] deltaQ = new int[] { 00, -1, -1, 00, +1, +1 };
            int[] deltaR = new int[] { -1, 00, +1, +1, 00, -1 };
            int[] cost = new int[] { 10, 10, 10, 10, 10, 10 };

            // (y, x) Closed(Visited) List
            bool[,] closed = new bool[hexRange, hexRange];

            // (y, x) OpenList, 가는 길을 한 번이라도 탐색했는지 여부 분류
            // - 발견 X => MaxValue
            // - 발견 O => F = G + H
            int[,] open = new int[hexRange, hexRange]; 
            for (int y = 0; y < hexRange; y++)
            {
                for (int x = 0; x < hexRange; x++)
                {
                    open[y, x] = Int32.MaxValue;
                }
            }

            HexPos[,] parent = new HexPos[hexRange, hexRange];

            // 오픈리스트에서 가장 좋은 값 추출
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // 경로 시작점 세팅
            open[start.Q, start.R] = 10 * (Math.Abs(dest.Q - start.Q) + Math.Abs(dest.R - start.R)); // H만 계산
            pq.Push(new PQNode() { F = 10 * (Math.Abs(dest.Q - start.Q) + Math.Abs(dest.R - start.R)), G = 0, Q = start.Q, R = start.R });
            parent[start.Q, start.R] = new HexPos(start.Q, start.R);

            // 경로 탐색
            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다.
                PQNode node = pq.Pop();
                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
                if (closed[node.Q, node.R])
                    continue;

                // 방문처리
                closed[node.Q, node.R] = true;
                // 목적지를 도착했으면 바로 종료
                if (node.Q == dest.Q && node.R == dest.R)
                    break;

                // 육각 타일에서 이동할 수 있는 좌표를 확인 후 예약
                for (int i = 0; i < deltaQ.Length; i++)
                {
                    int nextQ = node.Q + deltaQ[i];
                    int nextR = node.R + deltaR[i];

                    if (nextR < 0 || nextR >= hexRange || nextQ < 0 || nextQ >= hexRange)
                        continue;
                    if (hexTiles[nextQ, nextR] == null)
                        continue;
                    if (hexTiles[nextQ, nextR].HexType == HexTileType.Obstacle)
                        continue;
                    if (closed[nextQ, nextR])
                        continue;

                    // 비용 계산
                    int g = node.G + cost[i];
                    int h = 10 * (Math.Abs(dest.Q - nextQ) + Math.Abs(dest.R - nextR));
                    // 다른 경로에서 더 빠른 길을 이미 찾았으면 스킵
                    if (open[nextQ, nextR] < g + h)
                        continue;

                    // 예약 진행
                    open[nextQ, nextR] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Q = nextQ, R = nextR });
                    parent[nextQ, nextR] = new HexPos(node.Q, node.R);
                }
            }
            List<HexPos> points = CalcPathFromParent(parent, dest);

            return points;
        }

        List<HexPos> CalcPathFromParent(HexPos[,] parent, HexPos Dest)
        {
            List<HexPos> points = new List<HexPos>();

            int q = Dest.Q;
            int r = Dest.R;
            while (parent[q, r].Q != q || parent[q, r].R != r)
            {
                points.Add(new HexPos(q, r));
                HexPos pos = parent[q, r];
                q = pos.Q;
                r = pos.R;
            }
            points.Add(new HexPos(q, r));
            points.Reverse();

            return points;
        }
    }
}
