﻿using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class PatrolNode : MonoBehaviour
    {
        public PatrolNode nextNode;

        public Vector3 GetNextNodePosition()
        {
            return nextNode.GetPosition();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);
            if (nextNode != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(GetPosition(), GetNextNodePosition());
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}