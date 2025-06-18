using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class Lead
    {
        public int LeadID { get; set; }
        public Company Company { get; set; }     // Reference to associated company
        public string? Country { get; set; }      // Inherited from company
        public string? BusinessType { get; set; } // e.g., "Industry", "Retail"
        public string? Status { get; set; }       // e.g., "Draft", "Active"
        public Lead()//constructor
        {

        }

        public void InputLeadInfoCreate()
        {
            Console.WriteLine("=== Creating New Lead ===");

            Company company = new Company();
            company.ListCompanies();

            Console.Write("Enter Company ID: ");
            int companyID = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Business Type: ");
            string businessType = Console.ReadLine();

            CreateAndAddLead(companyID, businessType);
        }

        public void InputLeadInfoUpdate()
        {
            Console.WriteLine("=== Updating Lead ===");

            Company company = new Company();
            company.ListCompanies();
            ListLeads();

            Console.Write("Enter Lead ID: ");
            int LeadId = int.Parse(Console.ReadLine());

            Console.WriteLine("Press Enter to keep the current value.");

            Console.Write("Enter New Business Type: ");
            string businessType = Console.ReadLine();

            UpdateLead(LeadId, businessType);
        }
        
        public void CreateAndAddLead(int companyID, string businessType)
        {
            //get the company with this id
            Company selectedCompany = DataRepository.Companies.FirstOrDefault(c => c.ID == companyID);
            if (selectedCompany == null)
            {
                Console.WriteLine("Error: Company with this ID not found!");
                return;
            }

            string country = selectedCompany.Country;
            string status = "Draft";

            Lead newLead = new Lead();
            newLead.SetLeadInfo(selectedCompany, country, businessType, status);
            DataRepository.Leads.Add(newLead);
            Console.WriteLine($"Company created successfully with ID: {newLead.LeadID}");
        }

        public void SetLeadInfo(Company company, string country, string businessType, string status)
        {
            LeadID = DataRepository.Leads.Count + 1;
            Company = company;
            Country = country;
            BusinessType = businessType;
            Status = status;
        }
        public void UpdateLead(int LeadId, string businessType)
        {

            Lead leadToUpdate = DataRepository.Leads.FirstOrDefault(c => c.LeadID == LeadId);
            if (leadToUpdate == null)
            {
                Console.WriteLine("Error: Lead not found!");
                return;
            }


            if (businessType != "") { leadToUpdate.BusinessType = businessType; }
        }

        public void ListLeads()
        {
            Console.WriteLine("=== List of Leads ===");
            foreach (var lead in DataRepository.Leads)
            {
                Console.WriteLine($"ID: {lead.LeadID}, Company: {lead.Company.ID}, Country: {lead.Country}, Business Type: {lead.BusinessType}, Status: {lead.Status}");
            }
        }
    }
}