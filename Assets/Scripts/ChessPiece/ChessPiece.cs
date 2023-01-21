using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] private PieceType pieceType;
    [SerializeField] private TeamColor team;

    private Outline _pieceOutline;
    private BoardField _field;
    private int _countMoves = 0;

    public PieceType PieceType { get { return pieceType; } }
    public TeamColor Team { get { return team; } }
    public BoardField Field { set { _field = value; } get { return _field; } }
    public int CountMoves { set { _countMoves = value; } get { return _countMoves; } }

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
        transform.position = targetField.transform.position;
        CountMoves++;
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
    Pawn, Rook, Knight, Bishop, Queen, King
}

public enum TeamColor
{
    White, Black
}
