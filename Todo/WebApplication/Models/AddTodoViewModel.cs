using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
    }
}
