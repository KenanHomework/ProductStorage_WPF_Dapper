using Microsoft.Data.SqlClient;
using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ProductStorage_WPF_Dapper.Services
{
    public abstract class ProductService
    {

        public static List<Product> GetAllProducts()
        {
            IDbConnection db = new SqlConnection(App.ConnStr);
            string sql = "SELECT * FROM Products;";
            return db.Query<Product>(sql).ToList();
        }

        public static int Add(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product Can't be null ! <ProductService.Add>");

            IDbConnection db = new SqlConnection(App.ConnStr);
            string sql = @"
                            insert into Products (
                            	Name,
                            	Description ,
                            	ImageSource ,
                            	OriginCountry ,
                            	Price,
                            	Count
                            )
                            VALUES
                            	(
                            		@Name ,
                            		@Description ,
                            		@ImageSource ,
                            		@OriginCountry ,
                            		@Price ,
                            		@Count
                            	);SELECT CAST((SCOPE_IDENTITY()) AS int);";
            try
            {
                return db.Query<int>(sql, product).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Product Update(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product Can't be null ! <ProductService.Update>");

            IDbConnection db = new SqlConnection(App.ConnStr);
            string sql = @"
                        UPDATE
                        	Products
                        SET
                        	Name  = @Name,
                        	Description  = @Description,
                        	ImageSource  = @ImageSource,
                        	OriginCountry  = @OriginCountry,
                        	Price  = @Price,
                        	Count  = @Count
                        WHERE
                        	Id = @Id";

            try
            {
                db.Execute(sql, product);
            }
            catch (Exception)
            {

                throw;
            }
            return product;
        }

        public static void Remove(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product Can't be null ! <ProductService.Remove>");

            IDbConnection db = new SqlConnection(App.ConnStr);
            string sql = @"
                            DELETE
                            FROM
                            	Products
                            WHERE
                            	Id = @Id";
            try
            {
                db.Execute(sql, product);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Remove(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException("Id Can't be less than zero ! <ProductService.Remove>");

            IDbConnection db = new SqlConnection(App.ConnStr);
            string sql = @"
                            DELETE
                            FROM
                            	Products
                            WHERE
                            	Id = @Id";
            try
            {
                db.Execute(sql, new { @Id = id });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void RemoveRange(List<Product> products)
        {
            if (products == null && products.Count <= 0)
                throw new ArgumentException("Products Can't be null or empty ! <ProductService.RemoveRange>");

            try
            {
                products.ForEach(p => Remove(p));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
