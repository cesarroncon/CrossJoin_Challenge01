using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class Proposal
    {
        public int ProposalID { get; set; }
        public Lead? Lead { get; set; }              // Reference to associated lead
        public List<Product>? Products { get; set; } // List of associated products
        public int ProductionCost { get; set; }
        public int MonthlyProducedProducts { get; set; }
        public int ExpectedMonthlyProfit { get; set; }
        public string? Status { get; set; }         // e.g., "Draft", "Active"
        public string? Country { get; set; }        // Inherited from lead
        public string? BusinessType { get; set; }   // e.g., "Industry", "Retail"  // Inherited from lead

        public Proposal()//constructor
        {
        }
        public void InputProposalInfoConvert()
        {
            Lead lead = new Lead();
            lead.ListLeads();
            Console.Write("Enter Lead ID: ");
            int leadID = Convert.ToInt32(Console.ReadLine());
            ConvertLeadToProposal(leadID);
        }

        public void InputProposalInfoUpdate()
        {
            ListPorposals();

            Console.WriteLine("=== Updating Proposal ===");

            Console.Write("Enter Proposal ID: ");
            int ProposalId = int.Parse(Console.ReadLine());

            Console.WriteLine("Press Enter to keep the current value.");

            Console.Write("Enter New Production Cost: ");
            string productionCost = Console.ReadLine();

            Console.Write("Enter New Monthly Produced Products: ");
            string monthlyProducedProducts = Console.ReadLine();

            Console.Write("Enter New Expected Monthly Profit: ");
            string expectedMonthlyProfit = Console.ReadLine();

            UpdateProposal(ProposalId, productionCost, monthlyProducedProducts, expectedMonthlyProfit);
        }

        public void InputProposalInfoAddProduct()
        {
            ListPorposals();

            Console.WriteLine("=== Adding Product to Proposal ===");

            Console.Write("Enter Proposal ID: ");
            int proposalId = int.Parse(Console.ReadLine());

            Product product = new Product();
            product.ListProducts();

            Console.Write("Enter Product ID to add: ");
            int productId = int.Parse(Console.ReadLine());

            AddProductToProposal(proposalId, productId);

        }

        public void ConvertLeadToProposal(int leadID)
        {

            Lead selectedLead = DataRepository.Leads.FirstOrDefault(l => l.LeadID == leadID);
            if (selectedLead == null)
            {
                Console.WriteLine("Error: Lead with this ID not found!");
                return;
            }
            selectedLead.Status = "Active";

            Proposal convertProposal = new Proposal();
            convertProposal.SetProposalInfo(selectedLead);
            DataRepository.Proposals.Add(convertProposal);

        }

        public void SetProposalInfo(Lead lead)
        {
            ProposalID = DataRepository.Proposals.Count + 1;
            Lead = lead;
            Status = "Draft";
            Country = lead.Country;
            BusinessType = lead.BusinessType;
            Products = new List<Product>(); // Initialize the Products list
        }

        public void UpdateProposal(int ProposalId, string productionCost, string monthlyProducedProducts, string expectedMonthlyProfit)//method-Update Proposal
        {

            Proposal proposalToUpdate = DataRepository.Proposals.FirstOrDefault(p => p.ProposalID == ProposalId);
            if (proposalToUpdate == null)
            {
                Console.WriteLine("Error: Proposal not found!");
                return;
            }

            if (productionCost != "") { proposalToUpdate.ProductionCost = int.Parse(productionCost); }
            if (monthlyProducedProducts != "") { proposalToUpdate.MonthlyProducedProducts = int.Parse(monthlyProducedProducts); }
            if (expectedMonthlyProfit != "") { proposalToUpdate.ExpectedMonthlyProfit = int.Parse(expectedMonthlyProfit); }

            Console.WriteLine("Proposal updated successfully!");
            FinalizeProposal(proposalToUpdate);
        }

        public void AddProductToProposal(int proposalId, int productId)
        {

            Proposal proposalToUpdate = DataRepository.Proposals.FirstOrDefault(p => p.ProposalID == proposalId);
            if (proposalToUpdate == null)
            {
                Console.WriteLine("Error: Proposal not found!");
                return;
            }

            Product selectedProduct = DataRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (selectedProduct == null)
            {
                Console.WriteLine("Error: Product not found!");
                return;
            }

            AddProductWithDependencies(proposalToUpdate, selectedProduct);
            FinalizeProposal(proposalToUpdate);
    

        }

        private void AddProductWithDependencies(Proposal proposal, Product product)
        {
            if (proposal.Products.Any(p => p.ProductID == product.ProductID))
            {
                Console.WriteLine($"Product ID {product.ProductID} already in proposal.");
                return;
            }

            // Add dependency first, if exists
            if (product.DependentProduct != null)
            {
                AddProductWithDependencies(proposal, product.DependentProduct);
            }

            // Now add the product itself
            proposal.Products.Add(product);
            Console.WriteLine($"Added Product ID {product.ProductID} ({product.ProductType}) to Proposal ID {proposal.ProposalID}.");
        }
        
        public bool FinalizeProposal(Proposal proposalToUpdate)
        {
            if (proposalToUpdate == null)
                return false;

            if (proposalToUpdate.Lead == null)
                return false;

            if (proposalToUpdate.ProductionCost <= 0)
                return false;

            if (proposalToUpdate.MonthlyProducedProducts <= 0)
                return false;

            if (proposalToUpdate.ExpectedMonthlyProfit <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(proposalToUpdate.Country))
                return false;

            if (string.IsNullOrWhiteSpace(proposalToUpdate.BusinessType))
                return false;

            if (proposalToUpdate.Products == null || !proposalToUpdate.Products.Any())
                return false;

            proposalToUpdate.Status = "Active";
            Company company = DataRepository.Companies.FirstOrDefault(c => c.ID == proposalToUpdate.Lead.Company.ID);
            if (company != null)
            {
                company.Status = "Active";
                Console.WriteLine($"Company (ID: {company.ID}) status updated to Active!");
            }
            return true;
        }

        public void ListPorposals()
        {
            Console.WriteLine("===List of Proposals===");
            foreach (Proposal proposal in DataRepository.Proposals)
            {
                Console.WriteLine($"Proposal ID: {proposal.ProposalID}, Lead ID: {proposal.Lead.LeadID}, Country: {proposal.Country}, Business Type: {proposal.BusinessType}, Status: {proposal.Status}, Production Cost: {proposal.ProductionCost}, Monthly Produced Products: {proposal.MonthlyProducedProducts}, Expected Monthly Profit: {proposal.ExpectedMonthlyProfit}");
                Console.WriteLine("    Products:");
                foreach (var product in proposal.Products)
                {
                    Console.WriteLine($"    Product ID: {product.ProductID}, Product Type: {product.ProductType}, Dependent Product ID: {product.DependentProduct?.ProductID}");
                }
            }
        }
    }
}