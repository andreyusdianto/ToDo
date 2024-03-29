﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Todo_RestAPI.Models;

namespace Todo_RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _rootPath;
        private readonly List<ToDo> ToDoList;

        public ValuesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _rootPath = _hostingEnvironment.ContentRootPath;
            //load data from static file (assets/dbase.json)
            ToDoList = ToDo.LoadData(_rootPath);
        }

        //Create New To Do
        [HttpPost]
        public ActionResult<string> Create()
        {
            //get post body from request
            var title = HttpContext.Request.Form["Title"];
            var desc = HttpContext.Request.Form["Description"];
            var expiryDate = DateTime.Parse(HttpContext.Request.Form["ExpiryDate"]);
            var completePercentage = int.Parse(HttpContext.Request.Form["CompletePercentage"]);

            var id = 1;
            //If todo list not empty then get max id + 1
            if (ToDoList.Count > 0) id = ToDoList.OrderByDescending(f => f.Id).First().Id + 1;

            //push item to list
            ToDoList.Add(new ToDo(id, title, desc, completePercentage, expiryDate));
            ToDo.CommitChanges(ToDoList, _rootPath);

            return "ok";
        }

        //Get All To Do
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> Get()
        {
            //return everything
            return ToDoList;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<ToDo> Get(int id)
        {
            //find item in todo list by id
            var foundItem = ToDoList.Where(f => f.Id == id).FirstOrDefault();

            return foundItem;
        }

        //Get Incoming ToDo
        // GET api/values/datetime
        [HttpGet("IncomingToDo/{expiryDate}")]
        public ActionResult<IEnumerable<ToDo>> Get(DateTime expiryDate)
        {
            //find item in todo list with logic expirydate > expirydate request
            return ToDoList.Where(f => f.ExpiryDate > expiryDate).ToList();
        }

        
        //Update Todo as well (title, desc, expirydate)
        [HttpPost("Update")]
        public ActionResult<string> Update()
        {
            //get post body from request
            var id = int.Parse(HttpContext.Request.Form["id"]);
            var title = HttpContext.Request.Form["Title"];
            var desc = HttpContext.Request.Form["Description"];
            var expiryDate = DateTime.Parse(HttpContext.Request.Form["ExpiryDate"]);

            //find item in todo list by id
            var foundItem = ToDoList.Where(f => f.Id == id).FirstOrDefault();
            
            if (foundItem != null)
            {
                //overwrite below attributes that come from request
                foundItem.Title = title;
                foundItem.Description = desc;
                foundItem.ExpiryDate = expiryDate;
            }
            //update data changes and save to dbase.json
            ToDo.CommitChanges(ToDoList, _rootPath);

            return "ok";
        }

        [HttpPost("UpdatePercentage")]
        public ActionResult<string> UpdatePercentage()
        {
            //get post body from request
            var id = int.Parse(HttpContext.Request.Form["id"]);
            var completePercentage = int.Parse(HttpContext.Request.Form["CompletePercentage"]);

            //find item in todo list by id
            var foundItem = ToDoList.Where(f => f.Id == id).FirstOrDefault();

            if (foundItem != null)
            {
                //overwrite below attributes that come from request
                foundItem.CompletePercentage = completePercentage;
            }
            //update data changes and save to dbase.json
            ToDo.CommitChanges(ToDoList, _rootPath);

            return "ok";
        }


        [HttpPost("MarkAsDone")]
        public ActionResult<string> MarkAsDone()
        {
            //get post body from request
            var id = int.Parse(HttpContext.Request.Form["id"]);

            //find item in todo list by id using linq
            var foundItem = ToDoList.Where(f => f.Id == id).FirstOrDefault();

            if (foundItem != null)
            {
                //overwrite below attributes that come from request
                foundItem.IsDone = true;
                //assuming if mark as done then update complete percentage to 100%
                foundItem.CompletePercentage = 100;
            }
            //update data changes and save to dbase.json
            ToDo.CommitChanges(ToDoList, _rootPath);
            return "ok";
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //find item in todo list by id using linq
            var foundItem = ToDoList.Where(f => f.Id == id).FirstOrDefault();

            if (foundItem != null)
            {
                //remove item by found item object
                ToDoList.Remove(foundItem);
            }
            //update data changes and save to dbase.json
            ToDo.CommitChanges(ToDoList, _rootPath);
        }
    }
}
