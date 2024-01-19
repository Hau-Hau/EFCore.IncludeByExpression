using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.IncludeByExpression.Tests.Data.Entieties
{
    public class BEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BEntity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public BEntity(AEntity parent)
        {
            Parent = parent;
        }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public AEntity Parent { get; set; }
        public IEnumerable<CEntity>? Childs { get; set; }
    }
}
