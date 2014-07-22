namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.actlog")]
    public partial class actlog
    {
        [Display(Name = "�����s��")]
        public int id { get; set; }

        [Required]
        [Display(Name = "�ϥκ���")]
        [StringLength(40)]
        public string App { get; set; }

        [Required]
        [Display(Name = "�ާ@�H��")]
        [StringLength(40)]
        public string Pepo { get; set; }

        [Required]
        [Display(Name = "�ʧ@����")]
        [StringLength(200)]
        public string Act { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "�B�~����")]
        [StringLength(65535)]
        public string Ext { get; set; }

        [Display(Name = "�����ɶ�")]
        public DateTime Tm { get; set; }
    }
}
