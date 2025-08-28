using System;
using System.Security.Cryptography;
using UnityEngine;

namespace CodeBase.System.Services.Utilities
{
    /// <summary>Шифрованный float с детектом вмешательства.</summary>
    [Serializable]
    public struct SecureFloat :
        ISerializationCallbackReceiver, IComparable<SecureFloat>, IEquatable<SecureFloat>
    {
        [SerializeField] private int _cipher;
        [SerializeField] private int _key;
        private   float   _shadow;

        public static event Action CheatDetected;

        public SecureFloat(float value) : this()
        {
            RegenerateKey(value);
        }
       
        public float Value
        {
            get
            {
                float plain = IntToFloat(_cipher ^ _key);
                if (!Mathf.Approximately(plain, _shadow)) CheatDetected?.Invoke();

                if (Application.isPlaying)
                    RegenerateKey(plain);

                return plain;
            }
            set => RegenerateKey(value);
        }

        // ─────────────────────────── СЕРВИСНЫЕ ───────────────────────────────
        private static int   FloatToInt(float f) => BitConverter.ToInt32 (BitConverter.GetBytes(f), 0);
        private static float IntToFloat(int  i)  => BitConverter.ToSingle(BitConverter.GetBytes(i), 0);

        private static int NewKey()
        {
            Span<byte> buf = stackalloc byte[4];
            int k;
            do
            {
                RandomNumberGenerator.Fill(buf);
                k = BitConverter.ToInt32(buf);
            } while (k == 0);
            return k;
        }

        private void RegenerateKey(float newPlain)
        {
            _key    = NewKey();
            _cipher = FloatToInt(newPlain) ^ _key;
            _shadow = newPlain;
        }

        // ───────────────────────── СЕРИАЛИЗАЦИЯ ──────────────────────────────
        
        public void OnBeforeSerialize()  { }
        public void OnAfterDeserialize() { if (_key == 0) _key = 1; }
        
        // ───────────────────── КОНВЕРСИИ И ОПЕРАТОРЫ ─────────────────────────
        public static implicit operator float        (SecureFloat s) => s.Value;
        public static implicit operator SecureFloat  (float v)       => new SecureFloat(v);

        public static SecureFloat operator +(SecureFloat a, SecureFloat b) => new SecureFloat(a.Value + b.Value);
        public static SecureFloat operator -(SecureFloat a, SecureFloat b) => new SecureFloat(a.Value - b.Value);
        public static SecureFloat operator *(SecureFloat a, SecureFloat b) => new SecureFloat(a.Value * b.Value);
        public static SecureFloat operator /(SecureFloat a, SecureFloat b) => new SecureFloat(a.Value / b.Value);

        public static SecureFloat operator ++(SecureFloat a) { a.Value += 1f; return a; }
        public static SecureFloat operator --(SecureFloat a) { a.Value -= 1f; return a; }

        public static bool operator ==(SecureFloat a, SecureFloat b) => Mathf.Approximately(a.Value, b.Value);
        public static bool operator !=(SecureFloat a, SecureFloat b) => !Mathf.Approximately(a.Value, b.Value);
        public static bool operator  <(SecureFloat a, SecureFloat b) => a.Value <  b.Value;
        public static bool operator  >(SecureFloat a, SecureFloat b) => a.Value >  b.Value;
        public static bool operator <=(SecureFloat a, SecureFloat b) => a.Value <= b.Value;
        public static bool operator >=(SecureFloat a, SecureFloat b) => a.Value >= b.Value;

        public override bool   Equals(object obj)           => obj is SecureFloat other && Equals(other);
        public          bool   Equals(SecureFloat other)    => this == other;
        public          int    CompareTo(SecureFloat other) => Value.CompareTo(other.Value);
        public override int    GetHashCode()                => Value.GetHashCode();
        public override string ToString()                   => Value.ToString("R");
    }
}
