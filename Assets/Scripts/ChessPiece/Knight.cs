using System;

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
            //top
            targetY = Field.Y - 2;

            if (targetY >= 0)
            {
                targetX = Field.X - 1;
                if (targetX >= 0)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }

                targetX = Field.X + 1;
                if (targetX < 8)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }
            }

            //bottom
            targetY = Field.Y + 2;

            if (targetY < 8)
            {
                targetX = Field.X - 1;
                if (targetX >= 0)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }

                targetX = Field.X + 1;
                if (targetX < 8)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }
            }

            //left
            targetX = Field.X - 2;

            if (targetX >= 0)
            {
                targetY = Field.Y - 1;
                if (targetY >= 0)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }

                targetY = Field.Y + 1;
                if (targetY < 8)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }
            }

            //right
            targetX = Field.X + 2;

            if (targetX < 8)
            {
                targetY = Field.Y - 1;
                if (targetY >= 0)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }

                targetY = Field.Y + 1;
                if (targetY < 8)
                {
                    index = Array.IndexOf(protectFields, virtualBoard[targetX, targetY]);
                    if (index != -1 & protectFields.Length > 0 || index == -1 & protectFields.Length == 0)
                    {
                        if (!CheckField(virtualBoard[targetX, targetY], enableFields))
                            if (virtualBoard[targetX, targetY].GetChessPiece().Team != Team & enableFields)
                                possibleMoves++;

                        possibleMoves++;
                    }
                }
            }
        }

        if (possibleMoves > 0 & enableFields) EnableSelectOutline();

        return possibleMoves;
    }
}
