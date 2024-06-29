using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum IdleAbbreviation
{
    k,
    M,
    B,
    T,
    q,
    Q,
    s,
    S,
    o,
    N,
    d,
    U,
    D,
    Td
}

public static class DoubleUtilities
{
    public static string ToIdleNotation(double value)
    {

        if (value < 1000)
            return value.ToString("F2");

        double tValue = value;
        int abbreviationIndex = -1;

        while (tValue > 1000)
        {
            tValue /= 1000;
            abbreviationIndex++;

            // Ýndeks sýnýrlarýný kontrol et
            if (abbreviationIndex >= System.Enum.GetValues(typeof(IdleAbbreviation)).Length)
                return ToScientificNotation(value);
        }

        // Doðru indeksi kullanarak enum deðerini al
        IdleAbbreviation abbreviation = (IdleAbbreviation)abbreviationIndex;
        string idleAbbreviation = abbreviation.ToString();

        return tValue.ToString("F2") + idleAbbreviation;

        //if (value < 1000)
        //    return value.ToString("F2");

        //double tValue = value;
        //int abbreviationIndex = -1;

        //while (tValue > 1000)
        //{
        //    tValue /= 1000;
        //    abbreviationIndex++;
        //}

        //if (abbreviationIndex >= System.Enum.GetValues(typeof(IdleAbbreviation)).Length)
        //    return ToScientificNotation(value);

        //string idleAbbreviation = System.Enum.GetValues(typeof(IdleAbbreviation)).GetValue(abbreviationIndex).ToString();

        //return tValue.ToString("F2") + idleAbbreviation;
    }

    public static string ToScientificNotation(double value)
    {
        int exponent = 0;
        double tValue = value;

        if (value < 10)
            return value.ToString("F2");

        while(tValue > 10)
        {
            tValue /= 10;
            exponent++;
        }

        return tValue.ToString("F2") + "e" + exponent;
    }

    public static string ToCustomScientificNotation(double value)
    {
        if (value < Mathf.Pow(10, 12))
            return ToSeparatedThousands(value);

        return ToScientificNotation(value);
    }

    public static string ToSeparatedThousands(double value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();

        nfi.NumberGroupSeparator = " ";
        nfi.NumberDecimalSeparator = ".";

        return value.ToString("N", nfi);
    }

    internal static double ToIdleNotation(string value)
    {
        throw new NotImplementedException();
    }
}
