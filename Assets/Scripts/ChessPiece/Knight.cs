public class Knight : ChessPiece
{
    private void Start()
    {
        PieceType = PieceType.Knight;
    }

    public override int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        int possibleMoves = 0;
        int targetX, targetY;

        //top
        targetY = Field.Y - 2;

        if (targetY >= 0)
        {
            targetX = Field.X - 1;
            if (targetX >= 0)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }

            targetX = Field.X + 1;
            if (targetX < 8)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }
        }

        //bottom
        targetY = Field.Y + 2;

        if (targetY < 8)
        {
            targetX = Field.X - 1;
            if (targetX >= 0)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }

            targetX = Field.X + 1;
            if (targetX < 8)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }
        }

        //left
        targetX = Field.X - 2;

        if (targetX >= 0)
        {
            targetY = Field.Y - 1;
            if (targetY >= 0)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }

            targetY = Field.Y + 1;
            if (targetY < 8)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }
        }

        //right
        targetX = Field.X + 2;

        if (targetX < 8)
        {
            targetY = Field.Y - 1;
            if (targetY >= 0)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }

            targetY = Field.Y + 1;
            if (targetY < 8)
            {
                if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                        possibleMoves++;

                possibleMoves++;
            }
        }

        if (possibleMoves > 0) EnableSelectOutline();

        return possibleMoves;
    }
}
