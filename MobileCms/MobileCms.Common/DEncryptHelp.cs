using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Security;

namespace MobileCms.Common
{
    public class DEncryptHelp
    {
        public static string Decrypt(string encrypted, string key)
        {
            return Decrypt(encrypted, key, Encoding.Default);
        }

        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            return new TripleDESCryptoServiceProvider { Key = MakeMD5(key), Mode = CipherMode.ECB }.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            return new TripleDESCryptoServiceProvider { Key = MakeMD5(key), Mode = CipherMode.ECB }.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        public static string Encrypt(string original, int encryptFormat)
        {
            switch (encryptFormat)
            {
                case 0:
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(original, "SHA1");

                case 1:
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(original, "MD5");
            }
            return "";
        }

        public static string Encrypt(string original, string key)
        {
            byte[] buff = Encoding.Default.GetBytes(original);
            byte[] kb = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        public static string GetRandomNumber()
        {
            string randomNumber = "";
            randomNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            randomNumber = randomNumber.Substring(3, 3);

            Random rdm = new Random();
            //randomNumber = randomNumber + rdm.Next(0x989680, 0x5f5e0ff).ToString();
            rdm = null;
            return randomNumber;
        }

        public static string GetRandomNumberKey()
        {
            string randomNumber = "";
            randomNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Random rdm = new Random();
            //randomNumber = randomNumber + rdm.Next(0x989680, 0x5f5e0ff).ToString();
            rdm = null;
            return randomNumber;
        }

        public static string GetRandomNumber(int length, bool isSleep)
        {
            if (isSleep)
            {
                Thread.Sleep(3);
            }
            string result = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result = result + random.Next(10).ToString();
            }
            return result;
        }

        public static string GetRandWord(int length)
        {
            //string checkCode = string.Empty;
            //Guid randSeedGuid = Guid.NewGuid();
            //Random random = new Random(BitConverter.ToInt32(randSeedGuid.ToByteArray(), 0));
            //for (int i = 0; i < length; i++)
            //{
            //    char code;
            //    int number = random.Next(1000);
            //    if ((number % 2) == 0)
            //    {
            //        code = (char) (0x30 + ((ushort) (number % 10)));
            //    }
            //    else
            //    {
            //        code = (char) (0x41 + ((ushort) (number % 0x1a)));
            //    }
            //    checkCode = checkCode + code.ToString();
            //}
            //return checkCode;

            string StrOf = GetRndStrOnlyFor(length, true, true);

            System.Random RandomObj = new System.Random(GetNewSeed());
            string buildRndCodeReturn = null;
            for (int i = 0; i < length; i++)
            {
                buildRndCodeReturn += StrOf.Substring(RandomObj.Next(0, StrOf.Length - 1), 1);
            }
            return buildRndCodeReturn;

        }

        public static string GetRandWordNum(int length)
        {
            string StrOf = GetRndStrOnlyFor(length, false, true);

            System.Random RandomObj = new System.Random(GetNewSeed());
            string buildRndCodeReturn = null;
            for (int i = 0; i < length; i++)
            {
                buildRndCodeReturn += StrOf.Substring(RandomObj.Next(0, StrOf.Length - 1), 1);
            }
            return buildRndCodeReturn;

        }

        private static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        public static string GetRndStrOnlyFor(int LenOf, bool bUseUpper, bool bUseNumber)
        {
            //string sCharLow = "abcdefghijklmnopqrstuvwxyz";
            string sCharUpp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string sNumber = "0123456789";
            //string strTmp = sCharLow;
            string strTmp = "";
            if (bUseUpper)
                strTmp += sCharUpp;
            if (bUseNumber)
                strTmp += sNumber;

            return strTmp;
        }

        public static string GetPart3Str()
        {
            string str = String.Empty;

            DateTime now = DateTime.Now;

            string second = now.Second.ToString();
            string millisecond = now.Millisecond.ToString();

            if (second.Length == 1)
            {
                second = "0" + second;
            }

            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }

            str = second + millisecond;

            return str;
        }


        public static byte[] MakeMD5(byte[] original)
        {
            byte[] keyhash = new MD5CryptoServiceProvider().ComputeHash(original);
            return keyhash;
        }

        public static string MakeMD5(string original, string encoding)
        {
            byte[] byteOriginal = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(encoding).GetBytes(original));
            StringBuilder ciphertext = new StringBuilder(0x20);
            for (int i = 0; i < byteOriginal.Length; i++)
            {
                ciphertext.Append(byteOriginal[i].ToString("x").PadLeft(2, '0'));
            }
            return ciphertext.ToString();
        }
    }
}
