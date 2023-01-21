public class King : ChessPiece
{
    private void Start()
    {
        PieceType = PieceType.King;
    }

    public override int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        int possibleMoves = 0;
        int targetX, targetY;

        targetX = Field.X - 1;
        targetY = Field.Y - 1;

        if (targetX >= 0 & targetY >= 0)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X + 1;
        targetY = Field.Y + 1;

        if (targetX < 8 & targetY < 8)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X - 1;
        targetY = Field.Y + 1;

        if (targetX >= 0 & targetY < 8)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X + 1;
        targetY = Field.Y - 1;

        if (targetX < 8 & targetY >= 0)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;


        targetX = Field.X - 1;
        targetY = Field.Y;

        if (targetX >= 0 & targetY >= 0)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X + 1;
        targetY = Field.Y;

        if (targetX < 8 & targetY < 8)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X;
        targetY = Field.Y + 1;

        if (targetX >= 0 & targetY < 8)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X;
        targetY = Field.Y - 1;

        if (targetX < 8 & targetY >= 0)
            if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        if (possibleMoves > 0) EnableSelectOutline();

        return possibleMoves;
    }
}
