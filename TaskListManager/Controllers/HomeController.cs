using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskListManager.Models;

namespace Capstone_6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Error = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult RegisterNewUser(User newUser)
        {
            TaskListDBEntities ORM = new TaskListDBEntities();

            ORM.Users.Add(newUser);

            ORM.SaveChanges();

            return View("Index");
        }

        public ActionResult SignIn(string UserName, string Password)
        {
            TaskListDBEntities ORM = new TaskListDBEntities();

            User currentUser = ORM.Users.Find(UserName);

            if (currentUser == null)
            {
                ViewBag.Error = "Username does not exist. Did you mean to register?";
                return View("Index");
            }
            else if (currentUser.Password != Password)
            {
                ViewBag.Error = "Incorrect password.";
                return View("Index");
            }

            return RedirectToAction("UserTasks");
        }

        public ActionResult UserTasks()
        {
            TaskListDBEntities ORM = new TaskListDBEntities();
            ViewBag.TaskList= ORM.TasksTables.ToList();
            ViewBag.Message = "added!";
            return View();
        }

        public ActionResult AddTaskDetails()
        {
            return View();
        }

        public ActionResult AddNewTask(TasksTable NewTask)
        {
            if (ModelState.IsValid)
            {
                TaskListDBEntities ORM = new TaskListDBEntities();
                if (ORM.TasksTables.ToList().Count == 0)
                {
                    NewTask.TaskNumber = "1";
                }
                else
                {
                    NewTask.TaskNumber = (int.Parse(ORM.TasksTables.ToList().Last().TaskNumber) + 1).ToString();
                }
                ORM.TasksTables.Add(NewTask);
                ORM.SaveChanges();
                return RedirectToAction("UserTasks");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteTask(string TaskNumber)
        {
            TaskListDBEntities ORM = new TaskListDBEntities();
            TasksTable Found = ORM.TasksTables.Find(TaskNumber);

            if(Found != null)
            {
                ORM.TasksTables.Remove(Found);
                ORM.SaveChanges();
                return RedirectToAction("UserTasks");
            }
            else
            {
                ViewBag.ErrorMessage = "Task not found";
                return View("Error");
            }

        }

        public ActionResult UpdateTaskList(string TaskNumber)
        {
            TaskListDBEntities ORM = new TaskListDBEntities();
            TasksTable Found = ORM.TasksTables.Find(TaskNumber);

            if(Found != null)
            {
                return View("UpdatedTaskDetails",Found);
            }
            else
            {
                ViewBag.ErrorMessage = "Tasks Not Found";
                return View("Error");
            }
        }

        public ActionResult SaveTaskUpdates (TasksTable UpdateTaskList)
        {
            TaskListDBEntities ORM = new TaskListDBEntities();
            TasksTable OldTaskRecord = ORM.TasksTables.Find(UpdateTaskList.TaskNumber);

            if(OldTaskRecord !=null && ModelState.IsValid)
            {
                OldTaskRecord.Description = UpdateTaskList.Description;
                OldTaskRecord.DueDate = UpdateTaskList.DueDate;
                OldTaskRecord.Status = UpdateTaskList.Status;
                OldTaskRecord.TaskNumber = UpdateTaskList.TaskNumber;
                ORM.Entry(OldTaskRecord).State = System.Data.Entity.EntityState.Modified;
                ORM.SaveChanges();
                return RedirectToAction("UserTasks");
            }
            else
            {
                ViewBag.ErrorMessage = "Task not Found";
                return View("Error");
            }
        }

    }
   
}