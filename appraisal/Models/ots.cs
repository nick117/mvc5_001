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
        [Display(Name = "�϶��N��")]
        [StringLength(50)]
        public string Skey { get; set; }

        [Required]
        [Display(Name = "���")]
        [StringLength(200)]
        public string Vl { get; set; }
    }
}
