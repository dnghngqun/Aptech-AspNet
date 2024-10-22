using StudentApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
namespace StudentApp.Controllers{
    public class StudentController : Controller
    {
        private static List<Student> students = new List<Student>{
            new Student{Id=1,Name="Hung",Age=20,Email="Hung@gmail.com"},
            new Student{Id=2,Name="Huong",Age=21,Email="Huong@gmail.com"},
            new Student{Id=3,Name="Hoa",Age=18,Email="Hoa@gmail.com"}
        };
        //:get//Student/Index
        public IActionResult Index() {
            return View(students);
        }

        //Get:/Student/Create
        public IActionResult Create() {
            return View();
        }

        //POST :/Student/Create
        [HttpPost]//annotation
        [ValidateAntiForgeryToken] //để bảo vệ ứng dụng khỏi các cuộc tấn công Cross-Site Request Forgery (CSRF) (tấn công giả mạo yêu cầu giữa các trang web).
        public IActionResult Create(Student student) {
            if (ModelState.IsValid) {
                student.Id = students.Max(s => s.Id) + 1;
                students.Add(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        //Get:/Student/Edit
        public IActionResult Edit(int id) {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) {
                return NotFound();
            } else {
                return View(student);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student) {
            var studentToUpdate = students.FirstOrDefault(s => s.Id == student.Id);
            if (studentToUpdate == null) {
                return NotFound();
            } else {
                studentToUpdate.Name = student.Name;
                studentToUpdate.Age = student.Age;
                studentToUpdate.Email = student.Email;
                return RedirectToAction(nameof(Index));
            }
        }

        
        // GET: /Student/Delete/5
        public IActionResult Delete(int id) {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) {
                return NotFound();
            }
            return View("ConfirmDelete", student); // Hiển thị view xác nhận xóa
        }   

        // POST: /Student/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id) {
            var studentToDelete = students.FirstOrDefault(s => s.Id == id);
            if (studentToDelete == null) {
                return NotFound();
            } else {
                students.Remove(studentToDelete);
                return RedirectToAction(nameof(Index)); 
            }
        }



        // // get: /Student/Search
        // public IActionResult Search(){
        //     return View();
        // }


        // [HttpGet]
        
        // // GET: /Student/Search/Hung
        // public IActionResult Search(string query)
        // {
        //     List<Student> result;

        //     if (int.TryParse(query, out int id))
        //     {
        //         result = students.Where(s => s.Id == id).ToList();
        //     }
        //     else{
        //         result = students.Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        //         //StringComparison.OrdinalIgnoreCase: so sánh ko phân biệt hoa thường
        //     }
        //     return View(result);
        // }

        [HttpGet]
        
        public IActionResult Search(string query)
        {
            List<Student> result;

            // Nếu query null hoặc rỗng, hiển thị form tìm kiếm
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Student>()); // Trả về view rỗng nếu chưa tìm kiếm
            }

            // Nếu query là số, tìm theo ID
            if (int.TryParse(query, out int id))
            {
                result = students.Where(s => s.Id == id).ToList();
            }
            else
            {
                // Tìm theo tên
                result = students.Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(result);
        }

    }
}