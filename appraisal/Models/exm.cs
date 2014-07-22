namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.exm")]
    public partial class exm
    {
        public exm()
        {
            ts = new HashSet<ts>();
        }

         [Display(Name = "�Ǹ�")]
        public int id { get; set; }

        [Required]
        [Display(Name = "�������O")]
        [StringLength(200)]
        public string subject { get; set; }

        [Required]
        [Display(Name = "���֥N��")]
        [StringLength(8)]
        public string sn { get; set; }

        public virtual ICollection<ts> ts { get; set; }
    }
}
