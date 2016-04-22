using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public class BasicParticle : P_Entity
    {
        public static Random random = new Random();
        private static Texture2D image;
        private static Texture2D micro;
        private Color c;
        private float size;

        public delegate Vector2 move(Vector2 loc);

        public move m;
        protected int deathtime;

        public static void LoadContent(ContentManager cm)
        {
            image = cm.Load<Texture2D>("Package/p");
            micro = cm.Load<Texture2D>("Package/mp");
        }

        public BasicParticle(Vector2 point, Color color, float scale, move m2, int duration)
        {
            loc = point;
            c = color;
            size = scale;
            setImg(image);
            setupAnimation(10, 10, duration, false);
            deathtime = duration * 5;
            m = m2;
        }

        public BasicParticle(Vector2 point, Color color, float scale, move m2, int duration, int numFrames, int sizex, int sizey)
        {
            loc = point;
            c = color;
            size = scale;
            setImg(micro);
            setupAnimation(1, 1, duration, false);
            deathtime = duration * numFrames;
            m = m2;
        }

        public virtual void update()
        {
            loc = m(loc);
            deathtime--;
        }

        public void Draw(SpriteBatch sb)
        {
            base.DrawTint(sb, 0, size, new Vector2(0, 0), 0, c);
        }

        public static bool pred(BasicParticle b)
        {
            return b.done || b.deathtime == 0;
        }
    }

    public class SingleVarParticle : BasicParticle
    {
        public delegate Vector2 SVMove(Vector2 loc, ref float var);

        private SVMove movePattern;

        private float var1;

        public SingleVarParticle(Vector2 point, Color color, float scale, SVMove m2, int duration, float var1)
            : base(point, color, scale, null, duration)
        {
            this.var1 = var1;
            movePattern = m2;
        }

        public override void update()
        {
            loc = movePattern(loc, ref var1);
            deathtime--;
        }
    }

    public class DoubleVarParticle : BasicParticle
    {
        public delegate Vector2 DVMove(Vector2 loc, ref float var, ref float var2);

        private DVMove movePattern;

        private float var1;
        private float var2;

        public DoubleVarParticle(Vector2 point, Color color, float scale, DVMove m2, int duration, float var1, float var2)
            : base(point, color, scale, null, duration)
        {
            this.var1 = var1;
            this.var2 = var2;
            movePattern = m2;
        }

        public override void update()
        {
            loc = movePattern(loc, ref var1, ref var2);
            deathtime--;
        }
    }

    public class ManyVarParticle : BasicParticle
    {
        public delegate Vector2 MVMove(Vector2 loc, List<float> reqvar);

        private MVMove movePattern;

        private List<float> reqvar;

        public ManyVarParticle(Vector2 point, Color color, float scale, MVMove m2, int duration, List<float> var)
            : base(point, color, scale, null, duration)
        {
            reqvar = new List<float>();
            foreach (float f in var)
                reqvar.Add(f);
            movePattern = m2;
        }

        public override void update()
        {
            loc = movePattern(loc, reqvar);
            deathtime--;
        }
    }
}