using PhysicsExperiment.Physics.Models;
using System;

namespace PhysicsExperiment.Physics.Experiments
{
    public class SimleBodyFlightExperiment : IBodyFlightExperiment
    {
        public Entity Entity { get; private set; }

        public event Action<IBodyFlightExperiment> onTick;

        private static Vector gravity = new Vector(0.0, -9.8);

        public double LastTime => Entity.Velocity.Y / gravity.Y;

        public double MaxY { get; private set; }

        public double MaxX { get; private set; }

        public double MaxTime { get; private set; }

        public string Text => "experiment:\n"

                + "mass = " + Entity.Mass + "\n"
                + "maxX = " + MaxX + "\n"
                + "maxY = " + MaxY + "\n"

                + "max Time = " + MaxTime + "\n"
                + "current Time = " + LastTime + "\n"

                + "x = " + Entity.Position.X + "\n"
                + "y = " + Entity.Position.Y + "\n"
                + "vX = " + Entity.Velocity.X + "\n"
                + "vY = " + Entity.Velocity.Y + "\n";

        public SimleBodyFlightExperiment(Entity entity)
        {
            Entity = entity;
            MaxTime = -1.0 * entity.Velocity.Y / gravity.Y;
            MaxY = entity.Position.Y + entity.Velocity.Y * MaxTime / 2 + gravity.Y * Math.Pow(MaxTime / 2, 2) / 2;
            MaxTime += Math.Sqrt(- MaxY * 2 / gravity.Y);
            MaxX = entity.Position.X + entity.Velocity.X * MaxTime;
        }

        public void Tick(double dTime)
        {
            if (dTime <= 0)
                throw new ArgumentOutOfRangeException("Time can only be positive");

            if (Entity.Position.Y < 0)
                return;

            Entity.Position += Entity.Velocity * dTime + gravity * dTime * dTime / 2;
            Entity.Velocity += gravity * dTime;

            onTick?.Invoke(this);
        }
    }
}
