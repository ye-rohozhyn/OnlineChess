using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Layout", menuName = "Chess Game/Board Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSquareSetup
    {
        public Vector2Int position;
        public GameObject prefab;
    }

    [SerializeField] private BoardSquareSetup[] boardPieces;

    public int GetLength()
    {
        return boardPieces.Length;
    }

    public GameObject GetPrefab(int index)
    {
        return boardPieces[index].prefab;
    }

    public Vector2Int GetPosition(int index)
    {
        return boardPieces[index].position;
    }
}

