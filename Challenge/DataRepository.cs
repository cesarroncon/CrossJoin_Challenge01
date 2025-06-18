using System.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class DataRepository
    {
        public DataRepository()//constructor
        {
        }

        public static List<Company> Companies = new List<Company>();
        public static List<Lead> Leads = new List<Lead>();
        public static List<Product> Products = new List<Product>();
        public static List<Proposal> Proposals = new List<Proposal>();
    }
}