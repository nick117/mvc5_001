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


        [Display(Name = "部門編號")]
        public int id { get; set; }

        [Required]
        [Display(Name = "部門名稱")]
        [StringLength(200)]
        public string title { get; set; }

        public virtual ICollection<emp> emp { get; set; }
    }
}
