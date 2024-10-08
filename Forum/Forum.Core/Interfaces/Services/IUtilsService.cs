using Forum.Models;

namespace Forum.Core.Interfaces.Services
{
    public interface IUtilsService
    {
        Task<StatisticsDto> GetStatistics();
    }
}
