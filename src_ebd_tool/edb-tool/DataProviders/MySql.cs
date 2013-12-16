﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace edb_tool
{
    public class MySql : DataProvider
    {
        private MySqlConnection conn;
        private string connStr;

        public MySql()
        {

            //connStr = String.Format("server={0};user id={1}; password={2}; database=bdanton; pooling=false", "sql-devel.gipsa-lab.grenoble-inp.fr", "anton", "an2013!ton!");
            connStr = String.Format("server={0};user id={1}; password={2}; database=bdanton; pooling=false", "sql-devel.gipsa-lab.grenoble-inp.fr", "appli_anton", "mutra45");
        }

        #region subjects

        public void AddSubject(GSubject subject)
        {
            conn = new MySqlConnection(connStr);

            conn.Open();

            string query = "INSERT INTO subject (name, age, sex, idexperiment, iduser) VALUES (@name, @age, @sex, @idexperiment, @iduser)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", subject.name);
                if (subject.age != null) cmd.Parameters.AddWithValue("@age", subject.age); else cmd.Parameters.AddWithValue("@age", DBNull.Value);
                cmd.Parameters.AddWithValue("@sex", subject.sex);
                cmd.Parameters.AddWithValue("@idexperiment", subject.idexperiment);
                if (subject.iduser != null) cmd.Parameters.AddWithValue("@iduser", subject.iduser); else cmd.Parameters.AddWithValue("@iduser", DBNull.Value);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GSubject> ListSubjects(int iduser)
        {
            conn = new MySqlConnection(connStr);

            string query = "select * from subject where iduser = @iduser order by idsubject";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = new MySqlCommand(query, conn);
                myDA.SelectCommand.Parameters.AddWithValue("@iduser", iduser);

                DataTable table = new DataTable();
                myDA.Fill(table);

                var squery = from DataRow row in table.Rows
                             select new GSubject
                             {
                                 idsubject = Convert.ToInt32(row["idsubject"]),
                                 name = (string)row["name"],
                                 age = (row["age"] != System.DBNull.Value) ? Convert.ToInt32(row["age"]) : (int?)null,
                                 sex = Convert.ToInt32(row["sex"]),
                                 idexperiment = Convert.ToInt32(row["idexperiment"]),
                                 iduser = iduser,
                             };

                return squery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        public List<GSubject> ListSubjectsByExperimentId(int idexperiment, int iduser)
        {
            conn = new MySqlConnection(connStr);

            //modified 4/12/2013 not to consider iduser: //AND iduser = @iduser
            string query = "select * from subject where idexperiment = @idexperiment"; 

            try
            {
                conn.Open();

                MySqlDataAdapter myDA = new MySqlDataAdapter();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                //cmd.Parameters.AddWithValue("@iduser", iduser);

                myDA.SelectCommand = cmd;

                DataTable table = new DataTable();
                myDA.Fill(table);

                var squery = from DataRow row in table.Rows
                              select new GSubject
                              {
                                  idsubject = Convert.ToInt32(row["idsubject"]),
                                  name = (string)row["name"],
                                  age = (row["age"] != System.DBNull.Value) ? Convert.ToInt32(row["age"]) : (int?)null,
                                  sex = Convert.ToInt32(row["sex"]),
                                  idexperiment = Convert.ToInt32(row["idexperiment"]),
                                  iduser = iduser,
                              };

                return squery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        public void DeleteSubject(int id)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "delete from subject where idsubject = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void UpdateSubject(GSubject s)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "UPDATE subject set name=@name, age=@age, sex=@sex, idexperiment = @idexperiment where idsubject = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", s.name);
                cmd.Parameters.AddWithValue("@age", s.age ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@sex", s.sex);
                cmd.Parameters.AddWithValue("@id", s.idsubject);
                cmd.Parameters.AddWithValue("@idexperiment", s.idexperiment);             

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        #endregion

        #region experiments

        public void AddExperiment(GExperiment exp)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO experiment (name, comment, description, iduser,exp_date) VALUES (@name, @comment,@description, @iduser,@exp_date)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", exp.name);
                cmd.Parameters.AddWithValue("@comment", exp.comment);
                cmd.Parameters.AddWithValue("@description", exp.description);
                cmd.Parameters.AddWithValue("@iduser", exp.iduser);
                cmd.Parameters.AddWithValue("@exp_date", exp.exp_date);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GExperiment> ListExperiments(int iduser)
        {
            List<GExperiment> result;

            conn = new MySqlConnection(connStr);

            string query = "select * from experiment where iduser = @iduser order by idexperiment";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = new MySqlCommand(query, conn);
                myDA.SelectCommand.Parameters.AddWithValue("@iduser", iduser);

                DataTable table = new DataTable();
                myDA.Fill(table);

                //BindingSource bSource = new BindingSource();
                //bSource.DataSource = table;

                var equery = from DataRow row in table.Rows
                             select new GExperiment
                             {
                                 idexperiment = Convert.ToInt32(row.ItemArray[0]),
                                 name = (string) row.ItemArray[1],
                                 comment = Convert.ToString(row.ItemArray[2]),
                                 description = Convert.ToString((string) row.ItemArray[3]),
                                 iduser = (int) row.ItemArray[4],
                                 exp_date = (DateTime)row.ItemArray[5],
                             };

                result =  equery.ToList();  
            }
            catch (MySqlException ex)
            {
              //dbase.displayError(ex.Message, ex.Number);
                return null;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public List<GExperiment> ListExperimentsByExperimentIdUserId(int idexperiment, int iduser)
        {
            conn = new MySqlConnection(connStr);

            //modified 4/12/2013 not to consider the iduser
            string query = "select * from experiment where idexperiment = @idexperiment order by idexperiment"; //iduser = @iduser AND 

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = new MySqlCommand(query, conn);
                //myDA.SelectCommand.Parameters.AddWithValue("@iduser", iduser);
                myDA.SelectCommand.Parameters.AddWithValue("@idexperiment", idexperiment);

                DataTable table = new DataTable();
                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                             select new GExperiment
                             {
                                 idexperiment = Convert.ToInt32(row.ItemArray[0]),
                                 name = (string)row.ItemArray[1],
                                 comment = Convert.ToString(row.ItemArray[2]),
                                 description = Convert.ToString((string)row.ItemArray[3]),
                                 iduser = (int)row.ItemArray[4],
                                 exp_date = (DateTime)row.ItemArray[5],
                             };

                return equery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        public void DeleteExperiment(int id)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "delete from experiment where idexperiment = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void UpdateExperiment(GExperiment exp)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "UPDATE experiment set name=@name, comment = @comment, description = @description, exp_date = @exp_date where idexperiment = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", exp.name);
                cmd.Parameters.AddWithValue("@comment", exp.comment);
                cmd.Parameters.AddWithValue("@description", exp.description);
                cmd.Parameters.AddWithValue("@id", exp.idexperiment);
                cmd.Parameters.AddWithValue("@exp_date", exp.exp_date);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void AddSharedExperiment(int idexperiment, int owneruserid, int targetuserid)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO shared_experiment_user (idexperiment, owner_userid, target_userid) VALUES (@idexperiment, @owneruserid,@targetuserid)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                cmd.Parameters.AddWithValue("@owneruserid", owneruserid);
                cmd.Parameters.AddWithValue("@targetuserid", targetuserid);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GUser> ListTagetUsers(int idexperiment, int owner_userid)
        {
            DataTable table = new DataTable();
            conn = new MySqlConnection(connStr);
            string query = "SELECT * FROM user WHERE  iduser IN (SELECT target_userid FROM shared_experiment_user WHERE idexperiment=@idexperiment AND owner_userid=@owner_userid)";

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                cmd.Parameters.AddWithValue("@owner_userid", owner_userid);

                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                                select new GUser
                                {
                                    iduser = Convert.ToInt32(row["iduser"]),
                                    FirstName = (string)row["firstname"],
                                    LastName = (string)row["lastname"],
                                    EMail = (string)row["email"],
                                    Username = (string)row["username"],
                                };

                return equery.ToList();
                
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteSharedExperiment(int idexperiment, int owner_userid)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "delete from shared_experiment_user where idexperiment = @idexperiment and owner_userid = @owner_userid";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                cmd.Parameters.AddWithValue("@owner_userid", owner_userid);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GExperiment> ListExperimentsSharedToTheUserByOthers(int target_userid)
        {
            conn = new MySqlConnection(connStr);


            string query = "select * from experiment where idexperiment in (select idexperiment from shared_experiment_user where @target_userid = target_userid)";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = new MySqlCommand(query, conn);
                myDA.SelectCommand.Parameters.AddWithValue("@target_userid", target_userid);

                DataTable table = new DataTable();
                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                             select new GExperiment
                             {
                                 idexperiment = Convert.ToInt32(row.ItemArray[0]),
                                 name = (string)row.ItemArray[1],
                                 comment = Convert.ToString(row.ItemArray[2]),
                                 description = Convert.ToString((string)row.ItemArray[3]),
                                 iduser = (int)row.ItemArray[4],
                                 exp_date = (DateTime)row.ItemArray[5],
                             };

                return equery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        #endregion


        #region modality

        public void AddModality(GModality m)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO modality (name, comment, description) VALUES (@name, @comment,@description)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", m.name);
                if (m.comment != null) cmd.Parameters.AddWithValue("@comment", m.comment); else cmd.Parameters.AddWithValue("@comment", System.DBNull.Value);
                if (m.description != null) cmd.Parameters.AddWithValue("@description", m.description); else cmd.Parameters.AddWithValue("@description", System.DBNull.Value);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GModality> ListModalities()
        {
            conn = new MySqlConnection(connStr);


            string query = "select * from modality order by idmodality";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = new MySqlCommand(query, conn);

                DataTable table = new DataTable();
                myDA.Fill(table);

                BindingSource bSource = new BindingSource();
                bSource.DataSource = table;

                var equery = from DataRow row in table.Rows
                             select new GModality
                             {
                                 idmodality = Convert.ToInt32(row["idmodality"]),
                                 name = (string)row["name"],
                                 comment = (row["comment"] != System.DBNull.Value) ? Convert.ToString(row["comment"]) : null,
                                 description = (row["description"] != System.DBNull.Value) ? Convert.ToString(row["description"]) : null,
                             };

                return equery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        public void DeleteModality(int id)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "delete from modality where idmodality = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void UpdateModality(GModality m)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "UPDATE modality set name=@name, comment = @comment, description = @description where idmodality = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", m.name);
                cmd.Parameters.AddWithValue("@comment", m.comment);
                cmd.Parameters.AddWithValue("@description", m.description);
                cmd.Parameters.AddWithValue("@id", m.idmodality);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        #endregion

        #region tags

        public void AddTag(GTag tag)//, string comment, string description
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO tag (name) VALUES (@name)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", tag.name);
                //cmd.Parameters.AddWithValue("@comment", comment);
                //cmd.Parameters.AddWithValue("@description", description);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GTag> ListTags()
        {
            conn = new MySqlConnection(connStr);


            string query = "select * from tag order by idtag";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                myDA.SelectCommand = new MySqlCommand(query, conn);

                DataTable table = new DataTable();
                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                             select new GTag
                             {
                                 idtag = Convert.ToInt32(row["idtag"]),
                                 name = (string)row["name"],
                             };

                return equery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        public void DeleteTag(int id)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "delete from tag where idtag = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void UpdateTag(GTag tag) //, string comment, string description
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "UPDATE tag set name=@name where idtag = @id";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", tag.name);
                //cmd.Parameters.AddWithValue("@comment", comment);
                //cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@id", tag.idtag);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void AssociateTags(List<GTag> tags, int idfile, int idexperiment)
        {
            //long insertid = -1;

            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT list_tag (idtag, idfile, idexperiment) VALUES ";
            for (int i = 0; i < tags.Count; i++)
            {
                if (i != 0) query += ",";
                query += string.Format("(@idtag{0}, @idfile, @idexperiment)", i);
            }

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                for (int i = 0; i < tags.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@idtag" + i, tags[i].idtag);
                }

                cmd.Parameters.AddWithValue("@idfile", idfile);
                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);

                //Execute command
                cmd.ExecuteNonQuery();
                //insertid = cmd.LastInsertedId;

                //close connection
                conn.Close();
            }

            //return insertid;
        }

    
        #endregion

        public void AddModalityToExperiment(int idmodality,int idexperiment)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO list_modality (idmodality, idexperiment) VALUES (@idmodality, @idexperiment)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idmodality", idmodality);
                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GModality> ListModalitiesByExperimentID(int idexperiment)
        {
            conn = new MySqlConnection(connStr);


            string query = @"select modality.idmodality,modality.name 
                            from modality,list_modality 
                            where modality.idmodality = list_modality.idmodality 
                            and idexperiment = @idexperiment
                            order by list_modality.idlist_modality";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                myDA.SelectCommand = cmd;

                DataTable table = new DataTable();
                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                             select new GModality
                             {
                                 idmodality = Convert.ToInt32(row.ItemArray[0]),
                                 name = (string)row.ItemArray[1],
                             };

                return equery.ToList();
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        #region file

        public long AddFile(GFile file)
        {
            long insertid = -1;

            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO file (filename, pathname) VALUES (@filename, @pathname)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@filename", file.filename);
                cmd.Parameters.AddWithValue("@pathname", file.pathname);

                //Execute command
                cmd.ExecuteNonQuery();
                insertid = cmd.LastInsertedId;

                //close connection
                conn.Close();
            }

            return insertid;
        }

        public void AddFiles(List<GFile> files)
        {
            //long insertid = -1;

            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO file (filename, pathname) VALUES ";
            for(int i=0; i < files.Count; i++)
            {
                if (i!=0) query+= ",";
                query +="(@filename"+i+", @pathname"+i+")";

            }

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                for(int i=0; i < files.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@filename"+i, files[i].filename);
                    cmd.Parameters.AddWithValue("@pathname"+i, files[i].pathname);
                }
                //Execute command
                cmd.ExecuteNonQuery();
                //insertid = cmd.LastInsertedId;

                //close connection
                conn.Close();
            }

            //return insertid;
        }

        public void AssociateFile(int idexperiment, int idsubject, int idmodality,long idfile)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO list_file (idexperiment, idsubject, idmodality, idfile) VALUES (@idexperiment, @idsubject, @idmodality, @idfile)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                cmd.Parameters.AddWithValue("@idsubject", idsubject);
                cmd.Parameters.AddWithValue("@idmodality", idmodality);
                cmd.Parameters.AddWithValue("@idfile", idfile);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public List<GFile> ListFilesByExperimentSubjectModalityID(int idexperiment, int idsubject, int idmodality)
        {
            conn = new MySqlConnection(connStr);


            string query = @"select file.idfile,file.filename,file.pathname,file.tags
                            from file,list_file 
                            where file.idfile = list_file.idfile 
                            and idexperiment = @idexperiment
                            and idsubject = @idsubject
                            and idmodality = @idmodality
                            order by list_file.idlist_file";

            try
            {
                conn.Open();
                MySqlDataAdapter myDA = new MySqlDataAdapter();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                cmd.Parameters.AddWithValue("@idsubject", idsubject);
                cmd.Parameters.AddWithValue("@idmodality", idmodality);
                myDA.SelectCommand = cmd;

                DataTable table = new DataTable();
                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                             select new GFile
                             {
                                 idfile = Convert.ToInt32(row["idfile"]),
                                 filename = (string)row["filename"],
                                 pathname = (string)row["pathname"],
                                 tags = (row["tags"]==System.DBNull.Value) ? "" : (string)row["tags"],
                             };

                return equery.ToList();
                
            }
            //catch (MySqlException ex)
            //{
            //  dbase.displayError(ex.Message, ex.Number);
            //}
            finally
            {
                conn.Close();
            }
        }

        public void DeleteFilesByExperimentIdSubjectIdModalityId(int idexperiment, int idsubject, int idmodality)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = @"delete from list_file where  
                            idfile = @idfile 
                            and idexperiment = @idexperiment
                            and idsubject = @idsubject
                            and idmodality = @idmodality";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idexperiment", idexperiment);
                cmd.Parameters.AddWithValue("@idsubject", idsubject);
                cmd.Parameters.AddWithValue("@idmodality", idmodality);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void DeleteFilesByFileId(int idfile)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = @"delete from file where idfile = @idfile";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idfile", idfile);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void DeleteFilesByFileIdFromListFile(int idfile)
        {
            //list_file (provides the connection between the file and the experiment, subject)

            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = @"delete from list_file where idfile = @idfile";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idfile", idfile);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public void RemoveTags(int idfile)
        {
            //list_tag (provides the connection between the file and tags)

            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = @"delete from list_tag where idfile = @idfile";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idfile", idfile);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }


        #endregion

        #region users
        public void AddUser(GUser u,string password)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "INSERT INTO user (username, firstname, lastname, email, password) VALUES (@username, @firstname, @lastname, @email, @password)";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@username", u.Username);
                cmd.Parameters.AddWithValue("@firstname", u.FirstName);
                cmd.Parameters.AddWithValue("@lastname", u.LastName);
                cmd.Parameters.AddWithValue("@email", u.EMail);
                cmd.Parameters.AddWithValue("@password", password);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }

        public bool VerifyUserPassword(string username, string password,out int userid)
        {
            DataTable table = new DataTable();
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "SELECT iduser FROM user where username = @username AND password = @password;";
            userid = -1;

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataAdapter myDA = new MySqlDataAdapter();

                myDA.SelectCommand = cmd;

                myDA.Fill(table);

                if (table.Rows.Count>0)
                    userid = Convert.ToInt32(table.Rows[0].ItemArray[0]);

                //close connection
                conn.Close();
            }

            if (table.Rows.Count > 0) return true;
            else return false;
        }

        public List<GUser> ListUsers()
        {
            DataTable table = new DataTable();
            conn = new MySqlConnection(connStr);
            string query = "SELECT * FROM user";

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataAdapter myDA = new MySqlDataAdapter();

                myDA.SelectCommand = cmd;

                myDA.Fill(table);

                var equery = from DataRow row in table.Rows
                                select new GUser
                                {
                                    iduser = Convert.ToInt32(row["iduser"]),
                                    FirstName = (string)row["firstname"],
                                    LastName = (string)row["lastname"],
                                    EMail = (string)row["email"],
                                    Username = (string)row["username"],
                                };

                return equery.ToList();
                
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        public void UpdateFileTags(int[] idfiles, string tagLine)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "UPDATE file SET tags = @tags WHERE idfile in (";

            string[] idfilesstring = idfiles.Select(x => x.ToString()).ToArray();

            //separate somehow in the string 
            string idsLine = idfilesstring.Aggregate(new StringBuilder(), (current, next) => current.Append(", ").Append(next)).ToString();

            query += idsLine.Substring(1) + ")";

            //open connection
            if (conn.State == ConnectionState.Open == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@tags", tagLine);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
            }
        }
    }
}