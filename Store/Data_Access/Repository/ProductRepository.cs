using Store.Data_Access.Entity;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Data_Access.Repository
{
    public class ProductRepository
    {
        private Random rand = new Random();
        private readonly string abc = "abcdefghijklmnopqrstuvwxyz0123456789";

        #region CRUD
        public void AddProduct(Product newProduct)
        {

            Products products = new Products();
            products.Code = newProduct.Code;
            products.Name = newProduct.Name;
            products.Price = newProduct.Price;
            products.Barcode = newProduct.Barcode;

            using (Store.Data_Access.Entity.Entities db = new Entity.Entities())
            {
                db.Products.Add(products);
                db.SaveChanges();
            }
        }

        public void UpdateProduct(Product updateProduct)
        {
            using (Entities db = new Entities())
            {
                Products products = db.Products.Where(p => p.ProductID == updateProduct.ProductID).FirstOrDefault();

                if (products != null)
                {
                    products.Code = updateProduct.Code;
                    products.Name = updateProduct.Name;
                    products.Price = updateProduct.Price;
                    products.Barcode = updateProduct.Barcode;
                }

                db.SaveChanges();
            }
        }

        public Product GetProduct(int productID)
        {
            Product result = new Product();
            Products products;

            using (Entities db = new Entities())
            {
                products = db.Products.Where(p => p.ProductID == productID).FirstOrDefault();

                if (products != null)
                {
                    result.Code = products.Code;
                    result.Name = products.Name;
                    result.Price = products.Price;
                    result.Barcode = products.Barcode;
                }

                return result;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> result = new List<Product>();

            using (Entities db = new Entities())
            {
                List<Products> products = db.Products.ToList();

                foreach (var item in products)
                {
                    result.Add(new Product
                    {
                        ProductID=item.ProductID,
                        Code = item.Code,
                        Name = item.Name,
                        Price = item.Price,
                        Barcode = item.Barcode
                    });
                }
            }

            return result;
        }

        public void Delete(int productID)
        {
            using (Entities db = new Entities())
            {
                Products products = db.Products.Where(p => p.ProductID == productID).FirstOrDefault();
                if (products != null)
                {
                    db.Products.Remove(products);
                    db.SaveChanges();
                }
            }
        }

        private void DeleteAll()
        {
            using (Entities db = new Entities())
            {
                IEnumerable<Products> products = db.Products.ToList();

                foreach (var item in products)
                {
                    db.Products.Remove(item);
                }

                db.SaveChanges();

            }
        }
        #endregion

        #region GenerateProducts

        private int GenerateCode()
        {
            return (int)rand.Next(1, 100000);
        }

        private string GenerateString(int lenght)
        {
            var name = new char[lenght];

            for (int i = 0; i < lenght; i++)
            {
                name[i] = abc[rand.Next(0, abc.Length)];
            }

            return new String(name);
        }

        private string GenerateName()
        {
            int namelenght = rand.Next(5, 15);

            return GenerateString(namelenght);
        }

        private int GeneratePrice()
        {
            int price = rand.Next(1, 501);
            price *= 10;

            return price;
        }

        private string GenerateBarcode(int point)
        {
            if (point % 10 == 0)
            {
                return null;
            }

            return GenerateString(13);
        }

        private ProductUniqueEnum CheckUnique(Product product)
        {
            ProductUniqueEnum result = ProductUniqueEnum.success;

            using (Entities db = new Entities())
            {
                Products findProduct = db.Products.Where(p => p.Code == product.Code ||
                p.Name == product.Name ||(p.Barcode!=null&& p.Barcode == product.Barcode)).FirstOrDefault();

                if (findProduct != null)
                {
                    if (findProduct.Code == product.Code)
                    {
                        result = ProductUniqueEnum.codeNotUnique;
                    }
                    if (findProduct.Name == product.Name)
                    {
                        result = ProductUniqueEnum.nameNotUnique;
                    }
                    if (product.Barcode!=null && findProduct.Barcode == product.Barcode)
                    {
                        result = ProductUniqueEnum.barcodeNotUnique;
                    }
                }
            }

            return result;
        }

        public void GenerateProduct()
        {
            DeleteAll();

            int num = 0;

            while (num <= 50)
            {
                Product product = new Product
                {
                    Code = GenerateCode(),
                    Name = GenerateName(),
                    Price = GeneratePrice(),
                    Barcode = GenerateBarcode(num)
                };

                ProductUniqueEnum status = CheckUnique(product);

                while (status != ProductUniqueEnum.success)
                {
                    switch (status)
                    {
                        case ProductUniqueEnum.codeNotUnique:
                            {
                                product.Code = GenerateCode();
                            }
                            break;
                        case ProductUniqueEnum.nameNotUnique:
                            {
                                product.Name = GenerateName();
                            }
                            break;
                        case ProductUniqueEnum.barcodeNotUnique:
                            {
                                product.Barcode = GenerateBarcode(num);
                            }
                            break;
                    }

                    status = CheckUnique(product);
                }

                AddProduct(product);

                num++;
            }
        }

        #endregion

    }
}