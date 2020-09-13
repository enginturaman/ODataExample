using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ET.ODataExamples.Storage.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class BaseInfoEntity : BaseEntity
    {
        [Required]
        public bool Deleted { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
