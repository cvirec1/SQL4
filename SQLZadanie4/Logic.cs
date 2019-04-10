using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SQLZadanie4
{
    public class Logic
    {
        string connString = @"Server = VALJASEK\SQL2017; Database = AdventureWorks; Trusted_Connection = True;";
        //string sqlQuery1 = @" select max(BusinessEntityID)  FROM [AdventureWorks].[Person].[BusinessEntity];"; 
        //public int GetBussinesEntityId()
        //{            
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connString))
        //        {                    
        //            using (var sqlCmd = new SqlCommand(sqlQuery1, connection))
        //            {
        //                connection.Open();                        
        //                int max = Convert.ToInt32(sqlCmd.ExecuteScalar());
        //                return max + 1;   
        //            }
        //        }                
        //    }
        //    catch (Exception e)
        //    {
        //        e.Message.ToString();                
        //    }
        //    return 0;
        //}

        public void InsertBussinessEntity()
        {
            string sqlInsertBussinesEntity = @"insert into Person.BusinessEntity (rowguid,ModifiedDate) values (@rowguid,@date);";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlInsertBussinesEntity, connection))
                        {
                            Guid rowGuid = Guid.NewGuid();                            
                            DateTime date = DateTime.Now;                            
                            sqlCmd.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = rowGuid;
                            sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date;                          
                            bool success = (sqlCmd.ExecuteNonQuery() > 0);
                        }
                    }catch(Exception e)
                    {
                        e.Message.ToString();                        
                    }

                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
                Console.WriteLine("No connections");
            }
        }

        public void InsertPerson(string firstName, string lastName)
        {
            string sqlInsertPerson = @" insert into Person.Person (BusinessEntityID,PersonType,NameStyle,FirstName,LastName,rowguid,ModifiedDate) values(@id,'EM',0,@name,@surname,@guid,@date);";
            string sqlGetData = @" select top 1 BusinessEntityID,rowguid from Person.BusinessEntity  order by BusinessEntityID desc;";
            
            int id = 0;
            Guid guid = Guid.NewGuid();
            DateTime date = DateTime.Now;
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    try
                    {                        
                        using (SqlCommand sqlCmd = new SqlCommand(sqlGetData, connection))
                        {
                            try
                            {
                                using(SqlDataReader reader = sqlCmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        id = reader.GetInt32(0);
                                        guid = reader.GetGuid(1);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                e.Message.ToString();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        e.Message.ToString();
                    }

                }
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlInsertPerson, connection))
                        {
                            sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            sqlCmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = guid;
                            sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
                            sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = firstName;
                            sqlCmd.Parameters.Add("@surname", SqlDbType.NVarChar).Value = lastName;
                            bool success = (sqlCmd.ExecuteNonQuery() > 0);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;                         
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdatePerson(int id,string firstName, string lastName)
        {            
            string sqlUpdatePerson = @" update Person.Person set firstname = @name,lastname = @surname,modifieddate=@date where BusinessEntityID = @id;";
            DateTime date = DateTime.Now;
            try
            {               
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlUpdatePerson, connection))
                        {
                            sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;                            
                            sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
                            sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = firstName;
                            sqlCmd.Parameters.Add("@surname", SqlDbType.NVarChar).Value = lastName;
                            bool success = (sqlCmd.ExecuteNonQuery() > 0);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeletePerson(int id)
        {
            string sqlDeletePerson = @" delete Person.Person where BusinessEntityID = @id;";
            string sqlDeleteBusinessEntity = @" delete Person.BusinessEntity where BusinessEntityID = @id;";
            try
            {              
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlDeletePerson, connection))
                        {
                            sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;                           
                            bool success = (sqlCmd.ExecuteNonQuery() > 0);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlDeleteBusinessEntity, connection))
                        {
                            sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            bool success = (sqlCmd.ExecuteNonQuery() > 0);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataSet FillDataSet()
        {
            SqlConnection connection = new SqlConnection(connString);
            string sqlQuery = @"SELECT TOP (1000) [BusinessEntityID]
                            ,[PersonType]
                            ,[NameStyle]
                            ,[Title]
                            ,[FirstName]
                            ,[MiddleName]
                            ,[LastName]
                            ,[Suffix]
                            ,[EmailPromotion]
                            ,[AdditionalContactInfo]
                            ,[Demographics]
                            ,[rowguid]
                            ,[ModifiedDate]
                            FROM[AdventureWorks].[Person].[Person]";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);

            DataSet ds = new DataSet();
            adapter.Fill(ds,"Person");
            DataTable dt = ds.Tables["Person"];
            dt.Columns.Add("Name", typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                dr.SetField("Name", $"{dr["LastName"]} {dr["MiddleName"]} {dr["FirstName"]}");
            }
            return ds;
        }


    }
}
