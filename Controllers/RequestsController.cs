using Microsoft.AspNetCore.Mvc;
using Laba2FilmsBD.Models;
using Microsoft.Data.SqlClient;

namespace Laba2FilmsBD.Controllers
{
    public class RequestsController
    {
        static SqlConnection connectionString = new(@"Data Source=DESKTOP-QO2SGDT; Initial Catalog=LabaFilmsDB;Trusted_Connection=True;TrustServerCertificate=True;");

        private void openConnection()
        {
            if (connectionString.State == System.Data.ConnectionState.Closed)
            {
                connectionString.Open();
            }
        }
        private void closeConnection()
        {
            if (connectionString.State == System.Data.ConnectionState.Open)
            {
                connectionString.Close();
            }
        }
        private List<Actor> GetActors(string sql)
        {
            List<Actor> result = new();
            using (SqlCommand command = new(sql, connectionString))
            {
                openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmp = new Actor
                        {
                            Id = (int)reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        };
                        if (!reader.IsDBNull(3)) tmp.Gender = reader.GetString(3);
                        if (!reader.IsDBNull(4)) tmp.BirthDay = new DateTime(reader.GetDateTime(4).Year, reader.GetDateTime(4).Month, reader.GetDateTime(4).Day);
                        result.Add(tmp);
                    }
                }
            }
            closeConnection();
            return result;
        }

        private List<Film> GetFilms(string sql)
        {
            List<Film> result = new();
            using (SqlCommand command = new SqlCommand(sql, connectionString))
            {
                openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmp = new Film
                        {
                            Id = (int)reader.GetInt32(0),
                            Title = reader.GetString(1)                            
                        };
                        if (!reader.IsDBNull(2)) tmp.Description = reader.GetString(2);
                        if (!reader.IsDBNull(3)) tmp.ReleaseYear = reader.GetInt32(3);
                        if (!reader.IsDBNull(4)) tmp.Language = reader.GetString(4);
                        if (!reader.IsDBNull(5)) 
                            tmp.LenghtInMinutes = reader.GetInt32(5);
                        if (!reader.IsDBNull(6)) tmp.Genre = reader.GetString(6);                      
                        result.Add(tmp);
                    }
                }
            }
            closeConnection();
            return result;
        }

        private List<Customer> GetCustomers(string sql, bool totalAmount)
        {
            List<Customer> result = new();
            using (SqlCommand command = new(sql, connectionString))
            {
                openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmp = new Customer
                        {
                            Id = (int)reader.GetInt32(0),
                           
                            Email = reader.GetString(3),
                            
                        };
                        if (!reader.IsDBNull(1)) tmp.FirstName = reader.GetString(1); 
                        if (!reader.IsDBNull(2)) tmp.LastName = reader.GetString(2); 
                        if (!reader.IsDBNull(4)) tmp.LastActiveDate = reader.GetDateTime(4);                        
                        if (totalAmount && !reader.IsDBNull(5)) tmp.TotalAmount = reader.GetInt32(5);                        
                        result.Add(tmp);
                    }
                }
            }


            closeConnection();
            return result;

        }

        // Усі актори та акторки, які знімалися в хоча б в усіх тих самих фільмах, що і актор з id
        public List<Actor> Request1(int id)
        {               
                String sql = @"SELECT * 
                              FROM Actors
                              WHERE NOT EXISTS(
                                (
                                  SELECT DISTINCT A.FilmId
                                  FROM ActorsInFilms AS A
                                  WHERE A.ActorId = " + id.ToString() + 
                               @" ) 
                                EXCEPT(
                                  SELECT DISTINCT B.FilmId
                                  FROM ActorsInFilms As B
                                  WHERE B.ActorId = Actors.Id
                                ) 
                             )";
                
            return GetActors(sql);
        }
        // Усі покупці, які купили хоча б один фільм з актором id у головній ролі/другорядній/неважливо
        public List<Customer> Request2(int id, int role)
        {
            string add = "";
            if (role == 1) add = " IsMain = 1 AND ";
            if (role == 2) add = " IsMain = 0 AND ";
                String sql = @"SELECT * 
                                FROM Customers 
                                WHERE exists (
	                                (
	                                 SELECT FilmId
	                                 FROM Purchases
	                                 WHERE CustomerId = Customers.Id
	                                )
	                                INTERSECT 
	                                (
	                                 SELECT FilmId
	                                 FROM ActorsInFilms 
	                                 WHERE " + add + @"ActorId = " + id.ToString() +
	                               @" )
                                )";

                
            return GetCustomers(sql, false);
        }

        // Усі актори та акторки, які (НЕ) знімалися хоча б в одному фільмі з популярністю >= за persent відсотків
                // популярністю вважати: (кількість покупців, які купили фільм) / (загальна кількість покупців)
        public List<Actor> Request3(int persent, int ifnot)
        {
            string NOT = "";
            if (ifnot == 1) NOT = "NOT";            
            
                String sql = @"SELECT *
                                FROM actors
                                WHERE " + NOT + @" EXISTS(
	                                (	SELECT filmId
		                                FROM ActorsInFilms
		                                WHERE ActorId = Actors.id
	                                )
	                                INTERSECT (

	                                SELECT Films.Id
	                                FROM Films
		                                LEFT JOIN Purchases
		                                ON Purchases.FilmId = Films.Id
	                                GROUP BY Films.Id
	                                HAVING COUNT(CustomerId) * 100 >= " + persent.ToString() + @" * (SELECT count(*)FROM Customers)
                                )
                                )";

            return GetActors(sql);
        }
        // Усі фільми жанру Genre, де знімався актор id (в головній/не головній/будь-якій ролі)
        public List<Film> Request4(int ActorId, string Genre, int role)
        {  
            string add = "";
            if (role == 1) add = "AND ActorsInFilms.IsMain = 1 ";
            if (role == 2) add = "AND ActorsInFilms.IsMain = 0 ";


            String sql = @"SELECT DISTINCT films.Id
	                                    , films.Title
	                                    , films.Description
	                                    , films.Release_year
	                                    , films.Language
                                        , films.LenghtInMinutes
	                                    , films.Genre
                                    FROM Films
	                                    INNER JOIN ActorsInFilms
	                                    ON ActorsInFilms.ActorId = " + ActorId.ToString() + @" AND ActorsInFilms.FilmId = films.Id " + add + @" 
                                    WHERE films.Genre = '" + Genre + "'";

            
            return GetFilms(sql);
        }
        // Усі фільми тривалістю від __ до __, де не знімався актор id
        public List<Film> Request5(int ActorId, int MinLen, int MaxLen)
        {           
            String sql = @"SELECT * 
                            from films
                            where films.LenghtInMinutes >= " + MinLen.ToString() + @" AND films.LenghtInMinutes <= " + MaxLen.ToString() + @" AND
                            films.Id not in (
	                            SELECT X.Id	
	                            FROM Films AS X
		                            INNER JOIN ActorsInFilms
		                            ON ActorsInFilms.ActorId = " + ActorId.ToString() + @" AND ActorsInFilms.FilmId = X.Id
	                            )";

            return GetFilms(sql);
        }

        // Усі покупці, які витратили на покупки не менше ніж amount гривень
        public List<Customer> Request6(int amount)
        {          
            String sql = @"SELECT  Customers.Id
	                        , Customers.FirstName
	                        , Customers.LastName
	                        , Customers.Email
	                        , Customers.Last_Active_Date
	                        , SUM(Purchases.Cost) AS TotalAmount
                        from Customers
	                        left join Purchases
	                        on Purchases.CustomerId = Customers.Id
                        Group by Customers.Id , Customers.FirstName, Customers.LastName, Customers.Email, Customers.Last_Active_Date
                        Having SUM(Purchases.Cost) >=" + amount.ToString();

            return GetCustomers(sql, true);
        }
        // Усі актори (чоловіки/жінки/не важливо) які знімалися в фільмі FilmId
        
        public List<Actor> Request7(int FilmId, int gender)
        {           
            string add = "";
            if (gender == 1) add = "WHERE Actors.Gender = 'Чоловіча'";
            if (gender == 2) add = "WHERE Actors.Gender = 'Жіноча'";

            String sql = @"SELECT Actors.Id
	                            , Actors.FirstName
	                            , Actors.LastName
	                            , Actors.Gender
	                            , Actors.BirthDay
                          FROM Actors
	                      INNER JOIN ActorsInFilms
		                      ON ActorsInFilms.ActorId = Actors.Id and ActorsInFilms.FilmId = " + FilmId.ToString() + add;

            return GetActors(sql);
        }
        // Знайти для кожного актора покупця, який купив з ним найбільше фільмів,
              // якщо таких декілька, то вибрати з мінімальним/максимальним id

        public List<AdditionalClass> Request8(bool ifmax)
        {
            List<AdditionalClass> result = new();
            string COMP = "MIN";
            if (ifmax) COMP = "MAX";
            String sql = @"WITH CTE AS(
	                            select actors.Id AS Actor		
		                            , Customers.Id AS Customer
		                            , count(Purchases.FilmId) AS AmountPurchases
	                            from Actors
		                            left join ActorsInFilms
			                            on ActorsInFilms.ActorId = Actors.Id
		                            left join Purchases
			                            on Purchases.FilmId = ActorsInFilms.FilmId
		                            left join Customers
			                            on Customers.Id = Purchases.CustomerId
	                            group by actors.Id
		                            , actors.FirstName 
		                            , actors.LastName
		                            , Customers.Id 
                            )

                            select Actors.Id
	                            , Actors.FirstName
	                            , Actors.LastName
	                            , Customers.Id
	                            , Customers.FirstName
	                            , Customers.LastName
	                            , Customers.Email
                            from (
	                            select Actor
	                            , "+COMP+@"(Customer) as Customer
	                            , AmountPurchases
	                            from CTE
	                            where (CASE WHEN AmountPurchases = NULL THEN 0 ELSE AmountPurchases END) = 
		                            (
			                            select Distinct MAX(CASE WHEN X.AmountPurchases = NULL THEN 0 ELSE X.AmountPurchases END) 
			                            from CTE AS X
			                            where X.Actor = CTE.Actor
			                            group by X.Actor
		                            )
	                            group by Actor, AmountPurchases
	                            ) as CTENEXT
                            left join Actors
	                            on Actors.Id = CTENEXT.Actor
                            left join Customers
	                            on Customers.Id = CTENEXT.Customer";

            using (SqlCommand command = new SqlCommand(sql, connectionString))
            {
                openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmp = new AdditionalClass
                        {
                            ActorId = (int)reader.GetInt32(0),
                            ActorFirstName = reader.GetString(1),
                            ActorLastName = reader.GetString(2)
                        };
                        if (!reader.IsDBNull(3)) tmp.CustomerId = reader.GetInt32(3);
                        if (!reader.IsDBNull(4)) tmp.CustomerFirstName = reader.GetString(4);
                        if (!reader.IsDBNull(5)) tmp.CustomerLastName = reader.GetString(5);
                        if (!reader.IsDBNull(6)) tmp.CustomerEmail = reader.GetString(6);
                        
                        result.Add(tmp);
                    }
                }
            }


            closeConnection();
            return result;
        }
    }
}

