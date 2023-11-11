using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SortUtil<T>
{
    public delegate bool FilterFunc(T x);

    /// <summary>
    /// Filters the input list using the input function.
    /// Rejected items are collected in a separate list.
    /// If no list is input for rejected, they are not collected.
    /// </summary>
    /// <param name="func"></param>
    /// <param name="input"></param>
    /// <param name="rejected"></param>
    /// <returns></returns>
    public static List<T> FilterList(FilterFunc func, List<T> input, List<T> rejected)
    {
        if (rejected == null)
        {
            throw new System.ArgumentNullException("Rejected list cannot be null.");
        }

        List<T> accepted = new List<T>();
        for (int i = 0; i < input.Count; i++)
        {
            if (func.Invoke(input[i]))
            {
                accepted.Add(input[i]);
            }
            else
            {
                if (rejected != null) rejected.Add(input[i]);
            }
        }
        return accepted;
    }

    /// <summary>
    /// Filters the input list using the input function.
    /// Rejected items are collected in a separate list.
    /// If no list is input for rejected, they are not collected.
    /// </summary>
    /// <param name="func"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static List<T> FilterList(FilterFunc func, List<T> input)
    {
        List<T> accepted = new List<T>();
        for (int i = 0; i < input.Count; i++)
        {
            if (func.Invoke(input[i]))
            {
                accepted.Add(input[i]);
            }
        }
        return accepted;
    }
}
