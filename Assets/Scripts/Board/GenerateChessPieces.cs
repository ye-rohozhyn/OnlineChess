using UnityEngine;

public class GenerateChessPieces : MonoBehaviour
{
    public void GeneratePieces(BoardField[,] virtualBoard, BoardLayout boardLayout)
    {
        for (int i = 0; i < boardLayout.GetLength(); i++)
        {
            GameObject prefab = boardLayout.GetPrefab(i);
            Vector2Int pos = boardLayout.GetPosition(i);
            Transform field = virtualBoard[pos.x, pos.y].transform;

            Transform pieceTransform = Instantiate(prefab, field.position, prefab.transform.rotation).transform;
            pieceTransform.SetParent(field);
            ChessPiece piece = pieceTransform.GetComponent<ChessPiece>();

            piece.Field = virtualBoard[pos.x, pos.y];
            virtualBoard[pos.x, pos.y].SetChessPiece(piece);
        }
    }
}
