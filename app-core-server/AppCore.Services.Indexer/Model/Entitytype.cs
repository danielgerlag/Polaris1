using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Model
{
    [Table("EntityType", Schema = "Indexer")]
    public class EntityType
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(500)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
    }
}
