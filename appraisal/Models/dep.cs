namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.dep")]
    public partial class dep
    {
        public dep()
        {
            emp = new HashSet<emp>();
        }


        [Display(Name = "�����s��")]
        public int id { get; set; }

        [Required]
        [Display(Name = "�����W��")]
        [StringLength(200)]
        public string title { get; set; }

        public virtual ICollection<emp> emp { get; set; }
    }
}
