using Xunit;
using Challenge;
public class TestClass
{
    [Fact]
    public void Test()
    {
        /*======================================Set Up Rules======================================*/

        //SetRequired method, if "Portugal" then nif must be required
        ValidationConfig.ValidationRules.SetRequired("Company", "NIF", (obj) => ((Company)obj).Country == "Portugal", true);
        ValidationConfig.ValidationRules.SetRequired("Company", "NIF", (obj) => ((Company)obj).Country == "Espanha", true);
        ValidationConfig.ValidationRules.SetRequired("Company", "Country", (obj) => true, true);

        /*======================================Test Company======================================*/

        //Arrange
        DataRepository.Companies.Clear();
        Company company = new Company();

        //Test if rules blocked the creation of the company//
        //act
        company.CreateAndAddCompany("", "Rua 45 agosto", "Espanha", "Cesar", "email@gmail.pt");

        //Assert
        Assert.Empty(DataRepository.Companies);//Country "Espanha" then NIF must be required

        //act
        company.CreateAndAddCompany("", "Rua 45 agosto", "Portugal", "Cesar", "email@gmail.pt");

        //Assert
        Assert.Empty(DataRepository.Companies);//Country "Portugal" then NIF must be required

        //act
        company.CreateAndAddCompany("", "Rua 45 agosto", "", "Cesar", "email@gmail.pt");

        //Assert
        Assert.Empty(DataRepository.Companies);//Country must be required at all times

        //Test Create Company//
        //act
        company.CreateAndAddCompany("254657567", "Rua 25 de Aril", "Portugal", "Cesar", "email@gmail.pt");

        //Assert
        Assert.Single(DataRepository.Companies);
        Company addedCompany = DataRepository.Companies[0];
        Assert.Equal(1, addedCompany.ID);
        Assert.Equal("254657567", addedCompany.NIF);
        Assert.Equal("Rua 25 de Aril", addedCompany.Address);
        Assert.Equal("Portugal", addedCompany.Country);
        Assert.Equal("Draft", addedCompany.Status);
        Assert.Equal("Cesar", addedCompany.StakeHolder);
        Assert.Equal("email@gmail.pt", addedCompany.Contact);

        //Test Create Duplicated Company Nif//
        //act
        company.CreateAndAddCompany("254657567", "Rua 24 de Julho", "Portugal", "Ben", "Ben@gmail.com");

        //Assert
        Assert.Single(DataRepository.Companies);//Test if company is not added, still just one company

        
        //Test Update Company//
        //act
        company.UpdateCompany(1, "535345345", "", "", "Cesar Roncon", "");

        //Assert
        Assert.Equal("535345345", addedCompany.NIF);//Test NIF Updated
        Assert.Equal("Cesar Roncon", addedCompany.StakeHolder);//Test StakeHolder Updated

        /*======================================Test Lead======================================*/

        //Arrange
        Lead lead = new Lead();

        ///Test Create Lead//
        //act
        lead.CreateAndAddLead(1, "Industry");

        //Assert
        Assert.Single(DataRepository.Leads);
        Lead addedLead = DataRepository.Leads[0];
        Assert.Equal(1, addedLead.LeadID);
        Assert.Equal(1, addedLead.Company.ID);
        Assert.Equal("Portugal", addedLead.Country);
        Assert.Equal("Industry", addedLead.BusinessType);
        Assert.Equal("Draft", addedLead.Status);

        //Test Update Lead//
        //act
        lead.UpdateLead(1, "Industry4.0");

        //Assert
        Assert.Equal("Industry4.0", addedLead.BusinessType);

        /*======================================Test Product======================================*/

        //Arrange
        Product product = new Product();

        //act
        product.CreateAndAddProduct("n", 0, "Electronics");
        product.CreateAndAddProduct("n", 0, "Machinery");
        product.CreateAndAddProduct("y", 1, "");
        product.CreateAndAddProduct("y", 2, "");
        product.CreateAndAddProduct("y", 3, "");
        product.CreateAndAddProduct("y", 5, "");


        //Assert Create 4 Products, 2 of them dependent
        Product addedProduct1 = DataRepository.Products[0];
        Assert.Equal(1, addedProduct1.ProductID);
        Assert.Equal("Electronics", addedProduct1.ProductType);
        Assert.Null(addedProduct1.DependentProduct);

        Product addedProduct2 = DataRepository.Products[1];
        Assert.Equal(2, addedProduct2.ProductID);
        Assert.Equal("Machinery", addedProduct2.ProductType);
        Assert.Null(addedProduct2.DependentProduct);

        Product addedProduct3 = DataRepository.Products[2];
        Assert.Equal(3, addedProduct3.ProductID);
        Assert.Equal("Electronics", addedProduct3.ProductType);
        Assert.Equal(1, addedProduct3.DependentProduct.ProductID);

        Product addedProduct4 = DataRepository.Products[3];
        Assert.Equal(4, addedProduct4.ProductID);
        Assert.Equal("Machinery", addedProduct4.ProductType);
        Assert.Equal(2, addedProduct4.DependentProduct.ProductID);

        Product addedProduct5 = DataRepository.Products[4];
        Assert.Equal(5, addedProduct5.ProductID);
        Assert.Equal("Electronics", addedProduct5.ProductType);
        Assert.Equal(3, addedProduct5.DependentProduct.ProductID);

        Product addedProduct6 = DataRepository.Products[5];
        Assert.Equal(6, addedProduct6.ProductID);
        Assert.Equal("Electronics", addedProduct6.ProductType);
        Assert.Equal(5, addedProduct6.DependentProduct.ProductID);

        //Test Update Product, and every dependency and dependent
        Product updatedProduct = new Product();
        updatedProduct.UpdateProduct(3, "Development");
        Assert.Equal("Development", addedProduct1.ProductType);
        Assert.Equal("Development", addedProduct3.ProductType);
        Assert.Equal("Development", addedProduct5.ProductType);
        Assert.Equal("Development", addedProduct6.ProductType);



        /*======================================Test Proposal======================================*/

        //Arrange
        Proposal proposal = new Proposal();

        //act
        proposal.ConvertLeadToProposal(1);

        //Assert Convert Lead 1 to Proposal
        Assert.Single(DataRepository.Proposals);
        Proposal addedProposal = DataRepository.Proposals[0];

        Assert.Equal("Active", addedLead.Status);

        Assert.Equal(1, addedProposal.ProposalID);
        Assert.Equal(addedLead.LeadID, addedProposal.Lead.LeadID);
        Assert.Empty(addedProposal.Products);
        Assert.Equal(0, addedProposal.ProductionCost);
        Assert.Equal(0, addedProposal.MonthlyProducedProducts);
        Assert.Equal(0, addedProposal.ExpectedMonthlyProfit);
        Assert.Equal("Draft", addedProposal.Status);
        Assert.Equal(addedLead.Country, addedProposal.Country);
        Assert.Equal(addedLead.BusinessType, addedProposal.BusinessType);

        //Test Update Proposal
        //act
        proposal.UpdateProposal(1, "1000", "100", "10000");

        //Assert
        Assert.Equal(1000, addedProposal.ProductionCost);
        Assert.Equal(100, addedProposal.MonthlyProducedProducts);
        Assert.Equal(10000, addedProposal.ExpectedMonthlyProfit);
        Assert.Equal("Draft", addedProposal.Status);//Test if proposal status is Draft

        //Test Add Product to Proposal
        //act
        proposal.AddProductToProposal(1, 2);//Product 2 dosent have dependent product
        proposal.AddProductToProposal(1, 6);//Product 6 has dependent product, product 6 depend on 5 depend on 3 depend on 1

        //Assert
        Assert.Equal(5, addedProposal.Products.Count);
        Assert.Equal(2, addedProposal.Products[0].ProductID);
        Assert.Equal(1, addedProposal.Products[1].ProductID);
        Assert.Equal(3, addedProposal.Products[2].ProductID);
        Assert.Equal(5, addedProposal.Products[3].ProductID);
        Assert.Equal(6, addedProposal.Products[4].ProductID);

        Assert.Equal("Active", addedProposal.Status);//Test if proposal status is Active

        Assert.Equal("Active", addedCompany.Status);//Test if company status is Active

        



        









    }
}
