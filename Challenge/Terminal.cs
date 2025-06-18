using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class Terminal
    {
        public Terminal()
        {
        }
        public void Display()
        {
            Console.WriteLine("=======Welcome to the CRM=======");
            Console.WriteLine("1 - Create Company");
            Console.WriteLine("2 - Update Company");
            Console.WriteLine("3 - List Companies");
            Console.WriteLine("4 - Create Product");
            Console.WriteLine("5 - Update Product");
            Console.WriteLine("6 - List Products");
            Console.WriteLine("7 - Create Lead");
            Console.WriteLine("8 - Update Lead");
            Console.WriteLine("9 - List Leads");
            Console.WriteLine("10 - Convert Lead to Proposal");
            Console.WriteLine("11 - Update Proposal");
            Console.WriteLine("12 - Add Product to Proposal");
            Console.WriteLine("13 - List Proposals");
            Console.WriteLine("14 - Exit");
        }

        public void Start()
        {
            while (true)
            {
                Display();
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Company createCompany = new Company();
                        createCompany.InputCompanyInfoCreate();
                        break;

                    case "2":
                        Company updateCompany = new Company();
                        updateCompany.InputCompanyInfoUpdate();
                        break;
                    case "3":
                        Company listCompanies = new Company();
                        listCompanies.ListCompanies();
                        break;

                    case "4":
                        Product createProduct = new Product();
                        createProduct.InputProductInfoCreate();
                        break;

                    case "5":
                        Product updateProduct = new Product();
                        updateProduct.InputProductInfoUpdate();
                        break;

                    case "6":
                        Product listProduct = new Product();
                        listProduct.ListProducts();
                        break;

                    case "7":
                        Lead createlead = new Lead();
                        createlead.InputLeadInfoCreate();
                        break;

                    case "8":
                        Lead updatelead = new Lead();
                        updatelead.InputLeadInfoUpdate();
                        break;
                    case "9":
                        Lead listLeads = new Lead();
                        listLeads.ListLeads();
                        break;

                    case "10":
                        Proposal createProposal = new Proposal();
                        createProposal.InputProposalInfoConvert();
                        break;

                    case "11":
                        Proposal updateProposals = new Proposal();
                        updateProposals.InputProposalInfoUpdate();
                        break;

                    case "12":
                        Proposal AddProdusctsProposals = new Proposal();
                        AddProdusctsProposals.InputProposalInfoAddProduct();
                        break;

                    case "13":
                        Proposal listProposals = new Proposal();
                        listProposals.ListPorposals();
                        break;

                    case "14":
                        Console.WriteLine("Exiting...");
                        return;
                        
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
