using UnityEngine;

public class BoardField : MonoBehaviour
{
    [SerializeField] private ChessPiece _chessPiece;
    [SerializeField] private GameObject greenMarker;

    private int _x;
    public int X { set { _x = value; } get { return _x; } }

    private int _y;
    public int Y { set { _y = value; } get { return _y; } }

    private bool _isActive = false;

    #region - Chess Piece -

    public void SetChessPiece(ChessPiece chessPiece)
    {
        _chessPiece = chessPiece;
    }

    public ChessPiece GetChessPiece()
    {
        return _chessPiece;
    }

    public void ClearChessPiece()
    {
        _chessPiece = null;
        _isActive = false;
    }

    #endregion

    #region - Field -

    public void Enable()
    {
        _isActive = true;

        if (_chessPiece)
        {
            _chessPiece.EnableDangerOutline();

            return;
        }

        greenMarker.SetActive(true);
    }

    public void Disable()
    {
        _isActive = false;

        if (_chessPiece)
        {
            _chessPiece.DisableOutline();
            return;
        }

        greenMarker.SetActive(false);
    }

    public bool IsActive()
    {
        return _isActive;
    }

    #endregion
}
