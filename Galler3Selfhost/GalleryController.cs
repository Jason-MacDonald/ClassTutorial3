﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery3Selfhost 
{
    public class GalleryController : System.Web.Http.ApiController
    {
        public List<string> GetArtistNames()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }

        public clsArtist GetArtist(string Name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Artist WHERE Name = @Name", par);
            if (lcResult.Rows.Count > 0)
                return new clsArtist()
                {
                    Name = (string)lcResult.Rows[0]["Name"],
                    Speciality = (string)lcResult.Rows[0]["Speciality"],
                    Phone = (string)lcResult.Rows[0]["Phone"],
                    WorksList = getArtistWork(Name)
                };
            else
                return null;
        }

        private List<clsAllWork> getArtistWork(string prName)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", prName);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Work WHERE ArtistName = @Name", par);
            List<clsAllWork> lcWorks = new List<clsAllWork>();
            foreach (DataRow dr in lcResult.Rows)
                lcWorks.Add(dataRow2AllWork(dr));
            return lcWorks;

        }

        private clsAllWork dataRow2AllWork(DataRow prDataRow)
        {
            return new clsAllWork()
            {
                WorkType = Convert.ToChar(prDataRow["WorkType"]),
                Name = Convert.ToString(prDataRow["Name"]),
                Date = Convert.ToDateTime(prDataRow["Date"]),
                Value = Convert.ToDecimal(prDataRow["Value"]),
                Width = prDataRow["Width"] is DBNull ? (float?)null : Convert.ToSingle(prDataRow["Width"]),
                Height = prDataRow["Height"] is DBNull ? (float?)null : Convert.ToSingle(prDataRow["Height"]),
                Type = Convert.ToString(prDataRow["Name"]),
                Weight = prDataRow["Weight"] is DBNull ? (float?)null : Convert.ToSingle(prDataRow["Weight"]),
                Material = Convert.ToString(prDataRow["Material"]),
                ArtistName = Convert.ToString(prDataRow["ArtistName"])
            };

        }

        public string PutArtist(clsArtist prArtist)
        {   // update
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE Artist SET Speciality = @Speciality, Phone = @Phone WHERE Name = @Name",
                    prepareArtistParameters(prArtist));
                if (lcRecCount == 1)
                    return "One artist updated";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        public string PostArtist(clsArtist prArtist)
        {   // update
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "INSERT INTO Artist VALUES (@Name, @Speciality, @Phone)",
                    prepareArtistParameters(prArtist));
                if (lcRecCount == 1)
                    return "One artist added";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareArtistParameters(clsArtist prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(3);
            par.Add("Name", prArtist.Name);
            par.Add("Speciality", prArtist.Speciality);
            par.Add("Phone", prArtist.Phone);
            return par;
        }

        public string PutArtwork(clsAllWork prWork)
        {   // update
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE Work " +
                    "SET WorkType = @WorkType, Name = @Name, Date = @Date, Value = @Value, Width = @Width, Height = @Height, " +
                    "Type = @Type, Weight = @Weight, Material = @Material, ArtistName = @ArtistName " +
                    "WHERE Name = @Name AND ArtistName = @ArtistName",
                    prepareWorkParameters(prWork));
                if (lcRecCount == 1)
                    return "One artwork updated";
                else
                    return "Unexpected work update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        public string PostArtWork(clsAllWork prWork)
        {   // insert
            try
            {
                int lcRecCount = clsDbConnection.Execute("INSERT INTO Work " +
                "(WorkType, Name, Date, Value, Width, Height, Type, Weight, Material, ArtistName) " +
                "VALUES (@WorkType, @Name, @Date, @Value, @Width, @Height, @Type, @Weight, @Material, @ArtistName)",
                prepareWorkParameters(prWork));
                if (lcRecCount == 1)
                    return "One artwork inserted";
                else
                    return "Unexpected artwork insert count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareWorkParameters(clsAllWork prWork)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(10);
            par.Add("WorkType", prWork.WorkType);
            par.Add("Name", prWork.Name);
            par.Add("Date", prWork.Date);
            par.Add("Value", prWork.Value);
            par.Add("Width", prWork.Width);
            par.Add("Height", prWork.Height);
            par.Add("Type", prWork.Type);
            par.Add("Weight", prWork.Weight);
            par.Add("Material", prWork.Material);
            par.Add("ArtistName", prWork.ArtistName);

            // Etc: your turn:

            return par;
        }
    }
}
