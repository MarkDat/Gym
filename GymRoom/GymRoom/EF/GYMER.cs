namespace GymRoom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GYMER")]
    public partial class GYMER
    {
        public long id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public DateTime? dateCreate { get; set; }

        public DateTime? dateModify { get; set; }

        public DateTime? dateRegistraion { get; set; }

        public DateTime? dateExpired { get; set; }

        public int? numMonth { get; set; }

        [StringLength(250)]
        public string adress { get; set; }

        [StringLength(250)]
        public string note { get; set; }

        public bool? status { get; set; }
    }
}
