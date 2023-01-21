public class Rook : ChessPiece
{
    private void Start()
    {
        PieceType = PieceType.Rook;
    }

    public override int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        int possibleMoves = 0;

        int x = Field.X;
        int y = Field.Y;

        //check left
        for (int i = x - 1; i >= 0; i--)
        {
            if (!CheckField(virtualBoard[i, y], enableFields))
            {
                if (virtualBoard[i, y].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                break;
            }

            possibleMoves++;
        }

        //check right
        for (int i = x + 1; i <= 7; i++)
        {
            if (!CheckField(virtualBoard[i, y], enableFields))
            {
                if (virtualBoard[i, y].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                break;
            }

            possibleMoves++;
        }

        //check top
        for (int i = y - 1; i >= 0; i--)
        {
            if (!CheckField(virtualBoard[x, i], enableFields))
            {
                if (virtualBoard[x, i].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                break;
            }

            possibleMoves++;
        }

        //check bottom
        for (int i = y + 1; i <= 7; i++)
        {
            if (!CheckField(virtualBoard[x, i], enableFields))
            {
                if (virtualBoard[x, i].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                break;
            }

            possibleMoves++;
        }

        if (possibleMoves > 0) EnableSelectOutline();

        return possibleMoves;
    }
}
