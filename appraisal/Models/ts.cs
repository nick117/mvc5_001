namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.ts")]
    public partial class ts
    {
        [Display(Name = "序號")]
        public int id { get; set; }

        [Display(Name = "員工序號")]
        public int emp { get; set; }

        [Display(Name = "評核序號")]
        public int exm { get; set; }

        [Display(Name = "主管序號")]
        public int boss { get; set; }

        [Display(Name = "評核分數")]
        public decimal? vl { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "評核說明")]
        [StringLength(65535)]
        public string suggest { get; set; }

        [Display(Name = "鎖住")]
        [Column(TypeName = "bit")]
        public bool lockAudit { get; set; }
   
        [ForeignKey("emp")]
        public virtual emp emp1 { get; set; }

        [ForeignKey("boss")]
        public virtual emp emp2 { get; set; }

         [ForeignKey("exm")]
        public virtual exm exm1 { get; set; }
    }
}
