using System.Collections.Generic;

public static class SortUtil<T>
{
    public delegate bool FilterFunc(T x);

    /// <summary>
    /// Filters the input list using the input function.
    /// Rejected items are collected in a separate list.
    /// If no list is input for rejected, they are not collected.
    /// If no filter is input, just returns the input list.
    /// </summary>
    /// <param name="func"></param>
    /// <param name="input"></param>
    /// <param name="rejected"></param>
    /// <returns></returns>
    public static List<T> FilterList(FilterFunc func, List<T> input, List<T> rejected)
    {
        if (input == null)
        {
            throw new System.ArgumentNullException("Input list cannot be null.");
        }

        if (func == null)
        {
            return input;
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
                // TODO: Maybe make an internal delegate method that 
                if (rejected != null) rejected.Add(input[i]);
            }
        }
        return accepted;
    }

    /// <summary>
    /// Filters the input list using the input function.
    /// Rejected items are collected in a separate list.
    /// If no list is input for rejected, they are not collected.
    /// If no filter is input, just returns the input list.
    /// </summary>
    /// <param name="func"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static List<T> FilterList(FilterFunc func, List<T> input)
    {
        return FilterList(func, input, null);
    }

    /// <summary>
    /// Filters and sorts the input list using the designated filter and comparer.
    /// </summary>
    /// <param name="func"></param>
    /// <param name="comparer"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static List<T> FilterAndSortList(FilterFunc func, Comparer<T> comparer, List<T> input)
    {
        throw new System.NotImplementedException();
    }
}