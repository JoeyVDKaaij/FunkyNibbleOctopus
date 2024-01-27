using System;
using Game.Items;

namespace Game.Plates
{
    public struct Plate
    {
        public static readonly Plate Invalid = new (0, PlateType.Invalid);
        public static Plate Create(PlateType type) => new (UnityEngine.Random.Range(1, int.MaxValue), type);

        public readonly int ID;
        public readonly PlateType Type;

        private Plate (int id, PlateType type)
        {
            ID = id;
            Type = type;
        }

        public static bool operator == (Plate a, Plate b)
        {
            return a.ID == b.ID;
        }

        public static bool operator != (Plate a, Plate b)
        {
            return !(a == b);
        }

        public bool Equals (Plate other)
        {
            return ID == other.ID;
        }

        public override bool Equals (object obj)
        {
            return obj is Plate other && Equals(other);
        }

        public override int GetHashCode ()
        {
            return HashCode.Combine(ID, (int)Type);
        }
    }
}
