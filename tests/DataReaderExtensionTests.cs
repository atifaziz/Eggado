namespace Eggado.Tests
{
    #region Imports

    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Xml;
    using Mannex.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    [TestClass]
    public partial class DataReaderExtensionTests
    {
        class Product
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string EnglishName { get; set; }
            public string QuantityPerUnit { get; set; }
            public decimal UnitPrice { get; set; }
            public int UnitsInStock { get; set; }
            public int UnitsOnOrder { get; set; }
            public int? ReorderLevel { get; set; }
            public bool Discontinued { get; set; }
            public string Supplier { get; set; }
            public string Category { get; set; }
        }

        private DataTable GetProductsTable()
        {
            var table = new DataTable();
            using (var reader = GetType().Assembly.GetManifestResourceReader(GetType(), "Products.xml", Encoding.UTF8))
            {
                Assert.IsNotNull(reader);
                table.ReadXml(XmlReader.Create(reader));
            }
            return table;
        }

        [TestMethod]
        public void Select()
        {
            AssertProducts(GetProductsTable().Select<Product>());
        }
        
        [TestMethod]
        public void SelectViaSelector()
        {
            var products = GetProductsTable().Select(
            (
                int productId, string productName, string englishName, 
                string quantityPerUnit, decimal unitPrice, 
                int unitsInStock, int unitsOnOrder, int? reorderLevel, 
                bool discontinued, string supplier, string category
            ) 
            => new Product
            {
                ProductId       = productId,
                ProductName     = productName,
                EnglishName     = englishName,
                QuantityPerUnit = quantityPerUnit,
                UnitPrice       = unitPrice,
                UnitsInStock    = unitsInStock,
                UnitsOnOrder    = unitsOnOrder,
                ReorderLevel    = reorderLevel,
                Discontinued    = discontinued,
                Supplier        = supplier,
                Category        = category,
            });

            AssertProducts(products);
        }

        [TestMethod]
        public void SelectOrdinallyViaSelector()
        {
            var products = GetProductsTable().Select(
            (
                // ReSharper disable InconsistentNaming
                int _1, string _2, string _3,
                string _4, decimal _5,
                int _6, int _7, int? _8,
                bool _9, string _10, string _11
                // ReSharper restore InconsistentNaming
            )
            => new Product
            {
                ProductId       = _1,
                ProductName     = _2,
                EnglishName     = _3,
                QuantityPerUnit = _4,
                UnitPrice       = _5,
                UnitsInStock    = _6,
                UnitsOnOrder    = _7,
                ReorderLevel    = _8,
                Discontinued    = _9,
                Supplier        = _10,
                Category        = _11,
            });

            AssertProducts(products);
        }
        
        private static void AssertProducts(IEnumerable<Product> products)
        {
            using (var e = products.GetEnumerator())
            {
                AssertProducts(e);
                Assert.IsFalse(e.MoveNext());
            }
        }
    }
}
