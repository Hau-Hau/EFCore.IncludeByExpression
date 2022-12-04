using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.NavigationPropertyPathSchema.Tests.Unit.Data.Entieties
{
    public class CEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CEntity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public CEntity(BEntity parent)
        {
            Parent = parent;
        }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public BEntity Parent { get; set; }
        public IEnumerable<DEntity>? Childs { get; set; }
    }
}
