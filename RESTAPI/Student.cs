using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPI


{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public List<Course> Courses { get; set; }

    }
}
    