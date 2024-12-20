﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}


public class Movement : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;

    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public Vector3 dest;

    public Npc npcnpc;
    public bool isMoving = false;
    public bool isPathFinding = false;

    public int i = 0;

    public float delay;

    public int state = 0;
    private List<Vector2> eventPos = new List<Vector2>() {new(36,7),new(40,5),new(45,3),new(57,-4),new(21,4)};
    private int totalEventCount=0;
    public int eventKind;
    public bool arrived;
    EventAction @event = null;
    ItemAction @item = null;

    private void FixedUpdate()
    {
        delay += Time.fixedDeltaTime;
            if (state == 0) {
                if (delay > 0.4f) {
                    if (Random.Range(0, 100) <= 10) {
                        state = 1;
                    }
                    StartCoroutine(NpcMove(FinalNodeList));
                    delay = 0;
                }
            } else if (state == 1) {
                npcnpc.info.text = "<color=\"grey\">?</color>";
                if (delay > 1f) {
                    npcnpc.info.text = "";

                    StartCoroutine(Event());
                }
            }

        if (state == 2)
        {
            if (delay < 0) {
                npcnpc.info.text = "<color=\"red\">!</color>";
            } else {
                npcnpc.info.text = "";
            }
            if (delay > 0.4f) {
                StartCoroutine(NpcMove(FinalNodeList));
                delay = 0;
            }

            if (!isMoving) {
                @event.ActNpc = npcnpc;
                @event.StartAction();

                state = 3;
            }
        }
        else if(state == 3)
        {
            if (@event == null) {
                state = 0;
            } else {
                @event.InAction();
            
                if (@event.EndAction()) {
                    EventEnd();
                }
            }
        }

        if (state == 4) {
            if (delay > 0.4f) {
                StartCoroutine(NpcMove(FinalNodeList));
                delay = 0;
            }

            if (!isMoving) {
                @item.StartAction();

                state = 5;
            }
        } else if (state == 5) {
            if (@item == null) {
                state = 0;
            } else {
                @item.InAction();

                if (@item.EndAction()) {
                    ItemEnd();
                }
            }
        }
    }

    public void StartItem(ItemAction it) {
        @item = it;

        StartCoroutine(startItem());
    }

    IEnumerator startItem() {
        if(@item.Activated == false)
        {
            state = 4;

            dest = @item.Pos;
            isPathFinding = false;

            @item.Activated = true;

            yield return new WaitForSeconds(0.2f);
            PathFinding();
            if (FinalNodeList.Count > 0) FinalNodeList.RemoveAt(0);

            isMoving = true;

            yield break;
        }
    }

    public void EventEnd() {
        delay = 0;
        state = 0;

        if (@event != null) {
            if (!@event.StayActive) @event.Activated = false;
            @event.ActNpc = null;

            @event = null;
        }
    }
    public void ItemEnd() {
        delay = 0;
        state = 0;

        if (@item != null) {
            @item.Activated = false;
            @item.ActNpc = null;

            @item = null;
        }
    }

    public void randomPosition() {
        isMoving = true;
        System.Random rand = new System.Random();
        while (true) {
            dest = new Vector3(
                rand.Next(bottomLeft.x, topRight.x),
                rand.Next(bottomLeft.y, topRight.y),
                0
            );
            targetPos = new Vector2Int(Mathf.RoundToInt(dest.x), Mathf.RoundToInt(dest.y));
            bool isWall = false;
            foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(dest.x, dest.y), 0.4f)) {
                if (col.gameObject.layer != 0) isWall = true;
            }
            if (!isWall) {
                break;
            }
        }
        if (isPathFinding) {
            isPathFinding = false;
            PathFinding();
            FinalNodeList.RemoveAt(0);
            isMoving = true;
        }
    }
    private void Start(){
        Invoke("StartFinding", 0.5f);
    }
    void StartFinding() {
        isPathFinding = false;
        PathFinding();
        FinalNodeList.RemoveAt(0);
        isMoving = true;
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
        startPos = new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
        targetPos = new Vector2Int(Mathf.RoundToInt(dest.x), Mathf.RoundToInt(dest.y));
        if (!isMoving
            && Mathf.Round(this.transform.position.x) == Mathf.RoundToInt(dest.x)
            && Mathf.Round(this.transform.position.y) == Mathf.RoundToInt(dest.y)
        ) {
            if (state == 0) randomPosition();
        }
    }

    IEnumerator NpcMove(List<Node> optimizedPath)
    {
        if (i >= optimizedPath.Count) {
            isMoving = false;
            yield break; // 리스트의 들어있는 값 수보다 i가 높을 시 yield break
        }
        Vector3 destination = new Vector3(optimizedPath[i].x, optimizedPath[i].y, 0);
        //Debug.Log(destination);
        npcnpc.MoveTo(destination, 0.2f);
        yield return new WaitForSeconds(0.2f);
        i++;
    }
    IEnumerator Event(){
        eventKind = Random.Range(0,10 + GameManager.Instance.events.Count);
        if (eventKind < GameManager.Instance.events.Count)
        {
            @event = GameManager.Instance.events[eventKind];
            if(@event.Activated == false)
            {
                dest = @event.Pos;
                isPathFinding = false;

                state = 2;

                @event.Activated = true;

                yield return new WaitForSeconds(0.2f);
                PathFinding();
                if (FinalNodeList.Count > 0) FinalNodeList.RemoveAt(0);

                isMoving = true;

                delay = -1;

                yield break;
            }
        }
        else{
            state = 0;
        }
        yield return null;
    }
    public void PathFinding()
    {
        isPathFinding = true;

        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f)) {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;
                }

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0 && isPathFinding)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 0; i < OpenList.Count; i++)
                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H)) {
                    CurNode = OpenList[i];
                }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                //for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
        i = 0;
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }



    void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }
}
