using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Data.SqlClient;

namespace Carga_Prueba_BI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PruebaController : ControllerBase
    {

        private readonly ILogger<PruebaController> _logger;

        public PruebaController(ILogger<PruebaController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Prueba")]
        public string Get()
        {
            try
            {
                carga();
                return "Proceso realizado correctamente";
            }
            catch (Exception ex)
            {
                return "Proceso no realizado" + Environment.NewLine + ex.Message;
                throw;
            }
        }
        #region Extras
        static async Task carga()
        {
            try
            {
                string host = "35.185.84.183";
                int puerto = 27021;
                string username = "powerbi";
                string password = "1ajDpowerbi2020";
                string AuthmMongo = "SCRAM-SHA-1";
                MongoInternalIdentity internalIdentity = new MongoInternalIdentity("mdb_agentes_prod", username);
                PasswordEvidence passwordEvidence = new PasswordEvidence(password);
                MongoCredential mongoCredential = new MongoCredential(AuthmMongo, internalIdentity, passwordEvidence);
                List<MongoCredential> credentials = new List<MongoCredential> { mongoCredential };
                MongoClientSettings settings = new MongoClientSettings();
                settings.Credentials = credentials;
                MongoServerAddress address = new MongoServerAddress(host, puerto);
                settings.Server = address;
                settings.ConnectTimeout = TimeSpan.FromSeconds(2000);
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(2000);
                settings.SocketTimeout = TimeSpan.FromSeconds(2000);

                MongoClient client = new MongoClient(settings);
                var database = client.GetDatabase("mdb_agentes_prod");
                var server = client.ListDatabases().ToList();
                var appointmenttrackings = database.GetCollection<BsonDocument>("appointmenttrackings");
                var typologies = database.GetCollection<BsonDocument>("typologies");
                var customers = database.GetCollection<BsonDocument>("vw_customers");
                var users = database.GetCollection<BsonDocument>("vw_users");
                var minutes = database.GetCollection<BsonDocument>("minutes");
                var projects = database.GetCollection<BsonDocument>("vw_projects");
                var builder = database.GetCollection<BsonDocument>("builders");
                var vw_minutes = database.GetCollection<BsonDocument>("vw_minutes");
                var properties = database.GetCollection<BsonDocument>("properties");

                /* filtrar campos 
                //var filtro = Builders<BsonDocument>.Filter.Empty;
                //var campos = Builders<BsonDocument>.Projection.Include("mongo_propertysIds")
                //    .Include("typology_id") 
                //var resultado = typologies.Find<BsonDocument>(filtro).Project(campos).ToList();
                */


                await m_truncate();
                await m_appointmenttrackings(appointmenttrackings); // YA
                await m_typologies(typologies);
                //x    // await m_customers(customers); //NO
                await m_users(users); // YA
                await m_minutes(minutes);  // YA
                await m_projects(projects); // YA
                await m_builder(builder); // YA
                await m_vw_minutes(vw_minutes); // YA
                await m_properties_m(properties);
                Console.WriteLine("fin2");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private static async Task m_truncate()
        {
            //COMERCIAL
            SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

            // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");


            try
            {
                string SqlString =

                         "truncate table [dbo].[TMP_AGT_Historial_Minutes];" +
                          "truncate table[dbo].[TMP_AGT_Agentes];" +
                          "truncate table[dbo].[TMP_AGT_Typologies];" +
                       //    "truncate table[dbo].[TMP_AGT_Cliente];" +
                       "truncate table[dbo].[TMP_AGT_Historial_Citas]; " +
                         "truncate table[dbo].[TMP_AGT_Minutes];" +
                         "truncate table[dbo].[TMP_AGT_Builder];" +
                        "truncate table[dbo].[TMP_AGT_Properties_m];" +
                         "truncate table[dbo].[TMP_AGT_Proyects];";


                using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static async Task m_vw_minutes(IMongoCollection<BsonDocument> vw_minutes)
        {
            using (IAsyncCursor<BsonDocument> cursor = await vw_minutes.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {



                        // COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                        //  SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");



                        try
                        {
                            Console.WriteLine(document["_id"].ToString());
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Historial_Minutes]([id],[typeCurrency],[cashAdvance],[ordenCompra],[Signaturedate],[price],[ruc],[razonsocial],[cliente_Id],[rescheduled],[isvalidate],[rol],[agenteId],[appointment],[appointmentdate],[typologyName],[typologyId],[builderName],[commission],[builderId],[proyectName],[projectid],[apponintmentStatus],[apponintmentCreatedAT],[apponintmentUpdatedAT],[minuteStatus],[minuteCreatedAt],[minuteStatusdATe])" +
                            "VALUES ('" + document["_id"].ToString() + "',"
                            + document["typeCurrency"].ToInt32() + ","
                            + document["cashAdvance"].ToInt32() + ","
                            + document["ordenCompra"].ToInt32() + ",'"
                            + document["signatureDate"].ToLocalTime().ToString("yyyy-MM-dd") + "',"
                            + document["price"].ToInt32() + ",'"
                            + document["ruc"].ToString() + "','"
                            + document["razonSocial"].ToString().Replace("'", "") + "','"
                              //   + document["clienteId"].ToString() + "','"    antiguo 29/04/21

                              + (document.Contains("clienteId") ? document["clienteId"].ToString() : "") + "','"

                            + document["rescheduled"].ToBoolean().ToString() + "','"
                            + (document.Contains("isvalidate") ? document["isvalidate"].ToString() : "SinValor") + "','"
                            + document["rol"].ToString() + "','"
                            + document["agenteId"].ToString() + "','"
                            + document["appointmentId"].ToString() + "','"
                            + document["appointmentDate"].ToLocalTime().ToString("yyyy-MM-dd") + "','"
                            + document["typologyName"].ToString() + "',"

                            + document["typologyId"].ToInt32() + ",'"
                            + document["builderName"].ToString() + "',"
                            + (document["commission"].IsBsonNull ? 0 : document["commission"].ToInt32()) + ","
                            + (document["builderId"].IsBsonNull ? 0 : document["builderId"].ToInt32()) + ",'"
                            + document["projectName"].ToString() + "',"
                            + document["projectId"].ToInt32() + ","

                            + document["appointmentStatus"].ToInt32() + ",'"
                            + document["appointmentCreatedAt"].ToLocalTime().ToString("yyyy-MM-dd") + "','"
                            + document["appointmentUpdatedAt"].ToLocalTime().ToString("yyyy-MM-dd") + "',"
                            + document["minuteStatus"].ToInt32() + ",'"
                            + document["minuteCreatedAt"].ToLocalTime().ToString("yyyy-MM-dd H:mm:ss") + "','"  // CAMBIO
                            + (document["minuteStatusDate"].IsBsonNull ? "1980-01-01" : document["minuteStatusDate"].ToLocalTime().ToString("yyyy-MM-dd")) + "')";


                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }

        private static async Task m_appointmenttrackings(IMongoCollection<BsonDocument> appointmenttrackings)
        {
            using (IAsyncCursor<BsonDocument> cursor = await appointmenttrackings.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {

                        Console.WriteLine(document["_id"].ToString());
                        // COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                        //SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");


                        try
                        {
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Historial_Citas](ID_Temporal,rescheduled,rol,status,agente_id,cliente_id,appintment_id,typology_id,typology_Name,builder_id,builder_Name,project_id,comissiom,CreatedAt,updateAt,project_Name,appintment_date,parentId,dateElimination)" +

                            "VALUES ('" + document["_id"].ToString() + "','"
                            + document["rescheduled"].ToString() + "','"
                            + document["rol"].ToString() + "',"
                            + document["status"].ToInt32() + ",'"
                            + document["agenteId"].ToString() + "','"
                            //  + document["clienteId"].ToString() + "','" CAMBIO 15/04/21

                            + (document.Contains("clienteId") ? document["clienteId"].ToString() : "") + "','"

                            + document["appointmentId"].ToString() + "',"
                            + document["typologyId"].ToInt32() + ",'"
                            + document["typologyName"].ToString() + "',"
                            + (document.Contains("builderId") ? document["builderId"].ToInt32() : 0) + ",'"
                            + document["builderName"].ToString() + "',"
                            + document["projectId"].ToInt32() + ","
                            + document["commission"].ToInt32() + ",'"
                            //  + document["createdAt"].ToLocalTime().ToString("yyyy-MM-dd H:mm:ss") + "','"  ANTIGUO
                            + Convert.ToDateTime(document["createdAt"]).ToString("yyyy-MM-dd H:mm:ss") + "','"
                            + document["updatedAt"].ToLocalTime().ToString("yyyy-MM-dd H:mm:ss") + "','"


                            + document["projectName"].ToString() + "','"
                            + document["appointmentDate"].ToLocalTime().ToString("yyyy-MM-dd H:mm:ss") + "','"



                                // + (document.Contains("parentId")?document["parentId"].ToInt32():0) + ",'" ANTIGUO


                                + (document.Contains("parentId") ? document["parentId"].ToString() : "0") + "','"

                            + (document.Contains("dateElimination") ? document["dateElimination"].ToLocalTime().ToString("yyyy-MM-dd H:mm:ss") : "1980-01-01")
                            + "')";


                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }
        private static async Task m_users(IMongoCollection<BsonDocument> users)
        {
            var filtro = Builders<BsonDocument>.Filter.Eq("__t", "Agente");
            var resultado = users.Find<BsonDocument>(filtro).ToList();
            foreach (BsonDocument document in resultado)
            {
                string[] parent_id = new string[4];
                parent_id[0] = "0";
                if (document.Contains("parent") && document["parent"].IsBsonDocument)
                {
                    var l_parent_id = document["parent"].ToBsonDocument();
                    parent_id[0] = (l_parent_id.Contains("_id") ? l_parent_id["_id"].ToString() : "0");
                }

                // COMERCIAL
                SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");


                try
                {
                    Console.WriteLine(document["nroDoc"].ToString());
                    string SqlString = "INSERT INTO [dbo].[TMP_AGT_Agentes]([id],[logo],[codigoMVCS],[typoDoc],[nroDoc],[ruc],[nombres],[apellidoPaterno],[apellidoMaterno],[celular],[email],[asociado],[_t],[parent_id],[CreatedAt],[UpdatedAT],[lastRegistrationDate])" +

                    "VALUES ('" + document["_id"].ToString() + "','"
                    + (document.Contains("logo") ? document["logo"].ToString() : "") + "','"
                    + document["codigoMVCS"].ToString() + "','"
                    + document["typoDoc"].ToString() + "','"
                    + document["nroDoc"].ToString() + "','"
                    + (document.Contains("ruc") ? (document["ruc"].IsBsonNull ? "" : document["ruc"].ToString()) : "") + "','"
                    + document["nombres"].ToString().Replace("'", "") + "','"
                    + document["apellidoPaterno"].ToString().Replace("'", "") + "','"
                    + document["apellidoMaterno"].ToString().Replace("'", "") + "','"
                    + (document.Contains("celular") ? document["celular"].ToString() : "") + "','"
                    + (document.Contains("email") ? document["email"].ToString() : "") + "','"
                    + (document.Contains("corporateName") ? document["corporateName"].ToString() : "") + "','"
                    + document["__t"].ToString() + "','"
                    + parent_id[0] + "','"
                    + document["createdAt"].ToLocalTime().ToString("yyyy-MM-dd") + "','"
                    + (document["updatedAt"].IsBsonNull ? "1980-01-01" : document["updatedAt"].ToLocalTime().ToString("yyyy-MM-dd")) + "','"
                    + (document["lastRegistrationDate"].IsBsonNull ? "1980-01-01" : document["lastRegistrationDate"].ToLocalTime().ToString("yyyy-MM-dd")) + "')";


                    using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private static async Task m_typologies(IMongoCollection<BsonDocument> typologies)
        {
            using (IAsyncCursor<BsonDocument> cursor = await typologies.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        // COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                        // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");

                        try
                        {
                            Console.WriteLine(document["min_price"].ToString());
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Typologies]([id],[mongo_propetyIds],[typology_id],[project_id],[project_Name],[Type],[stock],[min_type_currency],[max_type_currency],[min_price],[max_price],[min_bedrooms],[max_bedrooms],[min_meters],[max_meters],[position],[status_value],[mongo_TypologyStatusValue],[Typologia_Name],[comisiontipologia],[CreatedAt],[UpdatedAT])" +

                            "VALUES ('" + document["_id"].ToString() + "','" + document["mongo_propertysIds"].ToString() + "','" + document["typology_id"].ToString() + "','" + document["project_id"].ToInt32() + "','" + document["name"].ToString().Replace("'", "") + "','" + (document["type"].IsBsonNull ? 0 : document["type"].ToInt32()) + "','" + (document["stock"].IsBsonNull ? 0 : document["stock"].ToInt32()) + "','" + (document["min_type_currency"].IsBsonNull ? 0 : document["min_type_currency"].ToInt32()) + "','" + (document["max_type_currency"].IsBsonNull ? 0 : document["max_type_currency"].ToInt32()) + "','" + (document["min_price"].IsBsonNull ? 0 : document["min_price"].ToInt32()) + "','" + (document["max_price"].IsBsonNull ? 0 : document["max_price"].ToInt32()) + "','" + (document["min_bedrooms"].IsBsonNull ? 0 : document["min_bedrooms"].ToInt32()) + "','" + (document["max_bedrooms"].IsBsonNull ? 0 : document["max_bedrooms"].ToInt32()) + "','" + (document["min_meters"].IsBsonNull ? 0 : document["min_meters"].ToInt32()) + "','" + (document["max_meters"].IsBsonNull ? 0 : document["max_meters"].ToInt32()) + "','" + (document["position"].IsBsonNull ? 0 : document["position"].ToInt32()) + "','" + document["status_value"].ToInt32() + "','" + (document.Contains("mongoTypologyStatusValue") ? document["mongoTypologyStatusValue"].ToInt32() : 0) + "','" + (document.Contains("nameTipologia") ? document["nameTipologia"].ToString().Replace("'", "") : "") + "','" + (document.Contains("comisionTipologia") ? (document["comisionTipologia"].IsBsonNull ? 0 : document["comisionTipologia"].ToInt32()) : 0) + "','" + document["date_create"].ToLocalTime().ToString("yyyy-MM-dd") + "','" + (document["update_create"].IsBsonNull ? "1980-01-01" : document["update_create"].ToLocalTime().ToString("yyyy-MM-dd")) + "')";

                            //                         "VALUES ('" + document["_id"].ToString() + "','" + document["mongo_propertysIds"].ToString() + "','" + document["typology_id"].ToString() + "'," + document["project_id"].ToInt32() + ",'" + document["name"].ToString().Replace("'", "") + "'," + (document["type"].IsBsonNull ? 0 : document["type"].ToInt32()) + "," + (document["stock"].IsBsonNull ? 0 : document["stock"].ToInt32()) + "," + (document["min_type_currency"].IsBsonNull ? 0 : document["min_type_currency"].ToInt32()) + "," + (document["max_type_currency"].IsBsonNull ? 0 : document["max_type_currency"].ToInt32()) + "," + (document["min_price"].IsBsonNull ? 0 : document["min_price"].ToInt32()) + "," + (document["max_price"].IsBsonNull ? 0 : document["max_price"].ToInt32()) + "," + (document["min_bedrooms"].IsBsonNull ? 0 : document["min_bedrooms"].ToInt32()) + "," + (document["max_bedrooms"].IsBsonNull ? 0 : document["max_bedrooms"].ToInt32()) + "," + (document["min_meters"].IsBsonNull ? 0 : document["min_meters"].ToInt32()) + "," + (document["max_meters"].IsBsonNull ? 0 : document["max_meters"].ToInt32()) + "," + (document["position"].IsBsonNull ? 0 : document["position"].ToInt32()) + "," + document["status_value"].ToInt32() + "," + document["mongoTypologyStatusValue"].ToInt32() + ",'" + document["nameTipologia"].ToString().Replace("'", "") + "'," + (document.Contains("comisionTipologia") ? (document["comisionTipologia"].IsBsonNull ? 0 : document["comisionTipologia"].ToInt32()) : 0) + ",'" + document["date_create"].ToLocalTime().ToString("yyyy-MM-dd") + "','" + (document["update_create"].IsBsonNull ? "1980-01-01" : document["update_create"].ToLocalTime().ToString("yyyy-MM-dd")) + "')";


                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }
        private static async Task m_customers(IMongoCollection<BsonDocument> customers)
        {
            using (IAsyncCursor<BsonDocument> cursor = await customers.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        // Conexion a ASEI COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                        // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");

                        try
                        {
                            Console.WriteLine(document["typoDoc"].ToString());
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Cliente]([id],[typoDoc],[nroDoc],[email],[nombres],[apellidoPaterno],[apellidoMaterno],[celular],[clienteId],[customerId],[tipo_cliente],[idPrincipalOAsociado],[CreatedAt],[UpdatedAT],[userId])" +

                            "VALUES ('" + document["_id"].ToString() + "','" + document["typoDoc"].ToString() + "','" + document["nroDoc"].ToString() + "','" + document["email"].ToString() + "','" + document["nombres"].ToString().Replace("'", "") + "','" + document["apellidoPaterno"].ToString().Replace("'", "") + "','" + document["apellidoMaterno"].ToString().Replace("'", "") + "','" + (document.Contains("celular") ? document["celular"].ToString() : "") + "','" + (document.Contains("clienteId") ? document["clienteId"].ToString() : "") + "','" + document["customerId"].ToString() + "','" + document["tipoCliente"].ToString() + "','" + document["idPrincipalOAsociado"].ToString() + "','" + document["createdAt"].ToLocalTime().ToString("yyyy-MM-dd") + "','" + (document["updatedAt"].IsBsonNull ? "1980-01-01" : document["updatedAt"].ToLocalTime().ToString("yyyy-MM-dd")) + "','" + document["userId"].ToString() + "')";


                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }


        private static async Task m_minutes(IMongoCollection<BsonDocument> minutes)
        {
            using (IAsyncCursor<BsonDocument> cursor = await minutes.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        // COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                        // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");

                        try
                        {
                            int contar = 0;
                            int contardocumentos = 0;
                            string[] estados = new string[3] { "100", "100", "100" };
                            string[] fecha_estados = new string[3] { "1980-01-01", "1980-01-01", "1980-01-01" }; ;
                            string[] tipologia = new string[4];
                            var l_tipologia = document["typology"].ToBsonDocument();
                            tipologia[0] = l_tipologia["_id"].ToString();
                            tipologia[1] = l_tipologia["typology_id"].ToString();
                            tipologia[2] = l_tipologia["project_id"].ToString();
                            tipologia[3] = (l_tipologia.Contains("comisionTipologia") ? l_tipologia["comisionTipologia"].ToString() : "");
                            var l_estados = document["states"];


                            //var prueba = l.GetValue(0).ToString(); 

                            foreach (var d_estados in document["states"].AsBsonArray)
                            {
                                estados[contar] = d_estados.ToBsonDocument()["value"].ToString();
                                fecha_estados[contar] = d_estados.ToBsonDocument()["createdAt"].ToString();
                                contar += 1;
                            }
                            //while (contar < contardocumentos)
                            //{
                            //    var l_s = l_estados[contar];

                            //    estados[contar] = l_s["value"].ToString();
                            //    fecha_estados[contar] = l_s["createdAt"].ToString();
                            //}

                            Console.WriteLine((estados[0].ToString().Length == 0 ? "100" : estados[2]));
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Minutes]([id],[typeCurrency],[cashAdvance],[ordencompra],[appointment],[signaturedate],[price],[ruc],[razonsocial],[cliente_Id],[typology__id],[typology_id],[project_id],[comisionTipologia],[states_value],[States_CreatedAt],[states_value2],[States_CreatedAt2],[states_value3],[States_CreatedAt3],[CreatedAt],[UpdatedAT])" +
                            // document["date_create"].ToLocalTime().ToString("yyyy-MM-dd")
                            // document["project_id"].ToInt32()
                            //  "VALUES ('" + document["_id"].ToString() + "','" + document["typoDoc"].ToString() + "','" + document["nroDoc"].ToString() + "','" + document["email"].ToString() + "','" + document["nombres"].ToString().Replace("'", "") + "','" + document["apellidoPaterno"].ToString().Replace("'", "") + "','" + document["apellidoMaterno"].ToString().Replace("'", "") + "','" + (document.Contains("celular") ? document["celular"].ToString() : "") + "','" + (document.Contains("clienteId") ? document["clienteId"].ToString() : "") + "','" + document["customerId"].ToString() + "','" + document["tipoCliente"].ToString() + "','" + document["idPrincipalOAsociado"].ToString() + "','" + document["createdAt"].ToLocalTime().ToString("yyyy-MM-dd") + "','" + (document["updatedAt"].IsBsonNull ? "1980-01-01" : document["updatedAt"].ToLocalTime().ToString("yyyy-MM-dd")) + "','" + document["userId"].ToString() + "')";
                            "VALUES ('" + document["_id"].ToString() + "',"
                            + document["typeCurrency"].ToInt32() + ","
                            + document["cashAdvance"].ToInt32() + ","
                            + document["ordenCompra"].ToInt32() + ",'"
                            + document["appointment"].ToString().Replace("'", "") + "','"
                            + document["signatureDate"].ToLocalTime().ToString("yyyy-MM-dd") + "',"
                            + document["price"].ToInt32() + ",'"
                            + (document.Contains("ruc") ? document["ruc"].ToString() : "") + "','"
                            + document["razonSocial"].ToString() + "','"
                            + (document.Contains("clienteId") ? document["clienteId"].ToString() : "") + "','"


                            + tipologia[0].ToString() + "',"
                            + tipologia[1].ToString() + ","
                            + tipologia[2].ToString() + ",'"
                            + tipologia[3].ToString() + "',"
                            + estados[0] + ",'"
                            + fecha_estados[0] + "',"
                            + estados[1] + ",'"
                            + fecha_estados[1] + "',"
                            + estados[2] + ",'"
                            + fecha_estados[2] + "','"
                            + document["createdAt"].ToLocalTime().ToString("yyyy-MM-dd") + "','"
                            + (document["updatedAt"].IsBsonNull ? "1980-01-01" : document["updatedAt"].ToLocalTime().ToString("yyyy-MM-dd"))


                            + "')";


                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }


        private static async Task m_projects(IMongoCollection<BsonDocument> projects)
        {
            using (IAsyncCursor<BsonDocument> cursor = await projects.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        // COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                        //   SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");

                        try
                        {
                            Console.WriteLine(document["_id"].ToString());
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Proyects]([id],[nombre_quita_firma],[confirma],[condicio_pago],[project_id],[builder_id],[slug],[project_Name],[project_description],[project_phase],[project_type],[financial_bank],[departamento],[provincia],[distrito],[direccion],[coord_lat],[coord_lng],[flag],[visit_count],[visit_like],[status_value],[date_creation],[by_creation],[update_creation],[by_update],[date_release],[date_delivery],[project_phase_construction],[number_total_units],[number_total_parking],[number_total_deposits],[number_total_stages],[number_total_floors],[number_total_basements],[number_total_lifts],[visibility_nexo],[id_bank],[id_bank_type],[solicita_dni],[PIActivoProyecto],[mongo_builderId],[builder_Name],[status_value_project],[contactvalidate] ,[commissionComplete],[CreatedAt],[UpdatedAT])" +
                            "VALUES ('" + document["_id"].ToString() + "','"
                            + document["nombre_quita_firma"].ToString() + "',"
                            + document["conFirma"].ToInt32() + ","
                            + document["condition_pago"].ToInt32() + ","
                            + document["project_id"].ToInt32() + ","
                            + document["builder_id"].ToInt32() + ",'"
                            + document["slug"].ToString().Replace("'", "") + "','"
                            + document["name"].ToString().Replace("'", "") + "','"
                            + (document.Contains("description") ? document["description"].ToString().Replace("'", "") : "") + "',"
                            + document["project_phase"].ToInt32() + ","
                            + document["type_project"].ToInt32() + ",'"
                            + document["finance_bank"].ToString() + "',"
                            + document["departamento"].ToInt32() + ","
                            + document["provincia"].ToInt32() + ","
                            + document["distrito"].ToInt32() + ",'"
                            + document["direccion"].ToString() + "','"

                            + document["coord_lat"].ToString() + "','"
                            + document["coord_lng"].ToString() + "',"
                            + document["flag"].ToInt32() + ","
                            + (document["visit_count"].IsBsonNull ? 0 : document["visit_count"].ToInt32()) + ","
                            + (document["visit_like"].IsBsonNull ? 0 : document["visit_like"].ToInt32()) + ","
                            + document["status_value"].ToInt32() + ",'"
                            + document["date_creation"].ToLocalTime().ToString("yyyy-MM-dd") + "',"

                            + document["by_creation"].ToInt32() + ",'"
                            + document["update_creation"].ToLocalTime().ToString("yyyy-MM-dd") + "',"
                            + document["by_update"].ToInt32() + ",'"
                            + document["date_release"].ToLocalTime().ToString("yyyy-MM-dd") + "','"
                            + document["date_delivery"].ToLocalTime().ToString("yyyy-MM-dd") + "',"
                            + document["project_phase_construction"].ToInt32() + ","
                            + document["number_total_units"].ToInt32() + ","

                            + document["number_total_parking"].ToInt32() + ","
                            + document["number_total_deposits"].ToInt32() + ","
                            + document["number_total_stages"].ToInt32() + ","
                            + document["number_total_floors"].ToInt32() + ","
                            + document["number_total_basements"].ToInt32() + ","
                            + document["number_total_lifts"].ToInt32() + ","

                            + document["visibility_nexo"].ToInt32() + ","

                            + (document["id_bank"].IsBsonNull ? 0 : document["id_bank"].ToInt32()) + ","

                            + (document["id_bank_type"].IsBsonNull ? 0 : document["id_bank_type"].ToInt32()) + ","
                            + document["solicita_dni"].ToInt32() + ","
                            + document["PIActivoProyecto"].ToInt32() + ",'"

                            + document["mongo_builderId"].ToString() + "','"

                            + document["mongo_builderId"].ToString() + "',"
                            + document["status_valueProject"].ToInt32() + ",'"
                            + document["contactvalidate"].ToBoolean().ToString() + "','"
                            + document["commissionComplete"].ToBoolean().ToString() + "','"
                            + document["createdAt"].ToLocalTime().ToString("yyyy-MM-dd") + "','"
                            + (document["updatedAt"].IsBsonNull ? "1980-01-01" : document["updatedAt"].ToLocalTime().ToString("yyyy-MM-dd H:mm:ss")) + "')";



                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }

        private static async Task m_builder(IMongoCollection<BsonDocument> builder)
        {
            using (IAsyncCursor<BsonDocument> cursor = await builder.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        // COMERCIAL
                        SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");


                        // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");

                        try
                        {
                            Console.WriteLine(document["_id"].ToString());
                            string SqlString = "INSERT INTO [dbo].[TMP_AGT_Builder]([id],[builder_id],[customer_Id],[description],[tradename],[url_asei])" +

                            "VALUES ('" + document["_id"].ToString() + "',"
                            + document["builder_id"].ToInt32() + ","
                            + document["customer_id"].ToInt32() + ",'"
                            + document["description"].ToString().Replace("'", "") + "','"
                            + document["tradename"].ToString().Replace("'", "") + "','"
                            + document["url_asei"].ToString().Replace("'", "")
                            + "')";


                            using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                            {
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }
        //   }
        //}


        private static async Task m_properties_m(IMongoCollection<BsonDocument> properties)
        {
            /*using (IAsyncCursor<BsonDocument> cursor = await properties.FindAsync(new BsonDocument()))
            {*/
            // var i = 0;
            var filtro = Builders<BsonDocument>.Filter.Empty;
            var resultado = properties.Find<BsonDocument>(filtro).ToList();
            foreach (BsonDocument document in resultado)
            {
                // COMERCIAL
                SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");
                // ,[mongo_project] ,[mongo_TypologyId]
                try
                {
                    //  i += 1;
                    Console.WriteLine(document["_id"].ToString());
                    //Console.WriteLine(i.ToString());
                    string SqlString = "INSERT INTO [dbo].[TMP_AGT_Properties_m]([id],[current_comision],[property_id] ,[typology_id] ,[propety_Name],[type_currency],[price],[status_value] ,[is_sold]   ,[price_PEN],[price_USD] ,[CreatedAt])" +

                    "VALUES ('" + document["_id"].ToString() + "',"
                     + (document["currentComision"].IsBsonNull ? 0 : document["currentComision"].ToInt32()) + ",'"
                    + document["property_id"].ToString() + "','"
                    + document["typology_id"].ToString() + "','"
                    + document["name"].ToString() + "',"
                    + document["type_currency"].ToInt32() + ","
                    + document["price"].ToInt32() + ","
                    + document["status_value"].ToInt32() + ","
                    + (document["is_sold"].IsBsonNull ? 0 : document["is_sold"].ToInt32()) + ","




                    //  + document["mongo_Project"].ToString().Replace("'", "") + "','"

                    //  + document["mongo_typologyId"].ToString().Replace("'", "") + "','"

                    + (document["pricePEN"].IsBsonNull ? 0 : document["pricePEN"].ToInt32()) + ","
                    + (document["priceUSD"].IsBsonNull ? 0 : document["priceUSD"].ToInt32()) + ",'"
                    + (document["date_create"].ToLocalTime().ToString("yyyy-MM-dd")) + "')";


                    using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            /*    var i = 0;
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (BsonDocument document in batch)
                {
                    // COMERCIAL
                    SqlConnection cn = new SqlConnection("Data Source=asei.database.windows.net;Initial Catalog=PowerBI_ASEI;Persist Security Info=True;User ID=aluna;Password=Server2015");

                    // SqlConnection cn = new SqlConnection("Data Source=aseiagentes.database.windows.net;Initial Catalog=PowerBIAgentes_ASEI;Persist Security Info=True;User ID=aseiagentes;Password=Server2021$");
                    // ,[mongo_project] ,[mongo_TypologyId]
                    try
                    {
                        i += 1;
                        Console.WriteLine(document["_id"].ToString());
                        Console.WriteLine(i.ToString());
                        string SqlString = "INSERT INTO [dbo].[TMP_AGT_Properties_m]([id],[current_comision],[property_id] ,[typology_id] ,[propety_Name],[type_currency],[price],[status_value] ,[is_sold]   ,[price_PEN],[price_USD] ,[CreatedAt])" +

                        "VALUES ('" + document["_id"].ToString() + "',"
                         + (document["currentComision"].IsBsonNull ? 0 : document["currentComision"].ToInt32()) + ",'"
                        + document["property_id"].ToString() + "','"
                        + document["typology_id"].ToString() + "','"
                        + document["name"].ToString() + "',"
                        + document["type_currency"].ToInt32() + ","
                        + document["price"].ToInt32() + ","
                        + document["status_value"].ToInt32() + ","
                        + (document["is_sold"].IsBsonNull ? 0 : document["is_sold"].ToInt32()) + ","




                        //  + document["mongo_Project"].ToString().Replace("'", "") + "','"

                        //  + document["mongo_typologyId"].ToString().Replace("'", "") + "','"

                        + (document["pricePEN"].IsBsonNull?0:document["pricePEN"].ToInt32()) + ","
                        + (document["priceUSD"].IsBsonNull ? 0 : document["priceUSD"].ToInt32()) + ",'"
                        + (document["date_create"].ToLocalTime().ToString("yyyy-MM-dd")) + "')";


                        using (SqlCommand cmd = new SqlCommand(SqlString, cn))
                        {
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }*/
        }
        #endregion
    }
}
