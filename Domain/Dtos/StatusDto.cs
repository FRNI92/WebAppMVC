using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos;

public class StatusDto
{
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;
}
