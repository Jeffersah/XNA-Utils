using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class MicroParticle : BasicParticle
    {
        public MicroParticle(Vector2 point, Color color, move m2, int duration)
            : base(point, color, 1, m2, duration, 50, 1, 1)
        {
        }
    }
}