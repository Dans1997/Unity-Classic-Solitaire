namespace Interfaces
{
    public interface IUserDataTracker
    {
        void TrackGameStart(float gameStartTime);
        void TrackGameEnd(IGameResults gameResults);
        void TrackCardMove(IGameMode gameMode, IGameMove gameMove, int moveNumber);
        void TrackUndoMove(IGameMode gameMode, IGameMove gameMove, int moveNumber);
        void TrackHintUsed(int hintCount, float gameStartTime);
    }
}