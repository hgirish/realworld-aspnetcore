using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Domain
{
  public class Person
  {
    [JsonIgnore]
    public int PersonId { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Bio { get; set; }

    public string Image { get; set; }
    [JsonIgnore]
    public byte[] Hash { get; set; }

    [JsonIgnore]
    public byte[] Salt { get; set; }
  }
}
