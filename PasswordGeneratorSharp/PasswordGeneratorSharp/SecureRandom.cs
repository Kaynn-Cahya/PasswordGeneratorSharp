using System;
using System.Security.Cryptography;

namespace PasswordGeneratorSharp {
    public sealed class SecureRandom {

        private readonly RandomNumberGenerator randomNumberGenerator;

        public SecureRandom(RandomNumberGenerator _randomNumberGenerator) {
            if (_randomNumberGenerator == null) {
                throw new ArgumentNullException(nameof(_randomNumberGenerator));
            }

            randomNumberGenerator = _randomNumberGenerator;
        }

        internal byte[] GetBytes(byte[] buffer) {
            randomNumberGenerator.GetBytes(buffer);

            return buffer;
        }

        internal byte[] GetBytes(int count) {
            return GetBytes(new byte[count]);
        }

        internal uint Range() {
            return BitConverter.ToUInt32(GetBytes(sizeof(uint)), 0);
        }

        internal uint Range(uint x, uint y) {
            if (x > y) {
                var temp = x;

                x = y;
                y = temp;
            }

            uint range = (y - x);

            if (range == 0) {
                return x;
            } else if (range == uint.MaxValue) {
                return Range();
            } else {
                return (Range(range) + x);
            }
        }

        private uint Range(uint exclusiveHigh) {
            var range = (uint.MaxValue - (((uint.MaxValue % exclusiveHigh) + 1) % exclusiveHigh));
            uint result;
            do {
                result = Range();
            } while (result > range);

            return (result % exclusiveHigh);
        }

        /// <summary>
        /// Creates a new instance of SecureRandom with default properties.
        /// </summary>
        /// <returns></returns>
        public static SecureRandom CreateDefaultInstance() {
            return new SecureRandom(RandomNumberGenerator.Create());
        }
    }
}
