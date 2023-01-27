using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] private TeamColor team;
    [SerializeField] private AnimationCurve yCurve;
    [SerializeField] private float animationSpeed = 1f;

    private PieceType _pieceType = PieceType.None;
    private Outline _pieceOutline;
    private BoardField _field;
    private int _countMoves = 0;
    private ChessPiece _attackingPiece;
    private List<BoardField> _protectDir = new();

    public PieceType PieceType { set { _pieceType = value; } get { return _pieceType; } }
    public TeamColor Team { get { return team; } }
    public BoardField Field { set { _field = value; } get { return _field; } }
    public int CountMoves { set { _countMoves = value; } get { return _countMoves; } }
    public ChessPiece AttackingPiece { get { return _attackingPiece; } }
    public List<BoardField> ProtectDirection { set { _protectDir = value; } get { return _protectDir; } }

    #region - Movement -

    public virtual int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        int possibleMoves = 0;

        if (possibleMoves > 0) EnableSelectOutline();

        return possibleMoves;
    }

    public virtual void MoveTo(BoardField targetField)
    {
        Field.ClearChessPiece();

        ChessPiece deletePiece = targetField.GetChessPiece();
        if (deletePiece) deletePiece.Death();

        Field = targetField;
        targetField.ClearChessPiece();
        targetField.SetChessPiece(this);
        StartCoroutine(Move(targetField.transform));
        CountMoves++;
    }

    protected IEnumerator Move(Transform target)
    {
        Vector3 fromPos = transform.position;
        Vector3 endPos = target.position;
        Vector3 targetPos;
        float progress = 0f;

        while (progress <= 1)
        {
            targetPos = fromPos + (endPos - fromPos) * progress;
            targetPos.y = yCurve.Evaluate(progress);
            transform.position = targetPos;
            progress += Time.deltaTime / animationSpeed;

            yield return null;
        }

        transform.position = endPos;
    }

    public virtual void Death()
    {
        _field.ClearChessPiece();
        gameObject.SetActive(false);
    }

    protected virtual bool CheckField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();

        if (piece)
        {
            if (piece.Team != Team & enableFields) field.Enable();
            return false;
        }

        if (enableFields) field.Enable();
        return true;
    }

    protected virtual bool PreventCheck(BoardField[,] virtualBoard, int x, int y)
    {
        ChessPiece king = GameManager.singleton.FindKing(Team);
        int kingX = king.Field.X, kingY = king.Field.Y;
        _attackingPiece = null;

        if (x > kingX & y < kingY) //top right
        {
            int targetX = x + 1, targetY = y - 1;

            for (int i = 1; targetX < 8 & targetY >= 0; i++)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                {
                    print(piece);

                    if ((piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }

                targetX = x + i;
                targetY = y - i;
            }
        }
        else if (x == kingX & y < kingY) //top
        {
            for (int i = y - 1; i >= 0; i--)
            {
                ChessPiece piece = virtualBoard[x, i].GetChessPiece();

                if (piece)
                {
                    if ((piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }
            }
        }
        else if (x < kingX & y < kingY) //top left
        {
            int targetX = x - 1, targetY = y - 1;

            for (int i = 1; targetX >= 0 & targetY >= 0; i++)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                {
                    print(piece);

                    if ((piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }

                targetX = x - i; 
                targetY = y - i;
            }
        }
        else if (x < kingX & y == kingY) //left
        {
            for (int i = 1; i < kingX - x; i++)
            {
                ChessPiece piece = virtualBoard[i, y].GetChessPiece();

                if (piece)
                {
                    if ((piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }
            }
        }
        else if (x < kingX & y > kingY) //bottom left
        {
            int targetX = x - 1, targetY = y + 1;

            for (int i = 1; targetX >= 0 & targetY < 8; i++)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                {
                    print(piece);

                    if ((piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }

                targetX = x - i;
                targetY = y + i;
            }
        }
        else if (x == kingX & y > kingY) //bottom
        {
            for (int i = 1; i < y - kingY; i++)
            {
                ChessPiece piece = virtualBoard[x, i].GetChessPiece();

                if (piece)
                {
                    if ((piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }
            }
        }
        else if (x > kingX & y > kingY) //bottom right
        {
            int targetX = x + 1, targetY = y + 1;

            for (int i = 1; targetX < 8 & targetY < 8; i++)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                {
                    print(piece);

                    if ((piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }

                targetX = x + i;
                targetY = y + i;
            }
        }
        else if (x > kingX & y == kingY) //right
        {
            for (int i = 1; i < x - kingX; i++)
            {
                ChessPiece piece = virtualBoard[i, y].GetChessPiece();

                if (piece)
                {
                    if ((piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen) & piece.Team != Team)
                    {
                        _attackingPiece = piece;
                        CreateProtectDirection(virtualBoard, x, y);
                        return false;
                    }

                    break;
                }
            }
        }

        return true;
    }

    protected virtual void CreateProtectDirection(BoardField[,] virtualBoard, int x, int y)
    {
        _protectDir.Clear();
        ChessPiece piece = virtualBoard[x, y].GetChessPiece();
        
        if (!piece || !AttackingPiece) return;
        if (piece.PieceType == PieceType.Knight || piece.PieceType == PieceType.Pawn ||
            AttackingPiece.PieceType == PieceType.Rook & piece.PieceType == PieceType.Bishop ||
            AttackingPiece.PieceType == PieceType.Bishop & piece.PieceType == PieceType.Rook) return;

        int stepX = AttackingPiece.Field.X - x == 0 ? 0 : AttackingPiece.Field.X - x < 0 ? -1 : 1;
        int stepY = AttackingPiece.Field.Y - y == 0 ? 0 : AttackingPiece.Field.Y - y < 0 ? -1 : 1;
        int targetX = x + stepX;
        int targetY = y + stepY;

        while (true)
        {
            if (targetX == AttackingPiece.Field.X & targetY == AttackingPiece.Field.Y)
            {
                _protectDir.Add(virtualBoard[targetX, targetY]);
                break;
            }

            _protectDir.Add(virtualBoard[targetX, targetY]);

            targetX += stepX;
            targetY += stepY;
        }
    }

    #endregion

    #region - Outline -

    private void Awake()
    {
        _pieceOutline = GetComponent<Outline>();
        DisableOutline();
    }

    public void EnableDangerOutline()
    {
        _pieceOutline.OutlineColor = Color.red;
        _pieceOutline.enabled = true;
    }

    public void EnableSelectOutline()
    {
        _pieceOutline.OutlineColor = Color.green;
        _pieceOutline.enabled = true;
    }

    public void DisableOutline()
    {
        _pieceOutline.enabled = false;
    }

    #endregion
}


public enum PieceType
{
    None, Pawn, Rook, Knight, Bishop, Queen, King
}

public enum TeamColor
{
    White, Black
}
