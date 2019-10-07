using System;

namespace NameGame.Service.Services.Interfaces
{
    public interface IStatisticsService
    {
        int GetLastAttempts(string userName);
        TimeSpan? HowLongDidLastChallengeTake(string userName);
    }
}
