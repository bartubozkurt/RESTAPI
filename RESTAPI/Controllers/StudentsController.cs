using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPI
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentsController : ControllerBase
    {
        private static readonly List<Student> _studentsInMemoryStore = new List<Student>();
        private static readonly List<Course> _coursesInMemoryStore = new List<Course>();

        public static List<Student> StudentsInMemoryStore => _studentsInMemoryStore;
        public static List<Course> CoursesInMemoryStore => _coursesInMemoryStore;


        [HttpGet]
        public ActionResult<List<Student>> Getall()
        {
            return _studentsInMemoryStore;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetById(int id)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);
            if (st == null)
            {
                return NotFound();
            }
            return st;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student> Create(Student st)
        {
            st.Id = _studentsInMemoryStore.Any() ? _studentsInMemoryStore.Max(p => p.Id) + 1 : 1;
            _studentsInMemoryStore.Add(st);
            return CreatedAtAction(nameof(GetById), new { id = st.Id }, st);

        }

        [HttpPut("{}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> UpdateById(int id, Student _st)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);
            if(st == null)
            {
                return NotFound();
            }
            st.Name = _st.Name;
            return st;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> DeleteByid(int id)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);
            if(st == null)
            {
                return NotFound();
            }
            _studentsInMemoryStore.Remove(st);
            return st;
        }

        [HttpGet("{id}/courses")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Course>> GetAll(int id)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);
            if (st == null)
            {
                return NotFound();
            }
            if (st.Courses == null)
            {
                return NotFound();
            }

            return st.Courses;
        }

        [HttpPost("{id}/courses")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Course> Create(int id, Course ct_n)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);
            bool existed = _coursesInMemoryStore.Exists(p => p.Name == ct_n.Name);
            var ct = existed ? _coursesInMemoryStore.FirstOrDefault(p => p.Name == ct_n.Name) : ct_n;

            if (st == null)
            {
                return NotFound();
            }
            if (st.Courses == null)
            {
                st.Courses = new List<Course>();
            }
            if (!existed)
            {
                ct.Id = _coursesInMemoryStore.Any() ?
                     _coursesInMemoryStore.Max(p => p.Id) + 1 : 1;
                _coursesInMemoryStore.Add(ct);
            }
            //if (!st.Courses.Exists(p => p.Id == ct.Id))
            {
                st.Courses.Add(ct);
            }
            if (ct.Students == null)
            {
                ct.Students = new List<int>();
            }
            if (!ct.Students.Exists(p => p == st.Id))
            {
                ct.Students.Add(st.Id);
            }
            return CreatedAtAction(nameof(GetById), new { id = st.Id, id_c = ct.Id }, ct);
        }


        [HttpGet("{id}/courses/{id_c}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Course> GetById(int id, int id_c)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);

            if (st == null)
            {
                return NotFound();
            }
            if (st.Courses == null)
            {
                return NotFound();
            }
            var ct = st.Courses.FirstOrDefault(p => p.Id == id_c);
            if (ct == null)
            {
                return NotFound();
            }

            return ct;
        }


        [HttpDelete("{id}/courses/{id_c}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<Course>> DeleteById(int id, int id_c)
        {
            var st = _studentsInMemoryStore.FirstOrDefault(p => p.Id == id);

            if (st == null)
            {
                return NotFound();
            }
            if (st.Courses == null)
            {
                return NotFound();
            }
            var ct = st.Courses.FirstOrDefault(p => p.Id == id_c);
            if (ct == null)
            {
                return NotFound();
            }
            st.Courses.Remove(ct);
            return st.Courses;
        }


    }
}
