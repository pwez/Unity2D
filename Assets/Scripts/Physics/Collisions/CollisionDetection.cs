using UnityEngine;

namespace Physics.Collisions {
    public class CollisionDetection : RayProjector {

        [Header("Collisions")]
        public LayerMask collisionLayers;
        public CollisionInfo collisionInfo;
        public int mode;
        
        public void Translate(Vector2 displacement) {
            UpdateRayOrigins();
            collisionInfo.Reset();
            collisionInfo.inputDisplacement = displacement;
            if (displacement.y < 0)
                CheckToDescendSlope(ref displacement);
            if (Mathf.Abs(displacement.x) > 0f)
                CheckForHorizontalCollision(ref displacement);
            if (Mathf.Abs(displacement.y) > 0f)
                CheckForVerticalCollision(ref displacement);
            transform.Translate(displacement);

            // if (collisionInfo.ascendingSlope) {
            //     if (collisionInfo.currentSurfaceAngle > 45f)
            //         transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90f * Mathf.Sign(displacement.x));
            // }
        }
        
        private void CheckToDescendSlope(ref Vector2 displacement) {
            var dx = (int) Mathf.Sign (displacement.x);
            var dy = (int) Mathf.Sign (displacement.y);
            var rayOrigin = dx < 0 ? rayOrigins.bottomRight : rayOrigins.bottomLeft;
            var ray = Physics2D.Raycast (rayOrigin, -transform.up, Mathf.Infinity, collisionLayers);
            Debug.DrawRay(rayOrigin, -transform.up, Color.magenta);
            if (ray) {
                var surfaceAngle = Vector2.Angle (ray.normal, transform.up);
                if (Mathf.Abs(surfaceAngle) > 0.00001f) {
                    if (mode == 0) {
                        var rayNormalXSign = (int) Mathf.Sign(ray.normal.x);
                        if (rayNormalXSign == dx) {
                            var verticalDistanceFromSlope = ray.distance - skinWidth;
                            var distanceToDescend = Mathf.Tan(surfaceAngle * Mathf.Deg2Rad) * Mathf.Abs(displacement.x);
                            if (verticalDistanceFromSlope <= distanceToDescend) {
                                var distance = Mathf.Abs(displacement.x);
                                displacement.x = Mathf.Cos(surfaceAngle * Mathf.Deg2Rad) * distance *
                                                 Mathf.Sign(displacement.x);
                                displacement.y -= Mathf.Sin(surfaceAngle * Mathf.Deg2Rad) * distance;
                                collisionInfo.currentSurfaceAngle = surfaceAngle;
                                collisionInfo.currentSurfaceNormal = ray.normal;
                                collisionInfo.descendingSlope = true;
                                collisionInfo.below = true;
                            }
                        }
                    }
                    else if (mode == 1) {
                        var rayNormalYSign = (int) Mathf.Sign(ray.normal.y);
                        if (rayNormalYSign == dy) {
                            var verticalDistanceFromSlope = ray.distance - skinWidth;
                            var distanceToDescend = Mathf.Tan(surfaceAngle * Mathf.Deg2Rad) * Mathf.Abs(displacement.y);
                            if (verticalDistanceFromSlope <= distanceToDescend) {
                                var distance = Mathf.Abs(displacement.y);
                                displacement.x += Mathf.Sign(surfaceAngle * Mathf.Deg2Rad) * distance;
                                displacement.y = Mathf.Cos(surfaceAngle * Mathf.Deg2Rad) * distance *
                                                 Mathf.Sign(displacement.y);
                                collisionInfo.currentSurfaceAngle = surfaceAngle;
                                collisionInfo.currentSurfaceNormal = ray.normal;
                                collisionInfo.descendingSlope = true;
                                collisionInfo.below = true;
                            }
                        }
                    }
                }
            }
        }
        
        private void CheckForHorizontalCollision(ref Vector2 displacement) {
            var element = transform;
            var up = (Vector2) element.up;
            var right = (Vector2) element.right;
            var dx = (int) Mathf.Sign(displacement.x);
            var length = Mathf.Abs(displacement.x) + skinWidth;
            for (var i = 0; i < horizontalRayCount; i++) {
                var origin = dx < 0 ? rayOrigins.bottomLeft : rayOrigins.bottomRight;
                origin += up * (horizontalRaySpacing * i);
                var destination = right * dx;
                var ray = Physics2D.Raycast(origin, destination, length, collisionLayers);
                
                if (!ray) 
                    continue;

                if (i >= horizontalRayCount / 4 && displacement.y < 0f && ray.collider.gameObject.tag.Equals("Ledge")) {
                    collisionInfo.ledge = true;
                    var ledge = ray.collider.gameObject;
                    var playerTop = transform.position.y + boxCollider.bounds.extents.y;
                    var ledgeTop = ledge.transform.position.y + ledge.GetComponent<BoxCollider2D>().bounds.extents.y;
                    var heightDifference = ledgeTop - playerTop;
                    transform.Translate(new Vector3(0f, heightDifference, 0f));
                    displacement.y = 0f;
                }

                var surfaceAngle = Vector2.Angle(ray.normal, transform.up);
                if (i == 0 && surfaceAngle > 0f && surfaceAngle < 90f) {
                    var distanceXToSlope = 0f;
                    if (Mathf.Abs(surfaceAngle - collisionInfo.previousSurfaceAngle) > 0.00001f) {
                        distanceXToSlope = ray.distance - skinWidth;
                        displacement.x -= distanceXToSlope * dx;
                    }
                    AscendSlope(ref displacement, surfaceAngle, ray.normal);
                    displacement.x += distanceXToSlope * dx;
                }
                if (!collisionInfo.ascendingSlope) {
                    length = ray.distance;
                    displacement.x = (ray.distance - skinWidth) * dx;
                    if (collisionInfo.ascendingSlope)
                        displacement.y = Mathf.Abs(displacement.x) * Mathf.Tan(collisionInfo.currentSurfaceAngle * Mathf.Deg2Rad);
                    collisionInfo.left = dx < 0f;
                    collisionInfo.right = dx > 0f;
                }
            }
        }
        
        private void AscendSlope(ref Vector2 displacement, float surfaceAngle, Vector2 rayNormal) {
            var angle = surfaceAngle * Mathf.Deg2Rad;
            var distance = Mathf.Abs(displacement.x);
            var displacementY = distance * Mathf.Sin(angle);
            if (displacement.y <= displacementY) {
                displacement.y = displacementY;
                displacement.x = distance * Mathf.Cos(angle) * Mathf.Sign(displacement.x);
                collisionInfo.currentSurfaceAngle = surfaceAngle;
                collisionInfo.currentSurfaceNormal = rayNormal;
                collisionInfo.ascendingSlope = true;
                collisionInfo.below = true;
            }
        }

        private void CheckForVerticalCollision(ref Vector2 displacement) {
            var element = transform;
            var up = (Vector2) element.up;
            var right = (Vector2) element.right;
            var dy = (int) Mathf.Sign(displacement.y);
            var length = Mathf.Abs(displacement.y) + skinWidth;
            for (var i = 0; i < verticalRayCount; i++) {
                var origin = dy < 0 ? rayOrigins.bottomLeft : rayOrigins.topLeft;
                origin += right * (verticalRaySpacing * i + displacement.x);
                var destination = up * dy;
                var ray = Physics2D.Raycast(origin, destination, length, collisionLayers);
                if (!ray) continue;
                
                var surfaceAngle = Vector2.Angle(ray.normal, ray.normal.x > 0 ? right : -right);
                if (mode == 0 && dy > 0 && (i == 0 || i == verticalRayCount - 1) && surfaceAngle < 90) {
                    var distanceYToSlope = 0f;
                    if (Mathf.Abs(surfaceAngle - collisionInfo.previousSurfaceAngle) > 0.00001f) {
                        distanceYToSlope = ray.distance - skinWidth;
                        displacement.y -= distanceYToSlope * dy;
                    }
                    AscendSlopeVertically(ref displacement, surfaceAngle, ray.normal);
                    displacement.y += distanceYToSlope * dy;
                }
                if (!collisionInfo.ascendingSlope) {
                    length = ray.distance;
                    displacement.y = (ray.distance - skinWidth) * dy;
                    if (collisionInfo.ascendingSlope)
                        displacement.x = displacement.y / Mathf.Tan(collisionInfo.currentSurfaceAngle * Mathf.Deg2Rad) * Mathf.Sign(displacement.x);
                    collisionInfo.below = dy < 0f;
                    collisionInfo.above = dy > 0f;
                }
            }
            if (collisionInfo.ascendingSlope) {
                var dx = (int) Mathf.Sign(displacement.x);
                length = Mathf.Abs(displacement.x) + skinWidth;
                var rayOrigin = (dx < 0 ? rayOrigins.bottomLeft : rayOrigins.bottomRight) + Vector2.up * displacement.y;
                var ray = Physics2D.Raycast(rayOrigin,Vector2.right * dx, length, collisionLayers);
                if (ray) {
                    var slopeAngle = Vector2.Angle(ray.normal,Vector2.up);
                    if (Mathf.Abs(slopeAngle - collisionInfo.currentSurfaceAngle) > 0.00001f) {
                        displacement.x = (ray.distance - skinWidth) * dx;
                        collisionInfo.currentSurfaceAngle = slopeAngle;
                        collisionInfo.currentSurfaceNormal = ray.normal;
                    }
                }
            }
        }

        private void AscendSlopeVertically (ref Vector2 displacement, float surfaceAngle, Vector2 rayNormal) {
            var angle = surfaceAngle * Mathf.Deg2Rad;
            var distance = Mathf.Abs(displacement.y);
            displacement.x = distance * Mathf.Sin(angle) * Mathf.Sign(rayNormal.x);
            displacement.y = distance * Mathf.Cos(angle) * Mathf.Sign(displacement.y); 
            collisionInfo.currentSurfaceAngle = surfaceAngle;
            collisionInfo.currentSurfaceNormal = rayNormal;
            collisionInfo.ascendingSlope = true;
        }
        
        public struct CollisionInfo {
            public bool above;
            public bool below;
            public bool right;
            public bool left;
            public bool ledge;
            public bool descendingSlope;
            public bool ascendingSlope;
            public float previousSurfaceAngle;
            public float currentSurfaceAngle;
            public Vector2 currentSurfaceNormal;
            public Vector2 inputDisplacement;

            public void Reset() {
                above = false;
                below = false;
                right = false;
                left = false;
                ledge = false;
                ascendingSlope = false;
                descendingSlope = false;
                previousSurfaceAngle = currentSurfaceAngle;
                currentSurfaceAngle = 0f;
                currentSurfaceNormal = Vector2.zero;
            }
        }
        
        // public override void Update() {
        //     base.Update();
        //     if (Input.GetKeyDown(KeyCode.Alpha1))
        //         ToggleBounds(0);
        //     else if (Input.GetKeyDown(KeyCode.Alpha2))
        //         ToggleBounds(1);
        // }
        
    }
}
