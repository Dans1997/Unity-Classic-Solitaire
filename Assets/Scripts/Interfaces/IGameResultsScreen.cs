namespace Interfaces
{
    public interface IGameResultsScreen : IScreen
    {
        void SetGameResultLabel(string text);
        void SetScoreLabel(int gameResultsScore);
        void SetTimeLabel(float timeTaken);
        void SetTotalMovesLabel(int moves);
    }
}