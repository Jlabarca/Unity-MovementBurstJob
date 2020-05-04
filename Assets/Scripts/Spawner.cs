using JLabarca.Jobs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace JLabarca
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject actorPrefab;
        [SerializeField] private Vector2 area;
        [SerializeField] private int quantity;

        private NativeList<Vector3> _position;
        private NativeList<Vector3> _velocity;
        private TransformAccessArray _transform;

        private void Start() {
            _position = new NativeList<Vector3>(quantity, Allocator.Persistent);
            _velocity = new NativeList<Vector3>(quantity, Allocator.Persistent);
            _transform = new TransformAccessArray(quantity);
            for (var i = 0; i < quantity; i++)
            {
                AddActor();
            }
        }

        private void AddActor() {
            var obj = Instantiate(actorPrefab).transform;
            var randomVector = Random.insideUnitSphere;

            // Over floor position
            randomVector.y = 0.05f;
            obj.position = randomVector * 10;
            _position.Add(Vector3.zero);

            // Only x/z movement
            randomVector.y = 0;
            _velocity.Add(randomVector * 4);

            _transform.Add(obj);

        }

        private void Update() {

            var moveJob = new MovementJob {
                Position = _position.AsDeferredJobArray(),
                Velocity = _velocity.AsDeferredJobArray(),
                DeltaTime = Time.deltaTime,
                Area = area
            };

            var moveHandler = moveJob.Schedule(_transform);
            moveHandler.Complete();
        }

        private void OnDestroy() {
            _position.Dispose();
            _velocity.Dispose();
            _transform.Dispose();
        }
    }
}
