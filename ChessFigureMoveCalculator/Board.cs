using System;

// Note that algorithms for calculating moves are independent of the properties/sizes of the board
// on which the figures that implement them are placed. In other words, they functions of (math semantic) figures'
// positions rather than of boards' properties. Thus, sizes of a board can be changed programmatically with no fear.

namespace ChessFigureMoveCalculator
{
    /// <summary>
    ///     Chess board on which instances of <see cref="Figure"/> can be placed.
    /// </summary>
    /// <remarks>
    ///     <example> 
    ///         Usage:
    ///         <code>
    ///             var board = new Board();
    ///             var position = new Board.Position(3, 7);
    /// 
    ///             board.PlaceFigure(ChessFigure.Kind.Pawn, 4, 4, out var placedFigure);
    ///             board.PlaceFigure(ChessFigure.Kind.Pawn, position, out placedFigure);
    /// 
    ///             board.CellIsOcupied(position);
    /// 
    ///             board[position] = board[2, 0];
    ///         </code>
    ///     </example>
    /// </remarks>
    public class Board
    {
        public static int LowerBound => 0;
        public static int UpperBound => 8;


        readonly Figure[,] _board = new Figure[UpperBound, UpperBound];
        /// <summary>
        ///     Overload of <see cref="PlaceFigure(Figure.Kinds, Position, out Figure)"/>.
        /// </summary>
        /// <param name="figureKind">kind of figure based on which conrete instances are created.</param>
        /// <param name="x">position on X axis.</param>
        /// <param name="y">position on Y axis.</param>
        /// <param name="placedFigure">newly created figure placed on the board.</param>
        public void PlaceFigure(Figure.Kinds figureKind, int x, int y, out Figure placedFigure) => PlaceFigure(figureKind, new(x, y), out placedFigure);
        /// <summary>
        ///     Factory method that produces <see cref="Figure"/> objects and places them on the <see cref="Board"/>.
        /// </summary>
        /// <param name="figureKind">kind of figure based on which conrete instances are created.</param>
        /// <param name="position">initial position to assign the newly created figure to.</param>
        /// <param name="placedFigure">newly created figure placed on the board.</param>
        public void PlaceFigure(Figure.Kinds figureKind, Position position, out Figure placedFigure)
        {

            placedFigure = null;
            try
            {
                if (CellIsOccupied(position)) throw new ArgumentException("Board can not be initialized with two figures occupying the same cell.", nameof(position));
            }
            catch (IndexOutOfRangeException innerException) { throw new IndexOutOfRangeException("Chess figure can't be placed outside the board.", innerException); }

            placedFigure
                    = this[position]
                        = new Figure(board: this, position, figureKind);
        }
        /// <summary>
        ///     Checks if the cell at the given <paramref name="position"/> is occupied.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>
        ///     <c>true</c> if the cell is occupied, <c>false</c> otherwise.
        /// </returns>
        public bool CellIsOccupied(Position position) => this[position] != null;


        public Figure this[Position position]
        {
            get => this[position.X, position.Y];
            private set => this[position.X, position.Y] = value;
        }
        public Figure this[int x, int y]
        {
            get => _board[x, y];
            private set => _board[x, y] = value;
        }


        /// <summary>
        ///     Coordinate on the <see cref="Board"/>.
        /// </summary>
        /// <remarks>
        ///     <para>Instances of this class can be addedd to and subtracted from each other. It's used in calculation of moves for figures in relation to their positions.<br/>
        ///     (the mechanic on which the program's inner implementations are based). <see cref="object.ToString"/> method is overriden, as well.</para>
        ///     <example>
        ///         Example:
        ///         <code>
        ///             var position = new Board.Position(Board.LowerBound, Board.UpperBound - 1);
        ///             bool @true = @true = position.ToString() == "(0, 7)";
        ///             @true = position.IsInBounds;
        ///             @true = position == new Board.Position(3, 4) + new Board.Position(2, 3) - new Board.Position(5, 0);
        ///         </code>
        ///     </example>
        /// </remarks>
        /// <param name="X">position on X axis.</param>
        /// <param name="Y">position on Y axis.</param>
        public record Position(int X, int Y)
        {
            public bool IsInBounds =>
                (X >= LowerBound && X < UpperBound)
                && (Y >= LowerBound && Y < UpperBound);

            /// <summary>
            ///     Override of <see cref="object.ToString"/> method.
            /// </summary>
            /// <returns>
            ///     String representation of the instance in format "(X, Y)"
            /// </returns>
            public override string ToString() => $"({X+1}, {Y+1})";
            /// <summary>
            ///     Adds two <see cref="Position"/> instances.
            /// </summary>
            /// <param name="this_">the augent</param>
            /// <param name="other">the addend</param>
            /// <returns>
            ///     New <see cref="Position"/> instance created as the result of a value-based addition.
            /// </returns>
            public static Position operator +(Position this_, Position other) => new(this_.X + other.X, this_.Y + other.Y);
            /// <summary>
            ///     Subtracts two <see cref="Position"/> instances.
            /// </summary>
            /// <param name="this_">the minuend</param>
            /// <param name="other">the subtrahend</param>
            /// <returns>
            ///     New <see cref="Position"/> instance created as the result of a value-based subtraction.
            /// </returns>
            public static Position operator -(Position this_, Position other) => new(this_.X - other.X, this_.Y - other.Y);
        }
    }
}
