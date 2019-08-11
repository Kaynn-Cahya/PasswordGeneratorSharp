using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace PasswordGeneratorSharp {
    public sealed class GenerationProperties {
        #region Default_Fields
        private static readonly char[] defaultSpecialChars;

        private const int defaultMinimumNumberOfNumericCharacters = 1;
        private const int defaultMinimumNumberOfLowerCaseCharacters = 1;
        private const int defaultMinimumNumberOfSpecialCharacters = 1;
        private const int defaultMinimumNumberOfUpperCaseCharacters = 1;
        private const int defaultPasswordLength = 15;
        #endregion

        public int MinimumNumberOfNumericCharacters { get; set; }
        public int MinimumNumberOfLowerCaseCharacters { get; set; }
        public int MinimumNumberOfSpecialCharacters { get; set; }
        public int MinimumNumberOfUpperCaseCharacters { get; set; }
        public int PasswordLength { get; set; }
        public IReadOnlyList<char> SpecialCharacters { get; set; }

        internal SecureRandom RandomNumberGenerator { get; set; }

        #region Constructors

        static GenerationProperties() {
            defaultSpecialChars = new[] {
                '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '`', '~', '_', '+', ',', '.', '\'', '"', ';', ':', '?', '|', '/', '\\', '[', ']', '{', '}', '<', '>'
            };
        }

        public GenerationProperties() {
            MinimumNumberOfLowerCaseCharacters = defaultMinimumNumberOfNumericCharacters;
            MinimumNumberOfNumericCharacters = defaultMinimumNumberOfLowerCaseCharacters;
            MinimumNumberOfSpecialCharacters = defaultMinimumNumberOfSpecialCharacters;
            MinimumNumberOfUpperCaseCharacters = defaultMinimumNumberOfUpperCaseCharacters;
            PasswordLength = defaultPasswordLength;
            RandomNumberGenerator = SecureRandom.CreateDefaultInstance();
            SpecialCharacters = defaultSpecialChars;
        }

        public GenerationProperties(int minimumNumberOfNumericCharacters, int minimumNumberOfLowerCaseCharacters, int minimumNumberOfSpecialCharacters, int minimumNumberOfUpperCaseCharacters, int outputLength, RandomNumberGenerator randomNumberGenerator, params char[] specialCharacters) {

            if (randomNumberGenerator == null) {
                throw new ArgumentNullException(nameof(randomNumberGenerator));
            }
            RandomNumberGenerator = new SecureRandom(randomNumberGenerator);

            MinimumNumberOfLowerCaseCharacters = minimumNumberOfLowerCaseCharacters;
            MinimumNumberOfNumericCharacters = minimumNumberOfNumericCharacters;
            MinimumNumberOfSpecialCharacters = minimumNumberOfSpecialCharacters;
            MinimumNumberOfUpperCaseCharacters = minimumNumberOfUpperCaseCharacters;
            PasswordLength = outputLength;

            if (!OutputLengthIsMoreThanAllMinimumNumberOfCharacters()) {
                throw new ArgumentOutOfRangeException("OutputLength must be greater than or equal to the sum of all MinimumNumber properties!", outputLength, nameof(outputLength));
            }

            SpecialCharacters = new ReadOnlyCollection<char>(specialCharacters);

            if (SpecialCharacters.Count == 0 && MinimumNumberOfSpecialCharacters > 0) {
                throw new ArgumentOutOfRangeException("SpecialCharacters does not have any characters in it, but MinimumNumberOfSpecialCharacters is more than 0!", specialCharacters, nameof(specialCharacters));
            }
        }

        #endregion

        #region Utils

        public void SetRandomNumberGenerator(RandomNumberGenerator randomNumberGenerator) {
            if (randomNumberGenerator == null) {
                throw new ArgumentNullException(nameof(randomNumberGenerator));
            }

            RandomNumberGenerator = new SecureRandom(randomNumberGenerator);
        }

        internal bool OutputLengthIsMoreThanAllMinimumNumberOfCharacters() {
            return PasswordLength > (MinimumNumberOfLowerCaseCharacters + MinimumNumberOfNumericCharacters + MinimumNumberOfSpecialCharacters + MinimumNumberOfUpperCaseCharacters);
        }

        #endregion
    }
}
