using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace JLabarca.Jobs
{
    [BurstCompile]
    public struct MovementJob : IJobParallelForTransform
    {
        public NativeArray<Vector3> Position;
        public NativeArray<Vector3> Velocity;
        public Vector2 Area;
        public float DeltaTime;

        void IJobParallelForTransform.Execute(int id, TransformAccess t) {
            var vel = Velocity[id];

            //area limits
            if (math.abs(t.position.x) > Area.x/2)
            {
                if(math.abs(t.position.x + vel.x) > math.abs(t.position.x) )
                    vel.x *= -1;
            }

            if (math.abs(t.position.z) > Area.y/2)
            {
                if(math.abs(t.position.z + vel.z) > math.abs(t.position.z) )
                    vel.z *= -1;
            }

            //movement
            Velocity[id] = vel;
            t.position += vel * DeltaTime;
            Position[id] = t.position;
        }
    }
}