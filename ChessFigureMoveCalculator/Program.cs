namespace ChessFigureMoveCalculator
{
    /* 
     * TODO:
     * Also, maybe `ChessFigure` class needs to be moved closer to `Board` class as it actually
     * can't be deployed and even created without a board instance. Consequently, chess figure
     * will need to be tested in regard to the board it's placed upon or it's actually a great idea
     * to use mock objects here or simply test ChessFigure as the part of a Board test.
     * But it violates SRP as one test will be responsible for testing of two objects.
     * Or must be the best way to go about this is to create a Board instance just for the purpose
     * of creating a ChessFigure instance on it to test.
     * 
     * Consider transfering logic of `Board.Position.IsInBounds()` method to properties' setters.
     * And, consequently, change logic in `ChessFigure.MovesInBounds` (I'm not sure if it's
     * the only place where logic depends on this functionality) to depend on try-catch statement
     * that will catch moves that can't be created by adding detached moves to the current position
     * and therefore they won't be added to the return value of `MovesInBounds`. 
     * Need to complete documentation for `Board.Position` after I decide on that.
     * 
     * Read about records, structs and reflection.
     * 
     * Tighten up access modifiers.
     * 
     * Refactor classes to utilize partial methods.
     */

    class Program
    {
        static void Main()
        {
            //Board board = new();
            //board.PlaceFigure(Kind.Pawn, new(4, 0), out var pawn_1);
            //board.PlaceFigure(Kind.King, new(4, 1), out var king_1);
            //board.PlaceFigure(Kinds.King, new(5, 1), out var king_1);
            //board.PlaceFigure(Kind.Rook, new(3, 4), out var rook_1);
            //board.PlaceFigure(Kind.Knight, new(4, 8), out var knight_1);
            //board.PlaceFigure(Kind.Bishop, new(1, 2), out var bishop_1);
            //board.PlaceFigure(Kinds.Queen, new(2, 7), out var queen_1);
            //pawn_1.ShowPossibleMoves();
            //rook_1.ShowPossibleMoves();
            //king_1.ShowPossibleMoves();
            //bishop_1.ShowPossibleMoves();
            //queen_1.ShowPossibleMoves();
            //knight_1.ShowPossibleMoves();
            var board = new Board();
            board.PlaceFigure(Figure.Kinds.Knight, 6, 3, out var knight_1);
            board.PlaceFigure(Figure.Kinds.Rook, 6, 2, out var rook_1);
            knight_1.ShowPossibleMoves();
            knight_1.MoveTo(7, 5);
            rook_1.ShowPossibleMoves();
        }
    }
}
