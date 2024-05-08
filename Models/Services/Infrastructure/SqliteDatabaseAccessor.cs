using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyCourse.Models.ValueTypes;


namespace MyCourse.Models.Services.Infrastructure
{

    public class SqliteDatabaseAccessor : IDatabaseAccessor //classe che implementera concretamente il servizio infrastrutturale cioe si connettera al database ed eseguira le query scritte in sql
    {

      private readonly IConfiguration configuration;//proprietà tramite cui setterò la connessione al database

      public SqliteDatabaseAccessor(IConfiguration configuration)
      {
        this.configuration=configuration;
      }
       public async Task<DataSet> QueryAsync(FormattableString formattableQuery) 
       { var queryArguments=formattableQuery.GetArguments(); //tramite questo metodo recupero tutti i valori iniettati dall applicazione nella query 
         var sqliteParameters = new List<SqliteParameter>();
       for (var i=0 ; i<queryArguments.Length; i++)
       {
          if(queryArguments[i] is Sql)
          {
            continue; //se nella query che viene eseguita aspdotnet core trova un argomento di tipo sql non lo aggiunge ai parametri SqliteParameter
          }
          var parameter= new SqliteParameter(i.ToString(), queryArguments[i]);
          sqliteParameters.Add(parameter);
          queryArguments[i]="@"+i;
       }
        
        string query = formattableQuery.ToString();
         //stabilire una connessione con il db
       //ho stabilito una connessione con il db sqlite MyCourse.db
         //using(var conn = new SqliteConnection("Data Source=Data/MyCourse.db"))
         string connectionString= configuration.GetConnectionString("Default");
         using(var conn = new SqliteConnection(connectionString))
         { 
          await conn.OpenAsync();
            //conn.Open(); //automaticamente ADO.net recuperera una nuova connessione dal collection pool 
            // eseguo la query sul db MyCourse.db
            using(var cmd=new SqliteCommand(query, conn)){ 

                  cmd.Parameters.AddRange(sqliteParameters);//setto i parametri della query

            // il metodo ExecuteREader esegue una query di tipo select che restituisce una tabella di risultati. il tipo di oggetto che viene restituito è sqliteDataReader
            // nel caso in cui devo eseguire una query che non restituisce tabelle (insert, create, update, delete) devo usare il metodo ExecuteNonquery() 
            // nel caso in cui devo eseguire una query che restituisce un numero devo usare il metodo ExecuteScalar()
              using(var reader = await cmd.ExecuteReaderAsync()){ 
                var dataSet = new DataSet();
                dataSet.EnforceConstraints=false;
                do{
                  var dataTable= new DataTable();
                  dataSet.Tables.Add(dataTable);
                  dataTable.Load(reader); 
                } while (!reader.IsClosed);
                //var dataTable= new DataTable();
                //dataSet.Tables.Add(dataTable);
                //dataTable.Load(reader); 
                
                // cosi evitiamo di leggere i data dalla tabella risultante riga per riga(while)
                  //while (reader.Read()){ // il metodo Read legge una riga per volta della tabella restituita
                    //string Titolo= (string)reader["Title"];//recupero il valore del campo Title della tabella Courses 
                  //}
                  return dataSet;
              }
            }
         }
        //conn.Dispose(); dato che abbiamo inserito using non ci serve chiudere la chiamata "open"

       }
    }
}