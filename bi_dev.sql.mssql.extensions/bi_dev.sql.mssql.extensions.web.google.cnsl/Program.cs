using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bi_dev.sql.mssql.extensions.web.google.drive;
using CommandLine;
using CommandLine.Text;

namespace bi_dev.sql.mssql.extensions.web.google.cnsl
{

    class Program
    {
        static void Main(string[] args)
        {
            var files = Utils.getFiles("ya29.A0ARrdaM-4TNZZ5x6sYbdzit4f66V9MoSeXoj5DsY58BGxppuCfjUj3p3BDsLtAyy26fLdN0iQiBf5UXuu9Mv1fV15pbqw6YTqQj5VMvzhAFCLE5jc33HHwb750raWN_D665fRPYXnGbhcqRVQUOYleITXZKbI");
            var file = files.FirstOrDefault(x => x.Id == "19vT59V4cSq84e6L38tSvyND2uMO3v3AD");
        }
       
    }
}
