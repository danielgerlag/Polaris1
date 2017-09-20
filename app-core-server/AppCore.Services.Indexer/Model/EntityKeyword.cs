using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Model
{
    [Table("EntityKeyword", Schema = "Indexer")]
    public class EntityKeyword
    {
        [Key]
        public long ID { get; set; }

        [Index]
        public long EntityIndexID { get; set; }

        public virtual EntityIndex EntityIndex { get; set; }

        [Required]
        [MaxLength(200)]
        public string Keyword { get; set; }

        public long KeywordHash { get; set; }

        [ForeignKey("ReferenceEntityType")]
        public int? ReferenceEntityTypeID { get; set; }

        public virtual EntityType ReferenceEntityType { get; set; }

        public int? ReferenceEntityKey { get; set; }
    }
}
