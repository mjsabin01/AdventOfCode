using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Template
{
    public void Run()
    {
        Stopwatch s = new Stopwatch();
        s.Start();

        var lines = TestInput.Split("\r\n");
        Part1(lines);

        s.Stop();
        Console.WriteLine("Took: {0} to complete.", s.Elapsed);
    }

    public void Part1(string[] lines)
    {

    }

    public void Part2(string[] lines)
    {

    }

    #region TestInput

    public string TestInput =
        @"";

    #endregion

    #region Input

    public string Input =
        @"";

    #endregion 
}

