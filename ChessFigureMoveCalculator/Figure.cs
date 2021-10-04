using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessFigureMoveCalculator
{
    /// <summary>
    ///     Figure on a <see cref="Board">Board</see>.
    /// </summary>
    public class Figure
    {
        Kinds Kind { get; }
        Board Board { get; }
        Board.Position Position { get; set; }
        /// <summary>
        ///     List of <see cref="RelativeMove"/> in bounds of the <see cref="Board">Board</see>.
        /// </summary>
        /// <remarks>
        ///     Determines whether the move is in bounds by traversing each step of each single <see cref="DetachedMove"/> (taken from <see cref="DetachedMove.Getter"/>) <br/>
        ///     and adding these to the <see cref="Position">Position</see> of the figure.
        /// </remarks>
        List<RelativeMove> MovesInBounds
        {
            get
            {
                List<RelativeMove> properMovesInBounds = new();

                foreach (DetachedMove deconstructedDetachedMove in DetachedMove.Getter.For(Kind))
                {
                    bool moveShouldBeAdded = true;
                    List<Board.Position> stepsRelativeToCurrentPosition = new();

                    foreach (var detachedStep in deconstructedDetachedMove)
                    {
                        var positionAfterMakingStep = Position + detachedStep;
                        if (!positionAfterMakingStep.IsInBounds)
                        {
                            moveShouldBeAdded = false;
                            break;
                        }
                        stepsRelativeToCurrentPosition.Add(positionAfterMakingStep);
                    }
                    if (moveShouldBeAdded) properMovesInBounds.Add(new RelativeMove(stepsRelativeToCurrentPosition, Position));
                }
                return properMovesInBounds;
            }
        }
        /// <summary>
        ///     Calculates the moves the figure can legally make at the given circumstances.
        /// </summary>
        /// <remarks>
        ///     Filters <see cref="MovesInBounds">MovesInBound</see> discarding moves that are blocked by other figures on the board.
        /// </remarks>
        List<RelativeMove> PossibleMoves => MovesInBounds.Where(move => !move.IsBlockedOn(this.Board)).ToList();


        /// <summary>
        ///     Initializes a new instance of the <see cref="Figure"/> of <paramref name="figureKind"/> on the given
        ///     <paramref name="board"/> at <paramref name="initialPosition"/>.
        /// </summary>
        /// <param name="board">board on which the figure needs to be placed.</param>
        /// <param name="initialPosition">initial position on which the figure needs to be placed.</param>
        /// <param name="figureKind">kind of the figure needed to be placed.</param>
        public Figure(Board board, Board.Position initialPosition, Kinds figureKind)
        {
            Board = board;
            Position = initialPosition;
            Kind = figureKind;
        }


        /// <summary>
        ///     Overload of <see cref="MoveTo(Board.Position)"/>
        /// </summary>
        /// <param name="x">position on X axis.</param>
        /// <param name="y">position on Y axis.</param>
        /// <returns>
        ///     <inheritdoc cref="MoveTo(Board.Position)"/>
        /// </returns>
        public bool MoveTo(int x, int y) => MoveTo(new(x, y));
        /// <summary>
        ///     Move a <see cref="Figure"/> at the given <paramref name="destinationPosition"/> to any other unoccupied one.
        /// </summary>
        /// <param name="destinationPosition"><see cref="Board.Position"/> to move the figure to.</param>
        /// <returns>
        ///     Success of the move.
        /// </returns>
        public bool MoveTo(Board.Position destinationPosition)
        {
            if (destinationPosition.IsInBounds && PossibleMoves.Any(move => destinationPosition == move.EndPoint))
            {
                Position = destinationPosition;
                return true;
            }
            return false;
        }
        /// <summary>
        ///     Show the moves returned from <see cref="PossibleMoves"/>.
        /// </summary>
        public void ShowPossibleMoves()
        {
            Console.WriteLine($"{Kind} : {Position}");

            int counter = 1;
            foreach (var move in PossibleMoves)
            {
                Console.WriteLine($"{counter++}. {move}");
            }
            Console.WriteLine();
        }


        /// <summary>
        ///     All available <see cref="Figure"/> kinds.
        /// </summary>
        public enum Kinds
        {
            Pawn,
            Bishop,
            Knight,
            Rook,
            Queen,
            King
        }
    }
}
