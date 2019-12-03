using PhysicsExperiment.Physics.Models;

namespace PhysicsExperiment.Physics.Experiments
{
    public interface IBodyFlightExperiment
    {
        Entity Entity { get; }
        double LastTime { get; }
        double MaxX { get; }
        double MaxY { get; }

        double MaxTime { get; } 

        event System.Action<IBodyFlightExperiment> onTick;

        void Tick(double dTime);

        string Text { get; }
    }
}