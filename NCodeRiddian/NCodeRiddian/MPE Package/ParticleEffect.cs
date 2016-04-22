using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public class ParticleEffect
    {
        private List<BasicParticle> bpl;
        private static Texture2D gImage;
        private bool glow;
        private P_Entity glowE;
        private float scale;

        public static void LoadContent(ContentManager cm)
        {
            gImage = cm.Load<Texture2D>("Package/Glow");
        }

        public void SetupGlow(Vector2 position, float scale, bool isActive)
        {
            glowE.loc = new Vector2(position.X - (50 * scale), (position.Y - (50 * scale)));
            this.scale = scale;
            glow = isActive;
            glowE.setImg(gImage);
        }

        public void setGlowing(bool glowState)
        {
            glow = glowState;
        }

        public ParticleEffect()
        {
            bpl = new List<BasicParticle>();
            glowE = new P_Entity();
            glowE.setImg(gImage);
        }

        public int num()
        {
            return bpl.Count;
        }

        public void update()
        {
            foreach (BasicParticle b in bpl)
            {
                b.update();
            }
            bpl.RemoveAll(BasicParticle.pred);
        }

        public void Add(BasicParticle bp)
        {
            bpl.Add(bp);
        }

        public void draw(SpriteBatch sb)
        {
            if (glow)
            {
                if (glowE.getImage() == null)
                    glowE.setImg(gImage);
                glowE.DrawAll(sb, 0, scale);
            }

            foreach (BasicParticle b in bpl)
            {
                b.Draw(sb);
            }
        }
    }
}