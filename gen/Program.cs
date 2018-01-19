using System;

static partial class Program
{
    static int Main(string[] args)
    {
        try
        {
            foreach (var s in GenerateDataSelectors())
                Console.Write(s);
            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetBaseException().Message);
            return 0xbad;
        }
    }
}
