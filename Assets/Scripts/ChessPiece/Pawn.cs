using System;
using UnityEngine;

public class Pawn : ChessPiece
{
    [SerializeField] private GameObject[] replacePieces;
    private bool _moveOn2Fields = false;
    private BoardField[,] _virtualBoard;
    private int _lastMoveNumber = 0;

    private void Start()
    {
        PieceType = PieceType.Pawn;
    }

    public override int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        _virtualBoard = virtualBoard;
        int possibleMoves = 0;
        int targetX, targetY;

        targetY = Team == TeamColor.White ? (Field.Y - 1) : (Field.Y + 1);
        targetX = Field.X;

        ChessPiece king = GameManager.singleton.FindKing(Team);
        int checkCount = GameManager.singleton.GetCheckCount(king.Field.X, king.Field.Y).Count;

        if (!PreventCheck(virtualBoard, Field.X, Field.Y) & checkCount == 0)
        {
            if (ProtectDirection.Count == 0) return 0;

            foreach (BoardField field in ProtectDirection)
            {
                if (enableFields)
                {
                    field.Enable();
                    possibleMoves++;
                }
            }
        }
        else
        {
            if (Team == TeamColor.White & targetY >= 0 || Team == TeamColor.Black & targetY < 8)
        {
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

            if (CountMoves == 0 & !virtualBoard[targetX, targetY].GetChessPiece())
            {
                targetY = Team == TeamColor.White ? (Field.Y - 2) : (Field.Y + 2);

                if (Team == TeamColor.White & targetY >= 0 || Team == TeamColor.Black & targetY < 8)
                {

                    if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;
                }

                targetY = Team == TeamColor.White ? (Field.Y - 1) : (Field.Y + 1);
            }

            targetX = Field.X - 1;

            if (targetX >= 0)
            {
                if (CheckAttackField(virtualBoard[targetX, targetY], enableFields))
                {
                    possibleMoves++;
                }
                else if (virtualBoard[targetX, Field.Y].GetChessPiece())
                {
                    Pawn pawn = virtualBoard[targetX, Field.Y].GetChessPiece().GetComponent<Pawn>();
                    if (pawn)
                    {
                        if (pawn.Team != Team & pawn.CountMoves == 1 & pawn._moveOn2Fields & pawn._lastMoveNumber == GameManager.singleton.GetCountMoves())
                        {
                            if (CheckField(virtualBoard[targetX, targetY], enableFields))
                                possibleMoves++;
                        }
                    }
                }
            }

            targetX = Field.X + 1;

            if (targetX < 8)
            {
                if (CheckAttackField(virtualBoard[targetX, targetY], enableFields))
                {
                    possibleMoves++;
                }
                else if (virtualBoard[targetX, Field.Y].GetChessPiece())
                {
                    Pawn pawn = virtualBoard[targetX, Field.Y].GetChessPiece().GetComponent<Pawn>();
                    if (pawn)
                    {
                        if (pawn.Team != Team & pawn.CountMoves == 1 & pawn._moveOn2Fields & pawn._lastMoveNumber == GameManager.singleton.GetCountMoves())
                        {
                            if (CheckField(virtualBoard[targetX, targetY], enableFields))
                                possibleMoves++;
                        }
                    }
                }
            }
        }
        }

        if (possibleMoves > 0 & enableFields) EnableSelectOutline();

        return possibleMoves;
    }

    public override void MoveTo(BoardField targetField)
    {
        Field.ClearChessPiece();

        if (CountMoves == 0)
            if (Mathf.Abs(Field.Y - targetField.Y) == 2)
                _moveOn2Fields = true;

        ChessPiece deletePiece = targetField.GetChessPiece();
        if (!deletePiece)
            if (targetField.X != Field.X) 
                deletePiece = _virtualBoard[targetField.X, Field.Y].GetChessPiece();

        if (deletePiece) deletePiece.Death();

        Field = targetField;
        targetField.ClearChessPiece();
        targetField.SetChessPiece(this);
        StartCoroutine(Move(targetField.transform));
        CountMoves++;
        _lastMoveNumber = GameManager.singleton.GetCountMoves() + 1;
    }

    protected override bool CheckField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();

        if (piece) return false;

        BoardField[] protectFields = GameManager.singleton.GetProtectFields().ToArray();
        int index = Array.IndexOf(protectFields, GameManager.singleton.GetVirtualBoard()[field.X, field.Y]);

        if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
        {
            if (enableFields) field.Enable();
            return true;
        }

        return false;
    }

    private bool CheckAttackField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();

        if (piece)
        {
            if (piece.Team != Team & enableFields)
            {
                BoardField[] protectFields = GameManager.singleton.GetProtectFields().ToArray();
                int index = Array.IndexOf(protectFields, GameManager.singleton.GetVirtualBoard()[field.X, field.Y]);

                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    field.Enable();
                }

                return true;
            }
        }

        return false;
    }

    protected override void CreateProtectDirection(BoardField[,] virtualBoard, int x, int y)
    {
        ProtectDirection.Clear();
        ChessPiece pawn = virtualBoard[x, y].GetChessPiece();
        int targetY = Team == TeamColor.White ? (Field.Y - 1) : (Field.Y + 1);

        if ((AttackingPiece.PieceType == PieceType.Rook || AttackingPiece.PieceType == PieceType.Queen) & AttackingPiece.Field.X == pawn.Field.X)
        {
            ChessPiece piece = virtualBoard[x, targetY].GetChessPiece();

            if (!piece)
            {
                ProtectDirection.Add(virtualBoard[x, targetY]);
                targetY = Team == TeamColor.White ? (Field.Y - 2) : (Field.Y + 2);

                if (CountMoves == 0)
                {
                    piece = virtualBoard[x, targetY].GetChessPiece();

                    if (!piece)
                    {
                        ProtectDirection.Add(virtualBoard[x, targetY]);
                    }
                }
            }
        }
        else if ((AttackingPiece.PieceType == PieceType.Bishop || AttackingPiece.PieceType == PieceType.Queen) & AttackingPiece.Field.Y - pawn.Field.Y == -1)
        {
            ChessPiece rPiece = virtualBoard[x + 1, targetY].GetChessPiece();

            if (rPiece)
            {
                ChessPiece lPiece = virtualBoard[x - 1, targetY].GetChessPiece();

                if (rPiece.Team != Team)
                {
                    if (lPiece)
                        if (lPiece.Team != Team) return;

                    if (rPiece.PieceType == PieceType.Bishop || rPiece.PieceType == PieceType.Queen)
                    {
                        ProtectDirection.Add(rPiece.Field);
                    }
                }
                else
                {
                    if (lPiece)
                    {
                        if (lPiece.Team != Team & lPiece.PieceType == PieceType.Bishop || lPiece.PieceType == PieceType.Queen)
                        {
                            ProtectDirection.Add(lPiece.Field);
                        }
                    }
                }
            }
            else
            {
                ChessPiece lpiece = virtualBoard[x - 1, targetY].GetChessPiece();

                if (lpiece)
                {
                    if (lpiece.Team != Team & lpiece.PieceType == PieceType.Bishop || lpiece.PieceType == PieceType.Queen)
                    {
                        ProtectDirection.Add(lpiece.Field);
                    }
                }
            }
        }
    }

    public GameObject[] GetReplacePieces()
    {
        return replacePieces;
    }
}