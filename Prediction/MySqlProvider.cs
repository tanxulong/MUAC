﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace AsterixDisplayAnalyser
{
    class MySqlProvider
    {
        // This is the My Sql Connection string
        private string connString;

        public MySqlProvider()
        {
            connString = "server=" + Properties.Settings.Default.MySqlServer +
                ";User Id=" + Properties.Settings.Default.MySqlUser + ";database=" + Properties.Settings.Default.mySqlDatabase;
        }

        // This class encapsulates one row of prediction data
        public class PredictionDataSetOneRow
        {
            public string ACID = null;
            public DateTime Time;
            public string Lat = null;
            public string Lon = null;
            public string FL = null;
            public string Accuracy = null;
        }

        // Enumeration, specifying prediction table/algorithim type
        public enum PredictionTableNumberType { One, Two, Three };

        public bool IsConnectionOK()
        {
            bool ConnectionOK = true;

            MySqlConnection conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch
            {
                ConnectionOK = false;
            }

            conn.Close();
            return ConnectionOK;
        }

        // This method returns table string to be used in SQL string
        private string GetTableString(PredictionTableNumberType TableNumber)
        {

            string PredictionTable = PredictionTable = "prediction1";
            switch (TableNumber)
            {
                case PredictionTableNumberType.One:
                    PredictionTable = "prediction1";
                    break;
                case PredictionTableNumberType.Two:
                    PredictionTable = "prediction2";
                    break;
                case PredictionTableNumberType.Three:
                    PredictionTable = "prediction3";
                    break;
                default:
                    PredictionTable = "prediction1";
                    break;
            }

            return PredictionTable;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This method returns all the available data from the given table number.
        public System.Collections.Generic.List<PredictionDataSetOneRow> GetAllData(PredictionTableNumberType TableNumber)
        {
            System.Collections.Generic.List<PredictionDataSetOneRow> DataRetreived = new System.Collections.Generic.List<PredictionDataSetOneRow>();

            string sql = @" 
                            SELECT
                                acid,
                                time,
                                lat,
                                lon,
                                fl,
                                acc
                            FROM " + GetTableString(TableNumber);

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PredictionDataSetOneRow OneRow = new PredictionDataSetOneRow();
                    if (rdr.GetString(0).Length > 0)
                    {
                        OneRow.ACID = rdr.GetString(0);
                        OneRow.Time = rdr.GetDateTime(1);
                        OneRow.Lat = rdr.GetString(2);
                        OneRow.Lon = rdr.GetString(3);
                        OneRow.FL = rdr.GetString(4);
                        OneRow.Accuracy = rdr.GetString(5);
                        DataRetreived.Add(OneRow);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            return DataRetreived;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This method returns all the available data from the given table number for the specifed ACID
        public System.Collections.Generic.List<PredictionDataSetOneRow> GetAllDataForACID(PredictionTableNumberType TableNumber, string ACID)
        {
            System.Collections.Generic.List<PredictionDataSetOneRow> DataRetreived = new System.Collections.Generic.List<PredictionDataSetOneRow>();

            // Get the data for ceratin time range
            string sql = @" 
                            SELECT
                                acid,
                                time,
                                lat,
                                lon,
                                fl,
                                acc
                            FROM " + GetTableString(TableNumber) +
                " WHERE acid = " + "'" + ACID + "'";

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PredictionDataSetOneRow OneRow = new PredictionDataSetOneRow();
                    if (rdr.GetString(0).Length > 0)
                    {
                        OneRow.ACID = rdr.GetString(0);
                        OneRow.Time = rdr.GetDateTime(1);
                        OneRow.Lat = rdr.GetString(2);
                        OneRow.Lon = rdr.GetString(3);
                        OneRow.FL = rdr.GetString(4);
                        OneRow.Accuracy = rdr.GetString(5);
                        DataRetreived.Add(OneRow);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            return DataRetreived;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This method returns all the available data from the given table number for the specifed Time
        public System.Collections.Generic.List<PredictionDataSetOneRow> GetAllDataForTime(PredictionTableNumberType TableNumber, TimeSpan TimeToAdd, bool DBM_Lookup)
        {
            System.Collections.Generic.List<PredictionDataSetOneRow> DataRetreived = new System.Collections.Generic.List<PredictionDataSetOneRow>();

            string Date_And_Time = MySqlDateTimeUtility.BuildMySqlDateTimeString(TimeToAdd, DBM_Lookup, TableNumber);
            
            //  Get the data for ceratin time range
            // Get the data for ceratin time range
            string sql = @" 
                            SELECT
                                acid,
                                time,
                                lat,
                                lon,
                                fl,
                                acc
                            FROM " + GetTableString(TableNumber) +
                " WHERE time >= " + Date_And_Time;

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PredictionDataSetOneRow OneRow = new PredictionDataSetOneRow();
                    if (rdr.GetString(0).Length > 0)
                    {
                        OneRow.ACID = rdr.GetString(0);
                        OneRow.Time = rdr.GetDateTime(1);
                        OneRow.Lat = rdr.GetString(2);
                        OneRow.Lon = rdr.GetString(3);
                        OneRow.FL = rdr.GetString(4);
                        OneRow.Accuracy = rdr.GetString(5);
                        DataRetreived.Add(OneRow);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            return DataRetreived;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This method returns one record of data for specifed Time and ACID
        public System.Collections.Generic.List<PredictionDataSetOneRow> GetOneRecordPerTimeAndACID(PredictionTableNumberType TableNumber, TimeSpan TimeToAdd, string ACID, bool DBM_Lookup)
        {
            System.Collections.Generic.List<PredictionDataSetOneRow> DataRetreived = new System.Collections.Generic.List<PredictionDataSetOneRow>();

            string Date_And_Time = MySqlDateTimeUtility.BuildMySqlDateTimeString(TimeToAdd, DBM_Lookup, TableNumber);

            //  Get the data for ceratin time range
            // Get the data for ceratin time range
            string sql = @" 
                            SELECT
                                acid,
                                time,
                                lat,
                                lon,
                                fl,
                                acc
                            FROM " + GetTableString(TableNumber) +
                " WHERE time >= " + Date_And_Time + " AND acid = " + "'" + ACID + "'" + " LIMIT 1";

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PredictionDataSetOneRow OneRow = new PredictionDataSetOneRow();
                    if (rdr.GetString(0).Length > 0)
                    {
                        OneRow.ACID = rdr.GetString(0);
                        OneRow.Time = rdr.GetDateTime(1);
                        OneRow.Lat = rdr.GetString(2);
                        OneRow.Lon = rdr.GetString(3);
                        OneRow.FL = rdr.GetString(4);
                        OneRow.Accuracy = rdr.GetString(5);
                        DataRetreived.Add(OneRow);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            return DataRetreived;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This method returns all available ACIDs, distinct
        public System.Collections.Generic.List<PredictionDataSetOneRow> GetAllACIDsDataDistinct(PredictionTableNumberType TableNumber)
        {
            System.Collections.Generic.List<PredictionDataSetOneRow> DataRetreived = new System.Collections.Generic.List<PredictionDataSetOneRow>();

            string sql = @" 
                            SELECT DISTINCT
                                acid
                            FROM " + GetTableString(TableNumber);

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PredictionDataSetOneRow OneRow = new PredictionDataSetOneRow();
                    if (rdr.GetString(0).Length > 0)
                    {
                        OneRow.ACID = rdr.GetString(0);
                        DataRetreived.Add(OneRow);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            return DataRetreived;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This method returns the time of the first record from a DBM.
        public System.Collections.Generic.List<PredictionDataSetOneRow> GetFirstTimeFromTable(PredictionTableNumberType TableNumber)
        {
            System.Collections.Generic.List<PredictionDataSetOneRow> DataRetreived = new System.Collections.Generic.List<PredictionDataSetOneRow>();

            string sql = @" 
                            SELECT 
                                time
                            FROM " + GetTableString(TableNumber) + " ORDER BY time ASC";

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PredictionDataSetOneRow OneRow = new PredictionDataSetOneRow();
                    try
                    {
                        if (rdr.GetDateTime(0) != null)
                        {
                            OneRow.Time = rdr.GetDateTime(0);
                            DataRetreived.Add(OneRow);
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            return DataRetreived;
        }
    }
}
