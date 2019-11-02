using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_RestAPI.Models
{
    public class ToDo
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public int CompletePercentage { set; get; }
        public DateTime ExpiryDate { set; get; }
        public bool IsDone { set; get; }

        public ToDo(int Id, string Title, string Description, int CompletePercentage, DateTime ExpiryDate)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.CompletePercentage = CompletePercentage;
            this.ExpiryDate = ExpiryDate;
            this.IsDone = false;
        }
    }
}
