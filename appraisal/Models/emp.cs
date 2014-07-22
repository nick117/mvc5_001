namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.emp")]
    public partial class emp
    {
        public emp()
        {
            ts1 = new HashSet<ts>();
            ts2 = new HashSet<ts>();
        }
        [Display(Name = "序號")]
        public int id { get; set; }

        [Required]
        [Display(Name = "卡號")]
        [StringLength(12)]
        public string eid { get; set; }

        [Required]
        [Display(Name = "姓名")]
        [StringLength(20)]
        public string cname { get; set; }

        [Display(Name = "部門編號")]
        public int dept { get; set; }

        [Required]
        [Display(Name = "職務")]
        [StringLength(40)]
        public string title { get; set; }

         [ForeignKey("dept")]
        public virtual dep dep { get; set; }

        [ForeignKey("emp")]
        public virtual ICollection<ts> ts1 { get; set; }

        [ForeignKey("boss")]
        public virtual ICollection<ts> ts2 { get; set; }

    }
}
