using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Generate Board")]
    [SerializeField] private BoardLayout boardLayout;
    [SerializeField] private GenerateVirtualBoard generateVirtualBoard;
    [SerializeField] private GenerateChessPieces generateChessPieces;
    [SerializeField] private LayerMask boardLayer;

    private static int t_boardSize = 8;
    private BoardField[,] _virtualBoard = new BoardField[t_boardSize, t_boardSize];
    private Camera _playerCamera;
    private ChessPiece _currentPiece;

    private void Awake()
    {
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
                        DisableFields();

                        return;
                    }

                    if (piece)
                    {
                        _currentPiece = piece;
                        DisableFields();
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
}
