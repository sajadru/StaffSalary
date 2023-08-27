using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffSalary.Core.EntityHelper
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        [Timestamp]
        public byte[] VersionCtrl { get; set; }
    }
}
