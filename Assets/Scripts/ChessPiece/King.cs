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
            if(PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X + 1;
        targetY = Field.Y + 1;

        if (targetX < 8 & targetY < 8)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X - 1;
        targetY = Field.Y + 1;

        if (targetX >= 0 & targetY < 8)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X + 1;
        targetY = Field.Y - 1;

        if (targetX < 8 & targetY >= 0)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;


        targetX = Field.X - 1;
        targetY = Field.Y;

        if (targetX >= 0)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X + 1;
        targetY = Field.Y;

        if (targetX < 8)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X;
        targetY = Field.Y + 1;

        if (targetY < 8)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        targetX = Field.X;
        targetY = Field.Y - 1;

        if (targetY >= 0)
            if (PreventCheck(virtualBoard, targetX, targetY))
                if (CheckField(virtualBoard[targetX, targetY], enableFields)) possibleMoves++;

        if (possibleMoves > 0) EnableSelectOutline();

        return possibleMoves;
    }

    protected override bool CheckField(BoardField field, bool enableFields)
    {
        ChessPiece piece = field.GetChessPiece();

        if (piece)
        {
            if (piece.Team != Team & enableFields) field.Enable();
            return false;
        }

        if (enableFields) field.Enable();
        return true;
    }

    private bool PreventCheck(BoardField[,] virtualBoard, int x, int y)
    {
        //horizontal & vertical
        int targetX, targetY;

        for (int i = x - 1; i >= 0; i--)
        {
            ChessPiece piece = virtualBoard[i, y].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == Field.X - 1))
                    {
                        return false;
                    }
                }

                if (i == Field.X) continue;

                break;
            }
        }

        for (int i = x + 1; i < 8; i++)
        {
            ChessPiece piece = virtualBoard[i, y].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == Field.X - 1))
                    {
                        return false;
                    }
                }

                if (i == Field.X) continue;

                break;
            }
        }

        for (int i = y - 1; i >= 0; i--)
        {
            ChessPiece piece = virtualBoard[x, i].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == Field.X - 1))
                    {
                        return false;
                    }
                }

                if (i == Field.Y) continue;

                break;
            }
        }

        for (int i = y + 1; i < 8; i++)
        {
            ChessPiece piece = virtualBoard[x, i].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Rook || piece.PieceType == PieceType.Queen || (piece.PieceType == PieceType.King & i == Field.X - 1))
                    {
                        return false;
                    }
                }

                if (i == Field.Y) continue;

                break;
            }
        }

        //diagonals

        targetX = x - 1;
        targetY = y + 1;

        for (int i = 2; targetX >= 0 & targetY < 8; i++)
        {
            ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        return false;
                    }
                }

                if (targetX == Field.X & targetY == Field.Y)
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
            ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        return false;
                    }
                }

                if (targetX == Field.X & targetY == Field.Y)
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
            ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        return false;
                    }
                }

                if (targetX == Field.X & targetY == Field.Y)
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
            ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

            if (piece)
            {
                if (Team != piece.Team)
                {
                    if (piece.PieceType == PieceType.Bishop || piece.PieceType == PieceType.Queen ||
                        (piece.PieceType == PieceType.King & i == 2) || (piece.PieceType == PieceType.Pawn & i == 2))
                    {
                        return false;
                    }
                }

                if (targetX == Field.X & targetY == Field.Y)
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
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }

            targetX = x + 1;
            if (targetX < 8)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }
        }

        targetY = y + 2;

        if (targetY < 8)
        {
            targetX = x - 1;
            if (targetX >= 0)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }

            targetX = x + 1;
            if (targetX < 8)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }
        }

        targetX = x - 2;

        if (targetX >= 0)
        {
            targetY = y - 1;
            if (targetY >= 0)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }

            targetY = y + 1;
            if (targetY < 8)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }
        }

        targetX = x + 2;

        if (targetX < 8)
        {
            targetY = y - 1;
            if (targetY >= 0)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }

            targetY = y + 1;
            if (targetY < 8)
            {
                ChessPiece piece = virtualBoard[targetX, targetY].GetChessPiece();

                if (piece)
                    if (Team != piece.Team)
                        if (piece.PieceType == PieceType.Knight)
                            return false;
            }
        }

        return true;
    }
}
