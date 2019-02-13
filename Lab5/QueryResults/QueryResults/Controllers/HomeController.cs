using QueryResults.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace QueryResults.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Form form)
        {
            List<Person> people = GetPeople(form);

            return View(people);
        }

        static readonly string connectionString = "Data Source=Desktop;Initial Catalog=AdventureWorks2017;Integrated Security=True";

        private List<Person> GetPeople(Form form)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sqlCommandText;
                command.Parameters.AddWithValue("@name", form.Person ?? string.Empty);
                command.Parameters.AddWithValue("@address", form.Address ?? string.Empty);
                command.Parameters.AddWithValue("@city", form.City ?? string.Empty);
                command.Parameters.AddWithValue("@state", form.State ?? string.Empty);

                var reader = command.ExecuteReader();

                var result = new List<Person>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Person
                        {
                            FullName = reader.GetString(0),
                            AddressType = reader.GetString(1),
                            Address = reader.GetString(2),
                            City = reader.GetString(3),
                            State = reader.GetString(4)
                        });
                    }
                }

                return result;
            }
        }

        const string sqlCommandText = @"
            SELECT
	            ISNULL(p.[Title] + ' ', '') + ISNULL(p.[FirstName] + ' ', '')
		            + ISNULL(p.[MiddleName] + ' ', '') + ISNULL(p.[LastName], '') FullName
	            ,t.[Name] AddressType
	            ,a.AddressLine1 [Address]
	            ,a.City
	            ,s.[Name] [State]
            FROM Person.Person p
            JOIN Person.BusinessEntityAddress bea ON p.BusinessEntityID = bea.BusinessEntityID
            JOIN Person.[Address] a ON bea.AddressID = a.AddressID
            JOIN Person.AddressType t ON bea.AddressTypeID = t.AddressTypeID
            JOIN Person.StateProvince s ON a.StateProvinceId = s.StateProvinceId
            WHERE (p.[Title] LIKE '%' + @name + '%' OR p.[FirstName] LIKE '%' + @name +'%' OR p.[MiddleName] LIKE '%' + @name +'%' OR p.[LastName] LIKE '%' + @name +'%')
            AND a.AddressLine1 LIKE '%'+ @address +'%'
            AND a.City LIKE '%'+ @city +'%'
            AND s.[Name] LIKE '%'+ @state +'%'";
    }
}