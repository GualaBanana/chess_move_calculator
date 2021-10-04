using System;
using System.Collections.Generic;

namespace ChessFigureMoveCalculator
{
    public partial class DetachedMove
    {
        /// <summary>
        ///     Gets steps for constructing of <see cref="DetachedMove"/> for each <see cref="Figure.Kinds"/> that need to be added to the <see cref="Board.Position"/> of a <see cref="Figure"/>.
        /// </summary>
        public static class Getter
        {
            /// <summary>
            ///     Factory method for creating <see cref="List{T}"/> of <see cref="DetachedMove"/> for the given <see cref="Figure.Kinds"/>.
            /// </summary>
            /// <param name="figureKind">figure kind for which moves are needed.</param>
            /// <returns>
            ///     <see cref="List{T}"/> of <see cref="DetachedMove"/> to add to the <see cref="Board.Position"/> of <see cref="Figure"/>.
            /// </returns>
            public static IEnumerable<DetachedMove> For(Figure.Kinds figureKind) => figureKind switch
            {
                Figure.Kinds.Pawn => ForPawn,
                Figure.Kinds.Bishop => ForBishop,
                Figure.Kinds.Knight => ForKnight,
                Figure.Kinds.Rook => ForRook,
                Figure.Kinds.Queen => ForQueen,
                Figure.Kinds.King => ForKing,
                _ => throw new ArgumentOutOfRangeException(nameof(figureKind), "There is no appropriate legal moves for non-existent kind of figure.")
            };


            static readonly Board.Position forward = new(+0, +1);
            static readonly Board.Position right = new(+1, +0);
            static readonly Board.Position back = new(+0, -1);
            static readonly Board.Position left = new(-1, +0);


            static List<DetachedMove> VerticalAndHorizontalMoves
            {
                get
                {
                    var detachedMoves = new List<DetachedMove>();

                    // These variables are needed to collect incrementing number of steps that constitute a single move
                    // that all differ only in the last added DetachedStep that is added to these steps and immedidatelly added as a new move to `detachedMoves`.
                    var detachedStepsForMovingForward = new List<Board.Position>(Board.UpperBound);
                    var detachedStepsForMovingRight = new List<Board.Position>(Board.UpperBound);
                    var detachedStepsForMovingBack = new List<Board.Position>(Board.UpperBound);
                    var detachedStepsForMovingLeft = new List<Board.Position>(Board.UpperBound);

                    for (int i = 1; i < Board.UpperBound; ++i)
                    {
                        detachedStepsForMovingForward.Add(forward with { Y = forward.Y * i });
                        detachedStepsForMovingRight.Add(right with { X = right.X * i });
                        detachedStepsForMovingBack.Add(back with { Y = back.Y * i });
                        detachedStepsForMovingLeft.Add(left with { X = left.X * i });

                        detachedMoves.Add(new(detachedStepsForMovingForward));
                        detachedMoves.Add(new(detachedStepsForMovingRight));
                        detachedMoves.Add(new(detachedStepsForMovingBack));
                        detachedMoves.Add(new(detachedStepsForMovingLeft));
                    }

                    return detachedMoves;
                }
            }
            static List<DetachedMove> DiagonalMoves
            { 
                get
                {
                    var detachedMoves = new List<DetachedMove>();

                    var detachedStepsForMovingForwardRight = new List<Board.Position>(Board.UpperBound);
                    var detachedStepsForMovingBackRight = new List<Board.Position>(Board.UpperBound);
                    var detachedStepsForMovingBackLeft = new List<Board.Position>(Board.UpperBound);
                    var detachedStepsForMovingForwardLeft = new List<Board.Position>(Board.UpperBound);

                    for (int i = 1; i < Board.UpperBound; ++i)
                    {
                        detachedStepsForMovingForwardRight.Add(forward + right with { X = right.X * i, Y = forward.Y * i });
                        detachedStepsForMovingBackRight.Add(back + right with { X = right.X * i, Y = back.Y * i });
                        detachedStepsForMovingBackLeft.Add(back + left with { X = left.X * i, Y = back.Y * i });
                        detachedStepsForMovingForwardLeft.Add(forward + left with { X = left.X * i, Y = forward.Y * i });

                        detachedMoves.Add(new(detachedStepsForMovingForwardRight));
                        detachedMoves.Add(new(detachedStepsForMovingBackRight));
                        detachedMoves.Add(new(detachedStepsForMovingBackLeft));
                        detachedMoves.Add(new(detachedStepsForMovingForwardLeft));
                    }

                    return detachedMoves;
                }
            }


            static List<DetachedMove> ForPawn => new(1) { new(forward) };
            static List<DetachedMove> ForBishop => DiagonalMoves;
            static List<DetachedMove> ForKnight => new()
                {
                    new(forward + forward + right),
                    new(forward + forward + left),
                    new(right + right + forward),
                    new(right + right + back),
                    new(back + back + right),
                    new(back + back + left),
                    new(left + left + forward),
                    new(left + left + back)
                };
            static List<DetachedMove> ForRook => VerticalAndHorizontalMoves;
            static List<DetachedMove> ForQueen
            {
                get
                {
                    var detachedMoves = new List<DetachedMove>();
                    detachedMoves.AddRange(VerticalAndHorizontalMoves);
                    detachedMoves.AddRange(DiagonalMoves);
                    return detachedMoves;
                }
            }
            static List<DetachedMove> ForKing => new List<DetachedMove>(8)
            {
                new(forward),
                new(forward + right),
                new(right),
                new(back + right),
                new(back),
                new(back + left),
                new(left),
                new(forward + left),
            };
        }
    }
}
