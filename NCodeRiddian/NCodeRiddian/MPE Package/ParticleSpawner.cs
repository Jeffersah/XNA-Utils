using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PhysicsLib;

namespace NCodeRiddian
{
    public static class ParticleSpawner
    {
        private static BasicParticle.move RandomMove;
        private static BasicParticle.move FireMove;
        private static BasicParticle.move SmokeMove;
        private static bool setupd = false;

        public static Vector2 random_move(Vector2 loc)
        {
            switch (BasicParticle.random.Next(5))
            {
                case 0:
                    return loc;
                case 1:
                    return new Vector2(loc.X + 2, loc.Y);
                case 2:
                    return new Vector2(loc.X - 2, loc.Y);
                case 3:
                    return new Vector2(loc.X, loc.Y + 2);
                case 4:
                    return new Vector2(loc.X, loc.Y - 2);
                default:
                    return loc;
            }
        }

        public static Vector2 fire_move(Vector2 loc)
        {
            switch (BasicParticle.random.Next(4))
            {
                case 0:
                    return loc;
                case 1:
                    return new Vector2(loc.X + 1, loc.Y);
                case 2:
                    return new Vector2(loc.X - 1, loc.Y);
                case 3:
                    return new Vector2(loc.X, loc.Y - 3);
                default:
                    return loc;
            }
        }

        public static Vector2 smoke_move(Vector2 loc)
        {
            switch (BasicParticle.random.Next(4))
            {
                case 0:
                    return loc;
                case 1:
                    return new Vector2(loc.X + 1, loc.Y);
                case 2:
                    return new Vector2(loc.X - 1, loc.Y);
                case 3:
                    return new Vector2(loc.X + 1, loc.Y - 1);
                default:
                    return loc;
            }
        }

        public static Vector2 simple_explosion(Vector2 loc, ref float angle)
        {
            return new Vector2(loc.X + (float)(Math.Cos(angle) * 3), loc.Y + (float)(Math.Sin(angle) * 3));
        }

        public static Vector2 speed_explosion(Vector2 loc, ref float angle, ref float speed)
        {
            return new Vector2(loc.X + (float)(Math.Cos(angle) * speed), loc.Y + (float)(Math.Sin(angle) * speed));
        }

        public static Vector2 grav_explosion(Vector2 loc, ref float angle, ref float GravDir)
        {
            Vector2 outv = new Vector2(loc.X + (float)(Math.Cos(angle) * 3), loc.Y + (float)(Math.Sin(angle) * 3));
            Vector2 angleAsVector = VManager.convertToXY(new Vector2(10, angle));
            Vector2 changeAsVector = VManager.convertToXY(new Vector2(1, GravDir));
            angle = VManager.convertToMA(Vector2.Add(angleAsVector, changeAsVector)).Y;
            return outv;
        }

        public static Vector2 MV_Grav_Explosion_4(Vector2 loc, List<float> req) //0: Angle, 1: GravDir, 2: Speed, 3: Angleratio
        {
            Vector2 outv = new Vector2(loc.X + (float)(Math.Cos(req[0]) * req[2]), loc.Y + (float)(Math.Sin(req[0]) * req[2]));
            Vector2 angleAsVector = VManager.convertToXY(new Vector2(req[3], req[0]));
            Vector2 changeAsVector = VManager.convertToXY(new Vector2(1, req[1]));
            req[0] = VManager.convertToMA(Vector2.Add(angleAsVector, changeAsVector)).Y;
            return outv;
        }

        public static void setup()
        {
            RandomMove = random_move;
            FireMove = fire_move;
            SmokeMove = smoke_move;
            setupd = true;
        }

        public static void addBasicFire(Vector2 point, ParticleEffect bpl)
        {
            addFire(point, new ColorSet(ColorSet.Fire), bpl);
        }

        public static void addBasicMicroFire(Vector2 point, ParticleEffect bpl)
        {
            addMicroFire(point, new ColorSet(ColorSet.Fire), bpl);
        }

        public static void addBasicSmoke(Vector2 point, ParticleEffect bpl)
        {
            addSmoke(point, new ColorSet(ColorSet.Smoke), bpl);
        }

        public static void addParticle(Vector2 point, ColorSet c, ParticleEffect bpl)
        {
            if (!setupd)
                throw new NoSetupException("ParticleSpawner.Setup() Must be called!");
            bpl.Add(new BasicParticle(point, c.getRandomColor(), 1, RandomMove, 20));
        }

        public static void addFire(Vector2 point, ColorSet c, ParticleEffect bpl)
        {
            if (!setupd)
                throw new NoSetupException("ParticleSpawner.Setup() Must be called!");
            bpl.Add(new BasicParticle(point, c.getRandomColor(), 1, FireMove, 20));
        }

        public static void addMicroFire(Vector2 point, ColorSet c, ParticleEffect bpl)
        {
            if (!setupd)
                throw new NoSetupException("ParticleSpawner.Setup() Must be called!");
            bpl.Add(new MicroParticle(point, c.getRandomColor(), FireMove, 10));
        }

        public static void addSmoke(Vector2 point, ColorSet c, ParticleEffect bpl)
        {
            if (!setupd)
                throw new NoSetupException("ParticleSpawner.Setup() Must be called!");
            bpl.Add(new BasicParticle(point, c.getRandomColor(), 1, SmokeMove, 50));
        }

        public static void addCustom(Vector2 point, ColorSet c, float scale, BasicParticle.move move, int duration, ParticleEffect bpl)
        {
            bpl.Add(new BasicParticle(point, c.getRandomColor(), scale, move, duration));
        }

        public static void addCustom(Vector2 point, ColorSet c, float scale, SingleVarParticle.SVMove move, int duration, ParticleEffect bpl, float var1)
        {
            bpl.Add(new SingleVarParticle(point, c.getRandomColor(), scale, move, duration, var1));
        }

        public static void addCustom(Vector2 point, ColorSet c, float scale, DoubleVarParticle.DVMove move, int duration, ParticleEffect bpl, float var1, float var2)
        {
            bpl.Add(new DoubleVarParticle(point, c.getRandomColor(), scale, move, duration, var1, var2));
        }

        public static void addCustom(Vector2 point, ColorSet c, float scale, ManyVarParticle.MVMove move, int duration, ParticleEffect bpl, List<float> reqvar)
        {
            bpl.Add(new ManyVarParticle(point, c.getRandomColor(), scale, move, duration, reqvar));
        }

        public static void addCustomMicro(Vector2 point, ColorSet c, BasicParticle.move move, int duration, ParticleEffect bpl)
        {
            bpl.Add(new MicroParticle(point, c.getRandomColor(), move, duration));
        }
    }

    public class NoSetupException : Exception
    {
        public NoSetupException(String s)
            : base(s)
        {
        }
    }
}