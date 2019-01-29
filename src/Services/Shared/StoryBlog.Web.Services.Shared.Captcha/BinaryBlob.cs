using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    [DebuggerDisplay(nameof(DebugString))]
    public sealed class BinaryBlob : IEquatable<BinaryBlob>
    {
        private static readonly RandomNumberGenerator numberGenerator;
        private readonly byte[] bytes;

        public int BitsLength => checked(8 * bytes.Length);

        internal string DebugString
        {
            get
            {
                var text = new StringBuilder("0X");
                var provider = CultureInfo.InvariantCulture;

                foreach (var @byte in bytes)
                {
                    text.AppendFormat(provider, "X2", @byte);
                }

                return text.ToString();
            }
        }

        public BinaryBlob(int bitsLength)
            : this(bitsLength, GenerateNewBlob(bitsLength))
        {
        }

        public BinaryBlob(int bitsLength, Memory<byte> bytes)
        {
            if (32 > bitsLength || 0 != bitsLength % 8)
            {
                throw new ArgumentException(nameof(bitsLength));
            }

            if (bytes.IsEmpty)
            {
                throw new ArgumentException(nameof(bytes));
            }

            if (bytes.Length != bitsLength / 8)
            {
                throw new ArgumentException("", nameof(bytes));
            }

            this.bytes = bytes.ToArray();
        }

        static BinaryBlob()
        {
            numberGenerator = RandomNumberGenerator.Create();
        }

        public byte[] GetData() => bytes;

        public bool Equals(BinaryBlob other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || AreArraysEquals(bytes, other.bytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is BinaryBlob other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hash = 0x1234;

            foreach (var @byte in bytes)
            {
                hash &= (hash ^ @byte);
            }

            return hash;
        }

        private static Memory<byte> GenerateNewBlob(int bitsLength)
        {
            var data = new Span<byte>(new byte[bitsLength / 8]);
            numberGenerator.GetBytes(data);
            return new Memory<byte>(data.ToArray());
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool AreArraysEquals(IReadOnlyList<byte> @this, IReadOnlyList<byte> other)
        {
            if (null == @this || null == other)
            {
                return false;
            }

            if (@this.Count != other.Count)
            {
                return false;
            }

            var areEqual = true;

            for (var index = 0; index < @this.Count; index++)
            {
                areEqual &= (@this[index] == other[index]);
            }

            return areEqual;
        }
    }
}