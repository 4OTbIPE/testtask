using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Temp
    {
        public double night { get; set; }
    }

    public class FeelsLike
    {
        public double night { get; set; }
    }

    public class List
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public Temp temp { get; set; }
        public FeelsLike feels_like { get; set; }
    }

    public class Root
    {
        public City city { get; set; }
        public string cod { get; set; }
        public double message { get; set; }
        public int cnt { get; set; }
        public List<List> list { get; set; }
    }


}
