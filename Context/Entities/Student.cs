using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFProject.Context.Entities
{
    class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1), MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1), MaxLength(30)]
        public string LastName { get; set; }

        [MinLength(1), MaxLength(30)]
        public string MiddleName { get; set; }

        [ForeignKey(nameof(GroupRefId))]
        public Group Group { get; set; }

        public int GroupRefId { get; set; }
    }
}
