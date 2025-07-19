// File: Interfaces/INotificationService.cs
using System.Threading.Tasks;
using MovieWebsite.Models;

namespace MovieWebsite.Interfaces
{
    public interface INotificationService
    {
        Task CreateNewEpisodeNotificationAsync(Episode newEpisode);
    }
}