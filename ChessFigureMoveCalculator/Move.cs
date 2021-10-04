using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChessFigureMoveCalculator
{
    /// <summary>
    ///     Empty abstract base class for all moves.
    /// </summary>
    /// <remarks>
    ///     Declared for documenting purposes.
    /// </remarks>
    public abstract class Move { }
    /// <summary>
    ///     <see cref="Move"/> that comprises steps (<see cref="List{T}"/> of <see cref="Board.Position"/>) not bound to any <see cref="Figure"/>.
    /// </summary>
    public partial class DetachedMove : Move, IEnumerable<Board.Position>
    {
        List<Board.Position> Steps { get; set; }
        /// <summary>
        ///     Destination <see cref="Board.Position"/> of the <see cref="Move"/>.
        /// </summary>
        public Board.Position EndPoint { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="DetachedMove"/> out of <paramref name="steps"/>.
        /// </summary>
        /// <param name="steps">collection of <see cref="Board.Position"/> representing steps that will make up the move.</param>
        /// <exception cref="ArgumentException"></exception>
        public DetachedMove(IEnumerable<Board.Position> steps)
        {
            if (!steps.Any()) throw new ArgumentException
                    ($"{this.GetType().Name} can't be constructed from empty sequence of steps.", nameof(steps));


            Steps = steps.ToList();
            EndPoint = Steps.Last();
        }
        /// <summary>
        ///     Initializes a new instance of the <see cref="DetachedMove"/> out of a single <paramref name="step"/>.
        /// </summary>
        /// <param name="step"></param>
        public DetachedMove(Board.Position step) : this(new List<Board.Position>(1) { step }) { }


        /// <summary>
        ///     Checks if the <see cref="Move"/> is blocked on the <paramref name="board"/> by another <see cref="Figure"/>.
        /// </summary>
        /// <returns>
        ///     If the <see cref="Move"/> is blocked by another <see cref="Figure"/> return <c>true</c>, <c>false</c> otherwise.
        /// </returns>
        public bool IsBlockedOn(Board board)
        {
            foreach (var step in Steps) if (board.CellIsOccupied(step)) return true;
            return false;
        }


        /// <summary>
        ///     <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        /// <returns>
        ///     <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        /// </returns>
        public IEnumerator<Board.Position> GetEnumerator() => new StepsEnumerator(Steps);
        /// <summary>
        ///     <inheritdoc cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        /// <returns>
        ///     <inheritdoc cref="IEnumerable.GetEnumerator"/>
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => new StepsEnumerator(Steps);


        class StepsEnumerator : IEnumerator<Board.Position>
        {
            int _pointer = -1;
            readonly List<Board.Position> _steps;

            internal StepsEnumerator(IEnumerable<Board.Position> steps) => _steps = steps.ToList();


            public Board.Position Current => _steps[_pointer];

            object IEnumerator.Current => _steps[_pointer];

            public bool MoveNext()
            {
                _pointer++;
                return _pointer < _steps.Count;
            }

            public void Reset() => _pointer = -1;

            public void Dispose() { }
        }
    }

    /// <summary>
    ///     <see cref="Move"/> with <see cref="InitialPosition"/> of the <see cref="Figure"/> it's bound to.
    /// </summary>
    public class RelativeMove : DetachedMove
    {
        Board.Position InitialPosition { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="RelativeMove"/> bound to <paramref name="initialPosition"/> of a <see cref="Figure"/>
        ///     from collection of <paramref name="steps"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="initialPosition"></param>
        public RelativeMove(IEnumerable<Board.Position> steps, Board.Position initialPosition) : base(steps) => InitialPosition = initialPosition;


        public override string ToString() => $"{InitialPosition} => {EndPoint}";
    }
}
