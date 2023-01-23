using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Generate Board")]
    [SerializeField] private BoardLayout boardLayout;
    [SerializeField] private GenerateVirtualBoard generateVirtualBoard;
    [SerializeField] private GenerateChessPieces generateChessPieces;
    [SerializeField] private LayerMask boardLayer;

    [Header("Settings")]
    [SerializeField] private LimitFrameRate limitFPS;

    private static int t_boardSize = 8;
    private BoardField[,] _virtualBoard = new BoardField[t_boardSize, t_boardSize];
    private Camera _playerCamera;
    private ChessPiece _currentPiece;
    private static int t_countMoves; 

    private void Awake()
    {
        Application.targetFrameRate = (int)limitFPS;

        _playerCamera = Camera.main;

        generateVirtualBoard.GenerateBoard(t_boardSize);
        _virtualBoard = generateVirtualBoard.GetVirtualBoard();

        generateChessPieces.GeneratePieces(_virtualBoard, boardLayout);
    }

    private void Update()
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
                        _currentPiece.MoveTo(field);
                        _currentPiece = null;
                        t_countMoves++;
                        DisableFields();

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

    private void DisableFields()
    {
        foreach (BoardField field in _virtualBoard)
        {
            field.Disable();
        }
    }

    public static int GetCountMoves()
    {
        return t_countMoves;
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