using System;
using System.Security.Cryptography;
using UnityEngine;

namespace CodeBase.System.Services.Utilities
{
    /// <summary>Шифрованный int с самопроверкой и событием cheat-детекта.</summary>
    [Serializable]
    public struct SecureInt :
        ISerializationCallbackReceiver, IComparable<SecureInt>, IEquatable<SecureInt>
    {
        [SerializeField] private int _cipher;
        [SerializeField] private int _key;   // не хранить «0»
        private   int     _shadow;           // «честное» значение для сверки

        /// Выстрелит при любом расхождении данных (глобально на все экземпляры)
        public static event Action CheatDetected;

        // ─────────────────────────── КОНСТРУКТОРЫ ────────────────────────────
        public SecureInt(int value) : this()
        {
            _key    = NewKey();
            _cipher = value ^ _key;
            _shadow = value;
        }

        // ───────────────────────────── СВОЙСТВО ──────────────────────────────
        public int Value
        {
            get
            {
                int plain = _cipher ^ _key;
                if (plain != _shadow)
                    CheatDetected?.Invoke();

                // необязательно, но полезно — значение «скачет» в памяти
                RegenerateKey(plain);
                return plain;
            }
            set => RegenerateKey(value);
        }

        // ─────────────────────────── ВСПОМОГАТЕЛЬНОЕ ─────────────────────────
        private static int NewKey()
        {
            // Полностью обходится без UnityAPI => работает в любой фазе
            Span<byte> buf = stackalloc byte[4];
            int k;
            do
            {
                RandomNumberGenerator.Fill(buf);
                k = BitConverter.ToInt32(buf);
            } while (k == 0);
            return k;
        }

        private void RegenerateKey(int newPlain)
        {
            _key    = NewKey();
            _cipher = newPlain ^ _key;
            _shadow = newPlain;
        }

        // ────────────────────── СЕРИАЛИЗАЦИЯ В UNITY ─────────────────────────
        // После загрузки сцены шифруем заново, чтобы ключ не был одинаковым в PlayerPrefs
        public void OnAfterDeserialize() => RegenerateKey(_shadow);
        public void OnBeforeSerialize()  { }   // не нужен

        // ───────────────────── КОНВЕРСИИ И ОПЕРАТОРЫ ─────────────────────────
        public static implicit operator int       (SecureInt s) => s.Value;
        public static implicit operator SecureInt (int v)       => new SecureInt(v);

        public static SecureInt operator +(SecureInt a, SecureInt b) => new SecureInt(a.Value + b.Value);
        public static SecureInt operator -(SecureInt a, SecureInt b) => new SecureInt(a.Value - b.Value);
        public static SecureInt operator *(SecureInt a, SecureInt b) => new SecureInt(a.Value * b.Value);
        public static SecureInt operator /(SecureInt a, SecureInt b) => new SecureInt(a.Value / b.Value);
        public static SecureInt operator %(SecureInt a, SecureInt b) => new SecureInt(a.Value % b.Value);

        public static SecureInt operator ++(SecureInt a) { a.Value += 1; return a; }
        public static SecureInt operator --(SecureInt a) { a.Value -= 1; return a; }

        public static bool operator ==(SecureInt a, SecureInt b) => a.Value == b.Value;
        public static bool operator !=(SecureInt a, SecureInt b) => a.Value != b.Value;
        public static bool operator  <(SecureInt a, SecureInt b) => a.Value  < b.Value;
        public static bool operator  >(SecureInt a, SecureInt b) => a.Value  > b.Value;
        public static bool operator <=(SecureInt a, SecureInt b) => a.Value <= b.Value;
        public static bool operator >=(SecureInt a, SecureInt b) => a.Value >= b.Value;

        public override bool   Equals(object obj)         => obj is SecureInt other && Equals(other);
        public          bool   Equals(SecureInt other)    => this == other;
        public          int    CompareTo(SecureInt other) => Value.CompareTo(other.Value);
        public override int    GetHashCode()              => Value.GetHashCode();
        public override string ToString()                 => Value.ToString();
    }
}
