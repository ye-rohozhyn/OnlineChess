using UnityEngine;

public class GenerateVirtualBoard : MonoBehaviour
{
    private BoardField[,] _virtualBoard;

    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float margin = 1f;
    [SerializeField] private GameObject fieldPrefab;

    public void GenerateBoard(int size)
    {
        _virtualBoard = new BoardField[size, size];

        float startX = spawnPosition.x;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Transform field = Instantiate(fieldPrefab, spawnPosition, Quaternion.identity).transform;
                field.name = $"X: {j}, Y: {i}";
                field.SetParent(transform);

                _virtualBoard[j, i] = field.GetComponent<BoardField>();
                _virtualBoard[j, i].X = j;
                _virtualBoard[j, i].Y = i;

                spawnPosition.x += margin;
            }

            spawnPosition.x = startX;
            spawnPosition.z -= margin;
        }
    }

    public BoardField[,] GetVirtualBoard()
    {
        return _virtualBoard;
    }
}
