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
        [Display(Name = "�Ǹ�")]
        public int id { get; set; }

        [Display(Name = "���u�Ǹ�")]
        public int emp { get; set; }

        [Display(Name = "���֧Ǹ�")]
        public int exm { get; set; }

        [Display(Name = "�D�ާǸ�")]
        public int boss { get; set; }

        [Display(Name = "���֤���")]
        public decimal? vl { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "���ֻ���")]
        [StringLength(65535)]
        public string suggest { get; set; }

        [Display(Name = "���")]
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
