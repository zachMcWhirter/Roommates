using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate FROM Roommate; ";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstnameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstnameValue = reader.GetString(firstnameColumnPosition);

                        int lastnameColumnPosition = reader.GetOrdinal("LastName");
                        string lastnameValue = reader.GetString(lastnameColumnPosition);

                        int rentPortionPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionPosition);

                        int MoveInDatePosition = reader.GetOrdinal("MoveInDate");
                        DateTime MoveinDateValue = reader.GetDateTime(MoveInDatePosition);

                        // Now let's create a new roommate object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstnameValue,
                            Lastname = lastnameValue,
                            RentPortion = rentPortionValue,
                            MovedInDate = MoveinDateValue,
                            Room = null,
                        };

                        // ...and add that roommate object to our list.
                        roommates.Add(roommate);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of roommates who whomever called this method.
                    return roommates;
                }
            }
        }

        // this method will return a Roommate 
        public Roommate GetById(int id)
        {
            // set up connection to Sql
            using (SqlConnection conn = Connection)
            {
                //open the tunnel by creating a new command that allows us to access the data on the other end
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //go to Sql and run your query first. Then put it here
                    cmd.CommandText = @"SELECT Id, 
                                                Firstname, 
                                                Lastname, 
                                                RentPortion, 
                                                MovedInDate, 
                                                RoomId
                                                FROM Room
                                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        // Type of variable(Roommate) name of variable(roommate) = new Object(new Roommate)
                        roommate = new Roommate()
                        {
                            // The next lines will get the data from Sql by using the Ordinal (column #) integer and turning it into the proper variable type
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MovedInDate")),
                            Room = null
                        };

                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
        /// <summary>
        ///  Add a new roommate to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>

    }
}
