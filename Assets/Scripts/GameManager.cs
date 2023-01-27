using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("Generate Board")]
    [SerializeField] private BoardLayout boardLayout;
    [SerializeField] private GenerateVirtualBoard generateVirtualBoard;
    [SerializeField] private GenerateChessPieces generateChessPieces;
    [SerializeField] private LayerMask boardLayer;

    [Header("UI")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject pawnChangeMenu;

    [Header("Settings")]
    [SerializeField] private LimitFrameRate limitFPS;
    [SerializeField] private GameState _gameState;

    private static int t_boardSize = 8;
    private BoardField[,] _virtualBoard = new BoardField[t_boardSize, t_boardSize];
    private Camera _playerCamera;
    private ChessPiece _currentPiece;
    private static int t_countMoves;
    public static GameManager singleton;
    private BoardField _moveField;

    private void Awake()
    {
        singleton = this;
        Application.targetFrameRate = (int)limitFPS;

        _playerCamera = Camera.main;

        generateVirtualBoard.GenerateBoard(t_boardSize);
        _virtualBoard = generateVirtualBoard.GetVirtualBoard();

        generateChessPieces.GeneratePieces(_virtualBoard, boardLayout);
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, boardLayer))
                {
                    Transform objectHit = hit.transform;

                    if (objectHit)
                    {
                        BoardField field = objectHit.GetComponent<BoardField>();
                        ChessPiece piece = field.GetChessPiece();

                        if (field.IsActive())
                        {
                            if (_currentPiece.PieceType == PieceType.Pawn & (_currentPiece.Team == TeamColor.White ? field.Y == 0 : field.Y == 7))
                            {
                                background.SetActive(true);
                                pawnChangeMenu.SetActive(true);
                                _moveField = field;

                                return;
                            }

                            _currentPiece.MoveTo(field);
                            _currentPiece = null;
                            t_countMoves++;
                            DisableFields();
                            ChangeGameState();

                            return;
                        }

                        DisableFields();
                        if (piece)
                        {
                            _currentPiece = piece;
                            piece.GetPossibleMoves(_virtualBoard, true);
                        }
                    }
                }
            }
        }
    }

    private void MovePieceAndChangeGameState(BoardField field)
    {
        if (_currentPiece.PieceType == PieceType.Pawn & (_currentPiece.Team == TeamColor.White ? field.Y == 0 : field.Y == 7))
        {
            background.SetActive(true);
            pawnChangeMenu.SetActive(true);
            Invoke(null, Mathf.Infinity);
        }

        _currentPiece.MoveTo(field);
        _currentPiece = null;
        t_countMoves++;
        DisableFields();
        ChangeGameState();
    }

    public void ChangeToPiece(int pieceNumber)
    {
        GameObject[] chessPieces = _currentPiece.GetComponent<Pawn>().GetReplacePieces();

        _currentPiece.gameObject.SetActive(false);
        Transform newPiece = Instantiate(chessPieces[pieceNumber], _currentPiece.transform.position, Quaternion.identity).transform;
        newPiece.transform.position = _currentPiece.transform.position;
        ChessPiece piece = newPiece.GetComponent<ChessPiece>();
        piece.Field = _currentPiece.Field;
        _currentPiece = piece;

        background.SetActive(false);
        pawnChangeMenu.SetActive(false);

        _currentPiece.MoveTo(_moveField);
        _currentPiece = null;
        t_countMoves++;
        DisableFields();
        ChangeGameState();
    }

    private void ChangeGameState()
    {
        TeamColor currentTeam = t_countMoves % 2 == 0 ? TeamColor.White : TeamColor.Black;

        ChessPiece king = FindKing(currentTeam);

        if (!king) return;

        int checkCount = GetCheckCount(king.Field.X, king.Field.Y).Count;
        int countTeamMoves = GetCountTeamMoves(currentTeam);
        int countKingMoves = king.GetPossibleMoves(_virtualBoard, false);

        if (checkCount == 0)
        {
            if (countTeamMoves == 0)
            {
                _gameState = GameState.Stalemate;
                return;
            }

            if (!CheckDraw())
            {
                _gameState = GameState.Draw;
                return;
            }

            _gameState = GameState.Normal;
        }
        else
        {
            if (!CheckDraw())
            {
                _gameState = GameState.Draw;
                return;
            }

            _gameState = currentTeam == TeamColor.White ? GameState.CheckW : GameState.CheckB;

            if (countKingMoves == 0 & checkCount > 1 || countTeamMoves == 0)
            {
                _gameState = currentTeam == TeamColor.White ? GameState.CheckmateW : GameState.CheckmateB;
                return;
            }
        }
    }

    private bool CheckDraw()
    {
        List<ChessPiece> whitePieces = new();
        List<ChessPiece> blackPieces = new();

        foreach (BoardField field in _virtualBoard)
        {
            ChessPiece piece = field.GetChessPiece();

            if (piece)
            {
                if (piece.Team == TeamColor.White) whitePieces.Add(piece);
                else blackPieces.Add(piece);
            }
        }

        if (whitePieces.Count <= 2 & blackPieces.Count <= 2)
        {
            if (whitePieces.Count == 2 & blackPieces.Count == 2)
            {
                ChessPiece whiteBishop = whitePieces[0].PieceType == PieceType.Bishop ? whitePieces[0] : whitePieces[1].PieceType == PieceType.Bishop ? whitePieces[1] : null;
                ChessPiece blackBishop = blackPieces[0].PieceType == PieceType.Bishop ? blackPieces[0] : blackPieces[1].PieceType == PieceType.Bishop ? blackPieces[1] : null;

                if (whiteBishop != null & blackBishop != null)
                {
                    bool whiteX = (whiteBishop.Field.X + 1) % 2 == 0;
                    bool whiteY = (whiteBishop.Field.Y + 1) % 2 == 0;
                    bool blackX = (blackBishop.Field.X + 1) % 2 == 0;
                    bool blackY = (blackBishop.Field.Y + 1) % 2 == 0;

                    if (((whiteX & whiteY & blackX & blackY) || (!whiteX & !whiteY & !blackX & !blackY)) ||
                        ((whiteX & !whiteY & blackX & !blackY) || (!whiteX & whiteY & !blackX & blackY)) ||
                        ((!whiteX & whiteY & blackX & !blackY) || (whiteX & !whiteY & !blackX & blackY)))
                        return false;
                }
            }
            else if (whitePieces.Count == 1 & blackPieces.Count == 1)
            {
                return false;
            }
            else if (whitePieces.Count == 1 & blackPieces.Count == 2)
            {
                if (blackPieces[0].PieceType == PieceType.Bishop || blackPieces[1].PieceType == PieceType.Bishop)
                    return false;

                if (blackPieces[0].PieceType == PieceType.Knight || blackPieces[1].PieceType == PieceType.Knight)
                    return false;
            }
            else if (whitePieces.Count == 2 & blackPieces.Count == 1)
            {
                if (whitePieces[0].PieceType == PieceType.Bishop || whitePieces[1].PieceType == PieceType.Bishop)
                    return false;

                if (whitePieces[0].PieceType == PieceType.Knight || whitePieces[1].PieceType == PieceType.Knight)
                    return false;
            }
        }

        return true;
    }

    public ChessPiece FindKing(TeamColor teamColor)
    {
        foreach (BoardField field in _virtualBoard)
        {
            ChessPiece piece = field.GetChessPiece();

            if (piece)
            {
                if (piece.PieceType == PieceType.King & piece.Team == teamColor)
                {
                    return piece;
                }
            }
        }

        return null;
    }

    private int GetCountTeamMoves(TeamColor teamColor)
    {
        int countTeamMoves = 0;

        foreach (BoardField field in _virtualBoard)
        {
            ChessPiece piece = field.GetChessPiece();

            if (piece)
            {
                if (piece.Team == teamColor)
                {
                    countTeamMoves += piece.GetPossibleMoves(_virtualBoard, false);
                }
            }
        }

        return countTeamMoves;
    }

    public List<BoardField> GetProtectFields()
    {
        ChessPiece king = FindKing(t_countMoves % 2 == 0 ? TeamColor.White : TeamColor.Black);
        List<ChessPiece> attackPieces = GetCheckCount(king.Field.X, king.Field.Y);
        List<BoardField> protectFields = new();

        if (attackPieces.Count == 1)
        {
            int kingX = king.Field.X, kingY = king.Field.Y, 
                x = attackPieces[0].Field.X, y = attackPieces[0].Field.Y;

            if (x > kingX & y < kingY) //top right
            {
                for (int i = 1; i < x - kingX; i++)
                    protectFields.Add(_virtualBoard[kingX + i, kingY - i]);
            }
            else if (x == kingX & y < kingY) //top
            {
                for (int i = 1; i < kingY - y; i++)
                    protectFields.Add(_virtualBoard[kingX, kingY - i]);
            }
            else if (x < kingX & y < kingY) //top left
            {
                for (int i = 1; i < kingX - x; i++)
                    protectFields.Add(_virtualBoard[kingX - i, kingY - i]);
            }
            else if (x < kingX & y == kingY) //left
            {
                for (int i = 1; i < kingX - x; i++)
                    protectFields.Add(_virtualBoard[kingX - i, kingY]);
            }
            else if (x < kingX & y > kingY) //bottom left
            {
                for (int i = 1; i < y - kingY; i++)
                    protectFields.Add(_virtualBoard[kingX - i, kingY + i]);
            }
            else if (x == kingX & y > kingY) //bottom
            {
                for (int i = 1; i < y - kingY; i++)
                    protectFields.Add(_virtualBoard[kingX, kingY + i]);
            }
            else if (x > kingX & y > kingY) //bottom right
            {
                for (int i = 1; i < y - kingY; i++)
                    protectFields.Add(_virtualBoard[kingX + i, kingY + i]);
            }
            else if (x > kingX & y == kingY) //right
            {
                for (int i = 1; i < x - kingX; i++)
                    protectFields.Add(_virtualBoard[kingX + i, kingY]);
            }

            protectFields.Add(attackPieces[0].Field);
        }

        return protectFields;
    }

    private void DisableFields()
    {
        foreach (BoardField field in _virtualBoard)
        {
            field.Disable();
        }
    }

    public int GetCountMoves()
    {
        return t_countMoves;
    }

    public BoardField[,] GetVirtualBoard()
    {
        return _virtualBoard;
    }

    public List<ChessPiece> GetCheckCount(int x, int y)
    {
        List<ChessPiece> attackPieces = new();
        ChessPiece king = _virtualBoard[x, y].GetChessPiece();

        //horizontal & vertical
        int targetX, targetY;

        for (int i = x - 1; i >= 0; i--)
        {
            ChessPiece piece = _virtualBoard[i, y].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == x - 1))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (i == king.Field.X) continue;

                break;
            }
        }

        for (int i = x + 1; i < 8; i++)
        {
            ChessPiece piece = _virtualBoard[i, y].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == x + 1))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (i == king.Field.X) continue;

                break;
            }
        }

        for (int i = y - 1; i >= 0; i--)
        {
            ChessPiece piece = _virtualBoard[x, i].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == y - 1))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (i == king.Field.Y) continue;

                break;
            }
        }

        for (int i = y + 1; i < 8; i++)
        {
            ChessPiece piece = _virtualBoard[x, i].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == y + 1))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (i == king.Field.Y) continue;

                break;
            }
        }

        //diagonals

        targetX = x - 1;
        targetY = y + 1;

        for (int i = 2; targetX >= 0 & targetY < 8; i++)
        {
            ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (targetX == king.Field.X & targetY == king.Field.Y)
                {
                    targetX = x - i;
                    targetY = y + i;

                    continue;
                }

                break;
            }

            targetX = x - i;
            targetY = y + i;
        }

        targetX = x + 1;
        targetY = y - 1;

        for (int i = 2; targetX < 8 & targetY >= 0; i++)
        {
            ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (targetX == king.Field.X & targetY == king.Field.Y)
                {
                    targetX = x + i;
                    targetY = y - i;

                    continue;
                }

                break;
            }

            targetX = x + i;
            targetY = y - i;
        }

        targetX = x + 1;
        targetY = y + 1;

        for (int i = 2; targetX < 8 & targetY < 8; i++)
        {
            ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (targetX == king.Field.X & targetY == king.Field.Y)
                {
                    targetX = x + i;
                    targetY = y + i;

                    continue;
                }

                break;
            }

            targetX = x + i;
            targetY = y + i;
        }

        targetX = x - 1;
        targetY = y - 1;

        for (int i = 2; targetX >= 0 & targetY >= 0; i++)
        {
            ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (king.Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        attackPieces.Add(piece);
                        break;
                    }
                }

                if (targetX == king.Field.X & targetY == king.Field.Y)
                {
                    targetX = x - i;
                    targetY = y - i;

                    continue;
                }

                break;
            }

            targetX = x - i;
            targetY = y - i;
        }

        //knight
        targetY = y - 2;

        if (targetY >= 0)
        {
            targetX = x - 1;
            if (targetX >= 0)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }

            targetX = x + 1;
            if (targetX < 8)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }
        }

        targetY = y + 2;

        if (targetY < 8)
        {
            targetX = x - 1;
            if (targetX >= 0)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }

            targetX = x + 1;
            if (targetX < 8)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }
        }

        targetX = x - 2;

        if (targetX >= 0)
        {
            targetY = y - 1;
            if (targetY >= 0)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }

            targetY = y + 1;
            if (targetY < 8)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }
        }

        targetX = x + 2;

        if (targetX < 8)
        {
            targetY = y - 1;
            if (targetY >= 0)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }

            targetY = y + 1;
            if (targetY < 8)
            {
                ChessPiece piece = _virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (king.Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            attackPieces.Add(piece);
            }
        }

        return attackPieces;
    }
}

public enum LimitFrameRate
{
    limit0 = 0,
    limit60 = 60,
    limit90 = 90,
    limit120 = 120,
    limit240 = 240
}

public enum GameState
{
    Normal, CheckW, CheckB, CheckmateW, CheckmateB, Stalemate, Draw 
}