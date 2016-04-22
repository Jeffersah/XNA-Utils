using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public class ParticleManager2
    {
        public static Stack<Particle2> ParticlePool;
        public static bool enablePooling;
        public static int maxPool;

        static ParticleManager2()
        {
            ParticlePool = new Stack<Particle2>();
            enablePooling = true;
            maxPool = 400;
        }

        public static ColorSet COLORSET_FIRE = new ColorSet(new Color[] { Color.Red, Color.Red, Color.Red, Color.OrangeRed, Color.OrangeRed, Color.Orange, Color.Yellow, Color.Yellow, Color.Yellow, Color.LightYellow });

        public List<Particle2> Particles;

        public List<ParticleEffect2> ActiveEffects;

        public ParticleManager2()
        {
            Particles = new List<Particle2>();
            ActiveEffects = new List<ParticleEffect2>();
        }

        public void update()
        {
            foreach (ParticleEffect2 pe in ActiveEffects)
            {
                Particle2[] temp = pe.update();
                if (temp != null && temp.Length > 0)
                {
                    Particles.AddRange(temp);
                }
            }
            foreach (Particle2 p in Particles)
                p.update();

            ActiveEffects.RemoveAll(x => !x.isActive);
            int idx = Particles.Count - 1;
            if (enablePooling && ParticlePool.Count < maxPool)
            {
                for (idx = Particles.Count - 1; idx >= 0 && ParticlePool.Count < maxPool; idx--)
                {
                    if (!Particles[idx].isActive)
                    {
                        ParticlePool.Push(Particles[idx]);
                        Particles.RemoveAt(idx);
                    }
                }
            }
            while (idx >= 0)
            {
                if (!Particles[idx].isActive)
                {
                    Particles.RemoveAt(idx);
                }
                idx--;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle2 p in Particles)
                p.Draw(sb);
        }
    }

    public class ParticleEffect2
    {
        public delegate Particle2[] UpdateEffect(ParticleEffect2 effect);

        public bool isActive;
        public int time;
        public object[] param;
        public UpdateEffect myUpdate;

        public ParticleEffect2(UpdateEffect updatemethod, object[] param)
        {
            this.param = param;
            myUpdate = updatemethod;
            time = 0;
            isActive = true;
        }

        public ParticleEffect2(UpdateEffect ue)
            : this(ue, null)
        {
        }

        public Particle2[] update()
        {
            time++;
            return myUpdate(this);
        }
    }

    public class Particle2
    {
        public static void Update_velocity(Particle2 particle)
        {
            particle.position.X += particle.velocity.X;
            particle.position.Y += particle.velocity.Y;
            if ((particle.time-=2)<= 0)
                particle.isActive = false;
        }

        public delegate void UpdateMethod(Particle2 particle);

        public delegate void OverDrawMethod(Particle2 particle, SpriteBatch sb);

        public Vector2 position;
        public Vector2 velocity;
        public int time;
        public int size;
        public UpdateMethod myUpdate;
        public OverDrawMethod DrawOver;
        public OverDrawMethod DrawUnder;
        public Color c;
        public bool isActive;

        public Particle2(Vector2 pos, Vector2 vel, UpdateMethod mymeth, Color c, int s)
        {
            UNPOOL(pos, vel, mymeth, c, s);
        }

        public static Particle2 GetNew(Vector2 pos, Vector2 vel, UpdateMethod mymeth, Color c, int s)
        {
            if (ParticleManager2.ParticlePool.Count > 0)
            {
                return ParticleManager2.ParticlePool.Pop().UNPOOL(pos, vel, mymeth, c, s);
            }
            return new Particle2(pos, vel, mymeth, c, s);
        }

        internal Particle2 UNPOOL(Vector2 pos, Vector2 vel, UpdateMethod mymeth, Color c, int s)
        {
            position = pos;
            velocity = vel;
            myUpdate = mymeth;
            time = 0;
            this.c = c;
            size = s;
            isActive = true;
            return this;
        }

        public void update()
        {
            myUpdate(this);
            time++;
        }

        public void Draw(SpriteBatch sb)
        {
            if (DrawUnder != null)
                DrawUnder(this, sb);
            Camera.drawGeneric(sb, MemSave.getr((int)position.X, (int)position.Y, size, size), c, null, 0, Vector2.Zero, SpriteEffects.None, 0);
            if (DrawOver != null)
                DrawOver(this, sb);
        }
    }
}