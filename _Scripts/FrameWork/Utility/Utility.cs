using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utility
{
    static public string AES_KEY = "";
    static public string AES_IV = "";

    public static void SetAES(string key, string iv)
    {
        AES_KEY = key;
        AES_IV = iv;
    }

    public static string Encrypt(string plainText, string key, string iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText, string key, string iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    static TimeSpan m_GapTime;


    static public DateTime DateTimeEx(string str, System.Globalization.CultureInfo info)
    {
        string[] formats =
        {
            "dd/MM/yyyy HH:mm:ss",
            "dd-MM-yyyy HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH.mm.ss",
            "yyyy-MM-dd tt HH:mm:ss",
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss.ffffff",
            "yyyy-MM-dd tt HH:mm:ss.ffffff",
            "yyyy - MM - dd HH: mm:ss",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy/MM/dd HH:mm",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff"
        };


        bool ret;
        DateTime result;
        try
        {
            if (str.Equals("null") == true)
                return new DateTime();

            ret = DateTime.TryParseExact(str, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result);
            if (ret == false)
            {
                bool bPm = false;
                bool bAm = false;

                if (str.Contains("PM 12"))
                    bPm = true;

                if (str.Contains("AM 12"))
                    bAm = true;

                string pattern = "[^0-9 /:-]";
                string replacement = "";
                Regex rgx = new Regex(pattern);
                string strTime = rgx.Replace(str, replacement);

                if (DateTime.TryParseExact(strTime, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result) == false)
                    result = DateTime.Parse(strTime, System.Globalization.CultureInfo.InvariantCulture);

                if (bPm == true)
                {
                    result = result.AddHours(12);
                }
                else if (bAm == true)
                {
                    result = result.AddHours(-12);
                }

            }
        }
        catch
        {
            return new DateTime();
        }

        return result;
    }

    static public void SetServerTime(string time)
    {
        DateTime cur_time = DateTime.UtcNow;

        DateTime server_time = DateTimeEx(time, System.Globalization.CultureInfo.InvariantCulture);
        m_GapTime = (server_time - cur_time);

        spanBefore = new TimeSpan(DateTime.UtcNow.Ticks);
    }

    static TimeSpan spanBefore = new TimeSpan(DateTime.UtcNow.Ticks);
    static public DateTime GetCurTime()
    {
        TimeSpan span = new TimeSpan(DateTime.UtcNow.Ticks);
        TimeSpan spanGap = spanBefore - span;

        if (Math.Abs(spanGap.Minutes) >= 2)
        {
            // 서버에 시간 요청
            span = spanBefore;

            DateTime dt = new DateTime();
            dt = dt.AddSeconds(Math.Floor(span.TotalSeconds));
            return dt.AddSeconds((int)m_GapTime.TotalSeconds);
        }
        else
        {

            spanBefore = span;

            DateTime dt = new DateTime();
            dt = dt.AddSeconds(Math.Floor(span.TotalSeconds));
            return dt.AddSeconds((int)m_GapTime.TotalSeconds);

        }
    }


    static public string LoadFile(string filePath)
    {
        string data = "";
        data = File.ReadAllText(filePath);
        return data;
    }

    static public void SaveFile(string filePath, string data)
    {
        File.WriteAllText(filePath, data);
    }

    static public byte[] LoadBinFile(string filePath)
    {
        string str = "";
        byte[] data;

        data = File.ReadAllBytes(filePath);
        return data;
    }

    static public void SaveBinFile(string filePath, byte[] data)
    {
        File.WriteAllBytes(filePath, data);
    }

    static public string GetByTheThousands(int Number)
    {
        return string.Format("{0:#,0}", Number);
    }

    public static void ShuffleList<T>(List<T> list)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 0; index < list.Count; ++index)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            tmp = list[random1];
            list[random1] = list[random2];
            list[random2] = tmp;
        }
    }
    public static bool CheckProbability(float probability)
    {
        // 0과 1 사이의 난수 생성
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        // 생성된 난수가 주어진 확률보다 작거나 같으면 true 반환
        return randomValue <= probability;
    }

    public static float ApplyDefenseReduction(float damage, float def)
    {
        float result = damage * (10f / (10f + (def * 0.1f)));
        return result;
    }
    public static string FormatNumber(float number)
    {
        if (number < 1000)
        {
            return Mathf.FloorToInt(number).ToString();
        }
        else if (number < 1000000)
        {
            return (number / 1000f).ToString("n1") + "K";
        }
        else if (number < 1000000000)
        {
            return (number / 1000000f).ToString("n1") + "M";
        }
        else
        {
            return (number / 1000000000f).ToString("n1") + "B";
        }
    }
}
