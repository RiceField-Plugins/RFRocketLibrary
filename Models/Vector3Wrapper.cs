using System;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public struct Vector3Wrapper
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3Wrapper(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Wrapper(Vector3 vector3)
        {
            X = vector3.x;
            Y = vector3.y;
            Z = vector3.z;
        }

        public Vector3Wrapper Create(Vector3 vector3)
        {
            return new Vector3Wrapper(vector3);
        }

        public Vector3 ToVector3() => new(X, Y, Z);
    }
}