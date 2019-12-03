using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsExperiment.Physics.Models;

namespace PhysicsExperiment.Physics.Experiments
{
    class HardBodyFlyExperiment : IBodyFlightExperiment
    {
        private Entity entity;

        public Entity Entity { get; private set; }

        public double LastTime => t;

        public double MaxX { get; private set; }

        public double MaxY { get; private set; }

        public double MaxTime { get; private set; }

        public string Text => "experiment:\n"
                + "mass = " + Entity.Mass + "\n"
                + "R = " + R + "\n"
                + "maxX = " + MaxX + "\n"
                + "maxY = " + MaxY + "\n"
                + "current Time = " + LastTime + "\n"
                + "x = " + Entity.Position.X + "\n"
                + "y = " + Entity.Position.Y + "\n";
                

        public event Action<IBodyFlightExperiment> onTick;

        private Vector postion(double t)
        {
            double vx = entity.Velocity.X;
            double vy = entity.Velocity.Y;

            double x = vx * entity.Mass / R + (vx) * entity.Mass * Math.Exp( - R * t / entity.Mass) / R;
            double y = -(entity.Mass + vy * R) * entity.Mass * Math.Exp( - R * t / entity.Mass) / R - 9.8 * entity.Mass * t / R + (entity.Mass * 9.8 + vy * R) *  entity.Mass / Math.Pow(R, 2);

            Vector c = entity.Position + new Vector(x, y);

            if (c.X > MaxX)
                MaxX = c.X;

            if (c.Y > MaxY)
                MaxY = c.Y;

            return c;
        }

        private double R;

        private double t = 0.0;

        public void Tick(double dTime)
        {
            t += dTime;
            Entity.Position = postion(t);
            onTick?.Invoke(this);
        }

        public HardBodyFlyExperiment(Entity entity, double r)
        {
            Entity = entity;
            this.entity = (Entity) entity.Clone();
            R = r;
            MaxX = entity.Position.X;
            MaxY = entity.Position.Y;
        }
    }
}
