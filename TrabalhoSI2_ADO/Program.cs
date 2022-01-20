using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using TrabalhoSI2.concrete;
using TrabalhoSI2.dal;
using TrabalhoSI2.mapper;
using TrabalhoSI2.model;
using System.Globalization;
using System.Data.SqlClient;
using TrabalhoSI2.helper;
using System.Data;
using TrabalhoSI2EF;


namespace TrabalhoSI2_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            //ADOApp adoApp = new ADOApp();
            //adoApp.Run();

            EFApp efApp= new EFApp();

            efApp.Run();


        }

    }
}
