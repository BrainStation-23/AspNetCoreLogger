using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Core.DataType
{
    public class TestExtention
    {
        public Enum1 Str { get; }

        public TestExtention(Enum1 str)
        {
            Str = str;
        }
    }

    public enum Enum1
    {
        First,
        Second,
        Third
    }



    public enum Enum2
    {
        First = 10,
        Second,
        Third
    }

    public enum Enum3
    {
        First = 31,
        Second = 22,
        Third = 13
    }
}