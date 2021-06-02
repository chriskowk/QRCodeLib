using System;
namespace ThoughtWorks.QRCode.Codec.Reader.Pattern
{

    /// <summary> This class returns the position of the position patterns</summary>
    public class LogicalSeed
    {
        /// <summary> The positions</summary>
        private static int[][] _seed;

        /// <summary> Returns all the seeds for a version</summary>
        public static int[] getSeed(int version)
        {
            return (_seed[version - 1]);
        }

        /// <summary> Returns a seed for a version and a pattern number</summary>
        public static int getSeed(int version, int patternNumber)
        {
            return (_seed[version - 1][patternNumber]);
        }
        /// <summary> The static constructor instanciates the values</summary>
        static LogicalSeed()
        {
            {
                _seed = new int[40][];
                _seed[0] = new int[] { 6, 14 };
                _seed[1] = new int[] { 6, 18 };
                _seed[2] = new int[] { 6, 22 };
                _seed[3] = new int[] { 6, 26 };
                _seed[4] = new int[] { 6, 30 };
                _seed[5] = new int[] { 6, 34 };
                _seed[6] = new int[] { 6, 22, 38 };
                _seed[7] = new int[] { 6, 24, 42 };
                _seed[8] = new int[] { 6, 26, 46 };
                _seed[9] = new int[] { 6, 28, 50 };
                _seed[10] = new int[] { 6, 30, 54 };
                _seed[11] = new int[] { 6, 32, 58 };
                _seed[12] = new int[] { 6, 34, 62 };
                _seed[13] = new int[] { 6, 26, 46, 66 };
                _seed[14] = new int[] { 6, 26, 48, 70 };
                _seed[15] = new int[] { 6, 26, 50, 74 };
                _seed[16] = new int[] { 6, 30, 54, 78 };
                _seed[17] = new int[] { 6, 30, 56, 82 };
                _seed[18] = new int[] { 6, 30, 58, 86 };
                _seed[19] = new int[] { 6, 34, 62, 90 };
                _seed[20] = new int[] { 6, 28, 50, 72, 94 };
                _seed[21] = new int[] { 6, 26, 50, 74, 98 };
                _seed[22] = new int[] { 6, 30, 54, 78, 102 };
                _seed[23] = new int[] { 6, 28, 54, 80, 106 };
                _seed[24] = new int[] { 6, 32, 58, 84, 110 };
                _seed[25] = new int[] { 6, 30, 58, 86, 114 };
                _seed[26] = new int[] { 6, 34, 62, 90, 118 };
                _seed[27] = new int[] { 6, 26, 50, 74, 98, 122 };
                _seed[28] = new int[] { 6, 30, 54, 78, 102, 126 };
                _seed[29] = new int[] { 6, 26, 52, 78, 104, 130 };
                _seed[30] = new int[] { 6, 30, 56, 82, 108, 134 };
                _seed[31] = new int[] { 6, 34, 60, 86, 112, 138 };
                _seed[32] = new int[] { 6, 30, 58, 86, 114, 142 };
                _seed[33] = new int[] { 6, 34, 62, 90, 118, 146 };
                _seed[34] = new int[] { 6, 30, 54, 78, 102, 126, 150 };
                _seed[35] = new int[] { 6, 24, 50, 76, 102, 128, 154 };
                _seed[36] = new int[] { 6, 28, 54, 80, 106, 132, 158 };
                _seed[37] = new int[] { 6, 32, 58, 84, 110, 136, 162 };
                _seed[38] = new int[] { 6, 26, 54, 82, 110, 138, 166 };
                _seed[39] = new int[] { 6, 30, 58, 86, 114, 142, 170 };
            }
        }
    }
}