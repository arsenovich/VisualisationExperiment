using System;

namespace PhysicsExperiment.Physics.Models
{
    public class Entity: ICloneable
    {
        private double mass;

        private Vector velocity;

        private Vector position;
        
        public double Mass { 
            get => mass; 
            set{
                if (value < 0)
                    throw new ArgumentOutOfRangeException("mass can only be positive");
                mass = value;
                onMassUpdate?.Invoke(this);
            }
        }

        public Vector Velocity 
        { 
            get => velocity;
            set 
            { 
                velocity = value;
                onVelocityUpdate?.Invoke(this);
            }
        }

        public Vector Position
        {
            get => position;
            set 
            { 
                position = value;
                onPositionUpdate?.Invoke(this);
            }
        }

        public event Action<Entity> onMassUpdate;

        public event Action<Entity> onPositionUpdate;

        public event Action<Entity> onVelocityUpdate;

        public event Action<Entity> onEntityUpdate;

        public Entity(double mass, Vector position, Vector velocity)
        {
            this.mass = mass;
            this.position = position;
            this.velocity = velocity;

            onMassUpdate += (entity) => onEntityUpdate?.Invoke(entity);
            onPositionUpdate += (entity) => onEntityUpdate?.Invoke(entity);
            onVelocityUpdate += (entity) => onEntityUpdate?.Invoke(entity);
        }

        public object Clone()
        {
            return new Entity(mass, position, velocity);
        }
    }
}
