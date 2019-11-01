using Dapper;
using System;
using System.Data.SqlClient;

namespace BaseCreator
{
    public class DbCreator
    {
        static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=edrpou;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public void Check()
        {
            using (var connection = new SqlConnection(connstr))
            {
                try
                {
                    connection.Query("SELECT TOP 1 * FROM Peoples");
                }
                catch(SqlException)
                {
                    CreateFounders();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    connection.Query("SELECT TOP 1 * FROM Records");
                }
                catch (SqlException)
                {
                    CreateRecords();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    connection.Query("SELECT TOP 1 * FROM RecordFounders");
                }
                catch (SqlException)
                {
                    CreateRecordFounders();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        void CreateFounders()
        {
            using (var connection = new SqlConnection(connstr))
            {
                connection.Query("CREATE TABLE [Peoples]( " +
                    "[id]   int NOT NULL , " +
                    "[name] nvarchar(max) NOT NULL ," +
                    "  CONSTRAINT [PK_Rounder] PRIMARY KEY CLUSTERED ([id] ASC));");
            }
        }

        void CreateRecords()
        {
            using (var connection = new SqlConnection(connstr))
            {
                connection.Query("CREATE TABLE [Records]( " +
                    "[id]         int NOT NULL , " +
                    "[name]       nvarchar(max) NOT NULL , " +
                    "[short_name] nvarchar(255) NULL , " +
                    "[edrpou]     int NOT NULL , " +
                    "[addres]     nvarchar(max) NOT NULL , " +
                    "[kved]       nvarchar(255) NULL , " +
                    "[stan]       nvarchar(255) NOT NULL , " +
                    "[boss]       int NULL , " +
                    "CONSTRAINT [PK_Records] PRIMARY KEY CLUSTERED ([id] ASC), " +
                    "CONSTRAINT [FK_215] FOREIGN KEY ([boss])  REFERENCES [Peoples]([id]));");
                connection.Query("CREATE NONCLUSTERED INDEX [fkIdx_215] ON [Records]  (  [boss] ASC )");
            }
        }

        void CreateRecordFounders()
        {
            using (var connection = new SqlConnection(connstr))
            {
                connection.Query("CREATE TABLE [RecordFounders](" +
                    " [record_id]  int NOT NULL ," +
                    " [founder_id] int NOT NULL ," +
                    " CONSTRAINT [FK_209] FOREIGN KEY ([record_id])  REFERENCES [Records]([id])," +
                    " CONSTRAINT [FK_212] FOREIGN KEY ([founder_id])  REFERENCES [Peoples]([id]));");
                connection.Query("CREATE NONCLUSTERED INDEX [fkIdx_209] ON [RecordFounders]  (  [record_id] ASC )");
                connection.Query("CREATE NONCLUSTERED INDEX [fkIdx_212] ON [RecordFounders]  (  [founder_id] ASC )");
            }
        }
    }
}
