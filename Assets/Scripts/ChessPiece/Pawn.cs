public class Pawn : ChessPiece
{
    public override int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        int possibleMoves = 0;
        int targetX, targetY;

        targetY = Team == TeamColor.White ? (Field.Y - 1) : (Field.Y + 1);
        targetX = Field.X;

        if (Team == TeamColor.White & targetY >= 0 || Team == TeamColor.Black & targetY < 8)
        {
            if (CountMoves == 0 & CheckField(virtualBoard[targetX, targetY], enableFields))
            {
                possibleMoves++;
                targetY = Team == TeamColor.White ? (Field.Y - 2) : (Field.Y + 2);

                if (Team == TeamColor.White & targetY >= 0 || Team == TeamColor.Black & targetY < 8)
                {
                    if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;
                }

                targetY = Team == TeamColor.White ? (Field.Y - 1) : (Field.Y + 1);
            }

            targetX = Field.X - 1;

            if (targetX >= 0)
                if (CheckAttackField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

            targetX = Field.X + 1;

            if (targetX < 8)
                if (CheckAttackField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;
        }

        if (possibleMoves > 0 & enableFields) EnableSelectOutline();

        return possibleMoves;
    }

    protected override bool CheckField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();

        if (piece) return false;

        if (enableFields) field.Enable();
        return true;
    }

    private bool CheckAttackField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();
        
        if (piece) 
            if (piece.Team != Team & enableFields) field.Enable();

        return false;
    }
}
