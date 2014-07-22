namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.ots")]
    public partial class ots
    {
        [Key]
        [Display(Name = "區間代號")]
        [StringLength(50)]
        public string Skey { get; set; }

        [Required]
        [Display(Name = "日期")]
        [StringLength(200)]
        public string Vl { get; set; }
    }
}
