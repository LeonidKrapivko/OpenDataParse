using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseCreator
{
    public class DbWorker
    {
        readonly string connstr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public void CreateRecord(Company record)
        {
            using (var connection = new SqlConnection(connstr))
            {
                string sql = $"INSERT INTO Records (name,short_name,edrpou,addres,boss,kved,stan)" +
                    $" VALUES (N'{record.Name}',N'{record.ShortName ?? "NULL" }', {record.Edrpou},N'{record.Address}',N'{record.Boss}',N'{record.Kved ?? "NULL"}',N'{record.Stan}')";
            }
        }
        public void CreateRecords(List<Company> records)
        {
            List<List<Company>> z = splitList(records, 200);
            foreach (var item in z)
            {
                _createRecord(item);
            }
        }
        private void _createRecord(List<Company> records)
        {
            using (var connection = new SqlConnection(connstr))
            {
                try
                {
                    string sql = $"INSERT INTO Records (id,name,short_name,edrpou,addres,boss,kved,stan) VALUES ({records[0].Id},N'{records[0].Name}',N'{records[0].ShortName}'" +
                        $", {records[0].Edrpou},N'{records[0].Address}',N'{records[0].Boss}',N'{records[0].Kved}',N'{records[0].Stan}')";
                    StringBuilder sb = new StringBuilder(sql);
                    for (int i = 1; i < records.Count; i++)
                    {
                        Company item = records[i];
                        sb.Append($", ({item.Id},N'{item.Name}',N'{item.ShortName}', {item.Edrpou},N'{item.Address}',N'{item.Boss}',N'{item.Kved}',N'{item.Stan}')");
                    }
                    SqlCommand command = new SqlCommand(sb.ToString(), connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void CreatePeoples(List<Person> peoples)
        {
            List<List<Person>> z = splitList(peoples, 200);
            foreach (var item in z)
            {
                _createPeople(item);
            }
        }
        private void _createPeople(List<Person> peoples)
        {
            using (var connection = new SqlConnection(connstr))
            {
                try
                {
                    string str = $"INSERT INTO Peoples (id,name) VALUES ({peoples[0].Id},N'{peoples[0].FullName}')";
                    StringBuilder sb = new StringBuilder(str);
                    for (int i = 1; i < peoples.Count; i++)
                    {
                        Person item = peoples[i];
                        string a = item.FullName;
                        Regex regex = new Regex("[`'\'()]");
                        a = regex.Replace(a, "");
                        sb.Append($", ({item.Id},N'{a}')");
                    }
                    connection.Open();

                    SqlCommand command = new SqlCommand(sb.ToString(), connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public static List<List<Person>> splitList(List<Person> locations, int nSize = 30)
        {
            var list = new List<List<Person>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }
        public static List<List<Company>> splitList(List<Company> locations, int nSize = 30)
        {
            var list = new List<List<Company>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }
            return list;
        }
        public static List<List<Relation>> splitList(List<Relation> locations, int nSize = 30)
        {
            var list = new List<List<Relation>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }
            return list;
        }
        public void CreateRelations(List<Relation> rel)
        {
            List<List<Relation>> z = splitList(rel, 300);
            foreach (var item in z)
            {
                _createRelation(item);
            }
        }
        private void _createRelation(List<Relation> rel)
        {
            using (var connection = new SqlConnection(connstr))
            {
                try
                {
                    string str = $"INSERT INTO RecordFounders (record_id,founder_id) VALUES ({rel[0].RecId},{rel[0].FounderId})";
                    StringBuilder sb = new StringBuilder(str);
                    for (int i = 1; i < rel.Count; i++)
                    {
                        Relation item = rel[i];
                        sb.Append($", ({item.RecId},{item.FounderId})");
                    }
                    SqlCommand command = new SqlCommand(sb.ToString(), connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public DbWorker(List<Person> founders, List<Company> records, List<Relation> relations)
        {
            DbCreator d = new DbCreator();
            d.Check();
            CreatePeoples(founders);
            CreateRecords(records);
            CreateRelations(relations);
        }
    }
}
