using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Javeriana.Api.DTO
{
    public class Tarea : IEquatable<Tarea>
    {
        [JsonPropertyName("id")]
        public long Id { get; set;}

        [Required]
        [MinLength(3)]
        [JsonPropertyName("name")]
        public string Name {get; set;}

        [JsonPropertyName("isComplete")]
        public bool IsComplete {get; set;}

        public override bool Equals(object obj)
        {
            if(obj == null) return false;

            Tarea other = obj as Tarea;
            
            return Equals(other);
        }

        public bool Equals(Tarea other)
        {
            if(other == null) return false;

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}