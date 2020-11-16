using System.Threading.Tasks;

namespace BombermanServer.Services
{
    public interface IEnemyMovementService
    {
        Task UpdateGhostMovement();
    }
}
