using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class Product
    {
        public int ProductID { get; set; }
        public Product? DependentProduct { get; set; } // Optional dependency
        public string? ProductType{ get; set; }

        public Product()//constructor
        {

        }

        public void InputProductInfoCreate()
        {
            Console.WriteLine("=== Creating New Product ==="); ;

            Console.Write("Does this products has a dependency?(y/n): ");
            string dependentProductID = Console.ReadLine();
            dependentProductID = dependentProductID.ToLower();
            if (dependentProductID == "y")
            {
                Product product = new Product();
                product.ListProducts();
                Console.WriteLine();
                Console.Write("Write the Id of the product you want to add as a dependency: ");
                int DependentProductId = Convert.ToInt32(Console.ReadLine());
                CreateAndAddProduct(dependentProductID, DependentProductId, null);
            }
            else
            {
                Console.Write("Enter Product Type: ");
                string productType = Console.ReadLine();
                CreateAndAddProduct(dependentProductID, 0, productType);
            }
        }

        public void InputProductInfoUpdate()
        {
            ListProducts();

            Console.WriteLine("=== Updating Product ===");

            Console.Write("Enter Product ID: ");
            int ProductId = int.Parse(Console.ReadLine());

            Console.WriteLine("Press Enter to keep the current value.");

            Console.Write("Enter New Product Type: ");
            string productType = Console.ReadLine();

            UpdateProduct(ProductId, productType);


        }
        public void CreateAndAddProduct(string dependentProductID, int DependentProductId, string productType)
        {

            if (dependentProductID == "y")
            {
                Product productDependency = DataRepository.Products.FirstOrDefault(p => p.ProductID == DependentProductId);
                Product newProduct = new Product();
                newProduct.SetCompanyInfo(productDependency.ProductType, productDependency);
                DataRepository.Products.Add(newProduct);
                Console.WriteLine($"Product created successfully with ID: {newProduct.ProductID}");
            }
            else
            {
                Product newProduct = new Product();
                newProduct.SetCompanyInfo(productType, null);
                DataRepository.Products.Add(newProduct);
                Console.WriteLine($"Product created successfully with ID: {newProduct.ProductID}");
            }
        }

        public void SetCompanyInfo(string productType, Product dependentProductID)
        {
            ProductID = DataRepository.Products.Count + 1;
            ProductType = productType;
            DependentProduct = dependentProductID;
        }

        //bloquei este porque nao faz muito sentido, como os produtos dependetes sao sempre d mesmo tipo, ao alterar se houver melahres deles entercalados, deveria de se mudar todos, por isso para que eles nao fujam à regra nao deixo alterar o tipo de produto
        public void UpdateProduct(int ProductId, string productType)//method-Update Product
        {

            Product productToUpdate = DataRepository.Products.FirstOrDefault(c => c.ProductID == ProductId);
            if (productToUpdate == null)
            {
                Console.WriteLine("Error: Product not found!");
                return;
            }

            if (string.IsNullOrEmpty(productType))
            {
                Console.WriteLine("No changes made.");
                return;
            }

            //if (productType != "") { productToUpdate.ProductType = productType; }
            UpdateProductAndDependents(productToUpdate, productType);

            Console.WriteLine("Product and related dependencies updated successfully.");
        }

        private void UpdateProductAndDependents(Product product, string newType)
        {
            if (product.ProductType == newType)
                return;

            Console.WriteLine($"Updating Product ID {product.ProductID} to type {newType}");
            product.ProductType = newType;

            // If it has a dependency, update it too
            if (product.DependentProduct != null)
            {
                UpdateProductAndDependents(product.DependentProduct, newType);
            }

            // Find products that depend on this product
            var dependentProducts = DataRepository.Products
                .Where(p => p.DependentProduct != null && p.DependentProduct.ProductID == product.ProductID)
                .ToList();

            foreach (var dependent in dependentProducts)
            {
                UpdateProductAndDependents(dependent, newType);
            }
        }

        public void ListProducts()
        {
            Console.WriteLine("=== List of Products ===");
            foreach (var product in DataRepository.Products)
            {
                Console.WriteLine($"Product ID: {product.ProductID}, Product Type: {product.ProductType}, Dependent Product ID: {product.DependentProduct?.ProductID}");
            }
        }
    }    
}