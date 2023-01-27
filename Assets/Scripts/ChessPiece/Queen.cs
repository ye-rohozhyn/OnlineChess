using System;

public class Queen : ChessPiece
{
    private void Start()
    {
        PieceType = PieceType.Queen;
    }

    public override int GetPossibleMoves(BoardField[,] virtualBoard, bool enableFields)
    {
        int possibleMoves = 0;
        BoardField[] protectFields = GameManager.singleton.GetProtectFields().ToArray();
        int index;
        ChessPiece king = GameManager.singleton.FindKing(Team);
        int checkCount = GameManager.singleton.GetCheckCount(king.Field.X, king.Field.Y).Count;

        if (!PreventCheck(virtualBoard, Field.X, Field.Y) & checkCount == 0)
        {
            if (ProtectDirection.Count == 0) return 0;

            foreach (BoardField field in ProtectDirection)
            {
                if (!CheckField(field, enableFields))
                    if (enableFields) possibleMoves++;
            }
        }
        else
        {
            int targetX, targetY;
            
            //check left
            for (int i = Field.X - 1; i >= 0; i--)
            {
                index = Array.IndexOf(protectFields, virtualBoard[i, Field.Y]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[i, Field.Y], enableFields))
                    {
                        if (virtualBoard[i, Field.Y].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }
            }

            //check right
            for (int i = Field.X + 1; i <= 7; i++)
            {
                index = Array.IndexOf(protectFields, virtualBoard[i, Field.Y]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[i, Field.Y], enableFields))
                    {
                        if (virtualBoard[i, Field.Y].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }
            }

            //check top
            for (int i = Field.Y - 1; i >= 0; i--)
            {
                index = Array.IndexOf(protectFields, virtualBoard[Field.X, i]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[Field.X, i], enableFields))
                    {
                        if (virtualBoard[Field.X, i].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }
            }

            //check bottom
            for (int i = Field.Y + 1; i <= 7; i++)
            {
                index = Array.IndexOf(protectFields, virtualBoard[Field.X, i]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[Field.X, i], enableFields))
                    {
                        if (virtualBoard[Field.X, i].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }
            }

            //check bottom left
            targetX = Field.X - 1;
            targetY = Field.Y + 1;

            for (int i = 2; targetX >= 0 & targetY < 8; i++)
            {
                index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    {
                        if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }

                targetX = Field.X - i;
                targetY = Field.Y + i;
            }

            //check top right
            targetX = Field.X + 1;
            targetY = Field.Y - 1;

            for (int i = 2; targetX < 8 & targetY >= 0; i++)
            {
                index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    {
                        if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }

                targetX = Field.X + i;
                targetY = Field.Y - i;
            }

            //check bottom right
            targetX = Field.X + 1;
            targetY = Field.Y + 1;

            for (int i = 2; targetX < 8 & targetY < 8; i++)
            {
                index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    {
                        if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }

                targetX = Field.X + i;
                targetY = Field.Y + i;
            }

            //check top left
            targetX = Field.X - 1;
            targetY = Field.Y - 1;

            for (int i = 2; targetX >= 0 & targetY >= 0; i++)
            {
                index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                {
                    if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                    {
                        if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields) possibleMoves++;
                        break;
                    }

                    possibleMoves++;
                }

                targetX = Field.X - i;
                targetY = Field.Y - i;
            }
        }

        if (possibleMoves > 0 & enableFields) EnableSelectOutline();

        return possibleMoves;
    }
}
