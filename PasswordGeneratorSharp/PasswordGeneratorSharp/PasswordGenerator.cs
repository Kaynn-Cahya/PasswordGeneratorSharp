using System;
using System.Collections.Generic;

namespace PasswordGeneratorSharp {
    public sealed class PasswordGenerator {

        private delegate char getASCII();

        private readonly getASCII getASCIILetterLowerCase;
        private readonly getASCII getASCIILetterUpperCase;
        private readonly getASCII getASCIINumeric;
        private readonly getASCII getASCIISpecial;

        private readonly GenerationProperties generationProperties;

        #region Constructors

        public PasswordGenerator(GenerationProperties _generationProperties) {
            if (_generationProperties == null) {
                throw new ArgumentNullException(nameof(_generationProperties));
            }

            if (!_generationProperties.OutputLengthIsMoreThanAllMinimumNumberOfCharacters()) {
                throw new ArgumentOutOfRangeException("OutputLength must be greater than or equal to the sum of all MinimumNumber properties!", _generationProperties.PasswordLength, nameof(_generationProperties.PasswordLength));
            }

            var randomNumberGenerator = _generationProperties.RandomNumberGenerator;
            var specialCharacters = _generationProperties.SpecialCharacters;

            
            getASCIILetterLowerCase = () => (char)randomNumberGenerator.Range(97, 123);
            getASCIILetterUpperCase = () => (char)randomNumberGenerator.Range(65, 91);
            getASCIINumeric = () => (char)randomNumberGenerator.Range(48, 58);
            getASCIISpecial = () => specialCharacters[(int)randomNumberGenerator.Range(0U, (uint)specialCharacters.Count)];
            generationProperties = _generationProperties;
        }

        public PasswordGenerator() {
            GenerationProperties _generationProperties = new GenerationProperties();

            generationProperties = _generationProperties;
            var randomNumberGenerator = _generationProperties.RandomNumberGenerator;
            var specialCharacters = _generationProperties.SpecialCharacters;

            getASCIILetterLowerCase = () => (char)randomNumberGenerator.Range(97, 123);
            getASCIILetterUpperCase = () => (char)randomNumberGenerator.Range(65, 91);
            getASCIINumeric = () => (char)randomNumberGenerator.Range(48, 58);
            getASCIISpecial = () => specialCharacters[(int)randomNumberGenerator.Range(0U, (uint)specialCharacters.Count)];
        }

        #endregion

        /// <summary>
        /// Generates a new password with this current generator.
        /// </summary>
        /// <returns></returns>
        public string Generate() {
            int currIndex = 0;
            char[] passwordGenerated = new char[generationProperties.PasswordLength];

            for (int i = 0; i < generationProperties.MinimumNumberOfLowerCaseCharacters; ++i) {
                passwordGenerated[currIndex] = getASCIILetterLowerCase();
                ++currIndex;
            }

            for (int i = 0; i < generationProperties.MinimumNumberOfNumericCharacters; ++i) {
                passwordGenerated[currIndex] = getASCIINumeric();
                ++currIndex;
            }

            for (int i = 0; i < generationProperties.MinimumNumberOfSpecialCharacters; ++i) {
                passwordGenerated[currIndex] = getASCIISpecial();
                ++currIndex;
            }

            for (int i = 0; i < generationProperties.MinimumNumberOfUpperCaseCharacters; ++i) {
                passwordGenerated[currIndex] = getASCIILetterUpperCase();
                ++currIndex;
            }

            bool useSpecial = 0 < generationProperties.SpecialCharacters.Count;

            for (var i = currIndex; i < generationProperties.PasswordLength; ++i) {
                char c;

                switch (generationProperties.RandomNumberGenerator.Range(0U, (useSpecial ? 4U : 3U))) {
                    case 3U:
                        c = getASCIISpecial();
                        break;
                    case 2U:
                        c = getASCIINumeric();
                        break;
                    case 1U:
                        c = getASCIILetterUpperCase();
                        break;
                    case 0U:
                        c = getASCIILetterLowerCase();
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                passwordGenerated[i] = c;
            }

            FisherYatesShuffle(generationProperties.RandomNumberGenerator, passwordGenerated);

            return new string(passwordGenerated);
        }

        #region Utils
        private static void SwapRandom<T>(SecureRandom randomNumberGenerator, IList<T> list, uint indexLowerBound, uint indexUpperBound) {
            var randomIndex = randomNumberGenerator.Range(indexLowerBound, indexUpperBound);
            var tempValue = list[(int)randomIndex];

            list[(int)randomIndex] = list[(int)indexUpperBound];
            list[(int)indexUpperBound] = tempValue;
        }
        private static void FisherYatesShuffle<T>(SecureRandom randomNumberGenerator, IList<T> list) {
            uint offset = 0U;

            while (offset < list.Count) {
                SwapRandom(randomNumberGenerator, list, 0U, offset);
                ++offset;
            }
        }

        #endregion
    }
}
