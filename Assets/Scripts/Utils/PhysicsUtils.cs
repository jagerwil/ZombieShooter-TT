using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Ducksten.Utils {
    public static class PhysicsUtils {
        private static readonly RaycastHit[] PhysicsCastHits = new RaycastHit[1000];
        private static readonly RaycastHit[] ConeCastHits = new RaycastHit[1000];
        private static readonly RaycastHit[] RaycastHits = new RaycastHit[1000];

        [CanBeNull]
        public static Transform GetClosestConeCast(Transform origin, Vector3 direction, float maxDistance, float coneAngle, int layerMask) {
            var coneHitsAmount = ConeCastNonAlloc(origin.position, direction, maxDistance, coneAngle, layerMask, ConeCastHits);
            return GetClosestRaycastHit(ConeCastHits, coneHitsAmount);
        }

        public static Transform GetClosestRaycastCylinder(Transform origin, Vector3 direction, float maxDistance, float closestRadius, float furthestRadius, float step, int layerMask) {
            var raycastsAmount = RaycastCylinderNonAlloc(origin, direction, maxDistance, closestRadius, furthestRadius,
                step, RaycastHits, layerMask);
            return GetClosestRaycastHit(RaycastHits, raycastsAmount);
        }

        public static int RaycastCylinderNonAlloc(Transform origin, Vector3 direction, float maxDistance, float closestRadius, 
            float furthestRadius, float step, RaycastHit[] hits, int layerMask) {
            var furthestSquared = furthestRadius * furthestRadius;

            var raycastsMade = 0;
            var num = Mathf.CeilToInt(furthestRadius / step - 0.01f);
            for (var x = -num; x <= num; x++) {
                for (var y = -num; y <= num; y++) {
                    var coords = new Vector2(x, y) * step;
                    if (coords.sqrMagnitude < furthestSquared) {
                        hits[raycastsMade] = RaycastCylinderPoint(origin, direction, maxDistance, closestRadius, furthestRadius, coords, layerMask);
                        raycastsMade += 1;
                    }
                }
            }
            return raycastsMade;
        }

        private static RaycastHit RaycastCylinderPoint(Transform origin, Vector3 direction, float maxDistance,
            float closestRadius, float furthestRadius, Vector2 furthestCoords, int layerMask) {
            var furthestOffset = origin.up * furthestCoords.y + origin.right * furthestCoords.x;
            var closestOffset = furthestOffset * (closestRadius / furthestRadius);
            var rootPosition = origin.position + closestOffset;
            var endPoint = origin.position + (direction * maxDistance) + furthestOffset;
            Physics.Raycast(rootPosition, (endPoint - rootPosition).normalized, out var hitInfo, maxDistance, layerMask);
            //Debug.DrawRay(rootPosition, (endPoint - rootPosition), Random.ColorHSV(), maxDistance);
            return hitInfo;
        }

        [CanBeNull]
        public static Transform GetClosestRaycastHit(RaycastHit[] hits, int hitsAmount) {
            var hitsToCheck = Mathf.Min(hits.Length, hitsAmount);
            if (hitsToCheck <= 0) {
                return null;
            }

            var closestTarget = hits[0].transform;
            var closestDistance = hits[0].distance;
            for (int i = 1; i < hitsToCheck; i++) {
                var transform = hits[i].transform;
                var distance = hits[i].distance;
                
                if (!closestTarget || transform && distance < closestDistance) {
                    closestTarget = transform;
                    closestDistance = distance;
                }
            }
            return closestTarget;
        }

        //https://github.com/walterellisfun/ConeCast
        public static RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle) {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
            List<RaycastHit> coneCastHitList = new List<RaycastHit>();
        
            if (sphereCastHits.Length > 0) {
                for (int i = 0; i < sphereCastHits.Length; i++) {
                    sphereCastHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                    Vector3 hitPoint = sphereCastHits[i].point;
                    Vector3 directionToHit = hitPoint - origin;
                    float angleToHit = Vector3.Angle(direction, directionToHit);

                    if (angleToHit < coneAngle) {
                        coneCastHitList.Add(sphereCastHits[i]);
                    }
                }
            }

            RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
            coneCastHits = coneCastHitList.ToArray();

            return coneCastHits;
        }

        //My modification
        public static int ConeCastNonAlloc(Vector3 origin, Vector3 direction, float maxDistance, float coneAngle, int layerMask, RaycastHit[] hits) {
            var radius = maxDistance * Mathf.Sin(coneAngle * Mathf.Deg2Rad);
            Physics.SphereCastNonAlloc(origin - new Vector3(0, 0, radius), radius, direction, PhysicsCastHits, maxDistance, layerMask);

            var coneHitsAmount = 0;
            var maxConeHits = Mathf.Min(hits.Length, PhysicsCastHits.Length);
            if (PhysicsCastHits.Length <= 0) {
                return 0;
            }

            for (int i = 0; i < PhysicsCastHits.Length; i++) {
                var collider = PhysicsCastHits[i].collider;
                if (!collider || !collider.enabled || !PhysicsCastHits[i].transform.gameObject.activeInHierarchy) {
                    break;
                }

                var hitPoint = PhysicsCastHits[i].point;
                var directionToHit = hitPoint - origin;
                var angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit >= coneAngle) {
                    continue;
                }

                hits[coneHitsAmount] = PhysicsCastHits[i];
                coneHitsAmount += 1;

                if (coneHitsAmount >= maxConeHits) {
                    Debug.LogWarning($"{nameof(PhysicsUtils)}.{nameof(ConeCastNonAlloc)}: maximum coneHitsAmount is reached ({coneHitsAmount}). Some ConeCast hits might be not processed");
                    return maxConeHits;
                }
            }

            return coneHitsAmount;
        }
    }
}

