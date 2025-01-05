public static class BinaryHexaConverter
{
    static readonly string hexaNumbers = "0123456789ABCDEF";
    static char[] hexaChars;

    public static string ConvertBinaryToHexa(bool[] binaryToConvert)
    {
        hexaChars = hexaNumbers.ToCharArray();

        int numbersMissingFromDiv4 = binaryToConvert.Length % 4;
        int current4Char = 0;
        string binaryString4 = string.Empty;
        string hexaString = string.Empty;

        if (numbersMissingFromDiv4 != 0)
        {
            numbersMissingFromDiv4 = 4 - numbersMissingFromDiv4;
        }

        while(numbersMissingFromDiv4 > 0)
        {
            binaryString4 += "0";
            current4Char++;
            numbersMissingFromDiv4--;
        }

        for (int i = 0; i < binaryToConvert.Length; i++)
        {
            if (binaryToConvert[i])
            {
                binaryString4 += "1";
            }
            else
            {
                binaryString4 += "0";
            }
            current4Char++;

            if (current4Char > 3)
            {
                char[] binary4Chars = binaryString4.ToCharArray();
                int hexaCharacter = (int.Parse(binary4Chars[0].ToString()) * 8) + (int.Parse(binary4Chars[1].ToString()) * 4) + (int.Parse(binary4Chars[2].ToString()) * 2) + int.Parse(binary4Chars[3].ToString());

                hexaString += hexaChars[hexaCharacter];
                binaryString4 = string.Empty;
                current4Char = 0;
            }
        }

        return hexaString;
    }

    public static bool[] ConvertHexaToBinary(string hexaToConvert, int boolLength)
    {
        hexaChars = hexaNumbers.ToCharArray();

        bool[] binaryBools = new bool[boolLength];
        int numbersMissingFromDiv4 = boolLength % 4;
        char[] hexaCharsArray = hexaToConvert.ToCharArray();
        string binaryString = string.Empty;

        if (numbersMissingFromDiv4 != 0)
        {
            numbersMissingFromDiv4 = 4 - numbersMissingFromDiv4;
        }

        for (int i = 0; i < hexaCharsArray.Length; i++)
        {
            bool hasFoundNumber = false;
            int currentNumber = 0;

            while (!hasFoundNumber)
            {
                if (hexaCharsArray[i] == hexaChars[currentNumber])
                {
                    hasFoundNumber = true;
                }
                else
                {
                    currentNumber++;
                }
            }

            if ((currentNumber - 8) >= 0) { binaryString += "1"; currentNumber -= 8; }
            else binaryString += "0";

            if ((currentNumber - 4) >= 0) { binaryString += "1"; currentNumber -= 4; }
            else binaryString += "0";

            if ((currentNumber - 2) >= 0) { binaryString += "1"; currentNumber -= 2; }
            else binaryString += "0";

            if ((currentNumber - 1) >= 0) { binaryString += "1"; currentNumber -= 1; }
            else binaryString += "0";
        }

        char[] binaryCharArray = binaryString.ToCharArray();
        for (int i = numbersMissingFromDiv4; i < binaryCharArray.Length; i++)
        {
            switch(binaryCharArray[i])
            {
                case '1':
                    binaryBools[i - numbersMissingFromDiv4] = true;
                    break;
                case '0':
                    binaryBools[i - numbersMissingFromDiv4] = false;
                    break;
            }
        }

        return binaryBools;
    }
}
