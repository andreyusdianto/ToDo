using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace Todo_RestAPI.Models
{
    public class ToDo
    {
        private static PhysicalFileProvider _fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public int CompletePercentage { set; get; }
        public DateTime ExpiryDate { set; get; }
        public bool IsDone { set; get; }

        const string dbaseName = "/assets/dbase.json";

        public ToDo(int Id, string Title, string Description, int CompletePercentage, DateTime ExpiryDate)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.CompletePercentage = CompletePercentage;
            this.ExpiryDate = ExpiryDate;
            this.IsDone = false;
        }

        public static void CommitChanges(List<ToDo> TodoList, string rootPath)
        {
            var filePath = rootPath + dbaseName;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(TodoList);
            System.IO.File.WriteAllText(filePath, jsonString);
        }

        public static List<ToDo> LoadData(string rootPath)
        {
            var filePath = rootPath + dbaseName;
            var dbase_content = System.IO.File.ReadAllText(filePath);
            var dbRes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDo>>(dbase_content);

            if (dbRes == null) dbRes = new List<ToDo>();
            return dbRes;
        }
    }
}
