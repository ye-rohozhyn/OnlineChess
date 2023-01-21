using UnityEngine;

public class Pawn : ChessPiece
{
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

        if (Team == TeamColor.White & targetY >= 0 || Team == TeamColor.Black & targetY < 8)
        {
            if (CountMoves == 0 & CheckField(virtualBoard[targetX, targetY], enableFields))
            {
                possibleMoves++;
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
                        if (pawn.CountMoves == 1 & pawn._moveOn2Fields & pawn._lastMoveNumber == GameManager.GetCountMoves())
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
                        if (pawn.CountMoves == 1 & pawn._moveOn2Fields & pawn._lastMoveNumber == GameManager.GetCountMoves())
                        {
                            if (CheckField(virtualBoard[targetX, targetY], enableFields))
                                possibleMoves++;
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
        transform.position = targetField.transform.position;
        CountMoves++;
        _lastMoveNumber = GameManager.GetCountMoves() + 1;
    }

    protected override bool CheckField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();

        if (piece) return false;

        if (enableFields) field.Enable();
        return true;
    }

    private bool CheckAttackField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();
        
        if (piece) 
            if (piece.Team != Team & enableFields) field.Enable();

        return false;
    }
}