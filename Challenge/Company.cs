using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class Company
    {
        //inicializa todos os atributos
        public int ID { get; set; }
        public string? NIF { get; set; }
        public string Address { get; set; }
        public string? Country { get; set; }
        public string? Status { get; set; }
        public string? StakeHolder { get; set; }
        public string? Contact { get; set; }//email...
        public Company()//constructor
        {
        }

        public void InputCompanyInfoCreate()
        {
            Console.WriteLine("=== Creating New Company ===");

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Country: ");
            string country = Console.ReadLine();

            Console.Write("Enter NIF: ");
            string nif = Console.ReadLine();

            Console.Write("Enter Stakeholder: ");
            string stakeholder = Console.ReadLine();

            Console.Write("Enter Contact (email): ");
            string contact = Console.ReadLine();
            
            CreateAndAddCompany(nif, address, country, stakeholder, contact);
        }
        public void InputCompanyInfoUpdate()
        {
            ListCompanies();
            Console.WriteLine("=== Updating Company ===");

            Console.Write("Enter Company ID: ");
            int companyId = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Press Enter to keep the current value.");

            Console.Write("Enter New Address: ");
            string newAddress = Console.ReadLine();

            Console.Write("Enter New Country: ");
            string newCountry = Console.ReadLine();

            Console.Write("Enter New NIF: ");
            string newNif = Console.ReadLine();
            
            Console.Write("Enter New Stakeholder: ");
            string newStakeholder = Console.ReadLine();

            Console.Write("Enter New Contact: ");
            string newContact = Console.ReadLine();
            
            UpdateCompany(companyId, newNif, newAddress, newCountry, newStakeholder, newContact);
        }
        public void CreateAndAddCompany(string nif, string address, string country, string stakeholder, string contact)
        {
            string status = "Draft";
            
            // Validar se NIF já existe
            if (DataRepository.Companies.Any(c => c.NIF == nif))
            {
                Console.WriteLine("Error: A company with this NIF already exists!");
                return;
            }

            Company newCompany = new Company()
            {
                Country = country,
                NIF = nif, // Missing NIF
            };

            var errors = ValidationConfig.ValidationRules.Validate(newCompany);

            foreach (var error in errors)
            {
                Console.WriteLine(error);
                return;
            }
            
            newCompany.SetCompanyInfo(nif, address, country, status, stakeholder, contact);
            DataRepository.Companies.Add(newCompany);
            Console.WriteLine($"Company created successfully with ID: {newCompany.ID}");
        }

        public void SetCompanyInfo(string nif, string address, string country, string status, string stakeholder, string contact)
        {
            ID = DataRepository.Companies.Count + 1;
            NIF = nif;
            Address = address;
            Country = country;
            Status = status;
            StakeHolder = stakeholder;
            Contact = contact;
        }

        public void UpdateCompany(int companyId ,string newNif, string newAddress, string newCountry, string newStakeholder, string newContact)
        {
            Company companyToUpdate = DataRepository.Companies.FirstOrDefault(c => c.ID == companyId);
            if (companyToUpdate == null)
            {
                Console.WriteLine("Error: Company not found!");
                return;
            }

            if (newNif != "") { companyToUpdate.NIF = newNif; }
            if (newAddress != "") { companyToUpdate.Address = newAddress; }
            if (newCountry != "") { companyToUpdate.Country = newCountry; }
            if (newStakeholder != "") { companyToUpdate.StakeHolder = newStakeholder; }
            if (newContact != "") { companyToUpdate.Contact = newContact; }
        }

        public void ListCompanies()
        {
            Console.WriteLine("=== List of Companies ===");
            foreach (var company in DataRepository.Companies)
            {
                Console.WriteLine($"ID: {company.ID}, Address: {company.Address}, Country: {company.Country}, NIF: {company.NIF}, Status: {company.Status}, Stakeholder: {company.StakeHolder}, Contact: {company.Contact}");
                Console.WriteLine("   Leads:");
                foreach (var lead in DataRepository.Leads)
                {
                    if (lead.Company.ID == company.ID)
                    {
                        Console.WriteLine($"    ID: {lead.LeadID}, Company: {lead.Company.ID}, Country: {lead.Country}, Business Type: {lead.BusinessType}, Status: {lead.Status}");
                    }
                }
                Console.WriteLine("   Proposals:");
                foreach (var proposal in DataRepository.Proposals)
                {
                    if (proposal.Lead.Company.ID == company.ID)
                    {
                        Console.WriteLine($"    ID: {proposal.ProposalID}, Lead: {proposal.Lead.LeadID}, Country: {proposal.Lead.Country}, Business Type: {proposal.Lead.BusinessType}, Status: {proposal.Status}, Production Cost: {proposal.ProductionCost}, Monthly Produced Products: {proposal.MonthlyProducedProducts}, Expected Monthly Profit: {proposal.ExpectedMonthlyProfit}");
                    }
                }

            }
        }
    }
}