using System.Drawing;

namespace BombermanServer.Services
{
    public interface IEnemyMovementService
    {
        void UpdateGhostMovement();
        PointF GetGhostCoordinates();
        void KillGhost();
    }
}
