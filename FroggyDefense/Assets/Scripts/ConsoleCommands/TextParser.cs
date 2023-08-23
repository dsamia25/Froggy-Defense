using System.Text.RegularExpressions;

public class TextParser
{

    /// <summary>
    /// Parses out a command line
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static CommandArgsPair ParseCommand(string line)
    {
        line = line.ToUpper();
        string[] input = Split(line);

        if (input.Length <= 0)
        {
            return new CommandArgsPair();
        }

        string[] args;
        if (input.Length > 1)
        {
            args = new string[input.Length - 1];
            System.Array.Copy(input, 1, args, 0, args.Length);
        } else
        {
            args = new string[0];
        }

        return new CommandArgsPair(input[0], args);
    }

    /// <summary>
    /// Splits the line by spaces.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static string[] Split(string line)
    {
        return Clean(line).Split(' ');
    }

    /// <summary>
    /// Cleans the string of any ' and " characters. Just removes them.
    /// Also trims excess spaces.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static string Clean(string line)
    {
        line = line.Replace("\'", "").Replace("\"", "");
        line = Regex.Replace(line, @"\s+", " ");
        return line.Trim();
    }

    /// <summary>
    /// Holds the string command with the arguments in a string array.
    /// </summary>
    public struct CommandArgsPair
    {
        public string command;
        public string[] args;

        internal CommandArgsPair (string _command, string[] _args)
        {
            command = _command;
            args = _args;
        }
    }
}
